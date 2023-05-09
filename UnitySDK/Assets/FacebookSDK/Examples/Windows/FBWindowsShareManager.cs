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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;
using System;
using System.IO;

public class FBWindowsShareManager : MonoBehaviour
{

    public FBWindowsLogsManager Logger;
    public InputField Caption;
    public InputField ImageFile;
    public InputField VideoFile;
    public InputField TravelID;
    public Toggle ShouldShowDialog;
    public Button ImageUploadButton;
    public Button VideoUploadButton;

    public void Button_UploadImage()
    {
        string imagePath = Path.GetFullPath(Application.streamingAssetsPath + "/" + ImageFile.text);
        ImageUploadButton.interactable = false;
        Logger.DebugLog("Uploading image file [" + imagePath + "] to media library");
        FB.UploadImageToMediaLibrary(Caption.text, new Uri(imagePath), ShouldShowDialog.isOn, TravelID.text, CallbackUploadImage);
    }

    public void Button_UploadVideo()
    {
        string videoPath = Path.GetFullPath(Application.streamingAssetsPath + "/" + VideoFile.text);
        VideoUploadButton.interactable = false;
        Logger.DebugLog("Uploading video file [" + videoPath + "] to media library");
        FB.UploadVideoToMediaLibrary(Caption.text, new Uri(videoPath), ShouldShowDialog.isOn, TravelID.text, CallbackUploadVideo);
    }

    private void CallbackUploadImage(IMediaUploadResult result)
    {
        ImageUploadButton.interactable = true;
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Image upload error: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Image uploaded. ID: " + result.MediaId);
        }
    }

    private void CallbackUploadVideo(IMediaUploadResult result)
    {
        VideoUploadButton.interactable = true;
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Video upload error: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Video uploaded. ID: " + result.MediaId);
        }
    }
}
