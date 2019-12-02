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

import java.math.BigDecimal;
import java.util.*;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.atomic.AtomicBoolean;

import android.annotation.TargetApi;
import android.content.Intent;
import android.app.Activity;
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
import com.facebook.share.widget.ShareDialog;

import org.json.JSONException;

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
    public static void UpdateUserProperties(String params_str) {
      Log.v(TAG, "UpdateUserProperties(" + params_str + ")");
      final UnityParams unityParams = UnityParams.parse(params_str);
      final Bundle params = unityParams.getStringParams();
      AppEventsLogger.updateUserProperties(params, null);
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
