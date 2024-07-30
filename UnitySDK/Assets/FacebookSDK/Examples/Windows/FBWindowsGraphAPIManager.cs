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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBWindowsGraphAPIManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public InputField QueryText;
    public Dropdown QueryType;
    public InputField GraphAPIVersionText;
    public Text GraphAPIVersion;
    private IDictionary<string, string> formData = null;

    void Start()
    {
        GraphAPIVersionText.text = Constants.GraphApiVersion;
    }

    void OnEnable()
    {
        GraphAPIVersion.text = " Current Graph API version: " + FB.GraphApiVersion + "\n SDK Graph API version: " + Constants.GraphApiVersion;
    }

    public void GraphAPI()
    {
        HttpMethod typeQuery = (HttpMethod)Enum.Parse(typeof(HttpMethod), QueryType.options[QueryType.value].text);
        FB.API(QueryText.text, typeQuery, (result) =>
        {
            Logger.DebugLog(result.RawResult);
        }, formData);
    }

    public void SetGraphAPiVersion()
    {
        if (GraphAPIVersionText.text != "")
        {
            FB.GraphApiVersion = GraphAPIVersionText.text;
        }
        OnEnable();
    }
}
