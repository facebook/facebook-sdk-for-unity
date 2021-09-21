using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class FBWindowsLoginManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public InputField Permissions;

    public void LogInReadButton()
    {
        if (FB.IsInitialized)
        {
            FB.LogInWithReadPermissions(Permissions.text.Split(','), AuthCallback);
        }
        else
        {
            Logger.DebugLog("Not Init");
        }
    }

    public void LogInPublishButton()
    {
        if (FB.IsInitialized)
        {
            FB.LogInWithPublishPermissions(Permissions.text.Split(','), AuthCallback);
        }
        else
        {
            Logger.DebugLog("Not Init");
        }
    }

    public void LogOutButton()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
            Logger.DebugLog("Logged out");
        }
        else
        {
            Logger.DebugLog("Login First");
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugLog("Error: " + result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Logger.DebugLog("aToken.UserId: " + aToken.UserId);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Logger.DebugLog("perm: " + perm);
                }
            }
            else
            {
                Logger.DebugLog("User cancelled login");
            }
        }
    }
}
