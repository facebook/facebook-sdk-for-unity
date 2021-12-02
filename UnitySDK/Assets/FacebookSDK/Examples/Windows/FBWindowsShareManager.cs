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
    public Toggle ShouldShowDialog;
    public Button ImageUploadButton;
    public Button VideoUploadButton;

    public void Button_UploadImage()
    {
        ImageUploadButton.interactable = false;
        Logger.DebugLog("Uploading image file [" + Path.GetFullPath(ImageFile.text) + "] to media library");
        FB.UploadImageToMediaLibrary(Caption.text, new Uri(Path.GetFullPath(ImageFile.text)), ShouldShowDialog.isOn, CallbackUploadImage);
    }

    public void Button_UploadVideo()
    {
        VideoUploadButton.interactable = false;
        Logger.DebugLog("Uploading video file [" + Path.GetFullPath(VideoFile.text) + "] to media library");
        FB.UploadVideoToMediaLibrary(Caption.text, new Uri(Path.GetFullPath(VideoFile.text)), ShouldShowDialog.isOn, CallbackUploadVideo);
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
