using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class AdsPage : MonoBehaviour {
  private LogScroller _logScroller;

    // Replace the below INTERSTITIAL_ID and VIDEO_PLACEMENT_ID with your app placement IDs
    private string INTERSTITIAL_PLACEMENT_ID = "123456";
    private string VIDEO_PLACEMENT_ID = "123456";

    private void Awake () {
    _logScroller = transform.root.GetComponent<UIState>().logScroller;
  }

  public void OnLoadInterstitialBtnClick () {
    _LogText("Loading Interstitial Ad");
    FBGamingServices.LoadInterstitialAd(INTERSTITIAL_PLACEMENT_ID, delegate(IInterstitialAdResult result) {
      if (result.Error == null && result.ResultDictionary != null) {
        _LogText("Interstitial Ad Loaded");
      } else {
        _LogText("ERR: Failed to load Interstitial Ad\n" + result.Error.ToString());
      }
    });
  }
  public void OnLoadVideoBtnClick () {
    _LogText("Loading Video Ad");
    FBGamingServices.LoadRewardedVideo(VIDEO_PLACEMENT_ID, delegate(IRewardedVideoResult result) {
      if (result.Error == null && result.ResultDictionary != null) {
        _LogText("Video Ad Loaded");
      } else {
        _LogText("ERR: Failed to load Video Ad\n" + result.Error.ToString());
      }
    });
  }
  public void OnViewInterstitialBtnClick () {
    _LogText("View Interstitial Ad");
    FBGamingServices.ShowInterstitialAd(INTERSTITIAL_PLACEMENT_ID, delegate(IInterstitialAdResult result) {
      if (result.Error == null && result.ResultDictionary != null) {
        _LogText("Interstitial Ad Viewed");
      } else {
        _LogText("ERR: Failed to view Interstitial Ad\n" + result.Error.ToString());
      }
    });
  }
  public void OnViewVideoBtnClick () {
    _LogText("View Video Ad");
    FBGamingServices.ShowRewardedVideo(VIDEO_PLACEMENT_ID, delegate(IRewardedVideoResult result) {
      if (result.Error == null && result.ResultDictionary != null) {
        _LogText("Video Ad Viewed");
      } else {
        _LogText("ERR: Failed to watch Video Ad\n" + result.Error.ToString());
      }
    });
  }

  // private
  private void _LogText (string text) {
    _logScroller.Log(text);
  }
}
