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
using System;

public class FBWindowsFriendsManager : MonoBehaviour {

    public FBWindowsLogsManager Logger;
    public Transform ReceivedInvitationsPanelTransform;
    public GameObject ShowReceivedInvitation;

    public void Button_OpenReceivedInvitations()
    {
        if (FB.IsLoggedIn)
        {
            FB.OpenFriendFinderDialog(OpenFriendsDialogCallBack);
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }

    private void OpenFriendsDialogCallBack(IGamingServicesFriendFinderResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Friends Dialog openned succesflly");
        }
    }

    public void Button_GetFriendFinderInvitations()
    {
        if (FB.IsLoggedIn)
        {
            FB.GetFriendFinderInvitations(GetFriendFinderInvitationsCallback);
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }

    private void GetFriendFinderInvitationsCallback(IFriendFinderInvitationResult receivedInvitations)
    {
        Logger.DebugLog("Processing received invitations...");
        if (FB.IsLoggedIn)
        {
            foreach (Transform child in ReceivedInvitationsPanelTransform)
            {
                Destroy(child.gameObject);
            }

            if (receivedInvitations.Error != null)
            {
                Logger.DebugErrorLog(receivedInvitations.Error);
            }
            else
            {
                if (receivedInvitations.Invitations.Count <= 0)
                {
                    Logger.DebugLog("No invitations received.");
                }
                else
                {
                    foreach (FriendFinderInviation item in receivedInvitations.Invitations)
                    {
                        GameObject obj = (GameObject)Instantiate(ShowReceivedInvitation);
                        obj.transform.SetParent(ReceivedInvitationsPanelTransform, false);
                        obj.transform.localScale = new Vector3(1, 1, 1);

                        Button button = obj.GetComponent<Button>();
                        button.onClick.AddListener(() => { button.interactable = false; FB.DeleteFriendFinderInvitation(item.Id, DeleteFriendFinderInvitationCallback); });

                        Text textComponent = obj.GetComponentInChildren<Text>();
                        textComponent.text = "ApplicationName: " + item.ApplicationName + "\nFrom: " + item.FromName + "\nTo: " + item.ToName + "\nMessage: " + item.Message + "\nTime: " + item.CreatedTime;
                    }
                }
            }
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }

    private void DeleteFriendFinderInvitationCallback(IFriendFinderInvitationResult receivedInvitations)
    {
        if (receivedInvitations.Error != null)
        {
            Logger.DebugErrorLog(receivedInvitations.Error);
        }
        else
        {
            Logger.DebugLog("Inviation deleted succesfully. Give a reward at this point if you want.");
        }
        Button_GetFriendFinderInvitations();
    }
}
