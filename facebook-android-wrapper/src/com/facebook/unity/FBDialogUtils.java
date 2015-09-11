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

import com.facebook.share.internal.ShareFeedContent;
import com.facebook.share.model.ShareLinkContent;
import com.facebook.share.widget.ShareDialog;

import android.net.Uri;
import android.os.Bundle;

class FBDialogUtils {
    public static ShareLinkContent.Builder createShareContentBuilder(Bundle params) {
        ShareLinkContent.Builder  builder = new ShareLinkContent.Builder();

        if (params.containsKey("content_title")) {
            builder.setContentTitle(params.getString("content_title"));
        }

        if (params.containsKey("content_description")) {
            builder.setContentDescription(params.getString("content_description"));
        }

        if (params.containsKey("content_url")) {
            builder.setContentUrl(Uri.parse(params.getString("content_url")));
        }

        if (params.containsKey("photo_url")) {
            builder.setImageUrl(Uri.parse(params.getString("photo_url")));
        }

        return builder;
    }

    public static ShareFeedContent.Builder createFeedContentBuilder(Bundle params) {
        ShareFeedContent.Builder builder = new ShareFeedContent.Builder();

        if (params.containsKey("toId")) {
            builder.setToId(params.getString("toId"));
        }

        if (params.containsKey("link")) {
            builder.setLink(params.getString("link"));
        }

        if (params.containsKey("linkName")) {
            builder.setLinkName(params.getString("linkName"));
        }

        if (params.containsKey("linkCaption")) {
            builder.setLinkCaption(params.getString("linkCaption"));
        }

        if (params.containsKey("linkDescription")) {
            builder.setLinkDescription(params.getString("linkDescription"));
        }

        if (params.containsKey("picture")) {
            builder.setPicture(params.getString("picture"));
        }

        if (params.containsKey("mediaSource")) {
            builder.setMediaSource(params.getString("mediaSource"));
        }

        return builder;
    }

    public static ShareDialog.Mode intToMode(int mode) {
        switch (mode) {
            case 0:
                return ShareDialog.Mode.AUTOMATIC;
            case 1:
                return ShareDialog.Mode.NATIVE;
            case 2:
                return ShareDialog.Mode.WEB;
            case 3:
                return ShareDialog.Mode.FEED;
            default:
                return null;
        }
    }
}
