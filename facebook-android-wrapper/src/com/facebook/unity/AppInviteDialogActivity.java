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

import android.os.Bundle;

import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.share.model.AppInviteContent;
import com.facebook.share.widget.AppInviteDialog;

public class AppInviteDialogActivity extends BaseActivity {
    public static final String DIALOG_PARAMS = "dialog_params";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        final UnityMessage response = new UnityMessage("OnAppInviteComplete");
        Bundle params = getIntent().getBundleExtra(DIALOG_PARAMS);

        AppInviteContent.Builder contentBuilder = new AppInviteContent.Builder();
        if (params.containsKey(Constants.CALLBACK_ID_KEY)) {
            response.put(Constants.CALLBACK_ID_KEY, params.getString(Constants.CALLBACK_ID_KEY));
        }

        if (params.containsKey("appLinkUrl")) {
            contentBuilder.setApplinkUrl(params.getString("appLinkUrl"));
        }

        if (params.containsKey("previewImageUrl")) {
            contentBuilder.setPreviewImageUrl(params.getString("previewImageUrl"));
        }

        AppInviteDialog dialog = new AppInviteDialog(this);
        dialog.registerCallback(this.mCallbackManager, new FacebookCallback<AppInviteDialog.Result>() {
            @Override
            public void onSuccess(AppInviteDialog.Result result) {
                response.put("didComplete", true);
                response.send();
            }

            @Override
            public void onCancel() {
                response.putCancelled();
                response.send();
            }

            @Override
            public void onError(FacebookException e) {
                response.sendError(e.getMessage());
            }
        });
        dialog.show(contentBuilder.build());
    }
}
