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

import android.app.Activity;
import android.util.Log;

import java.lang.reflect.Method;

/**
 * This class is a hack. Currently we can't have both the interative console and this
 * aar include the unity_classes.jar. Unity will fail to build due to the duplicate jar to
 * get around this we use reflection instead.
 */
public class UnityReflection {
    private static Class<?> unityPlayer;

    public static void SendMessage(String unityObject, String unityMethod, String message) {
        try {
            if (unityPlayer == null) {
                unityPlayer = Class.forName("com.unity3d.player.UnityPlayer");
            }

            Method method = unityPlayer.getMethod(
                    "UnitySendMessage", String.class, String.class, String.class);
            method.invoke(unityPlayer, unityObject, unityMethod, message);
        } catch (Exception ex) {
            Log.d("TODO", ex.toString());
        }
    }

    public static Activity GetUnityActivity() {
        try {
            if (unityPlayer == null) {
                unityPlayer = Class.forName("com.unity3d.player.UnityPlayer");
            }

            Activity activity = (Activity) unityPlayer.getField("currentActivity").get(null);
            if (activity == null) {
                Log.d(FB.TAG, "Current unity activity is null");
            }
            return activity;
        } catch (Exception ex) {
            Log.d(FB.TAG, ex.toString());
            return null;
        }
    }
}
