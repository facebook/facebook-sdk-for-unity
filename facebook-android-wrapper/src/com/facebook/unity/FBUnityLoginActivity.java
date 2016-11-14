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

import com.facebook.CallbackManager;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class FBUnityLoginActivity extends BaseActivity {
    public static final String LOGIN_PARAMS = "login_params";
    public static final String LOGIN_TYPE = "login_type";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        LoginType type = (LoginType) getIntent().getSerializableExtra(LOGIN_TYPE);
        String loginParams = getIntent().getStringExtra(LOGIN_PARAMS);
        switch (type) {
            case READ:
                FBLogin.loginWithReadPermissions(loginParams, this);
                break;
            case PUBLISH:
                FBLogin.loginWithPublishPermissions(loginParams, this);
                break;
            case TV_READ:
                FBLogin.loginForTVWithReadPermissions(loginParams, this);
                break;
            case TV_PUBLISH:
                FBLogin.loginForTVWithPublishPermissions(loginParams, this);
                break;
        }
    }

    public CallbackManager getCallbackManager() {
        return mCallbackManager;
    }

    public enum LoginType {
        READ,
        PUBLISH,
        TV_READ,
        TV_PUBLISH
    }

}
