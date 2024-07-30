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

public class FBWindowsA2UNotificationsManager : MonoBehaviour
{

    public FBWindowsLogsManager Logger;
    public InputField TitleText;
    public InputField BodyText;
    public InputField MediaText;
    public InputField PayloadText;
    public InputField TimeIntervalText;

    public void ScheduleButton()
    {
        Logger.DebugLog("Scheduling notification ...");
        FB.ScheduleAppToUserNotification(TitleText.text, BodyText.text, new System.Uri(MediaText.text), int.Parse(TimeIntervalText.text), PayloadText.text, A2UNotificationCallback);
    }

    private void A2UNotificationCallback(IScheduleAppToUserNotificationResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Notification scheduled correctly.");
        }
    }


}
