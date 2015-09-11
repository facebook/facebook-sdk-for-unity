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

namespace Facebook.Unity.Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class DialogShare : MenuBase
    {
        // Custom Share Link
        private string shareLink = "https://developers.facebook.com/";
        private string shareTitle = "Link Title";
        private string shareDescription = "Link Description";
        private string shareImage = "http://i.imgur.com/j4M7vCO.jpg";

        // Custom Feed Share
        private string feedTo = string.Empty;
        private string feedLink = "https://developers.facebook.com/";
        private string feedTitle = "Test Title";
        private string feedCaption = "Test Caption";
        private string feedDescription = "Test Description";
        private string feedImage = "http://i.imgur.com/zkYlB.jpg";
        private string feedMediaSource = string.Empty;

        protected override bool ShowDialogModeSelector()
        {
            #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            return true;
            #else
            return false;
            #endif
        }

        protected override void GetGui()
        {
            if (this.Button("Share - Link"))
            {
                FB.ShareLink(new Uri("https://developers.facebook.com/"), callback: this.HandleResult);
            }

            // Note: Web dialog doesn't support photo urls.
            if (this.Button("Share - Link Photo"))
            {
                FB.ShareLink(
                    new Uri("https://developers.facebook.com/"),
                    "Link Share",
                    "Look I'm sharing a link",
                    new Uri("http://i.imgur.com/j4M7vCO.jpg"),
                    callback: this.HandleResult);
            }

            this.LabelAndTextField("Link", ref this.shareLink);
            this.LabelAndTextField("Title", ref this.shareTitle);
            this.LabelAndTextField("Description", ref this.shareDescription);
            this.LabelAndTextField("Image", ref this.shareImage);
            if (this.Button("Share - Custom"))
            {
                FB.ShareLink(
                    new Uri(this.shareLink),
                    this.shareTitle,
                    this.shareDescription,
                    new Uri(this.shareImage),
                    this.HandleResult);
            }

            if (this.Button("Feed Share - No To"))
            {
                FB.FeedShare(
                    string.Empty,
                    new Uri("https://developers.facebook.com/"),
                    "Test Title",
                    "Test caption",
                    "Test Description",
                    new Uri("http://i.imgur.com/zkYlB.jpg"),
                    string.Empty,
                    this.HandleResult);
            }

            this.LabelAndTextField("To", ref this.feedTo);
            this.LabelAndTextField("Link", ref this.feedLink);
            this.LabelAndTextField("Title", ref this.feedTitle);
            this.LabelAndTextField("Caption", ref this.feedCaption);
            this.LabelAndTextField("Description", ref this.feedDescription);
            this.LabelAndTextField("Image", ref this.feedImage);
            this.LabelAndTextField("Media Source", ref this.feedMediaSource);
            if (this.Button("Feed Share - Custom"))
            {
                FB.FeedShare(
                    this.feedTo,
                    string.IsNullOrEmpty(this.feedLink) ? null : new Uri(this.feedLink),
                    this.feedTitle,
                    this.feedCaption,
                    this.feedDescription,
                    string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage),
                    this.feedMediaSource,
                    this.HandleResult);
            }
        }
    }
}
