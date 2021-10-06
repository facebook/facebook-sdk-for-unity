using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBWindowsA2UNotificationsManager : MonoBehaviour {

	public FBWindowsLogsManager Logger;
	public InputField TitleText;
	public InputField BodyText;
	public InputField MediaText;
	public InputField PayloadText;
	public InputField TimeIntervalText;

	public void ScheduleButton () {
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
