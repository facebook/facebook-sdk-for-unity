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

namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor;
    using Facebook.Unity.Mobile;
    using Facebook.Unity.Mobile.Android;
    using Facebook.Unity.Mobile.IOS;
    using UnityEngine;

    /// <summary>
    /// Static class for exposing the facebook integration.
    /// </summary>
    public sealed class FB : ScriptableObject
    {
        private const string DefaultJSSDKLocale = "en_US";
        private static IFacebook facebook;
        private static bool isInitCalled = false;
        private static string facebookDomain = "facebook.com";
        private static string graphApiVersion = Constants.GraphApiVersion;

        private delegate void OnDLLLoaded();

        /// <summary>
        /// Gets the app identifier. AppId might be different from FBSettings.AppId
        /// if using the programmatic version of FB.Init().
        /// </summary>
        /// <value>The app identifier.</value>
        public static string AppId { get; private set; }

        /// <summary>
        /// Gets or sets the graph API version.
        /// The Unity sdk is by default pegged to the lastest version of the graph api
        /// at the time of the SDKs release. To override this value to use a different
        /// version set this value.
        /// <remarks>
        /// This value is only valid for direct api calls made through FB.Api and the
        /// graph api version used by the javscript sdk when running on the web. The
        /// underlyting Android and iOS SDKs will still be pegged to the graph api
        /// version of this release.
        /// </remarks>
        /// </summary>
        /// <value>The graph API version.</value>
        public static string GraphApiVersion
        {
            get
            {
                return FB.graphApiVersion;
            }

            set
            {
                FB.graphApiVersion = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a user logged in.
        /// </summary>
        /// <value><c>true</c> if is logged in; otherwise, <c>false</c>.</value>
        public static bool IsLoggedIn
        {
            get
            {
                return (facebook != null) && FacebookImpl.LoggedIn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is the SDK is initialized.
        /// </summary>
        /// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
        public static bool IsInitialized
        {
            get
            {
                return (facebook != null) && facebook.Initialized;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FB"/> limit app event usage.
        /// If the player has set the limitEventUsage flag to YES, your app will continue
        /// to send this data to Facebook, but Facebook will not use the data to serve
        /// targeted ads. Facebook may continue to use the information for other purposes,
        /// including frequency capping, conversion events, estimating the number of unique
        /// users, security and fraud detection, and debugging.
        /// </summary>
        /// <value><c>true</c> if limit app event usage; otherwise, <c>false</c>.</value>
        public static bool LimitAppEventUsage
        {
            get
            {
                return (facebook != null) && facebook.LimitEventUsage;
            }

            set
            {
                if (facebook != null)
                {
                    facebook.LimitEventUsage = value;
                }
            }
        }

        internal static IFacebook FacebookImpl
        {
            get
            {
                if (FB.facebook == null)
                {
                    throw new NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
                }

                return FB.facebook;
            }

            set
            {
                FB.facebook = value;
            }
        }

        internal static string FacebookDomain
        {
            get
            {
                return FB.facebookDomain;
            }

            set
            {
                FB.facebookDomain = value;
            }
        }

        private static OnDLLLoaded OnDLLLoadedDelegate { get; set; }

        /// <summary>
        /// This is the preferred way to call FB.Init(). It will take the facebook app id specified in your "Facebook"
        /// => "Edit Settings" menu when it is called.
        /// </summary>
        /// <param name="onInitComplete">
        /// Delegate is called when FB.Init() finished initializing everything. By passing in a delegate you can find
        /// out when you can safely call the other methods.
        /// </param>
        /// <param name="onHideUnity">A delegate to invoke when unity is hidden.</param>
        /// <param name="authResponse">Auth response.</param>
        public static void Init(InitDelegate onInitComplete = null, HideUnityDelegate onHideUnity = null, string authResponse = null)
        {
            Init(
                FacebookSettings.AppId,
                FacebookSettings.Cookie,
                FacebookSettings.Logging,
                FacebookSettings.Status,
                FacebookSettings.Xfbml,
                FacebookSettings.FrictionlessRequests,
                authResponse,
                FB.DefaultJSSDKLocale,
                onHideUnity,
                onInitComplete);
        }

        /// <summary>
        /// If you need a more programmatic way to set the facebook app id and other setting call this function.
        /// Useful for a build pipeline that requires no human input.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        /// <param name="cookie">If set to <c>true</c> cookie.</param>
        /// <param name="logging">If set to <c>true</c> logging.</param>
        /// <param name="status">If set to <c>true</c> status.</param>
        /// <param name="xfbml">If set to <c>true</c> xfbml.</param>
        /// <param name="frictionlessRequests">If set to <c>true</c> frictionless requests.</param>
        /// <param name="authResponse">Auth response.</param>
        /// <param name="javascriptSDKLocale">
        /// The locale of the js sdk used see
        /// https://developers.facebook.com/docs/internationalization#plugins.
        /// </param>
        /// <param name="onHideUnity">
        /// A delegate to invoke when unity is hidden.
        /// </param>
        /// <param name="onInitComplete">
        /// Delegate is called when FB.Init() finished initializing everything. By passing in a delegate you can find
        /// out when you can safely call the other methods.
        /// </param>
        public static void Init(
            string appId,
            bool cookie = true,
            bool logging = true,
            bool status = true,
            bool xfbml = false,
            bool frictionlessRequests = true,
            string authResponse = null,
            string javascriptSDKLocale = FB.DefaultJSSDKLocale,
            HideUnityDelegate onHideUnity = null,
            InitDelegate onInitComplete = null)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException("appId cannot be null or empty!");
            }

            FB.AppId = appId;

            if (!isInitCalled)
            {
                isInitCalled = true;

                if (Constants.IsEditor)
                {
                    FB.OnDLLLoadedDelegate = delegate
                    {
                        ((EditorFacebook)FB.facebook).Init(onHideUnity, onInitComplete);
                    };

                    ComponentFactory.GetComponent<EditorFacebookLoader>();
                }
                else
                {
                    switch (Constants.CurrentPlatform)
                    {
                        case FacebookUnityPlatform.WebGL:
                        case FacebookUnityPlatform.WebPlayer:
                            FB.OnDLLLoadedDelegate = delegate
                            {
                                ((CanvasFacebook)FB.facebook).Init(
                                    appId,
                                    cookie,
                                    logging,
                                    status,
                                    xfbml,
                                    FacebookSettings.ChannelUrl,
                                    authResponse,
                                    frictionlessRequests,
                                    javascriptSDKLocale,
                                    onHideUnity,
                                    onInitComplete);
                            };
                            ComponentFactory.GetComponent<CanvasFacebookLoader>();
                            break;
                        case FacebookUnityPlatform.IOS:
                            FB.OnDLLLoadedDelegate = delegate
                            {
                                ((IOSFacebook)FB.facebook).Init(
                                    appId,
                                    frictionlessRequests,
                                    onHideUnity,
                                    onInitComplete);
                            };
                            ComponentFactory.GetComponent<IOSFacebookLoader>();
                            break;
                        case FacebookUnityPlatform.Android:
                            FB.OnDLLLoadedDelegate = delegate
                            {
                                ((AndroidFacebook)FB.facebook).Init(
                                    appId,
                                    onHideUnity,
                                    onInitComplete);
                            };
                            ComponentFactory.GetComponent<AndroidFacebookLoader>();
                            break;
                        default:
                            throw new NotImplementedException("Facebook API does not yet support this platform");
                    }
                }
            }
            else
            {
                FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
            }
        }

        /// <summary>
        /// Logs the user in with the requested publish permissions.
        /// </summary>
        /// <param name="permissions">A list of requested permissions.</param>
        /// <param name="callback">Callback to be called when request completes.</param>
        public static void LogInWithPublishPermissions(
            IEnumerable<string> permissions = null,
            FacebookDelegate<ILoginResult> callback = null)
        {
            FacebookImpl.LogInWithPublishPermissions(permissions, callback);
        }

        /// <summary>
        /// Logs the user in with the requested read permissions.
        /// </summary>
        /// <param name="permissions">A list of requested permissions.</param>
        /// <param name="callback">Callback to be called when request completes.</param>
        public static void LogInWithReadPermissions(
            IEnumerable<string> permissions = null,
            FacebookDelegate<ILoginResult> callback = null)
        {
            FacebookImpl.LogInWithReadPermissions(permissions, callback);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public static void LogOut()
        {
            FacebookImpl.LogOut();
        }

        /// <summary>
        /// Apps the request.
        /// </summary>
        /// <param name="message">The request string the recipient will see, maximum length 60 characters.</param>
        /// <param name="actionType">Request action type for structured request.</param>
        /// <param name="objectId">
        /// Open Graph object ID for structured request.
        /// Note the type of object should belong to this app.
        /// </param>
        /// <param name="to">A list of Facebook IDs to which to send the request.</param>
        /// <param name="data">
        /// Additional data stored with the request on Facebook,
        /// and handed back to the app when it reads the request back out.
        /// Maximum length 255 characters.</param>
        /// <param name="title">The title for the platform multi-friend selector dialog. Max length 50 characters..</param>
        /// <param name="callback">A callback for when the request completes.</param>
        public static void AppRequest(
            string message,
            OGActionType actionType,
            string objectId,
            IEnumerable<string> to,
            string data = "",
            string title = "",
            FacebookDelegate<IAppRequestResult> callback = null)
        {
            FacebookImpl.AppRequest(message, actionType, objectId, to, null, null, null, data, title, callback);
        }

        /// <summary>
        /// Apps the request.
        /// </summary>
        /// <param name="message">The request string the recipient will see, maximum length 60 characters.</param>
        /// <param name="actionType">Request action type for structured request.</param>
        /// <param name="objectId">
        /// Open Graph object ID for structured request.
        /// Note the type of object should belong to this app.
        /// </param>
        /// <param name="filters">
        /// The configuration of the platform multi-friend selector.
        /// It should be a List of filter strings.
        /// </param>
        /// <param name="excludeIds">
        /// A list of Facebook IDs to exclude from the platform multi-friend selector dialog.
        /// This list is currently not supported for mobile devices.
        /// </param>
        /// <param name="maxRecipients">
        /// Platform-dependent The maximum number of recipients the sender should be able to
        /// choose in the platform multi-friend selector dialog.
        /// Only guaranteed to work in Unity Web Player app.
        /// </param>
        /// <param name="data">
        /// Additional data stored with the request on Facebook, and handed
        /// back to the app when it reads the request back out.
        /// Maximum length 255 characters.
        /// </param>
        /// <param name="title">
        /// The title for the platform multi-friend selector dialog. Max length 50 characters.
        /// </param>
        /// <param name="callback">A callback for when the request completes.</param>
        public static void AppRequest(
            string message,
            OGActionType actionType,
            string objectId,
            IEnumerable<object> filters = null,
            IEnumerable<string> excludeIds = null,
            int? maxRecipients = null,
            string data = "",
            string title = "",
            FacebookDelegate<IAppRequestResult> callback = null)
        {
            FacebookImpl.AppRequest(message, actionType, objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
        }

        /// <summary>
        /// Apps the request.
        /// </summary>
        /// <param name="message">The request string the recipient will see, maximum length 60 characters.</param>
        /// <param name="to">A list of Facebook IDs to which to send the request.</param>
        /// <param name="filters">
        /// The configuration of the platform multi-friend selector.
        /// It should be a List of filter strings.
        /// </param>
        /// <param name="excludeIds">
        /// A list of Facebook IDs to exclude from the platform multi-friend selector dialog.
        /// This list is currently not supported for mobile devices.
        /// </param>
        /// <param name="maxRecipients">
        /// Platform-dependent The maximum number of recipients the sender should be able to
        /// choose in the platform multi-friend selector dialog.
        /// Only guaranteed to work in Unity Web Player app.
        /// </param>
        /// <param name="data">
        /// Additional data stored with the request on Facebook, and handed
        /// back to the app when it reads the request back out.
        /// Maximum length 255 characters.
        /// </param>
        /// <param name="title">
        /// The title for the platform multi-friend selector dialog. Max length 50 characters.
        /// </param>
        /// <param name="callback">A callback for when the request completes.</param>
        public static void AppRequest(
            string message,
            IEnumerable<string> to = null,
            IEnumerable<object> filters = null,
            IEnumerable<string> excludeIds = null,
            int? maxRecipients = null,
            string data = "",
            string title = "",
            FacebookDelegate<IAppRequestResult> callback = null)
        {
            FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
        }

        /// <summary>
        /// Opens a share dialog for sharing a link.
        /// </summary>
        /// <param name="contentURL">The URL or the link to share.</param>
        /// <param name="contentTitle">The title to display for this link..</param>
        /// <param name="contentDescription">
        /// The description of the link.  If not specified, this field is automatically populated by
        /// information scraped from the link, typically the title of the page.
        /// </param>
        /// <param name="photoURL">The URL of a picture to attach to this content.</param>
        /// <param name="callback">A callback for when the request completes.</param>
        public static void ShareLink(
            Uri contentURL = null,
            string contentTitle = "",
            string contentDescription = "",
            Uri photoURL = null,
            FacebookDelegate<IShareResult> callback = null)
        {
            FacebookImpl.ShareLink(
                contentURL,
                contentTitle,
                contentDescription,
                photoURL,
                callback);
        }

        /// <summary>
        /// Legacy feed share. Only use this dialog if you need the legacy parameters otherwiese use
        /// <see cref="FB.ShareLink(System.String, System.String, System.String, System.String, Facebook.FacebookDelegate"/>.
        /// </summary>
        /// <param name="toId">
        ///     The ID of the profile that this story will be published to.
        ///     If this is unspecified, it defaults to the value of from.
        ///     The ID must be a friend who also uses your app.
        /// </param>
        /// <param name="link">The link attached to this post.</param>
        /// <param name="linkName">The name of the link attachment.</param>
        /// <param name="linkCaption">
        ///     The caption of the link (appears beneath the link name).
        ///     If not specified, this field is automatically populated
        ///     with the URL of the link.
        /// </param>
        /// <param name="linkDescription">
        ///     The description of the link (appears beneath the link caption).
        ///     If not specified, this field is automatically populated by information
        ///     scraped from the link, typically the title of the page.
        /// </param>
        /// <param name="picture">
        ///     The URL of a picture attached to this post.
        ///     The picture must be at least 200px by 200px.
        ///     See our documentation on sharing best practices for more information on sizes.
        /// </param>
        /// <param name="mediaSource">
        ///     The URL of a media file (either SWF or MP3) attached to this post.
        ///     If SWF, you must also specify picture to provide a thumbnail for the video.
        /// </param>
        /// <param name="callback">The callback to use upon completion.</param>
        public static void FeedShare(
            string toId = "",
            Uri link = null,
            string linkName = "",
            string linkCaption = "",
            string linkDescription = "",
            Uri picture = null,
            string mediaSource = "",
            FacebookDelegate<IShareResult> callback = null)
        {
            FacebookImpl.FeedShare(
                toId,
                link,
                linkName,
                linkCaption,
                linkDescription,
                picture,
                mediaSource,
                callback);
        }

        /// <summary>
        /// Makes a call to the Facebook Graph API.
        /// </summary>
        /// <param name="query">
        /// The Graph API endpoint to call.
        /// You may prefix this with a version string to call a particular version of the API.
        /// </param>
        /// <param name="method">The HTTP method to use in the call.</param>
        /// <param name="callback">The callback to use upon completion.</param>
        /// <param name="formData">The key/value pairs to be passed to the endpoint as arguments.</param>
        public static void API(
            string query,
            HttpMethod method,
            FacebookDelegate<IGraphResult> callback = null,
            IDictionary<string, string> formData = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "The query param cannot be null or empty");
            }

            FacebookImpl.API(query, method, formData, callback);
        }

        /// <summary>
        /// Makes a call to the Facebook Graph API.
        /// </summary>
        /// <param name="query">
        /// The Graph API endpoint to call.
        /// You may prefix this with a version string to call a particular version of the API.
        /// </param>
        /// <param name="method">The HTTP method to use in the call.</param>
        /// <param name="callback">The callback to use upon completion.</param>
        /// <param name="formData">Form data for the request.</param>
        public static void API(
            string query,
            HttpMethod method,
            FacebookDelegate<IGraphResult> callback,
            WWWForm formData)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "The query param cannot be null or empty");
            }

            FacebookImpl.API(query, method, formData, callback);
        }

        /// <summary>
        /// Sends an app activation event to Facebook when your app is activated.
        ///
        /// On iOS and Android this event is logged automatically. You may still
        /// need to call this event if you are running on web.
        ///
        /// On iOS the activate event is fired when the app becomes active
        /// On Android the activate event is fired during FB.Init
        /// </summary>
        public static void ActivateApp()
        {
            FacebookImpl.ActivateApp(AppId);
        }

        /// <summary>
        /// Gets the deep link if available.
        /// </summary>
        /// <param name="callback">The callback to use upon completion.</param>
        public static void GetAppLink(
            FacebookDelegate<IAppLinkResult> callback)
        {
            if (callback == null)
            {
                // No point in fetching the data if there is no callback
                return;
            }

            FacebookImpl.GetAppLink(callback);
        }

        /// <summary>
        /// Opens a dialog to create a new game group.
        /// </summary>
        /// <param name="name">The name of the group you wish to create.</param>
        /// <param name="description">A short description of the group's purpose.</param>
        /// <param name="privacy">
        /// The privacy of the group.
        /// OPEN groups' content is visible to anyone
        /// CLOSED groups can be found by anyone but their content is only visible to members
        /// SECRET groups can only be found by their members.
        /// </param>
        /// <param name="callback">The callback to use upon completion.</param>
        public static void GameGroupCreate(
            string name,
            string description,
            string privacy = "CLOSED",
            FacebookDelegate<IGroupCreateResult> callback = null)
        {
            FacebookImpl.GameGroupCreate(name, description, privacy, callback);
        }

        /// <summary>
        /// Opens a dialog to join a game group.
        /// </summary>
        /// <param name="id">The group ID of the group to which you'd like to add the user.</param>
        /// <param name="callback">The callback to use upon completion.</param>
        public static void GameGroupJoin(
            string id,
            FacebookDelegate<IGroupJoinResult> callback = null)
        {
            FacebookImpl.GameGroupJoin(id, callback);
        }

        /// <summary>
        /// Logs an app event.
        /// </summary>
        /// <param name="logEvent">The name of the event to log.</param>
        /// <param name="valueToSum">A number representing some value to be summed when reported.</param>
        /// <param name="parameters">Any parameters needed to describe the event.</param>
        public static void LogAppEvent(
            string logEvent,
            float? valueToSum = null,
            Dictionary<string, object> parameters = null)
        {
            FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
        }

        /// <summary>
        /// Logs the purchase.
        /// </summary>
        /// <param name="logPurchase">The amount of currency the user spent.</param>
        /// <param name="currency">The 3-letter ISO currency code.</param>
        /// <param name="parameters">
        /// Any parameters needed to describe the event.
        /// Elements included in this dictionary can't be null.
        /// </param>
        public static void LogPurchase(
            float logPurchase,
            string currency = null,
            Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(currency))
            {
                currency = "USD";
            }

            FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
        }

        private static void LogVersion()
        {
            // If we have initlized we can also get the underlying sdk version
            if (facebook != null)
            {
                FacebookLogger.Info(string.Format(
                    "Using Facebook Unity SDK v{0} with {1}",
                    FacebookSdkVersion.Build,
                    FB.FacebookImpl.SDKUserAgent));
            }
            else
            {
                FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0}", FacebookSdkVersion.Build));
            }
        }

        /// <summary>
        /// Contains methods specific to the Facebook Games Canvas platform.
        /// </summary>
        public sealed class Canvas
        {
            private static IPayFacebook FacebookPayImpl
            {
                get
                {
                    IPayFacebook impl = FacebookImpl as IPayFacebook;

                    if (impl == null)
                    {
                        throw new InvalidOperationException("Attempt to call Facebook pay interface on unsupported platform");
                    }

                    return impl;
                }
            }

            /// <summary>
            /// Pay the specified product, action, quantity, quantityMin, quantityMax, requestId, pricepointId,
            /// testCurrency and callback.
            /// </summary>
            /// <param name="product">The URL of your og:product object that the user is looking to purchase.</param>
            /// <param name="action">Should always be purchaseitem.</param>
            /// <param name="quantity">
            /// The amount of this item the user is looking to purchase - typically used when implementing a virtual currency purchase.
            /// </param>
            /// <param name="quantityMin">
            /// The minimum quantity of the item the user is able to purchase.
            /// This parameter is important when handling price jumping to maximize the efficiency of the transaction.
            /// </param>
            /// <param name="quantityMax">
            /// The maximum quantity of the item the user is able to purchase.
            /// This parameter is important when handling price jumping to maximize the efficiency of the transaction.
            /// </param>
            /// <param name="requestId">
            /// The developer defined unique identifier for this transaction, which becomes
            /// attached to the payment within the Graph API.
            /// </param>
            /// <param name="pricepointId">
            /// Used to shortcut a mobile payer directly to the
            /// mobile purchase flow at a given price point.
            /// </param>
            /// <param name="testCurrency">
            /// This parameter can be used during debugging and testing your implementation to force the dialog to
            /// use a specific currency rather than the current user's preferred currency. This allows you to
            /// rapidly prototype your payment experience for different currencies without having to repeatedly
            /// change your personal currency preference settings. This parameter is only available for admins,
            /// developers and testers associated with the app, in order to minimize the security risk of a
            /// malicious JavaScript injection. Provide the 3 letter currency code of the intended forced currency.
            /// </param>
            /// <param name="callback">The callback to use upon completion.</param>
            public static void Pay(
                string product,
                string action = "purchaseitem",
                int quantity = 1,
                int? quantityMin = null,
                int? quantityMax = null,
                string requestId = null,
                string pricepointId = null,
                string testCurrency = null,
                FacebookDelegate<IPayResult> callback = null)
            {
                FacebookPayImpl.Pay(
                    product,
                    action,
                    quantity,
                    quantityMin,
                    quantityMax,
                    requestId,
                    pricepointId,
                    testCurrency,
                    callback);
            }
        }

        /// <summary>
        /// A class containing the settings specific to the supported mobile platforms.
        /// </summary>
        public sealed class Mobile
        {
            /// <summary>
            /// Gets or sets the share dialog mode.
            /// </summary>
            /// <value>The share dialog mode.</value>
            public static ShareDialogMode ShareDialogMode
            {
                get
                {
                    return Mobile.MobileFacebookImpl.ShareDialogMode;
                }

                set
                {
                    Mobile.MobileFacebookImpl.ShareDialogMode = value;
                }
            }

            private static IMobileFacebook MobileFacebookImpl
            {
                get
                {
                    IMobileFacebook impl = FacebookImpl as IMobileFacebook;
                    if (impl == null)
                    {
                        throw new InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
                    }

                    return impl;
                }
            }

            /// <summary>
            /// Show the app invite dialog.
            /// </summary>
            /// <param name="appLinkUrl">
            /// App Link for what should be opened when the recipient clicks on the
            /// install/play button on the app invite page.
            /// </param>
            /// <param name="previewImageUrl">A url to an image to be used in the invite.</param>
            /// <param name="callback">A callback for when the dialog completes.</param>
            public static void AppInvite(
                Uri appLinkUrl,
                Uri previewImageUrl = null,
                FacebookDelegate<IAppInviteResult> callback = null)
            {
                MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
            }

            /// <summary>
            /// Fetchs the deferred app link data.
            /// </summary>
            /// <param name="callback">A callback for when the call is complete.</param>
            public static void FetchDeferredAppLinkData(
                FacebookDelegate<IAppLinkResult> callback = null)
            {
                if (callback == null)
                {
                    // No point in fetching the data if there is no callback
                    return;
                }

                Mobile.MobileFacebookImpl.FetchDeferredAppLink(callback);
            }

            /// <summary>
            /// Refreshs the current access to get a new access token if possible.
            /// </summary>
            /// <param name="callback">A on refresh access token compelte callback.</param>
            public static void RefreshCurrentAccessToken(
                FacebookDelegate<IAccessTokenRefreshResult> callback = null)
            {
                Mobile.MobileFacebookImpl.RefreshCurrentAccessToken(callback);
            }
        }

        /// <summary>
        /// Contains code specific to the Android Platform.
        /// </summary>
        public sealed class Android
        {
            /// <summary>
            /// Gets the key hash.
            /// </summary>
            /// <value>The key hash.</value>
            public static string KeyHash
            {
                get
                {
                    var androidFacebook = FacebookImpl as AndroidFacebook;
                    return (androidFacebook != null) ? androidFacebook.KeyHash : string.Empty;
                }
            }
        }

        internal abstract class CompiledFacebookLoader : MonoBehaviour
        {
            protected abstract FacebookGameObject FBGameObject { get; }

            public void Start()
            {
                FB.facebook = this.FBGameObject.Facebook;
                FB.OnDLLLoadedDelegate();
                FB.LogVersion();
                MonoBehaviour.Destroy(this);
            }
        }
    }
}
