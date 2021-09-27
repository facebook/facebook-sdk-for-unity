using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class FBWindowsADSManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;

    public InputField InputInterstitialAd;
    public InputField InputRewardedVideo;

    public void LoadRewardedVideo(string placementID)
    {
        Logger.DebugLog("Loading Rewarded Video");
        FB.LoadRewardedVideo(placementID, delegate (IRewardedVideoResult rewardedVideoResult)
        {
            if (rewardedVideoResult.Error != null)
            {
                Logger.DebugErrorLog(rewardedVideoResult.Error);
            }
            else
            {
                Logger.DebugLog("Rewarded Video "+ placementID + " loaded correctly!");
            }           
        });
    }

    public void ShowRewardedVideo(string placementID)
    {
        Logger.DebugLog("Showing Rewarded Video");
        FB.ShowRewardedVideo(placementID, delegate (IRewardedVideoResult rewardedVideoResult)
        {
            if (rewardedVideoResult.Error != null)
            {
                Logger.DebugErrorLog(rewardedVideoResult.Error);
            }
            else
            {
                Logger.DebugLog("Rewarded Video " + placementID + " showed correctly!");
                Logger.DebugLog("Here you can give a reward.");
            }
        });
    }


    public void LoadInterstitialAd(string placementID)
    {
        Logger.DebugLog("Loading Interstitial AD");
        FB.LoadInterstitialAd(placementID, delegate (IInterstitialAdResult interstitialAdResult)
        {
            if (interstitialAdResult.Error != null)
            {
                Logger.DebugErrorLog(interstitialAdResult.Error);
            }
            else
            {
                Logger.DebugLog("Interstitial AD " + placementID + " loaded correctly!");
            }
        });
    }

    public void ShowInterstitialAd(string placementID)
    {
        Logger.DebugLog("Showing Interstitial AD");
        FB.ShowInterstitialAd(placementID, delegate (IInterstitialAdResult interstitialAdResult)
        {
            if (interstitialAdResult.Error != null)
            {
                Logger.DebugErrorLog(interstitialAdResult.Error);
            }
            else
            {
                Logger.DebugLog("Interstitial AD " + placementID + " showed correctly!");
            }
        });
    }


    public void OnButtonLoadInterstitialAd()
    {
        if (!String.IsNullOrEmpty(InputInterstitialAd.text)) {
            LoadInterstitialAd(InputInterstitialAd.text);
        }
        else
        {
            Logger.DebugWarningLog("Please, insert interstitial placement ID code.");
        }
    }

    public void OnButtonShowInterstitialAd()
    {
        if (!String.IsNullOrEmpty(InputInterstitialAd.text))
        {
            ShowInterstitialAd(InputInterstitialAd.text);
        }
        else
        {
            Logger.DebugWarningLog("Please, insert interstitial placement ID code.");
        }
    }

    public void OnButtonLoadRewardedVideo()
    {
        if (!String.IsNullOrEmpty(InputRewardedVideo.text))
        {
            LoadRewardedVideo(InputRewardedVideo.text);
        }
        else
        {
            Logger.DebugWarningLog("Please, insert rewarded video placement ID code.");
        }
    }

    public void OnButtonShowRewardedVideo()
    {
        if (!String.IsNullOrEmpty(InputRewardedVideo.text))
        {
            ShowRewardedVideo(InputRewardedVideo.text);
        }
        else
        {
            Logger.DebugWarningLog("Please, insert rewarded video placement ID code.");
        }
    }
}
