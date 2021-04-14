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
    internal abstract class MobileFacebookGameObject : FacebookGameObject,IMobileFacebookCallbackHandler
    {
        private IMobileFacebookImplementation MobileFacebook
        {
            get
            {
                return (IMobileFacebookImplementation)this.Facebook;
            }
        }

        public void OnFetchDeferredAppLinkComplete(string message)
        {
            this.MobileFacebook.OnFetchDeferredAppLinkComplete(new ResultContainer(message));
        }

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(new ResultContainer(message));
        }

        public void OnFriendFinderComplete(string message)
        {
            this.MobileFacebook.OnFriendFinderComplete(new ResultContainer(message));
        }

        public void OnUploadImageToMediaLibraryComplete(string message)
        {
            this.MobileFacebook.OnUploadImageToMediaLibraryComplete(new ResultContainer(message));
        }

        public void OnUploadVideoToMediaLibraryComplete(string message)
        {
            this.MobileFacebook.OnUploadVideoToMediaLibraryComplete(new ResultContainer(message));
        }

        public void OnOnIAPReadyComplete(string message)
        {
            this.MobileFacebook.OnOnIAPReadyComplete(new ResultContainer(message));
        }

        public void OnGetCatalogComplete(string message)
        {
            this.MobileFacebook.OnGetCatalogComplete(new ResultContainer(message));
        }

        public void OnGetPurchasesComplete(string message)
        {
            this.MobileFacebook.OnGetPurchasesComplete(new ResultContainer(message));
        }

        public void OnPurchaseComplete(string message)
        {
            this.MobileFacebook.OnPurchaseComplete(new ResultContainer(message));
        }

        public void OnConsumePurchaseComplete(string message)
        {
            this.MobileFacebook.OnConsumePurchaseComplete(new ResultContainer(message));
        }

        public void OnInitCloudGameComplete(string message)
        {
            this.MobileFacebook.OnInitCloudGameComplete(new ResultContainer(message));
        }

        public void OnScheduleAppToUserNotificationComplete(string message)
        {
            this.MobileFacebook.OnScheduleAppToUserNotificationComplete(new ResultContainer(message));
        }

        public void OnLoadInterstitialAdComplete(string message)
        {
            this.MobileFacebook.OnLoadInterstitialAdComplete(new ResultContainer(message));
        }

        public void OnShowInterstitialAdComplete(string message)
        {
            this.MobileFacebook.OnShowInterstitialAdComplete(new ResultContainer(message));
        }

        public void OnLoadRewardedVideoComplete(string message)
        {
            this.MobileFacebook.OnLoadRewardedVideoComplete(new ResultContainer(message));
        }

        public void OnShowRewardedVideoComplete(string message)
        {
            this.MobileFacebook.OnShowRewardedVideoComplete(new ResultContainer(message));
        }

        public void OnGetPayloadComplete(string message)
        {
            this.MobileFacebook.OnGetPayloadComplete(new ResultContainer(message));
        }

        public virtual void OnPostSessionScoreComplete(string message)
        {
            this.MobileFacebook.OnPostSessionScoreComplete(new ResultContainer(message));
        }

        public virtual void OnPostTournamentScoreComplete(string message)
        {
            this.MobileFacebook.OnPostTournamentScoreComplete(new ResultContainer(message));
        }

        public virtual void OnGetTournamentComplete(string message)
        {
            this.MobileFacebook.OnGetTournamentComplete(new ResultContainer(message));
        }

        public virtual void OnShareTournamentComplete(string message)
        {
            this.MobileFacebook.OnShareTournamentComplete(new ResultContainer(message));
        }

        public virtual void OnCreateTournamentComplete(string message)
        {
            this.MobileFacebook.OnCreateTournamentComplete(new ResultContainer(message));
        }

        public void OnOpenAppStoreComplete(string message)
        {
            this.MobileFacebook.OnOpenAppStoreComplete(new ResultContainer(message));
        }
    }
}
