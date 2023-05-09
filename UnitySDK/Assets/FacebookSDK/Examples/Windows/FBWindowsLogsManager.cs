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

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FBWindowsLogsManager : MonoBehaviour
{
    public Text LogText;
    public ScrollRect ScrollView;

    void Awake()
    {
#if !UNITY_EDITOR_WIN
			Debug.Log("This example is only for Windows devices.");
#else
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows && EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
        {
            Debug.Log("This example is only for Windows build target (x86 or x84).");
        }
#endif
    }

    public void DebugLog(string message)
    {
        LogText.text += "<color=white>" + message + "</color> \n";
        ScrollToTheBottom();
    }

    public void DebugErrorLog(string message)
    {
        LogText.text += "<color=red>" + message + "</color> \n";
        ScrollToTheBottom();
    }

    public void DebugWarningLog(string message)
    {
        LogText.text += "<color=yellow>" + message + "</color> \n";
        ScrollToTheBottom();
    }

    public void DebugClean()
    {
        LogText.text = "";
    }

    private void ScrollToTheBottom()
    {
        ScrollView.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }
}
