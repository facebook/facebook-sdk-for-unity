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

import java.util.Arrays;
import java.util.List;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.gamingservices.ContextChooseDialog;
import com.facebook.gamingservices.model.ContextChooseContent;

import org.json.JSONArray;
import org.json.JSONObject;

public class FBUnityChooseGamingContextActivity extends BaseActivity {
    private static String TAG = FBUnityChooseGamingContextActivity.class.getName();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Intent intent = getIntent();
        Bundle params = intent.getBundleExtra(BaseActivity.ACTIVITY_PARAMS);

        final UnityMessage response = new UnityMessage("OnChooseGamingContextComplete");
        String callbackID = params.getString(Constants.CALLBACK_ID_KEY);
        Log.e(TAG, "callbackID: " + callbackID);
        if (callbackID != null) {
            response.put(Constants.CALLBACK_ID_KEY, callbackID);
        }

        ContextChooseContent.Builder builder = new ContextChooseContent.Builder();
        // TODO add filter, maxSize and minSize
        Log.v(TAG, "ChooseGamingContext(" + params + ")");

        List<String> filters;
        Integer maxSize = null;
        Integer minSize = null;
        try {
            filters = params.getStringArrayList("filters");
            Log.v(TAG, "ChooseGamingContext filters: " + filters + "");

            String minSizeString = params.getString("minSize");
            if (!minSizeString.isEmpty()) {
                minSize = Integer.parseInt(minSizeString);
            }

            String maxSizeString = params.getString("maxSize");
            if (!maxSizeString.isEmpty()) {
                maxSize = Integer.parseInt(maxSizeString);
            }
        } catch (Exception e) {
            response.sendError(String.format("Invalid params: %s", e.getMessage()));
            return;
        }

        ContextChooseContent content = builder.build();
        if (filters != null) {
            builder.setFilters(filters);
        }
        if (minSize != null) {
            builder.setMinSize(minSize);
        }
        if (maxSize != null) {
            builder.setMaxSize(maxSize);
        }

        ContextChooseDialog dialog = new ContextChooseDialog(this);
        dialog.registerCallback(mCallbackManager, new FacebookCallback<ContextChooseDialog.Result>() {
            @Override
            public void onSuccess(ContextChooseDialog.Result result) {
                response.put("contextId", result.getContextID());
                response.send();
                FBUnityChooseGamingContextActivity.this.finish();
            }

            @Override
            public void onCancel() {
                response.putCancelled();
                response.send();
                FBUnityChooseGamingContextActivity.this.finish();
            }

            @Override
            public void onError(FacebookException e) {
                response.sendError(e.getMessage());
                FBUnityChooseGamingContextActivity.this.finish();
            }
        });
        dialog.show(content);
    }

}
