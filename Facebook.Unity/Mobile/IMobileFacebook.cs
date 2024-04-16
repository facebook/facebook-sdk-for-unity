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

namespace Facebook.Unity.Mobile
{
    using System;
    using System.Collections.Generic;

    internal interface IMobileFacebook : IFacebook
    {
        ShareDialogMode ShareDialogMode { get; set; }

        string UserID { get; set; }

        void EnableProfileUpdatesOnAccessTokenChange(bool enable);

        void LoginWithTrackingPreference(string tracking, IEnumerable<string> permissions, string nonce,
            FacebookDelegate<ILoginResult> callback);

        void FetchDeferredAppLink(
            FacebookDelegate<IAppLinkResult> callback);

        void RefreshCurrentAccessToken(
            FacebookDelegate<IAccessTokenRefreshResult> callback);

        bool IsImplicitPurchaseLoggingEnabled();

        void SetPushNotificationsDeviceTokenString(string token);

        void SetAutoLogAppEventsEnabled(bool autoLogAppEventsEnabled);

        void SetAdvertiserIDCollectionEnabled(bool advertiserIDCollectionEnabled);

        bool SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        bool SetDomainErrorEnabled(bool domainErrorEnabled);

        void SetDataProcessingOptions(IEnumerable<string> options, int country, int state);

        void OnIAPReady(FacebookDelegate<IIAPReadyResult> callback);

        void InitCloudGame(FacebookDelegate<IInitCloudGameResult> callback);

        void GameLoadComplete(FacebookDelegate<IGameLoadCompleteResult> callback);

        void GetPayload(FacebookDelegate<IPayloadResult> callback);

        void GetTournaments(FacebookDelegate<IGetTournamentsResult> callback);

        void UpdateTournament(string tournamentID, int score, FacebookDelegate<ITournamentScoreResult> callback);

        void UpdateAndShareTournament(string tournamentID, int score, FacebookDelegate<IDialogResult> callback);

        void CreateAndShareTournament(
            int initialScore,
            string title,
            TournamentSortOrder sortOrder,
            TournamentScoreFormat scoreFormat,
            long endTime,
            string payload,
            FacebookDelegate<IDialogResult> callback);

        void OpenAppStore(FacebookDelegate<IOpenAppStoreResult> callback);

        void CreateGamingContext(string playerID, FacebookDelegate<ICreateGamingContextResult> callback);

        void SwitchGamingContext(string gamingContextID, FacebookDelegate<ISwitchGamingContextResult> callback);

        void ChooseGamingContext(List<string> filters, int minSize, int maxSize, FacebookDelegate<IChooseGamingContextResult> callback);

        void GetCurrentGamingContext(FacebookDelegate<IGetCurrentGamingContextResult> callback);

        AuthenticationToken CurrentAuthenticationToken();

    }
}
