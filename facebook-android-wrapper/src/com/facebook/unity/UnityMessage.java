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

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;
import android.util.Log;

public class UnityMessage {
    private String methodName;
    private Map<String, Serializable> params = new HashMap<String, Serializable>();

    public UnityMessage(String methodName) {
        this.methodName = methodName;
    }

    public UnityMessage put(String name, Serializable value) {
        params.put(name, value);
        return this;
    }

    public UnityMessage putCancelled() {
        put("cancelled", true);
        return this;
    }

    public UnityMessage putID(String id) {
        put("id", id);
        return this;
    }

    public void sendError(String errorMsg) {
        this.put("error", errorMsg);
        send();
    }

    public void send() {
        assert methodName != null : "no method specified";
        String message = new UnityParams(this.params).toString();
        Log.v(FB.TAG, "sending to Unity " + this.methodName + "(" + message + ")");
        try {
            UnityReflection.SendMessage(FB.FB_UNITY_OBJECT, this.methodName, message);
        } catch (UnsatisfiedLinkError e) {
            Log.v(FB.TAG, "message not send, Unity not initialized");
        }
    }

    public static UnityMessage createWithCallbackFromParams(
            String methodName,
            UnityParams params) {
        UnityMessage unityMessage = new UnityMessage(methodName);
        if (params.hasString("callback_id")) {
            unityMessage.put("callback_id", params.getString("callback_id"));
        }

        return unityMessage;
    }
}
