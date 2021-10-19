using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FBWindowsLogsManager : MonoBehaviour
{
    public Text LogText;

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
    }

    public void DebugErrorLog(string message)
    {
        LogText.text += "<color=red>" + message + "</color> \n";
    }

    public void DebugWarningLog(string message)
    {
        LogText.text += "<color=yellow>" + message + "</color> \n";
    }

    public void DebugClean()
    {
        LogText.text = "";
    }
}
