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
    using System.IO;
    using UnityEngine;

    internal class UploadToMediaLibrary : MenuBase
    {
        private bool imageShouldLaunchMediaDialog = true;
        private string imageCaption = "Image Caption";
        private string imageFile = "meta-logo.png";

        private bool videoShouldLaunchMediaDialog = true;
        private string videoCaption = "Video Caption";
        private string videoFile = "meta.mp4";

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && FB.IsLoggedIn;

            GUILayout.Space(24);

            this.LabelAndTextField("Image caption:", ref this.imageCaption);
            if (this.Button("Image Dialog: " + imageShouldLaunchMediaDialog.ToString()))
            {
                imageShouldLaunchMediaDialog = !imageShouldLaunchMediaDialog;
            }
            GUILayout.Space(24);

            string imagePath = Path.GetFullPath(Application.streamingAssetsPath + "/" + imageFile);
            if (File.Exists(imagePath))
            {
                if (this.Button("Upload Image to media library"))
                {

                    FB.Mobile.UploadImageToMediaLibrary(imageCaption, new Uri(imagePath), imageShouldLaunchMediaDialog, this.HandleResult);
                }
            }
            else
            {
                GUILayout.Label("Image not exist: " + imagePath);
            }
            GUILayout.Space(24);

            this.LabelAndTextField("Video caption:", ref this.videoCaption);
            if (this.Button("Video Dialog: " + videoShouldLaunchMediaDialog.ToString()))
            {
                videoShouldLaunchMediaDialog = !videoShouldLaunchMediaDialog;
            }
            GUILayout.Space(24);

            string videoPath = Path.GetFullPath(Application.streamingAssetsPath + "/" + videoFile);
            if (File.Exists(videoPath))
            {
                if (this.Button("Upload Video to media library"))
                {
                    FB.Mobile.UploadVideoToMediaLibrary(videoCaption, new Uri(videoPath), videoShouldLaunchMediaDialog, this.HandleResult);
                }
            }
            else
            {
                GUILayout.Label("Video not exist: " + videoPath);
            }
        }
    }
}
