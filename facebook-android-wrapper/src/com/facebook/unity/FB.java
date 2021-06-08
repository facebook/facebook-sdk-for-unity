/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

package com.facebook.unity;

import java.io.FileNotFoundException;
import java.math.BigDecimal;
import java.util.*;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.atomic.AtomicBoolean;

import android.annotation.TargetApi;
import android.content.Intent;
import android.app.Activity;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.util.Base64;
import android.content.pm.*;
import android.content.pm.PackageManager.NameNotFoundException;

import com.facebook.*;
import com.facebook.appevents.AppEventsLogger;
import com.facebook.appevents.internal.ActivityLifecycleTracker;
import com.facebook.appevents.internal.AutomaticAnalyticsLogger;
import com.facebook.applinks.AppLinkData;
import com.facebook.internal.BundleJSONConverter;
import com.facebook.internal.Utility;
import com.facebook.internal.InternalSettings;
import com.facebook.login.LoginManager;
import com.facebook.gamingservices.cloudgaming.AppToUserNotificationSender;
import com.facebook.gamingservices.cloudgaming.CloudGameLoginHandler;
import com.facebook.gamingservices.cloudgaming.DaemonRequest;
import com.facebook.gamingservices.cloudgaming.GameFeaturesLibrary;
import com.facebook.gamingservices.cloudgaming.InAppAdLibrary;
import com.facebook.gamingservices.cloudgaming.InAppPurchaseLibrary;
import com.facebook.gamingservices.cloudgaming.PlayableAdsLibrary;
import com.facebook.gamingservices.GamingImageUploader;
import com.facebook.gamingservices.GamingVideoUploader;
import com.facebook.share.widget.ShareDialog;
import com.facebook.LoginStatusCallback;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class FB {
    static final String TAG = FB.class.getName();
    // i.e. the game object that receives this message
    static final String FB_UNITY_OBJECT = "UnityFacebookSDKPlugin";
    private static Intent intent;
    private static Intent clearedIntent;
    private static AppEventsLogger appEventsLogger;
    private static AtomicBoolean activateAppCalled = new AtomicBoolean();
    static ShareDialog.Mode ShareDialogMode = ShareDialog.Mode.AUTOMATIC;

    private static AppEventsLogger getAppEventsLogger() {
        if (appEventsLogger == null) {
            appEventsLogger = AppEventsLogger.newLogger(getUnityActivity().getApplicationContext());
        }
        return appEventsLogger;
    }

    public static Activity getUnityActivity() {
        return UnityReflection.GetUnityActivity();
    }

    @UnityCallable
    public static void Init(final String params_str) {
        Log.v(TAG, "Init(" + params_str + ")");
        UnityParams unity_params = UnityParams.parse(params_str, "couldn't parse init params: "+params_str);
        final String appID;
        if (unity_params.hasString("appId")) {
            appID = unity_params.getString("appId");
        } else {
            appID = Utility.getMetadataApplicationId(getUnityActivity());
        }

        FacebookSdk.setApplicationId(appID);
        FacebookSdk.sdkInitialize(FB.getUnityActivity(), new FacebookSdk.InitializeCallback() {
            @Override
            public void onInitialized() {
                final UnityMessage unityMessage = new UnityMessage("OnInitComplete");
                // If we have a cached access token send it back as well
                AccessToken token = AccessToken.getCurrentAccessToken();
                if (token != null) {
                    FBLogin.addLoginParametersToMessage(unityMessage, token, null);
                } else {
                    unityMessage.put("key_hash", FB.getKeyHash());
                }

                if (FacebookSdk.getAutoLogAppEventsEnabled()) {
                    FB.ActivateApp(appID);
                }

                unityMessage.send();
            }
        });
    }

    @UnityCallable
    public static void LoginWithReadPermissions(String params_str) {
        Log.v(TAG, "LoginWithReadPermissions(" + params_str + ")");
        Intent intent = new Intent(getUnityActivity(), FBUnityLoginActivity.class);
        intent.putExtra(FBUnityLoginActivity.LOGIN_PARAMS, params_str);
        intent.putExtra(FBUnityLoginActivity.LOGIN_TYPE, FBUnityLoginActivity.LoginType.READ);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void LoginWithPublishPermissions(String params_str) {
        Log.v(TAG, "LoginWithPublishPermissions(" + params_str + ")");
        Intent intent = new Intent(getUnityActivity(), FBUnityLoginActivity.class);
        intent.putExtra(FBUnityLoginActivity.LOGIN_PARAMS, params_str);
        intent.putExtra(FBUnityLoginActivity.LOGIN_TYPE, FBUnityLoginActivity.LoginType.PUBLISH);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void Logout(String params_str) {
        Log.v(TAG, "Logout(" + params_str + ")");
        LoginManager.getInstance().logOut();
        UnityMessage message = new UnityMessage("OnLogoutComplete");
        message.put("did_complete", true);
        message.send();
    }

    @UnityCallable
    public static void RetrieveLoginStatus(String params_str) {
        Log.v(TAG, "RetrieveLoginStatus(" + params_str + ")");

        if (!FacebookSdk.isInitialized()) {
            Log.w(FB.TAG, "Facebook SDK not initialized. Call init() before calling login()");
            return;
        }

        final UnityMessage unityMessage = new UnityMessage("OnLoginStatusRetrieved");
        unityMessage.put("key_hash", getKeyHash());

        UnityParams unity_params = UnityParams.parse(params_str,
            "couldn't parse login params: " + params_str);
        String callbackIDString = null;
        if (unity_params.has(Constants.CALLBACK_ID_KEY)) {
            callbackIDString = unity_params.getString(Constants.CALLBACK_ID_KEY);
            unityMessage.put(Constants.CALLBACK_ID_KEY, callbackIDString);
        }
        final String callbackID = callbackIDString;

        LoginManager.getInstance().retrieveLoginStatus(
            getUnityActivity(),
            new LoginStatusCallback() {
                @Override
                public void onCompleted(final AccessToken accessToken) {
                    FBLogin.addLoginParametersToMessage(unityMessage, accessToken, callbackID);
                    unityMessage.send();
                }

                @Override
                public void onFailure() {
                    unityMessage.put("failed", true);
                    unityMessage.send();
                }

                @Override
                public void onError(Exception exception) {
                    unityMessage.sendError(exception.getMessage());
                }

            }
        );
    }

    @UnityCallable
    public static void loginForTVWithReadPermissions(String params_str) {
        Log.v(TAG, "loginForTVWithReadPermissions(" + params_str + ")");
        Intent intent = new Intent(getUnityActivity(), FBUnityLoginActivity.class);
        intent.putExtra(FBUnityLoginActivity.LOGIN_PARAMS, params_str);
        intent.putExtra(FBUnityLoginActivity.LOGIN_TYPE, FBUnityLoginActivity.LoginType.TV_READ);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void LoginForTVWithPublishPermissions(String params_str) {
        Log.v(TAG, "LoginForTVWithPublishPermissions(" + params_str + ")");
        Intent intent = new Intent(getUnityActivity(), FBUnityLoginActivity.class);
        intent.putExtra(FBUnityLoginActivity.LOGIN_PARAMS, params_str);
        intent.putExtra(FBUnityLoginActivity.LOGIN_TYPE, FBUnityLoginActivity.LoginType.TV_PUBLISH);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void AppRequest(String params_str) {
        Log.v(TAG, "AppRequest(" + params_str + ")");
        Intent intent = new Intent(getUnityActivity(), FBUnityGameRequestActivity.class);
        UnityParams unity_params = UnityParams.parse(params_str);
        Bundle params = unity_params.getStringParams();
        intent.putExtra(FBUnityGameRequestActivity.GAME_REQUEST_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void GameGroupCreate(String params_str) {
        Log.v(TAG, "GameGroupCreate(" + params_str + ")");
        final UnityParams unity_params = UnityParams.parse(params_str);
        final Bundle params = unity_params.getStringParams();
        Intent intent = new Intent(getUnityActivity(), FBUnityCreateGameGroupActivity.class);
        intent.putExtra(FBUnityCreateGameGroupActivity.CREATE_GAME_GROUP_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void GameGroupJoin(String params_str) {
        Log.v(TAG, "GameGroupJoin(" + params_str + ")");
        final UnityParams unity_params = UnityParams.parse(params_str);
        final Bundle params = unity_params.getStringParams();
        Intent intent = new Intent(getUnityActivity(), FBUnityJoinGameGroupActivity.class);
        intent.putExtra(FBUnityJoinGameGroupActivity.JOIN_GAME_GROUP_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void ShareLink(String params_str) {
        Log.v(TAG, "ShareLink(" + params_str + ")");
        final UnityParams unity_params = UnityParams.parse(params_str);
        final Bundle params = unity_params.getStringParams();
        Intent intent = new Intent(getUnityActivity(), FBUnityDialogsActivity.class);
        intent.putExtra(FBUnityDialogsActivity.DIALOG_TYPE, ShareDialogMode);
        intent.putExtra(FBUnityDialogsActivity.SHARE_DIALOG_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void FeedShare(String params_str) {
        Log.v(TAG, "FeedShare(" + params_str + ")");
        final UnityParams unityParams = UnityParams.parse(params_str);
        final Bundle params = unityParams.getStringParams();
        Intent intent = new Intent(getUnityActivity(), FBUnityDialogsActivity.class);
        intent.putExtra(FBUnityDialogsActivity.DIALOG_TYPE, ShareDialog.Mode.FEED);
        intent.putExtra(FBUnityDialogsActivity.FEED_DIALOG_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void SetUserID(String params_str) {
        Log.v(TAG, "SetUserID(" + params_str + ")");
        AppEventsLogger.setUserID(params_str);
    }

    @UnityCallable
    public static String GetUserID() {
        if (!FacebookSdk.isInitialized()) {
            return null;
        }
        return AppEventsLogger.getUserID();
    }

    @UnityCallable
    public static String GetCurrentProfile() {
        if (!FacebookSdk.isInitialized()) {
            return null;
        }
        Profile profile = Profile.getCurrentProfile();
        try {
            JSONObject json = new JSONObject();
            if (profile != null) {
                String userID = profile.getId();
                String firstName = profile.getFirstName();
                String middleName = profile.getMiddleName();
                String lastName = profile.getLastName();
                String name = profile.getName();
                Uri linkUri = profile.getLinkUri();
                if (userID != null) {
                    json.put("userID", userID);
                }
                if (firstName != null) {
                    json.put("firstName", firstName);
                }
                if (middleName != null) {
                    json.put("middleName", middleName);
                }
                if (lastName != null) {
                    json.put("lastName", lastName);
                }
                if (name != null) {
                    json.put("name", name);
                }
                if (linkUri != null) {
                    json.put("linkURL", linkUri.toString());
                }
            }
            return json.toString();
        } catch (Exception e) {
        }
        return "";
    }

    @UnityCallable
    public static void UpdateUserProperties(String params_str) {
      Log.v(TAG, "UpdateUserProperties(" + params_str + ")");
      final UnityParams unityParams = UnityParams.parse(params_str);
      final Bundle params = unityParams.getStringParams();
      AppEventsLogger.updateUserProperties(params, null);
    }

    @UnityCallable
    public static void SetDataProcessingOptions(String params_str) {
      Log.v(TAG, "SetDataProcessingOptions(" + params_str + ")");
      final UnityParams unityParams = UnityParams.parse(params_str);
      try {
        JSONObject json = unityParams.json;
        JSONArray array = json.getJSONArray("options");
        int country = json.optInt("country", 0);
        int state = json.optInt("state", 0);
        String[] options = new String[array.length()];
        for (int i = 0; i < array.length(); i++) {
          options[i] = array.getString(i);
        }
        FacebookSdk.setDataProcessingOptions(options, country, state);
      } catch (Exception e) {
      }
  }

    public static void SetIntent(Intent intent) {
        FB.intent = intent;
    }

    public static void SetLimitEventUsage(String params_str) {
        Log.v(TAG, "SetLimitEventUsage(" + params_str + ")");
        FacebookSdk.setLimitEventAndDataUsage(
                getUnityActivity().getApplicationContext(),
                Boolean.valueOf(params_str));
    }

    @UnityCallable
    public static void LogAppEvent(String params_str) {
        Log.v(TAG, "LogAppEvent(" + params_str + ")");
        UnityParams unity_params = UnityParams.parse(params_str);

        Bundle parameters = new Bundle();
        if (unity_params.has("parameters")) {
            UnityParams unity_params_parameter = unity_params.getParamsObject("parameters");
            parameters = unity_params_parameter.getStringParams();
        }

        if (unity_params.has("logPurchase")) {
            FB.getAppEventsLogger().logPurchase(
                    new BigDecimal(unity_params.getDouble("logPurchase")),
                    Currency.getInstance(unity_params.getString("currency")),
                    parameters
            );
        } else if (unity_params.hasString("logEvent")) {
            if (unity_params.has("valueToSum")) {
                FB.getAppEventsLogger().logEvent(
                        unity_params.getString("logEvent"),
                        unity_params.getDouble("valueToSum"),
                        parameters
                );
            } else {
                FB.getAppEventsLogger().logEvent(
                        unity_params.getString("logEvent"),
                        parameters
                );
            }
        } else {
            Log.e(TAG, "couldn't logPurchase or logEvent params: " + params_str);
        }
    }

    @UnityCallable
    public static boolean IsImplicitPurchaseLoggingEnabled() {
      return AutomaticAnalyticsLogger.isImplicitPurchaseLoggingEnabled();
    }

    @UnityCallable
    public static void SetShareDialogMode(String mode) {
        Log.v(TAG, "SetShareDialogMode(" + mode + ")");
        if (mode.equalsIgnoreCase("NATIVE")) {
            ShareDialogMode = ShareDialog.Mode.NATIVE;
        } else if (mode.equalsIgnoreCase("WEB")) {
            ShareDialogMode = ShareDialog.Mode.WEB;
        } else if (mode.equalsIgnoreCase(("FEED"))) {
            ShareDialogMode = ShareDialog.Mode.FEED;
        } else {
            ShareDialogMode = ShareDialog.Mode.AUTOMATIC;
        }
    }

    @UnityCallable
    public static void SetAutoLogAppEventsEnabled(String autoLogAppEventsEnabled) {
        Log.v(TAG, "SetAutoLogAppEventsEnabled(" + autoLogAppEventsEnabled + ")");
        FacebookSdk.setAutoLogAppEventsEnabled(
            Boolean.valueOf(autoLogAppEventsEnabled));
    }

    @UnityCallable
    public static void SetAdvertiserIDCollectionEnabled(String advertiserIDCollectionEnabled) {
        Log.v(TAG, "SetAdvertiserIDCollectionEnabled(" + advertiserIDCollectionEnabled + ")");
        FacebookSdk.setAdvertiserIDCollectionEnabled(
            Boolean.valueOf(advertiserIDCollectionEnabled));
    }

    @UnityCallable
    public static String GetSdkVersion() {
        return FacebookSdk.getSdkVersion();
    }

    @UnityCallable
    public static void SetUserAgentSuffix(String suffix) {
        Log.v(TAG, "SetUserAgentSuffix(" + suffix + ")");
        InternalSettings.setCustomUserAgent(suffix);
    }

    @UnityCallable
    public static void FetchDeferredAppLinkData(String paramsStr) {
        FB.LogMethodCall("FetchDeferredAppLinkData", paramsStr);

        UnityParams unityParams = UnityParams.parse(paramsStr);
        final UnityMessage unityMessage = new UnityMessage("OnFetchDeferredAppLinkComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        AppLinkData.fetchDeferredAppLinkData(
                getUnityActivity(),
                new AppLinkData.CompletionHandler() {
                    @Override
                    public void onDeferredAppLinkDataFetched(AppLinkData appLinkData) {
                        FB.addAppLinkToMessage(unityMessage, appLinkData);
                        unityMessage.send();
                    }
                });
    }

    @UnityCallable
    public static void GetAppLink(String paramsStr) {
        Log.v(TAG, "GetAppLink(" + paramsStr + ")");
        final UnityParams unityParams = UnityParams.parse(paramsStr);
        UnityMessage unityMessage = UnityMessage.createWithCallbackFromParams(
                "OnGetAppLinkComplete",
                unityParams);

        // If we don't have an intent return
        if (intent == null)
        {
            unityMessage.put("did_complete", true);
            unityMessage.send();
            return;
        }

        // If the app link has been used already
        if (intent == clearedIntent) {
            unityMessage.put("did_complete", true);
            unityMessage.send();
            return;
        }

        // Check to see if we have any app link data on the intent
        AppLinkData appLinkData = AppLinkData.createFromAlApplinkData(intent);
        if (appLinkData != null) {
            // We have an app link
            FB.addAppLinkToMessage(unityMessage, appLinkData);
            unityMessage.put("url", intent.getDataString());
        } else if (intent.getData() != null) {
            // We have a deep link
            unityMessage.put("url", intent.getDataString());
        } else {
            // No deep link or app link was provided when activity was started
            unityMessage.put("did_complete", true);
        }

        unityMessage.send();
    }

    @UnityCallable
    public static void ClearAppLink() {
        Log.v(TAG, "ClearAppLink");
        clearedIntent = intent;
    }

    @UnityCallable
    public static void RefreshCurrentAccessToken(String paramsStr) {
        FB.LogMethodCall("RefreshCurrentAccessToken", paramsStr);

        UnityParams unityParams = UnityParams.parse(paramsStr);
        final UnityMessage unityMessage = new UnityMessage("OnRefreshCurrentAccessTokenComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        AccessToken.refreshCurrentAccessTokenAsync(new AccessToken.AccessTokenRefreshCallback() {
            @Override
            public void OnTokenRefreshed(AccessToken accessToken) {
                FBLogin.addLoginParametersToMessage(unityMessage, accessToken, null);
                unityMessage.send();
            }

            @Override
            public void OnTokenRefreshFailed(FacebookException e) {
                unityMessage.sendError(e.getMessage());
            }
        });
    }

    /**
     * Provides the key hash to solve the openSSL issue with Amazon
     * @return key hash
     */
    @TargetApi(Build.VERSION_CODES.FROYO)
    public static String getKeyHash() {
        try {
            // In some cases the unity activity may not exist. This can happen when we are
            // completing a login and unity activity was killed in the background. In this
            // situation it's not necessary to send back the keyhash since the app will overwrite
            // the value with the value they get during the init call and the unity activity
            // wil be created by the time init is called.
            Activity activity = getUnityActivity();
            if (activity == null) {
                return "";
            }

            PackageInfo info = activity.getPackageManager().getPackageInfo(
                    activity.getPackageName(),
                    PackageManager.GET_SIGNATURES);
            for (Signature signature : info.signatures){
                MessageDigest md = MessageDigest.getInstance("SHA");
                md.update(signature.toByteArray());
                String keyHash = Base64.encodeToString(md.digest(), Base64.DEFAULT);
                Log.d(TAG, "KeyHash: " + keyHash);
                return keyHash;
            }
        } catch (NameNotFoundException e) {
        } catch (NoSuchAlgorithmException e) {
        }
        return "";
    }

    @UnityCallable
    public static void ActivateApp() {
        AppEventsLogger.activateApp(getUnityActivity());
    }

    @UnityCallable
    public static void OpenFriendFinderDialog(String params_str) {
        Log.v(TAG, "OpenFriendFinderDialog(" + params_str + ")");
        final UnityParams unity_params = UnityParams.parse(params_str);
        final Bundle params = unity_params.getStringParams();

        Intent intent = new Intent(getUnityActivity(), FBUnityGamingServicesFriendFinderActivity.class);
        intent.putExtra(FBUnityGamingServicesFriendFinderActivity.DIALOG_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    @UnityCallable
    public static void UploadImageToMediaLibrary(String params_str) {
        Log.v(TAG, "UploadImageToMediaLibrary(" + params_str + ")");
        UnityParams unityParams = UnityParams.parse(params_str);
        String caption = unityParams.getString("caption");
        Uri imageUri = Uri.parse(unityParams.getString("imageUri"));
        // As a convenience, convert the URI to file:// if it has no Scheme.
        // this is so that Unity code can pass just the path to the local
        // file.
        if (imageUri.getScheme() == null) {
            imageUri = imageUri.buildUpon().scheme("file").build();
        }
        boolean shouldLaunchMediaDialog = Boolean.parseBoolean(unityParams.getString("shouldLaunchMediaDialog"));

        final UnityMessage unityMessage = new UnityMessage("OnUploadImageToMediaLibraryComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        GamingImageUploader imageUploader = new GamingImageUploader(getUnityActivity());
        try {
            imageUploader.uploadToMediaLibrary(
                caption,
                imageUri,
                shouldLaunchMediaDialog,
                new GraphRequest.Callback() {
                    @Override
                    public void onCompleted(GraphResponse response) {
                        if (response.getError() != null) {
                            unityMessage.sendError(response.getError().toString());
                        } else {
                            String id = response.getJSONObject().optString("id", null);
                            if (id == null) {
                                unityMessage.sendError("Response did not contain ImageID");
                            } else {
                                unityMessage.put("id", id);
                                unityMessage.send();
                            }
                        }
                    }
                }
            );
        } catch (FileNotFoundException e) {
            unityMessage.sendError(e.toString());
        }
    }

    @UnityCallable
    public static void UploadVideoToMediaLibrary(String params_str) {
        Log.v(TAG, "UploadVideoToMediaLibrary(" + params_str + ")");
        UnityParams unityParams = UnityParams.parse(params_str);
        String caption = unityParams.getString("caption");
        Uri videoUri = Uri.parse(unityParams.getString("videoUri"));
        // As a convenience, convert the URI to file:// if it has no Scheme.
        // this is so that Unity code can pass just the path to the local
        // file.
        if (videoUri.getScheme() == null) {
            videoUri = videoUri.buildUpon().scheme("file").build();
        }

        final UnityMessage unityMessage = new UnityMessage("OnUploadVideoToMediaLibraryComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        GamingVideoUploader videoUploader = new GamingVideoUploader(getUnityActivity());
        try {
            videoUploader.uploadToMediaLibrary(
                caption,
                videoUri,
                new GraphRequest.OnProgressCallback() {
                    @Override
                    public void onCompleted(GraphResponse response) {
                        if (response.getError() != null) {
                            unityMessage.sendError(response.getError().toString());
                        } else {
                            String id = response.getJSONObject().optString("video_id", null);
                            if (id == null) {
                                unityMessage.sendError("Response did not contain ImageID");
                            } else {
                                unityMessage.put("video_id", id);
                                unityMessage.send();
                            }
                        }
                    }
                    @Override
                    public void onProgress(long current, long total) {

                    }
                }
            );
        } catch (FileNotFoundException e) {
            unityMessage.sendError(e.toString());
        }
    }

    @UnityCallable
    public static void OnIAPReady(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnOnIAPReadyComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        InAppPurchaseLibrary.onReady(
            getUnityActivity().getApplicationContext(),
            new JSONObject(),
            createDaemonCallback(unityMessage)
        );
    }

    @UnityCallable
    public static void GetCatalog(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnGetCatalogComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        InAppPurchaseLibrary.getCatalog(
            getUnityActivity().getApplicationContext(),
            new JSONObject(),
            createDaemonCallback(unityMessage)
        );
    }

    @UnityCallable
    public static void GetPurchases(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnGetPurchasesComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        InAppPurchaseLibrary.getPurchases(
            getUnityActivity().getApplicationContext(),
            new JSONObject(),
            createDaemonCallback(unityMessage)
        );
    }

    @UnityCallable
    public static void Purchase(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnPurchaseComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String productID = unityParams.getString("productID");
        String developerPayload = unityParams.getString("developerPayload");

        try {
            JSONObject params = new JSONObject().put(InAppPurchaseLibrary.PRODUCT_ID, productID);
            if (!developerPayload.isEmpty()) {
                params.put(InAppPurchaseLibrary.DEVELOPER_PAYLOAD, developerPayload);
            }

            InAppPurchaseLibrary.purchase(
                getUnityActivity().getApplicationContext(),
                params,
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void ConsumePurchase(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnConsumePurchaseComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String purchaseToken = unityParams.getString("purchaseToken");

        try {
            InAppPurchaseLibrary.consumePurchase(
                getUnityActivity().getApplicationContext(),
                (new JSONObject()).put(InAppPurchaseLibrary.PURCHASE_TOKEN, purchaseToken),
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void InitCloudGame(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnInitCloudGameComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        try {
            // will throw an exception if fails
            // extending basic timeout
            AccessToken accessToken = CloudGameLoginHandler.init(getUnityActivity().getApplicationContext(), 25);
            UnityMessage loginUnityMessage = new UnityMessage("OnLoginComplete");
            if (accessToken == null) {
                unityMessage.sendError("Failed to receive access token.");
                return;
            }
            FBLogin.addLoginParametersToMessage(loginUnityMessage, accessToken, null);
            loginUnityMessage.send();

            unityMessage.put("success", "");
            unityMessage.send();
        } catch(FacebookException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void ScheduleAppToUserNotification(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnScheduleAppToUserNotificationComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String title = unityParams.getString("title");
        String body = unityParams.getString("body");
        Uri media = Uri.parse(unityParams.getString("media"));
        // As a convenience, convert the URI to file:// if it has no Scheme.
        // this is so that Unity code can pass just the path to the local
        // file.
        if (media.getScheme() == null) {
            media = media.buildUpon().scheme("file").build();
        }
        int timeInterval;
        try {
            timeInterval = Integer.parseInt(unityParams.getString("timeInterval"));
        } catch(NumberFormatException e) {
            unityMessage.sendError(String.format("Invalid timeInterval: %s", e.getMessage()));
            return;
        }
        String payload = unityParams.getString("payload");
        if (payload.equals("null")) {
            payload = null;
        }

        GraphRequest.Callback callback = new GraphRequest.Callback() {
            @Override
            public void onCompleted(GraphResponse response) {
                if (response.getError() != null) {
                    unityMessage.sendError(response.getError().toString());
                } else {
                    unityMessage.put("success", "");
                    unityMessage.send();
                }
            }
        };

        try {
            AppToUserNotificationSender.scheduleAppToUserNotification(
                title, body, media, timeInterval, payload, callback);
        } catch (FileNotFoundException e) {
            unityMessage.sendError(String.format(e.getMessage()));
        }
    }

    @UnityCallable
    public static void LoadInterstitialAd(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnLoadInterstitialAdComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String placementID = unityParams.getString("placementID");

        try {
            InAppAdLibrary.loadInterstitialAd(
                getUnityActivity().getApplicationContext(),
                (new JSONObject()).put(InAppAdLibrary.PLACEMENT_ID, placementID),
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void ShowInterstitialAd(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnShowInterstitialAdComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String placementID = unityParams.getString("placementID");

        try {
            InAppAdLibrary.showInterstitialAd(
                getUnityActivity().getApplicationContext(),
                (new JSONObject()).put(InAppAdLibrary.PLACEMENT_ID, placementID),
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void LoadRewardedVideo(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnLoadRewardedVideoComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String placementID = unityParams.getString("placementID");

        try {
            InAppAdLibrary.loadRewardedVideo(
                getUnityActivity().getApplicationContext(),
                (new JSONObject()).put(InAppAdLibrary.PLACEMENT_ID, placementID),
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void ShowRewardedVideo(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnShowRewardedVideoComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }
        String placementID = unityParams.getString("placementID");

        try {
            InAppAdLibrary.showRewardedVideo(
                getUnityActivity().getApplicationContext(),
                (new JSONObject()).put(InAppAdLibrary.PLACEMENT_ID, placementID),
                createDaemonCallback(unityMessage)
            );
        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void GetPayload(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnGetPayloadComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        GameFeaturesLibrary.getPayload(
            getUnityActivity().getApplicationContext(),
            new JSONObject(),
            createDaemonCallback(unityMessage)
        );
    }

    @UnityCallable
    public static void PostSessionScore(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnPostSessionScoreComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        int score;
        try {
            score = Integer.parseInt(unityParams.getString("score"));
        } catch(NumberFormatException e) {
            unityMessage.sendError(String.format("Invalid score: %s", e.getMessage()));
            return;
        }

        try {
            GameFeaturesLibrary.postSessionScoreAsync(
                getUnityActivity().getApplicationContext(),
                score,
                createDaemonCallback(unityMessage)
            );

        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void PostTournamentScore(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnPostTournamentScoreComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        int score;
        try {
            score = Integer.parseInt(unityParams.getString("score"));
        } catch(NumberFormatException e) {
            unityMessage.sendError(String.format("Invalid score: %s", e.getMessage()));
            return;
        }

        try {
            GameFeaturesLibrary.postTournamentScoreAsync(
                getUnityActivity().getApplicationContext(),
                score,
                createDaemonCallback(unityMessage)
            );

        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void GetTournament(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnGetTournamentComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        GameFeaturesLibrary.getTournamentAsync(
            getUnityActivity().getApplicationContext(),
            createDaemonCallback(unityMessage)
        );
    }


    @UnityCallable
    public static void ShareTournament(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnShareTournamentComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        int score;
        try {
            score = Integer.parseInt(unityParams.getString("score"));
        } catch(NumberFormatException e) {
            unityMessage.sendError(String.format("Invalid score: %s", e.getMessage()));
            return;
        }

        JSONObject data = new JSONObject();
        Bundle dataBundle = unityParams.getParamsObject("data").getStringParams();
        Set<String> keys = dataBundle.keySet();
        for (String key : keys) {
            try {
                data.put(key, dataBundle.get(key));
            } catch(JSONException e) {
                unityMessage.sendError(String.format("Invalid data payload: %s", e.getMessage()));
            }
        }       


        try {
            GameFeaturesLibrary.shareTournamentAsync(
                getUnityActivity().getApplicationContext(),
                score,
                data,
                createDaemonCallback(unityMessage)
            );

        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    @UnityCallable
    public static void CreateTournament(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnCreateTournamentComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        int initialScore;
        try {
            initialScore = Integer.parseInt(unityParams.getString("initialScore"));
        } catch(NumberFormatException e) {
            unityMessage.sendError(String.format("Invalid initialScore: %s", e.getMessage()));
            return;
        }

        String title = unityParams.getString("title");
        String image = unityParams.getString("imageBase64DataUrl");
        String sortOrder = unityParams.getString("sortOrder");
        String scoreFormat = unityParams.getString("scoreFormat");

        Bundle dataBundle = unityParams.getParamsObject("data").getStringParams();
        JSONObject data = new JSONObject();
        Set<String> keys = dataBundle.keySet();
        for (String key : keys) {
            try {
                data.put(key, dataBundle.get(key));
            } catch(JSONException e) {
                unityMessage.sendError(String.format("Invalid data payload: %s", e.getMessage()));
            }
        }     

        try {
            GameFeaturesLibrary.createTournamentAsync(
                getUnityActivity().getApplicationContext(),
                initialScore,
                title,
                image,
                sortOrder,
                scoreFormat,
                null, // endTime
                data,
                createDaemonCallback(unityMessage)
            );

        } catch(JSONException e) {
            unityMessage.sendError(e.getMessage());
        }
    }

    public static void OpenAppStore(String params_str) {
        UnityParams unityParams = UnityParams.parse(params_str);
        final UnityMessage unityMessage = new UnityMessage("OnOpenAppStoreComplete");
        if (unityParams.hasString("callback_id")) {
            unityMessage.put("callback_id", unityParams.getString("callback_id"));
        }

        PlayableAdsLibrary.openAppStore(
            getUnityActivity().getApplicationContext(),
            new JSONObject(),
            createDaemonCallback(unityMessage)
        );
    }

    private static DaemonRequest.Callback createDaemonCallback(final UnityMessage unityMessage) {
        return (new DaemonRequest.Callback() {
            @Override
            public void onCompleted(GraphResponse response) {
                FacebookRequestError error = response.getError();
                if (error != null) {
                    try {
                        JSONObject errorJSON = new JSONObject();
                        errorJSON.put("errorCode", error.getErrorCode());
                        errorJSON.put("subErrorCode", error.getSubErrorCode());
                        errorJSON.put("errorType", error.getErrorType());
                        errorJSON.put("errorMessage", error.getErrorMessage());
                        unityMessage.sendError(errorJSON.toString());
                    } catch (JSONException e) {
                        // default, will not be parseable as JSON in Unity
                        unityMessage.sendError(error.toString());
                    }
                } else if (response.getJSONObject() != null) {
                    unityMessage.put("success", response.getJSONObject().toString());
                    unityMessage.send();
                } else if (response.getJSONArray() != null) {
                    unityMessage.put("success", response.getJSONArray().toString());
                    unityMessage.send();
                } else {
                    unityMessage.sendError("invalid response");
                }
            }
        });
    }

    private static void ActivateApp(String appId) {
        if (!activateAppCalled.compareAndSet(false, true)) {
            Log.w(TAG, "Activite app only needs to be called once");
            return;
        }
        final Activity unityActivity = getUnityActivity();
        if (appId != null) {
            AppEventsLogger.activateApp(
                    unityActivity.getApplication(),
                    appId);
        } else {
            AppEventsLogger.activateApp(unityActivity.getApplication());
        }

        // We already have a running activity so we need to call create activity to ensure the
        // logging is correct
        new Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                // These calls should be run on the ui thread.
                ActivityLifecycleTracker.onActivityCreated(unityActivity);
                ActivityLifecycleTracker.onActivityResumed(unityActivity);
            }
        });
    }

    private static void startActivity(Class<?> cls, String paramsStr) {
        Intent intent = new Intent(getUnityActivity(), cls);
        UnityParams unityParams = UnityParams.parse(paramsStr);
        Bundle params = unityParams.getStringParams();
        intent.putExtra(BaseActivity.ACTIVITY_PARAMS, params);
        getUnityActivity().startActivity(intent);
    }

    private static void LogMethodCall(String methodName, String paramsStr) {
        Log.v(TAG, String.format(Locale.ROOT, "%s(%s)", methodName, paramsStr));
    }

    private static void addAppLinkToMessage(UnityMessage unityMessage, AppLinkData appLinkData) {
        if (appLinkData == null) {
            unityMessage.put("did_complete", true);
            return;
        }

        unityMessage.put("ref", appLinkData.getRef());
        unityMessage.put("target_url", appLinkData.getTargetUri().toString());
        try {
            if (appLinkData.getArgumentBundle() != null) {
                unityMessage.put("extras", BundleJSONConverter.convertToJSON(
                        appLinkData.getArgumentBundle()).toString());
            }
        } catch (JSONException ex) {
            Log.e(TAG, ex.getLocalizedMessage());
        }
    }
}
