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

namespace Facebook.Unity.Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal class GameGroups : MenuBase
    {
        private string gamerGroupName = "Test group";
        private string gamerGroupDesc = "Test group for testing.";
        private string gamerGroupPrivacy = "closed";
        private string gamerGroupCurrentGroup = string.Empty;

        protected override void GetGui()
        {
            if (this.Button("Game Group Create - Closed"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "CLOSED",
                    this.HandleResult);
            }

            if (this.Button("Game Group Create - Open"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "OPEN",
                    this.HandleResult);
            }

            this.LabelAndTextField("Group Name", ref this.gamerGroupName);
            this.LabelAndTextField("Group Description", ref this.gamerGroupDesc);
            this.LabelAndTextField("Group Privacy", ref this.gamerGroupPrivacy);

            if (this.Button("Call Create Group Dialog"))
            {
                this.CallCreateGroupDialog();
            }

            this.LabelAndTextField("Group To Join", ref this.gamerGroupCurrentGroup);
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && !string.IsNullOrEmpty(this.gamerGroupCurrentGroup);
            if (this.Button("Call Join Group Dialog"))
            {
                this.CallJoinGroupDialog();
            }

            GUI.enabled = enabled && FB.IsLoggedIn;
            if (this.Button("Get All App Managed Groups"))
            {
                this.CallFbGetAllOwnedGroups();
            }

            if (this.Button("Get Gamer Groups Logged in User Belongs to"))
            {
                this.CallFbGetUserGroups();
            }

            if (this.Button("Make Group Post As User"))
            {
                this.CallFbPostToGamerGroup();
            }

            GUI.enabled = enabled;
        }

        private void GroupCreateCB(IGroupCreateResult result)
        {
            this.HandleResult(result);
            if (result.GroupId != null)
            {
                this.gamerGroupCurrentGroup = result.GroupId;
            }
        }

        private void GetAllGroupsCB(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.RawResult))
            {
                this.LastResponse = result.RawResult;
                var resultDictionary = result.ResultDictionary;
                if (resultDictionary.ContainsKey("data"))
                {
                    var dataArray = (List<object>)resultDictionary["data"];

                    if (dataArray.Count > 0)
                    {
                        var firstGroup = (Dictionary<string, object>)dataArray[0];
                        this.gamerGroupCurrentGroup = (string)firstGroup["id"];
                    }
                }
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                this.LastResponse = result.Error;
            }
        }

        private void CallFbGetAllOwnedGroups()
        {
            FB.API(FB.AppId + "/groups", HttpMethod.GET, this.GetAllGroupsCB);
        }

        private void CallFbGetUserGroups()
        {
            FB.API("/me/groups?parent=" + FB.AppId, HttpMethod.GET, this.HandleResult);
        }

        private void CallCreateGroupDialog()
        {
            FB.GameGroupCreate(
                this.gamerGroupName,
                this.gamerGroupDesc,
                this.gamerGroupPrivacy,
                this.GroupCreateCB);
        }

        private void CallJoinGroupDialog()
        {
            FB.GameGroupJoin(
                this.gamerGroupCurrentGroup,
                this.HandleResult);
        }

        private void CallFbPostToGamerGroup()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["message"] = "herp derp a post";

            FB.API(
                this.gamerGroupCurrentGroup + "/feed",
                HttpMethod.POST,
                this.HandleResult,
                dict);
        }
    }
}
