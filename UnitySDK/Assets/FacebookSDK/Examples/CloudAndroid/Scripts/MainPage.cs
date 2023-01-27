using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class MainPage : MonoBehaviour {
  [SerializeField]
  private Menu _menu;

  private LogScroller _logScroller;

  private void Awake () {
    _logScroller = transform.root.GetComponent<UIState>().logScroller;
    _logScroller.Log(Application.identifier);

    if (!FB.IsInitialized) {
      _LogText("FB.Init called");
      FB.Init(_OnInitComplete);
    }
  }

  public void OnLogAcessTokenBtnClick () {
    AccessToken currentToken = AccessToken.CurrentAccessToken;
    if (currentToken == null) {
      _LogText("CurrentAccessToken is empty");
    } else {
      _LogText("CurrentAccessToken: " + currentToken.ToString());
    }
  }
  public void OnLogPayloadBtnClick () {
    _LogText("Getting Payload");
    FBGamingServices.GetPayload(delegate (IPayloadResult result) {
      _LogText("payload\n" + JsonUtility.ToJson(result.Payload));
    });
  }
  public void OnNavBtnClick (string pageName) {
    _menu.NavToPage(pageName);
  }

  // private
  private void _LogText (string text) {
    _logScroller.Log(text);
  }
  private void _OnInitComplete () {
    if (FB.IsInitialized) {
      _LogText("FB.Init Successful; FBGamingServices.InitCloudGame called");
      FBGamingServices.InitCloudGame(_OnInitCloud);
    } else {
      _LogText("ERR: FB.Init failed");
    }
  }
  private void _OnInitCloud (IInitCloudGameResult result) {
    if (result.Error == null && result.ResultDictionary != null) {
      _LogText("InitCloudGame Successful");
    } else {
      _LogText("ERR: InitCloudGame failed\n" + result.Error.ToString());
    }
  }
}
