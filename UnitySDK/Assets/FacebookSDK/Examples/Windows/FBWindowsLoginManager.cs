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
    public RawImage UserImage;
    public Text UserName;


    public void LogInReadButton()
    {
        if (FB.IsInitialized)
        {
            FB.LogInWithReadPermissions(Permissions.text.Split(','), AuthCallback);
        }
        else
        {
            Logger.DebugWarningLog("Not Init");
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
            Logger.DebugWarningLog("Not Init");
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
            Logger.DebugWarningLog("Login First");
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Error: " + result.Error);
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
                //get current profile data
                GetCurrentProfile();
            }
            else
            {
                Logger.DebugWarningLog("User cancelled login");
            }
        }
    }

    public void GetCurrentProfile()
    {
        Logger.DebugLog("Getting current user profile ...");

        FB.CurrentProfile((IProfileResult result) =>
        {
            if (result.Error != null)
            {
                Logger.DebugErrorLog(result.Error);
            }
            else
            {
                Logger.DebugLog(result.CurrentProfile.UserID);
                Logger.DebugLog(result.CurrentProfile.Name);
                Logger.DebugLog(result.CurrentProfile.FirstName);
                Logger.DebugLog(result.CurrentProfile.Email);
                Logger.DebugLog(result.CurrentProfile.ImageURL);

                UserName.text = result.CurrentProfile.Name + " " + result.CurrentProfile.LastName;
                if (result.CurrentProfile.ImageURL != "" && result.CurrentProfile.ImageURL != null)
                {
                    StartCoroutine(LoadPictureFromUrl(result.CurrentProfile.ImageURL, UserImage));
                }
            }
        });
    }

    IEnumerator LoadPictureFromUrl(string url, RawImage itemImage)
    {
        Texture2D UserPicture = new Texture2D(32, 32);

        WWW www = new WWW(url);
        yield return www;

        www.LoadImageIntoTexture(UserPicture);
        www.Dispose();
        www = null;

        itemImage.texture = UserPicture;
    }
}
