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
