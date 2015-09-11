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
    using System.Linq;
    using UnityEngine;

    internal sealed class MainMenu : MenuBase
    {
        protected override bool ShowBackButton()
        {
            return false;
        }

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            if (this.Button("FB.Init"))
            {
                FB.Init(this.OnInitComplete, this.OnHideUnity);
                this.Status = "FB.Init() called with " + FB.AppId;
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Login"))
            {
                this.CallFBLogin();
                this.Status = "Login called";
            }

            GUI.enabled = FB.IsLoggedIn;
            if (this.Button("Get publish_actions"))
            {
                this.CallFBLoginForPublish();
                this.Status = "Login (for publish_actions) called";
            }

            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
            if (Button("Logout"))
            {
                CallFBLogout();
                this.Status = "Logout called";
            }
            #endif
            GUILayout.EndHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Share Dialog"))
            {
                this.SwitchMenu(typeof(DialogShare));
            }

            bool savedEnabled = GUI.enabled;
            GUI.enabled = enabled &&
                AccessToken.CurrentAccessToken != null &&
                AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions");
            if (this.Button("Game Groups"))
            {
                this.SwitchMenu(typeof(GameGroups));
            }

            GUI.enabled = savedEnabled;

            if (this.Button("App Requests"))
            {
                this.SwitchMenu(typeof(AppRequests));
            }

            if (this.Button("Graph Request"))
            {
                this.SwitchMenu(typeof(GraphRequest));
            }

            if (Constants.IsWeb && this.Button("Pay"))
            {
                this.SwitchMenu(typeof(Pay));
            }

            if (this.Button("App Events"))
            {
                this.SwitchMenu(typeof(AppEvents));
            }

            if (this.Button("App Links"))
            {
                this.SwitchMenu(typeof(AppLinks));
            }

            if (Constants.IsMobile && this.Button("App Invites"))
            {
                this.SwitchMenu(typeof(AppInvites));
            }

            GUI.enabled = enabled;
        }

        private void CallFBLogin()
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
        }

        private void CallFBLoginForPublish()
        {
            // It is generally good behavior to split asking for read and publish
            // permissions rather than ask for them all at once.
            //
            // In your own game, consider postponing this call until the moment
            // you actually need it.
            FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, this.HandleResult);
        }

        private void CallFBLogout()
        {
            FB.LogOut();
        }

        private void OnInitComplete()
        {
            this.Status = "Success - Check logk for details";
            this.LastResponse = "Success Response: OnInitComplete Called\n";
            LogView.AddLog("OnInitComplete Called");
        }

        private void OnHideUnity(bool isGameShown)
        {
            this.Status = "Success - Check logk for details";
            this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            LogView.AddLog("Is game shown: " + isGameShown);
        }
    }
}
