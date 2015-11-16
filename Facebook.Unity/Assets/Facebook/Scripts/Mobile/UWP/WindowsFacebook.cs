using UnityEngine;
using Facebook.Unity;
using Facebook.Unity.Mobile;
using System;
using System.Collections.Generic;
#if UNITY_UWP
using Windows.Foundation.Collections;
#endif

namespace Facebook.Unity.Mobile.UWP
{
    internal sealed class WindowsFacebook : MobileFacebook
    {
#if UNITY_UWP
        private FBSession facebookWindows = null;
#endif

        private bool limitEventUsage;

        public WindowsFacebook() : this(new CallbackManager()){}
        public WindowsFacebook(CallbackManager callbackManager) : base(callbackManager){}

        public override bool LimitEventUsage
        {
            get
            {
                return limitEventUsage;
            }

            set
            {
                limitEventUsage = value;
            }
        }

        public override string SDKName
        {
            get
            {
                return "winsdkfb";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return "1.0";
            }
        }

        public override void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
#if UNITY_UWP
            base.Init(appId, cookie, logging, status, xfbml, channelUrl, authResponse, frictionlessRequests, hideUnityDelegate, onInitComplete);

            facebookWindows = Facebook.FBSession.ActiveSession;
            facebookWindows.FBAppId = appId;
            facebookWindows.WinAppId = Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();

            //We don't want to call the Facebook-callback (OnInitComplete) because it won't trigger our callback and it will ALSO trigger OnLoginComplete which is wrong
            if (onInitComplete != null)
                onInitComplete();
#endif
        }

        public override void ActivateApp(string appId = null)
        {
            throw new NotImplementedException();
        }

        public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
        {
#if UNITY_UWP
            InvokeOnUIThread(async () =>
            {
                Dictionary<string, object> response = new Dictionary<string, object>();

                PropertySet parameters = new PropertySet();
                parameters.Add("message", message);
                parameters.Add("actionType", actionType);
                parameters.Add("objectId", objectId);
                parameters.Add("to", to);
                parameters.Add("filters", filters);
                parameters.Add("excludeIds", excludeIds);
                parameters.Add("maxRecipients", maxRecipients);
                parameters.Add("data", data);
                parameters.Add("title", title);

                FBResult fbResponse = await facebookWindows.ShowRequestsDialogAsync(parameters);

                if (fbResponse.Succeeded)
                {
                    FBAppRequest appRequest = fbResponse.Object as FBAppRequest;
                    response.Add("to", appRequest.RecipientIds.ToCommaSeparateList());
                    response.Add("request", appRequest.RequestId);
                }
                else
                {
                    if (fbResponse.ErrorInfo == null)
                        response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    else
                    {
                        if (fbResponse.ErrorInfo.Code == 4201)
                            response.Add("cancelled", true);
                        else
                            response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    }
                }

                InvokeOnAppThread(() =>
                {
                    response.Add("callback_id", CallbackManager.AddFacebookDelegate(callback).ToString());
                    OnAppRequestsComplete(MiniJSON.Json.Serialize(response));
                });
            });
#endif
        }

        public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
        {
#if UNITY_UWP
            InvokeOnUIThread(async () =>
            {
                Dictionary<string, object> response = new Dictionary<string, object>();

                PropertySet parameters = new PropertySet();
                parameters.Add("toID", toId);
                parameters.Add("link", link.AbsoluteUri);
                parameters.Add("linkName", linkName);
                parameters.Add("linkCaption", linkCaption);
                parameters.Add("linkDescription", linkDescription);
                parameters.Add("picture", picture);
                parameters.Add("mediaSource", mediaSource);

                FBResult fbResponse = await facebookWindows.ShowFeedDialogAsync(parameters);

                if (fbResponse.Succeeded)
                {
                    FBFeedRequest feedRequest = fbResponse.Object as FBFeedRequest;
                    response.Add("id", feedRequest.PostId);
                    response.Add("posted", true);
                }
                else
                {
                    if (fbResponse.ErrorInfo == null)
                        response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    else
                    {
                        if (fbResponse.ErrorInfo.Code == 4201)
                            response.Add("cancelled", true);
                        else
                            response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    }
                }

                InvokeOnAppThread(() =>
                {
                    response.Add("callback_id", CallbackManager.AddFacebookDelegate(callback).ToString());
                    OnShareLinkComplete(MiniJSON.Json.Serialize(response));
                });
            });
#endif
        }

        public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void LogInWithPublishPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
        {
            LogInWithReadPermissions(scope, callback);
        }

