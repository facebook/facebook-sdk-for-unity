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
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;



    internal sealed class MainMenu : MenuBase
    {

        enum Scope {
            PublicProfile   = 1,
            UserFriends     = 2,
            UserBirthday    = 3,
            UserAgeRange    = 4,
            PublishActions  = 5,
            UserLocation    = 6,
            UserHometown    = 7,
            UserGender      = 8,
        };

        protected override bool ShowBackButton()
        {
            return false;
        }

        protected override void GetGui()
        {
            GUILayout.BeginVertical();


            bool enabled = GUI.enabled;
            if (this.Button("FB.Init"))
            {
                FB.Init(this.OnInitComplete, this.OnHideUnity);
                this.Status = "FB.Init() called with " + FB.AppId;
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Classic login"))
            {
                this.CallFBLogin(LoginTracking.ENABLED, new HashSet<Scope> { Scope.PublicProfile });
                this.Status = "Classic login called";
            }
            if (this.Button("Get publish_actions"))
            {
                this.CallFBLoginForPublish();
                this.Status = "Login (for publish_actions) called";
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (this.Button("Limited login"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile });
                this.Status = "Limited login called";

            }
            if (this.Button("Limited login +friends"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserFriends });
                this.Status = "Limited login +friends called";

            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (this.Button("Limited Login+bday"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserBirthday });
                this.Status = "Limited login +bday called";
            }

            if (this.Button("Limited Login+agerange"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserAgeRange });
                this.Status = "Limited login +agerange called";
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (this.Button("Limited Login + location"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserLocation });
            }

            if (this.Button("Limited Login + Hometown"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserHometown });
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (this.Button("Limited Login + Gender"))
            {
                this.CallFBLogin(LoginTracking.LIMITED, new HashSet<Scope> { Scope.PublicProfile, Scope.UserGender });
            }


            GUI.enabled = FB.IsLoggedIn;


            // Fix GUILayout margin issues
            GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            // Fix GUILayout margin issues
            GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
            GUILayout.EndHorizontal();

            #if !UNITY_WEBGL
            if (this.Button("Logout"))
            {
                CallFBLogout();
                this.Status = "Logout called";
            }
            #endif

            GUI.enabled = enabled && FB.IsInitialized;
            if (this.Button("Share Dialog"))
            {
                this.SwitchMenu(typeof(DialogShare));
            }

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

            if (this.Button("Tournaments"))
            {
                this.SwitchMenu(typeof(TournamentsMenu));
            }

            if (Constants.IsMobile && this.Button("Access Token"))
            {
                this.SwitchMenu(typeof(AccessTokenMenu));
            }

            if (this.Button("UploadToMediaLibrary"))
            {
                this.SwitchMenu(typeof(UploadToMediaLibrary));
            }

            if (Constants.IsWeb && this.Button("Subscription"))
            {
                this.SwitchMenu(typeof(Subscription));
            }

            GUILayout.EndVertical();

            GUI.enabled = enabled;
        }

        private void CallFBLogin(LoginTracking mode, HashSet<Scope> scope)
        {
            List<string> scopes = new List<string>();

            if(scope.Contains(Scope.PublicProfile)) {
                scopes.Add("public_profile");
            }
            if(scope.Contains(Scope.UserFriends))
            {
                scopes.Add("user_friends");
            }
            if(scope.Contains(Scope.UserBirthday))
            {
                scopes.Add("user_birthday");
            }
            if(scope.Contains(Scope.UserAgeRange))
            {
                scopes.Add("user_age_range");
            }

            if(scope.Contains(Scope.UserLocation))
            {
                scopes.Add("user_location");
            }

            if(scope.Contains(Scope.UserHometown))
            {
                scopes.Add("user_hometown");
            }

            if(scope.Contains(Scope.UserGender))
            {
                scopes.Add("user_gender");
            }

            if (Constants.IsWeb && this.Button("Subscription"))
            {
                this.SwitchMenu(typeof(Subscription));
            }

            if (mode == LoginTracking.ENABLED)
            {
                if (Constants.CurrentPlatform == FacebookUnityPlatform.IOS) {
                    FB.Mobile.LoginWithTrackingPreference(LoginTracking.ENABLED, scopes, "classic_nonce123", this.HandleResult);
                } else {
                    FB.LogInWithReadPermissions(scopes, this.HandleResult);
                }
            }
            else // mode == loginTracking.LIMITED
            {
                FB.Mobile.LoginWithTrackingPreference(LoginTracking.LIMITED, scopes, "limited_nonce123", this.HandleLimitedLoginResult);
            }

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
            this.Status = "Success - Check log for details";
            this.LastResponse = "Success Response: OnInitComplete Called\n";
            string logMessage = string.Format(
                "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
                FB.IsLoggedIn,
                FB.IsInitialized);
            LogView.AddLog(logMessage);
            if (AccessToken.CurrentAccessToken != null)
            {
                LogView.AddLog(AccessToken.CurrentAccessToken.ToString());
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            LogView.AddLog("Is game shown: " + isGameShown);
        }
    }
}
