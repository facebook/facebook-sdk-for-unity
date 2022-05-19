using Facebook.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FBWindowsVirtualGamepadManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public InputField NewVirtualGamepadLayout;

    public void SetVirtualGamepadLayout()
    {
        Logger.DebugLog("Setting Virtual Gamepad Layout to " + NewVirtualGamepadLayout.text);
        FB.Windows.SetVirtualGamepadLayout(NewVirtualGamepadLayout.text, CallbackSetVirtualGamepadLayout);
    }

    private void CallbackSetVirtualGamepadLayout(IVirtualGamepadLayoutResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Error setting Virtual Gamepad Layout: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Virtual Gamepad Layout set: " + result.Success);
        }
    }
}
