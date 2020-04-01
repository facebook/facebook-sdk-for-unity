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

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import android.text.TextUtils;
import android.util.Log;

import com.facebook.AccessToken;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.FacebookSdk;
import com.facebook.login.DeviceLoginManager;
import com.facebook.login.LoginBehavior;
import com.facebook.login.LoginManager;
import com.facebook.login.LoginResult;

public class FBLogin {
    public static void loginWithReadPermissions(
            String params,
            final FBUnityLoginActivity activity) {
        login(params, activity, false, false);
    }

    public static void loginWithPublishPermissions(
            String params,
            final FBUnityLoginActivity activity) {
        login(params, activity, true, false);
    }

    public static void loginForTVWithReadPermissions(
            String params,
            final FBUnityLoginActivity activity) {
        login(params, activity, false, true);
    }

    public static void loginForTVWithPublishPermissions(
            String params,
            final FBUnityLoginActivity activity) {
        login(params, activity, true, true);
    }

    public static void sendLoginSuccessMessage(AccessToken token, String callbackID) {
        UnityMessage unityMessage = new UnityMessage("OnLoginComplete");
        FBLogin.addLoginParametersToMessage(unityMessage, token, callbackID);
        unityMessage.send();
    }

    public static void addLoginParametersToMessage(
            UnityMessage unityMessage,
            AccessToken token,
            String callbackID) {
        unityMessage.put("key_hash", FB.getKeyHash());
        unityMessage.put("opened", true);
        unityMessage.put("access_token", token.getToken());
        Long expiration = token.getExpires().getTime() / 1000;
        unityMessage.put("expiration_timestamp", expiration.toString());
        unityMessage.put("user_id", token.getUserId());
        unityMessage.put("permissions",
                TextUtils.join(",", token.getPermissions()));
        unityMessage.put("declined_permissions",
                TextUtils.join(",", token.getDeclinedPermissions()));
        unityMessage.put("graph_domain", token.getGraphDomain() != null ? token.getGraphDomain() : "facebook");

        if (token.getLastRefresh() != null) {
            Long lastRefresh = token.getLastRefresh().getTime() / 1000;
            unityMessage.put("last_refresh", lastRefresh.toString());
        }

        if (callbackID != null && !callbackID.isEmpty()) {
            unityMessage.put(Constants.CALLBACK_ID_KEY, callbackID);
        }
    }

    private static void login(
            String params,
            final FBUnityLoginActivity activity,
            boolean isPublishPermLogin,
            boolean isDeviceAuthLogin) {
        if (!FacebookSdk.isInitialized()) {
            Log.w(FB.TAG, "Facebook SDK not initialized. Call init() before calling login()");
            return;
        }

        final UnityMessage unityMessage = new UnityMessage("OnLoginComplete");
        unityMessage.put("key_hash", FB.getKeyHash());
        UnityParams unity_params = UnityParams.parse(params,
                "couldn't parse login params: " + params);

        List<String> permissions = null;
        if (unity_params.hasString("scope")) {
            permissions = new ArrayList<>(
                    Arrays.asList(unity_params.getString("scope").split(",")));
        }

        String callbackIDString = null;
        if (unity_params.has(Constants.CALLBACK_ID_KEY)) {
            callbackIDString = unity_params.getString(Constants.CALLBACK_ID_KEY);
            unityMessage.put(Constants.CALLBACK_ID_KEY, callbackIDString);
        }

        final String callbackID = callbackIDString;

        // For now only web login
        LoginManager.getInstance().registerCallback(
                activity.getCallbackManager(),
                new FacebookCallback<LoginResult>() {
                    @Override
                    public void onSuccess(LoginResult loginResult) {
                        sendLoginSuccessMessage(loginResult.getAccessToken(), callbackID);
                    }

                    @Override
                    public void onCancel() {
                        unityMessage.putCancelled();
                        unityMessage.send();
                    }

                    @Override
                    public void onError(FacebookException e) {
                        unityMessage.sendError(e.getMessage());
                    }
                });

        LoginManager loginManager;
        if (isDeviceAuthLogin) {
            loginManager = DeviceLoginManager.getInstance();
        } else {
            loginManager = LoginManager.getInstance();
        }

        if (isPublishPermLogin) {
            loginManager.logInWithPublishPermissions(activity, permissions);
        } else {
            loginManager.logInWithReadPermissions(activity, permissions);
        }
    }
}
