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
