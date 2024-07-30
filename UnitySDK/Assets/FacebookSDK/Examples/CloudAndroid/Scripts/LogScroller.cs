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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScroller : MonoBehaviour
{
    [SerializeField]
    private GameObject _content;

    [HideInInspector]
    public List<String> texts = new List<String>();

    private string DATE_FORMAT = @"M/d/yyyy hh:mm:ss tt";
    private int index = 0;

    void Start()
    {
    }

    public void ClearLogs()
    {
        texts.Clear();
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void Log(string text)
    {
        GameObject nText = new GameObject("text-" + index++);
        nText.transform.parent = _content.transform;
        nText.transform.localPosition = new Vector3(0, 0, 0);
        nText.transform.localScale = new Vector3(1, 1, 1);
        nText.transform.SetAsFirstSibling();

        string formattedText = string.Format("{0}\n{1}\n", DateTime.Now.ToString(DATE_FORMAT), text);
        texts.Insert(0, formattedText);
        Text textComp = nText.AddComponent<Text>();
        textComp.text = formattedText;
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComp.color = new Color(1, 1, 1, 1);
    }
}