        public override void LogInWithReadPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
        {
#if UNITY_UWP
            InvokeOnUIThread(async ()=>
            {
                //TODO: Need this, but commented out for now until we create an InternalGetPermissions(...) method
                FBPermissions permissions = scope == null ? new FBPermissions(new List<string>()) : new FBPermissions(new List<string>(scope));
                Dictionary<string, object> response = new Dictionary<string, object>();

                FBResult fbResponse = await facebookWindows.LoginAsync(permissions);

                if (fbResponse.Succeeded)
                {
                    //TODO: If expiration_timestamp doesn't work try StackOverflow's solution:
                    //http://stackoverflow.com/questions/9946269/what-date-format-does-facebook-use-for-the-expiration-access-token
                    response.Add("permissions", facebookWindows.AccessTokenData.GrantedPermissions.Values.ToCommaSeparateList());
                    response.Add("expiration_timestamp", (DateTime.UtcNow.AddMilliseconds(facebookWindows.AccessTokenData.ExpirationDate.Offset.TotalMilliseconds).TotalSeconds() * 1000).ToString());
                    response.Add("access_token", facebookWindows.AccessTokenData.AccessToken);
                    response.Add("user_id", facebookWindows.User.Id);
                    response.Add("granted_permissions", new List<string>(facebookWindows.AccessTokenData.GrantedPermissions.Values));
                    response.Add("declined_permissions", new List<string>(facebookWindows.AccessTokenData.DeclinedPermissions.Values));
                }
                else
                {
                    if (fbResponse.ErrorInfo == null)
                        response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    else
                    {
                        if (fbResponse.ErrorInfo.Code == 4201)
                            response.Add("cancelled", true);
                        else
                            response.Add("error", createErrorMessageAccordingToFacebookUnitySDK(fbResponse.ErrorInfo));
                    }
                }

                InvokeOnAppThread(()=>
                {
                    //Will set access token etc internally
                    response.Add("callback_id", CallbackManager.AddFacebookDelegate(callback).ToString());
                    OnLoginComplete(MiniJSON.Json.Serialize(response));
                });
            });
#endif
        }

        public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
        {
            throw new NotImplementedException();
        }

        public override void LogOut()
        {
#if UNITY_UWP
            InvokeOnUIThread(async ()=>
            {
                await facebookWindows.LogoutAsync();

                InvokeOnAppThread(() =>
                {
                    OnLogoutComplete(null);
                });
            });
#endif
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            throw new NotImplementedException();
        }

        #region Helper-methods that is only available on UWP
#if UNITY_UWP
        private string createErrorMessageAccordingToFacebookUnitySDK(FBError error)
        {
            if (error != null)
                return "{FacebookDialogException: errorCode: " + error.Code + ", message: " + error.Message + ", url: unknown (winsdkfb doesn't expose this)}";
            else
                return "{FacebookDialogException: errorCode: -999, message: internal error (ErrorInfo is null), url: unknown (winsdkfb doesn't expose this)}";
        }
#endif

        private void InvokeOnUIThread(Action callback)
        {
#if UNITY_UWP
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                if (callback != null)
                    callback();

            }, false);
#endif
        }

        private void InvokeOnAppThread(Action callback)
        {
#if UNITY_UWP
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                if (callback != null)
                    callback();

            }, false);
#endif
        }
#endregion
    }
}


//If FeedShare was successful: (example)
//{"id":"10153763649240087_10153806534915087","callback_id":"4","posted":true}
//If FeedShare was cancelled (example)
//{"cancelled":true,"callback_id":"4"}
//If FeedShare had an error (example)
//{"error":"{FacebookDialogException: errorCode: -2, message: net::ERR_NAME_NOT_RESOLVED, url: https:\/\/m.facebook.com\/v2.5\/dialog\/app_requests\/submit}","callback_id":"6"}


//If AppRequest was successful (example)
//{"to":"1074841552539546","request":"1644435765832100","callback_id":"2"}
//If AppRequest was cancelled (example)
//{"cancelled":true,"callback_id":"4"}
//If AppRequest had an error (example)
//{"error":"{FacebookDialogException: errorCode: -2, message: net::ERR_NAME_NOT_RESOLVED, url: https:\/\/m.facebook.com\/v2.5\/dialog\/app_requests\/submit}","callback_id":"6"}

//If LoginWithReadPermissions was successful (example)
//{"access_token":"CAAXQerl5lWMBADRNQ3VMY93eqIkzxdCpbgwRBShxL8tCBU1qaCxrNW3XgecnlEOFoa545FGiIbXJr5p991X5Un5e46ftVGhESRIjC1ZCYZBDAmKQ3FJ8rA8ZCZCWLTWmNZAUJXmilUOronOWZCxwrszNcrj5pUu7ZCtZBpOL11dfGD0ZCovW4tGv25WiVc6fFRKWCEjOq5MI9jyk4JMLTFmPI4gk1U53YDGt5KpFhvjwBjwZDZD","key_hash":"zgF719HeIJnfeKhUbk5\/iq1e+ag=\n","permissions":"public_profile,user_friends,publish_pages,email,manage_pages","opened":true,"expiration_timestamp":"9223372036854775","callback_id":"1","user_id":"10153763649240087","declined_permissions":""}