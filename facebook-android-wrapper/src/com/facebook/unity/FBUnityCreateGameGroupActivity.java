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
import android.content.Intent;
import android.os.Bundle;

import com.facebook.CallbackManager;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.share.model.AppGroupCreationContent;
import com.facebook.share.model.AppGroupCreationContent.AppGroupPrivacy;
import com.facebook.share.widget.CreateAppGroupDialog;

import java.util.Locale;

public class FBUnityCreateGameGroupActivity extends BaseActivity {
    public static String CREATE_GAME_GROUP_PARAMS = "create_game_group_params";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        AppGroupCreationContent.Builder contentBuilder = new AppGroupCreationContent.Builder();
        Bundle params = getIntent().getBundleExtra(CREATE_GAME_GROUP_PARAMS);
        final UnityMessage response = new UnityMessage("OnGroupCreateComplete");
        if (params.containsKey(Constants.CALLBACK_ID_KEY)) {
            response.put(Constants.CALLBACK_ID_KEY, params.getString(Constants.CALLBACK_ID_KEY));
        }

        if (params.containsKey("name")) {
            contentBuilder.setName(params.getString("name"));
        }

        if (params.containsKey("description")) {
            contentBuilder.setDescription(params.getString("name"));
        }

        if (params.containsKey("privacy")) {
            String privacyStr = params.getString("privacy");
            AppGroupPrivacy privacy = AppGroupPrivacy.Closed;
            if (privacyStr.equalsIgnoreCase("closed")) {
                privacy = AppGroupPrivacy.Closed;
            } else if (privacyStr.equalsIgnoreCase("open")) {
                privacy = AppGroupPrivacy.Open;
            } else {
                response.sendError(
                        String.format(
                                Locale.ROOT,
                                "Unknown privacy setting for group creation: %s",
                                privacyStr));
                finish();
            }
            contentBuilder.setAppGroupPrivacy(privacy);
        }

        CreateAppGroupDialog dialog = new CreateAppGroupDialog(this);
        dialog.registerCallback(mCallbackManager, new FacebookCallback<CreateAppGroupDialog.Result>() {
            @Override
            public void onSuccess(CreateAppGroupDialog.Result result) {
                response.put("id", result.getId());
                response.send();
            }

            @Override
            public void onCancel() {
                response.putCancelled();
                response.send();
            }

            @Override
            public void onError(FacebookException e) {
                response.sendError(e.getLocalizedMessage());
            }
        });
        dialog.show(contentBuilder.build());
    }
}
