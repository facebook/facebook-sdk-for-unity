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
    internal interface IMobileFacebookCallbackHandler : IFacebookCallbackHandler
    {
        void OnFetchDeferredAppLinkComplete(string message);

        void OnRefreshCurrentAccessTokenComplete(string message);

        void OnFriendFinderComplete(string message);

        void OnUploadImageToMediaLibraryComplete(string message);

        void OnUploadVideoToMediaLibraryComplete(string message);

        void OnOnIAPReadyComplete(string message);

        void OnGetCatalogComplete(string message);

        void OnGetPurchasesComplete(string message);

        void OnPurchaseComplete(string message);

        void OnConsumePurchaseComplete(string message);

        void OnInitCloudGameComplete(string message);

        void OnScheduleAppToUserNotificationComplete(string message);

        void OnLoadInterstitialAdComplete(string message);

        void OnShowInterstitialAdComplete(string message);

        void OnLoadRewardedVideoComplete(string message);

        void OnShowRewardedVideoComplete(string message);

        void OnGetPayloadComplete(string message);

        void OnPostSessionScoreComplete(string message);

        void OnGetTournamentComplete(string message);

        void OnShareTournamentComplete(string message);

        void OnCreateTournamentComplete(string message);

        void OnPostTournamentScoreComplete(string message);

        void OnOpenAppStoreComplete(string message);
    }
}
