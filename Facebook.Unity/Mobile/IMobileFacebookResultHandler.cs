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
    internal interface IMobileFacebookResultHandler : IFacebookResultHandler
    {
        void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer);

        void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer);

        void OnFriendFinderComplete(ResultContainer resultContainer);

        void OnUploadImageToMediaLibraryComplete(ResultContainer resultContainer);

        void OnUploadVideoToMediaLibraryComplete(ResultContainer resultContainer);

        void OnOnIAPReadyComplete(ResultContainer resultContainer);

        void OnGetCatalogComplete(ResultContainer resultContainer);

        void OnGetPurchasesComplete(ResultContainer resultContainer);

        void OnPurchaseComplete(ResultContainer resultContainer);

        void OnConsumePurchaseComplete(ResultContainer resultContainer);

        void OnInitCloudGameComplete(ResultContainer resultContainer);

        void OnScheduleAppToUserNotificationComplete(ResultContainer resultContainer);

        void OnLoadInterstitialAdComplete(ResultContainer resultContainer);

        void OnShowInterstitialAdComplete(ResultContainer resultContainer);

        void OnLoadRewardedVideoComplete(ResultContainer resultContainer);

        void OnShowRewardedVideoComplete(ResultContainer resultContainer);

        void OnGetPayloadComplete(ResultContainer resultContainer);

        void OnPostSessionScoreComplete(ResultContainer resultContainer);

        void OnGetTournamentComplete(ResultContainer resultContainer);

        void OnShareTournamentComplete(ResultContainer resultContainer);

        void OnCreateTournamentComplete(ResultContainer resultContainer);

        void OnPostTournamentScoreComplete(ResultContainer resultContainer);

        void OnOpenAppStoreComplete(ResultContainer resultContainer);
    }
}
