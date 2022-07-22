using Facebook.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FBWindowsSoftKeyboardManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;

    public void SetSoftKeyboardOpenButton()
    {
        Logger.DebugLog("Setting Soft Keyboard Open");
        FB.Windows.SetSoftKeyboardOpen(true, CallbackSetSoftKeyboardOpen);
    }

    private void CallbackSetSoftKeyboardOpen(ISoftKeyboardOpenResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Error setting Soft Keyboard Open: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Soft Keyboard Open set: " + result.Success);
        }
    }
}
