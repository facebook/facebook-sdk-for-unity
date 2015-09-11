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

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.share.Sharer;
import com.facebook.share.model.ShareContent;
import com.facebook.share.widget.ShareDialog;

import java.util.Locale;

public class FBUnityDialogsActivity extends BaseActivity {
    private static String TAG = FBUnityDialogsActivity.class.getName();
    public static final String DIALOG_TYPE = "dialog_type";
    public static final String SHARE_DIALOG_PARAMS = "share_dialog_params";
    public static final String FEED_DIALOG_PARAMS = "feed_dialog_params";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Intent intent = getIntent();
        ShareContent shareContent;
        Bundle params;
        if (intent.hasExtra(SHARE_DIALOG_PARAMS)) {
            params = intent.getBundleExtra(SHARE_DIALOG_PARAMS);
            shareContent = FBDialogUtils.createShareContentBuilder(params).build();
        } else if (intent.hasExtra(FEED_DIALOG_PARAMS)) {
            params = intent.getBundleExtra(FEED_DIALOG_PARAMS);
            shareContent = FBDialogUtils.createFeedContentBuilder(params).build();
        } else {
            Log.e(TAG,
                    String.format(
                            Locale.ROOT,
                            "Failed to find extra %s or %s",
                            SHARE_DIALOG_PARAMS,
                            FEED_DIALOG_PARAMS));
            finish();
            return;
        }

        ShareDialog dialog = new ShareDialog(this);
        final UnityMessage response = new UnityMessage("OnShareLinkComplete");
        String callbackID = params.getString(Constants.CALLBACK_ID_KEY);
        if (callbackID != null) {
            response.put(Constants.CALLBACK_ID_KEY, callbackID);
        }

        dialog.registerCallback(mCallbackManager, new FacebookCallback<Sharer.Result>() {
            @Override
            public void onSuccess(Sharer.Result result) {
                if (result.getPostId() != null) {
                    response.putID(result.getPostId());
                }
                // Unity SDK requires to have at least one key beside callback_id.
                response.put("posted", true);
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
        ShareDialog.Mode mode = (ShareDialog.Mode) getIntent().getSerializableExtra(DIALOG_TYPE);
        dialog.show(shareContent, mode);
    }
}
