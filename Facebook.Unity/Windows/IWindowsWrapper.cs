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

namespace Facebook.Unity.Windows
{
    using System;
    using System.Collections.Generic;

    internal interface IWindowsWrapper
    {
        bool Init(string appId, string clientToken);

        void LogInWithScopes(IEnumerable<string> scope, string callbackId, CallbackManager callbackManager);

        bool IsLoggedIn();

        void LogOut();

        void Tick();

        void Deinit();

        void GetCatalog(string callbackId, CallbackManager callbackManager);

        void GetPurchases(string callbackId, CallbackManager callbackManager);

        void Purchase(string productID, string developerPayload, string callbackId, CallbackManager callbackManager);

        void ConsumePurchase(string productToken, string callbackId, CallbackManager callbackManager);

        void CurrentProfile(string callbackId, CallbackManager callbackManager);

        void LoadInterstitialAd(string placementID, string callbackId, CallbackManager callbackManager);

        void ShowInterstitialAd(string placementID, string callbackId, CallbackManager callbackManager);

        void LoadRewardedVideo(string placementID, string callbackId, CallbackManager callbackManager);

        void ShowRewardedVideo(string placementID, string callbackId, CallbackManager callbackManager);

        void OpenFriendFinderDialog(string callbackId, CallbackManager callbackManager);

        void GetFriendFinderInvitations(string callbackId, CallbackManager callbackManager);

        void DeleteFriendFinderInvitation(string invitationId, string callbackId, CallbackManager callbackManager);

        void ScheduleAppToUserNotification(string title, string body, Uri media, int timeInterval, string payload, string callbackId, CallbackManager callbackManager);

        void PostSessionScore(int score, string callbackId, CallbackManager callbackManager);

        void PostTournamentScore(int score, string callbackId, CallbackManager callbackManager);

        void GetTournament(string callbackId, CallbackManager callbackManager);

        void ShareTournament(int score, Dictionary<string, string> data, string callbackId, CallbackManager callbackManager);

        void CreateTournament(int initialScore, string title, string imageBase64DataUrl, string sortOrder, string scoreFormat, Dictionary<string, string> data, string callbackId, CallbackManager callbackManager);

        void UploadImageToMediaLibrary(string caption, Uri imageUri, bool shouldLaunchMediaDialog, string callbackId, string travelId, CallbackManager callbackManager);

        void UploadVideoToMediaLibrary(string caption, Uri videoUri, bool shouldLaunchMediaDialog, string callbackId, string travelId, CallbackManager callbackManager);

        void SetVirtualGamepadLayout(string layout, string callbackId, CallbackManager callbackManager);

        void GetUserLocale(string callbackId, CallbackManager callbackManager);
    }
}
