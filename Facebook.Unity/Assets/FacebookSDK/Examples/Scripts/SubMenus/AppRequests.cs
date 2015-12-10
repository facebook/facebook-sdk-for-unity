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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class AppRequests : MenuBase
    {
        private string requestMessage = string.Empty;
        private string requestTo = string.Empty;
        private string requestFilter = string.Empty;
        private string requestExcludes = string.Empty;
        private string requestMax = string.Empty;
        private string requestData = string.Empty;
        private string requestTitle = string.Empty;
        private string requestObjectID = string.Empty;
        private int selectedAction = 0;
        private string[] actionTypeStrings =
        {
            "NONE",
            OGActionType.SEND.ToString(),
            OGActionType.ASKFOR.ToString(),
            OGActionType.TURN.ToString()
        };

        protected override void GetGui()
        {
            if (this.Button("Select - Filter None"))
            {
                FB.AppRequest("Test Message", callback: this.HandleResult);
            }

            if (this.Button("Select - Filter app_users"))
            {
                List<object> filter = new List<object>() { "app_users" };

                // workaround for mono failing with named parameters
                FB.AppRequest("Test Message", null, filter, null, 0, string.Empty, string.Empty, this.HandleResult);
            }

            if (this.Button("Select - Filter app_non_users"))
            {
                List<object> filter = new List<object>() { "app_non_users" };
                FB.AppRequest("Test Message", null, filter, null, 0, string.Empty, string.Empty, this.HandleResult);
            }

            // Custom options
            this.LabelAndTextField("Message: ", ref this.requestMessage);
            this.LabelAndTextField("To (optional): ", ref this.requestTo);
            this.LabelAndTextField("Filter (optional): ", ref this.requestFilter);
            this.LabelAndTextField("Exclude Ids (optional): ", ref this.requestExcludes);
            this.LabelAndTextField("Filters: ", ref this.requestExcludes);
            this.LabelAndTextField("Max Recipients (optional): ", ref this.requestMax);
            this.LabelAndTextField("Data (optional): ", ref this.requestData);
            this.LabelAndTextField("Title (optional): ", ref this.requestTitle);

            GUILayout.BeginHorizontal();
            GUILayout.Label(
                "Request Action (optional): ",
                this.LabelStyle,
                GUILayout.MaxWidth(200 * this.ScaleFactor));

            this.selectedAction = GUILayout.Toolbar(
                this.selectedAction,
                this.actionTypeStrings,
                this.ButtonStyle,
                GUILayout.MinHeight(ConsoleBase.ButtonHeight * this.ScaleFactor),
                GUILayout.MaxWidth(ConsoleBase.MainWindowWidth - 150));

            GUILayout.EndHorizontal();
            this.LabelAndTextField("Request Object ID (optional): ", ref this.requestObjectID);

            if (this.Button("Custom App Request"))
            {
                OGActionType? action = this.GetSelectedOGActionType();
                if (action != null)
                {
                    FB.AppRequest(
                        this.requestMessage,
                        action.Value,
                        this.requestObjectID,
                        this.requestTo != null ? this.requestTo.Split(',') : null,
                        this.requestData,
                        this.requestTitle,
                        this.HandleResult);
                }
                else
                {
                    FB.AppRequest(
                        this.requestMessage,
                        string.IsNullOrEmpty(this.requestTo) ? null : this.requestTo.Split(','),
                        string.IsNullOrEmpty(this.requestFilter) ? null : this.requestFilter.Split(',').OfType<object>().ToList(),
                        string.IsNullOrEmpty(this.requestExcludes) ? null : this.requestExcludes.Split(','),
                        string.IsNullOrEmpty(this.requestMax) ? 0 : int.Parse(this.requestMax),
                        this.requestData,
                        this.requestTitle,
                        this.HandleResult);
                }
            }
        }

        private OGActionType? GetSelectedOGActionType()
        {
            string actionString = this.actionTypeStrings[this.selectedAction];
            if (actionString == OGActionType.SEND.ToString())
            {
                return OGActionType.SEND;
            }
            else if (actionString == OGActionType.ASKFOR.ToString())
            {
                return OGActionType.ASKFOR;
            }
            else if (actionString == OGActionType.TURN.ToString())
            {
                return OGActionType.TURN;
            }
            else
            {
                return null;
            }
        }
    }
}
