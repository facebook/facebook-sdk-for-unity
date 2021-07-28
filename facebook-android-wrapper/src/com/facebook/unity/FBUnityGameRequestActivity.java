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
import android.text.TextUtils;

import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.share.model.GameRequestContent;
import com.facebook.gamingservices.GameRequestDialog;

import java.util.Arrays;
import java.util.Locale;

public class FBUnityGameRequestActivity extends BaseActivity {
    public static final String GAME_REQUEST_PARAMS = "game_request_params";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (savedInstanceState == null) {
          showDialog();
        }
    }

    private void showDialog() {
      final Bundle params = getIntent().getBundleExtra(GAME_REQUEST_PARAMS);
      final UnityMessage response = new UnityMessage("OnAppRequestsComplete");

      if (params.containsKey(Constants.CALLBACK_ID_KEY)) {
          response.put(Constants.CALLBACK_ID_KEY, params.getString(Constants.CALLBACK_ID_KEY));
      }

      GameRequestContent.Builder contentBuilder = new GameRequestContent.Builder();
      if (params.containsKey("message")) {
          contentBuilder.setMessage(params.getString("message"));
      }

      if (params.containsKey("action_type")) {
          String actionTypeStr = params.getString("action_type");
          try {
              GameRequestContent.ActionType type =
                      GameRequestContent.ActionType.valueOf(actionTypeStr);
              contentBuilder.setActionType(type);
          } catch (IllegalArgumentException exception) {
              response.sendError("Unknown action type: " + actionTypeStr);
              finish();
              return;
          }
      }

      if(params.containsKey("object_id")) {
          contentBuilder.setObjectId(params.getString("object_id"));

      }

      if (params.containsKey("to")) {
          String toStr = params.getString("to");
          contentBuilder.setRecipients(Arrays.asList(toStr.split(",")));
      }

      if (params.containsKey("filters")) {
          String filtersStr = params.getString("filters").toUpperCase(Locale.ROOT);
          try {
              GameRequestContent.Filters filters = GameRequestContent.Filters.valueOf(filtersStr);
              contentBuilder.setFilters(filters);
          } catch (IllegalArgumentException exception) {
              response.sendError("Unsupported filter type: " + filtersStr);
              finish();
              return;
          }
      }

      if (params.containsKey("data")) {
          contentBuilder.setData(params.getString("data"));
      }

      if (params.containsKey("title")) {
          contentBuilder.setTitle(params.getString("title"));
      }

      final GameRequestContent content = contentBuilder.build();


      GameRequestDialog requestDialog = new GameRequestDialog(this);
      requestDialog.registerCallback(
              mCallbackManager,
              new FacebookCallback<GameRequestDialog.Result>() {
                  @Override
                  public void onSuccess(GameRequestDialog.Result result) {
                      response.put("request", result.getRequestId());
                      response.put("to", TextUtils.join(",",result.getRequestRecipients()));
                      response.send();
                      FBUnityGameRequestActivity.this.finish();
                  }

                  @Override
                  public void onCancel() {
                      response.putCancelled();
                      response.send();
                      FBUnityGameRequestActivity.this.finish();
                  }

                  @Override
                  public void onError(FacebookException e) {
                      response.sendError(e.getMessage());
                      FBUnityGameRequestActivity.this.finish();
                  }
              });

      try {
          requestDialog.show(content);
      } catch (IllegalArgumentException exception) {
          response.sendError("Unexpected exception encountered: " + exception.toString());
          finish();
      }
    }
}
