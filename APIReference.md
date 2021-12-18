<a name='assembly'></a>
# Facebook.Unity

## Contents

- [AccessToken](#T-Facebook-Unity-AccessToken 'Facebook.Unity.AccessToken')
  - [#ctor(tokenString,userId,expirationTime,permissions,lastRefresh)](#M-Facebook-Unity-AccessToken-#ctor-System-String,System-String,System-DateTime,System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-DateTime},System-String- 'Facebook.Unity.AccessToken.#ctor(System.String,System.String,System.DateTime,System.Collections.Generic.IEnumerable{System.String},System.Nullable{System.DateTime},System.String)')
  - [CurrentAccessToken](#P-Facebook-Unity-AccessToken-CurrentAccessToken 'Facebook.Unity.AccessToken.CurrentAccessToken')
  - [ExpirationTime](#P-Facebook-Unity-AccessToken-ExpirationTime 'Facebook.Unity.AccessToken.ExpirationTime')
  - [GraphDomain](#P-Facebook-Unity-AccessToken-GraphDomain 'Facebook.Unity.AccessToken.GraphDomain')
  - [LastRefresh](#P-Facebook-Unity-AccessToken-LastRefresh 'Facebook.Unity.AccessToken.LastRefresh')
  - [Permissions](#P-Facebook-Unity-AccessToken-Permissions 'Facebook.Unity.AccessToken.Permissions')
  - [TokenString](#P-Facebook-Unity-AccessToken-TokenString 'Facebook.Unity.AccessToken.TokenString')
  - [UserId](#P-Facebook-Unity-AccessToken-UserId 'Facebook.Unity.AccessToken.UserId')
  - [ToString()](#M-Facebook-Unity-AccessToken-ToString 'Facebook.Unity.AccessToken.ToString')
- [Android](#T-Facebook-Unity-FB-Android 'Facebook.Unity.FB.Android')
  - [KeyHash](#P-Facebook-Unity-FB-Android-KeyHash 'Facebook.Unity.FB.Android.KeyHash')
  - [RetrieveLoginStatus(callback)](#M-Facebook-Unity-FB-Android-RetrieveLoginStatus-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginStatusResult}- 'Facebook.Unity.FB.Android.RetrieveLoginStatus(Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginStatusResult})')
- [AppEventName](#T-Facebook-Unity-AppEventName 'Facebook.Unity.AppEventName')
  - [AchievedLevel](#F-Facebook-Unity-AppEventName-AchievedLevel 'Facebook.Unity.AppEventName.AchievedLevel')
  - [ActivatedApp](#F-Facebook-Unity-AppEventName-ActivatedApp 'Facebook.Unity.AppEventName.ActivatedApp')
  - [AddedPaymentInfo](#F-Facebook-Unity-AppEventName-AddedPaymentInfo 'Facebook.Unity.AppEventName.AddedPaymentInfo')
  - [AddedToCart](#F-Facebook-Unity-AppEventName-AddedToCart 'Facebook.Unity.AppEventName.AddedToCart')
  - [AddedToWishlist](#F-Facebook-Unity-AppEventName-AddedToWishlist 'Facebook.Unity.AppEventName.AddedToWishlist')
  - [CompletedRegistration](#F-Facebook-Unity-AppEventName-CompletedRegistration 'Facebook.Unity.AppEventName.CompletedRegistration')
  - [CompletedTutorial](#F-Facebook-Unity-AppEventName-CompletedTutorial 'Facebook.Unity.AppEventName.CompletedTutorial')
  - [InitiatedCheckout](#F-Facebook-Unity-AppEventName-InitiatedCheckout 'Facebook.Unity.AppEventName.InitiatedCheckout')
  - [Purchased](#F-Facebook-Unity-AppEventName-Purchased 'Facebook.Unity.AppEventName.Purchased')
  - [Rated](#F-Facebook-Unity-AppEventName-Rated 'Facebook.Unity.AppEventName.Rated')
  - [Searched](#F-Facebook-Unity-AppEventName-Searched 'Facebook.Unity.AppEventName.Searched')
  - [SpentCredits](#F-Facebook-Unity-AppEventName-SpentCredits 'Facebook.Unity.AppEventName.SpentCredits')
  - [UnlockedAchievement](#F-Facebook-Unity-AppEventName-UnlockedAchievement 'Facebook.Unity.AppEventName.UnlockedAchievement')
  - [ViewedContent](#F-Facebook-Unity-AppEventName-ViewedContent 'Facebook.Unity.AppEventName.ViewedContent')
- [AppEventParameterName](#T-Facebook-Unity-AppEventParameterName 'Facebook.Unity.AppEventParameterName')
  - [ContentID](#F-Facebook-Unity-AppEventParameterName-ContentID 'Facebook.Unity.AppEventParameterName.ContentID')
  - [ContentType](#F-Facebook-Unity-AppEventParameterName-ContentType 'Facebook.Unity.AppEventParameterName.ContentType')
  - [Currency](#F-Facebook-Unity-AppEventParameterName-Currency 'Facebook.Unity.AppEventParameterName.Currency')
  - [Description](#F-Facebook-Unity-AppEventParameterName-Description 'Facebook.Unity.AppEventParameterName.Description')
  - [Level](#F-Facebook-Unity-AppEventParameterName-Level 'Facebook.Unity.AppEventParameterName.Level')
  - [MaxRatingValue](#F-Facebook-Unity-AppEventParameterName-MaxRatingValue 'Facebook.Unity.AppEventParameterName.MaxRatingValue')
  - [NumItems](#F-Facebook-Unity-AppEventParameterName-NumItems 'Facebook.Unity.AppEventParameterName.NumItems')
  - [PaymentInfoAvailable](#F-Facebook-Unity-AppEventParameterName-PaymentInfoAvailable 'Facebook.Unity.AppEventParameterName.PaymentInfoAvailable')
  - [RegistrationMethod](#F-Facebook-Unity-AppEventParameterName-RegistrationMethod 'Facebook.Unity.AppEventParameterName.RegistrationMethod')
  - [SearchString](#F-Facebook-Unity-AppEventParameterName-SearchString 'Facebook.Unity.AppEventParameterName.SearchString')
  - [Success](#F-Facebook-Unity-AppEventParameterName-Success 'Facebook.Unity.AppEventParameterName.Success')
- [AuthenticationToken](#T-Facebook-Unity-AuthenticationToken 'Facebook.Unity.AuthenticationToken')
  - [#ctor(tokenString,nonce)](#M-Facebook-Unity-AuthenticationToken-#ctor-System-String,System-String- 'Facebook.Unity.AuthenticationToken.#ctor(System.String,System.String)')
  - [Nonce](#P-Facebook-Unity-AuthenticationToken-Nonce 'Facebook.Unity.AuthenticationToken.Nonce')
  - [TokenString](#P-Facebook-Unity-AuthenticationToken-TokenString 'Facebook.Unity.AuthenticationToken.TokenString')
  - [ToString()](#M-Facebook-Unity-AuthenticationToken-ToString 'Facebook.Unity.AuthenticationToken.ToString')
- [Canvas](#T-Facebook-Unity-FB-Canvas 'Facebook.Unity.FB.Canvas')
  - [Pay(product,action,quantity,quantityMin,quantityMax,requestId,pricepointId,testCurrency,callback)](#M-Facebook-Unity-FB-Canvas-Pay-System-String,System-String,System-Int32,System-Nullable{System-Int32},System-Nullable{System-Int32},System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}- 'Facebook.Unity.FB.Canvas.Pay(System.String,System.String,System.Int32,System.Nullable{System.Int32},System.Nullable{System.Int32},System.String,System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult})')
  - [PayWithProductId(productId,action,quantity,quantityMin,quantityMax,requestId,pricepointId,testCurrency,callback)](#M-Facebook-Unity-FB-Canvas-PayWithProductId-System-String,System-String,System-Int32,System-Nullable{System-Int32},System-Nullable{System-Int32},System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}- 'Facebook.Unity.FB.Canvas.PayWithProductId(System.String,System.String,System.Int32,System.Nullable{System.Int32},System.Nullable{System.Int32},System.String,System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult})')
  - [PayWithProductId(productId,action,developerPayload,testCurrency,callback)](#M-Facebook-Unity-FB-Canvas-PayWithProductId-System-String,System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}- 'Facebook.Unity.FB.Canvas.PayWithProductId(System.String,System.String,System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult})')
- [ComponentFactory](#T-Facebook-Unity-ComponentFactory 'Facebook.Unity.ComponentFactory')
  - [AddComponent\`\`1()](#M-Facebook-Unity-ComponentFactory-AddComponent``1 'Facebook.Unity.ComponentFactory.AddComponent``1')
  - [GetComponent\`\`1()](#M-Facebook-Unity-ComponentFactory-GetComponent``1-Facebook-Unity-ComponentFactory-IfNotExist- 'Facebook.Unity.ComponentFactory.GetComponent``1(Facebook.Unity.ComponentFactory.IfNotExist)')
- [Constants](#T-Facebook-Unity-Constants 'Facebook.Unity.Constants')
  - [GraphUrl](#P-Facebook-Unity-Constants-GraphUrl 'Facebook.Unity.Constants.GraphUrl')
  - [UnitySDKUserAgent](#P-Facebook-Unity-Constants-UnitySDKUserAgent 'Facebook.Unity.Constants.UnitySDKUserAgent')
  - [UnitySDKUserAgentSuffixLegacy](#P-Facebook-Unity-Constants-UnitySDKUserAgentSuffixLegacy 'Facebook.Unity.Constants.UnitySDKUserAgentSuffixLegacy')
- [CurrencyAmount](#T-Facebook-Unity-CurrencyAmount 'Facebook.Unity.CurrencyAmount')
  - [#ctor(amount,currency)](#M-Facebook-Unity-CurrencyAmount-#ctor-System-String,System-String- 'Facebook.Unity.CurrencyAmount.#ctor(System.String,System.String)')
  - [Amount](#P-Facebook-Unity-CurrencyAmount-Amount 'Facebook.Unity.CurrencyAmount.Amount')
  - [Currency](#P-Facebook-Unity-CurrencyAmount-Currency 'Facebook.Unity.CurrencyAmount.Currency')
  - [ToString()](#M-Facebook-Unity-CurrencyAmount-ToString 'Facebook.Unity.CurrencyAmount.ToString')
- [CustomUpdateContent](#T-Facebook-Unity-CustomUpdateContent 'Facebook.Unity.CustomUpdateContent')
- [CustomUpdateContentBuilder](#T-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder')
  - [#ctor(contextTokenId,text,image)](#M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-#ctor-System-String,Facebook-Unity-CustomUpdateLocalizedText,UnityEngine-Texture2D- 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder.#ctor(System.String,Facebook.Unity.CustomUpdateLocalizedText,UnityEngine.Texture2D)')
  - [#ctor(contextTokenId,text,media)](#M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-#ctor-System-String,Facebook-Unity-CustomUpdateLocalizedText,Facebook-Unity-CustomUpdateMedia- 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder.#ctor(System.String,Facebook.Unity.CustomUpdateLocalizedText,Facebook.Unity.CustomUpdateMedia)')
  - [build()](#M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-build 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder.build')
  - [setCTA(cta)](#M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-setCTA-Facebook-Unity-CustomUpdateLocalizedText- 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder.setCTA(Facebook.Unity.CustomUpdateLocalizedText)')
  - [setData(data)](#M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-setData-System-String- 'Facebook.Unity.CustomUpdateContent.CustomUpdateContentBuilder.setData(System.String)')
- [CustomUpdateLocalizedText](#T-Facebook-Unity-CustomUpdateLocalizedText 'Facebook.Unity.CustomUpdateLocalizedText')
  - [#ctor(defaultText,localizations)](#M-Facebook-Unity-CustomUpdateLocalizedText-#ctor-System-String,System-Collections-Generic-IDictionary{System-String,System-String}- 'Facebook.Unity.CustomUpdateLocalizedText.#ctor(System.String,System.Collections.Generic.IDictionary{System.String,System.String})')
- [CustomUpdateMedia](#T-Facebook-Unity-CustomUpdateMedia 'Facebook.Unity.CustomUpdateMedia')
  - [#ctor(gif,video)](#M-Facebook-Unity-CustomUpdateMedia-#ctor-Facebook-Unity-CustomUpdateMediaInfo,Facebook-Unity-CustomUpdateMediaInfo- 'Facebook.Unity.CustomUpdateMedia.#ctor(Facebook.Unity.CustomUpdateMediaInfo,Facebook.Unity.CustomUpdateMediaInfo)')
- [CustomUpdateMediaInfo](#T-Facebook-Unity-CustomUpdateMediaInfo 'Facebook.Unity.CustomUpdateMediaInfo')
- [FB](#T-Facebook-Unity-FB 'Facebook.Unity.FB')
  - [AppId](#P-Facebook-Unity-FB-AppId 'Facebook.Unity.FB.AppId')
  - [ClientToken](#P-Facebook-Unity-FB-ClientToken 'Facebook.Unity.FB.ClientToken')
  - [GraphApiVersion](#P-Facebook-Unity-FB-GraphApiVersion 'Facebook.Unity.FB.GraphApiVersion')
  - [IsInitialized](#P-Facebook-Unity-FB-IsInitialized 'Facebook.Unity.FB.IsInitialized')
  - [IsLoggedIn](#P-Facebook-Unity-FB-IsLoggedIn 'Facebook.Unity.FB.IsLoggedIn')
  - [LimitAppEventUsage](#P-Facebook-Unity-FB-LimitAppEventUsage 'Facebook.Unity.FB.LimitAppEventUsage')
  - [API(query,method,callback,formData)](#M-Facebook-Unity-FB-API-System-String,Facebook-Unity-HttpMethod,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult},System-Collections-Generic-IDictionary{System-String,System-String}- 'Facebook.Unity.FB.API(System.String,Facebook.Unity.HttpMethod,Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult},System.Collections.Generic.IDictionary{System.String,System.String})')
  - [API(query,method,callback,formData)](#M-Facebook-Unity-FB-API-System-String,Facebook-Unity-HttpMethod,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult},UnityEngine-WWWForm- 'Facebook.Unity.FB.API(System.String,Facebook.Unity.HttpMethod,Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult},UnityEngine.WWWForm)')
  - [ActivateApp()](#M-Facebook-Unity-FB-ActivateApp 'Facebook.Unity.FB.ActivateApp')
  - [AppRequest(message,actionType,objectId,to,data,title,callback)](#M-Facebook-Unity-FB-AppRequest-System-String,Facebook-Unity-OGActionType,System-String,System-Collections-Generic-IEnumerable{System-String},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}- 'Facebook.Unity.FB.AppRequest(System.String,Facebook.Unity.OGActionType,System.String,System.Collections.Generic.IEnumerable{System.String},System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult})')
  - [AppRequest(message,actionType,objectId,filters,excludeIds,maxRecipients,data,title,callback)](#M-Facebook-Unity-FB-AppRequest-System-String,Facebook-Unity-OGActionType,System-String,System-Collections-Generic-IEnumerable{System-Object},System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-Int32},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}- 'Facebook.Unity.FB.AppRequest(System.String,Facebook.Unity.OGActionType,System.String,System.Collections.Generic.IEnumerable{System.Object},System.Collections.Generic.IEnumerable{System.String},System.Nullable{System.Int32},System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult})')
  - [AppRequest(message,to,filters,excludeIds,maxRecipients,data,title,callback)](#M-Facebook-Unity-FB-AppRequest-System-String,System-Collections-Generic-IEnumerable{System-String},System-Collections-Generic-IEnumerable{System-Object},System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-Int32},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}- 'Facebook.Unity.FB.AppRequest(System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEnumerable{System.Object},System.Collections.Generic.IEnumerable{System.String},System.Nullable{System.Int32},System.String,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult})')
  - [ClearAppLink()](#M-Facebook-Unity-FB-ClearAppLink 'Facebook.Unity.FB.ClearAppLink')
  - [FeedShare(toId,link,linkName,linkCaption,linkDescription,picture,mediaSource,callback)](#M-Facebook-Unity-FB-FeedShare-System-String,System-Uri,System-String,System-String,System-String,System-Uri,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult}- 'Facebook.Unity.FB.FeedShare(System.String,System.Uri,System.String,System.String,System.String,System.Uri,System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult})')
  - [GetAppLink(callback)](#M-Facebook-Unity-FB-GetAppLink-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult}- 'Facebook.Unity.FB.GetAppLink(Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult})')
  - [Init(onInitComplete,onHideUnity,authResponse)](#M-Facebook-Unity-FB-Init-Facebook-Unity-InitDelegate,Facebook-Unity-HideUnityDelegate,System-String- 'Facebook.Unity.FB.Init(Facebook.Unity.InitDelegate,Facebook.Unity.HideUnityDelegate,System.String)')
  - [Init(appId,clientToken,cookie,logging,status,xfbml,frictionlessRequests,authResponse,javascriptSDKLocale,onHideUnity,onInitComplete)](#M-Facebook-Unity-FB-Init-System-String,System-String,System-Boolean,System-Boolean,System-Boolean,System-Boolean,System-Boolean,System-String,System-String,Facebook-Unity-HideUnityDelegate,Facebook-Unity-InitDelegate- 'Facebook.Unity.FB.Init(System.String,System.String,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.String,System.String,Facebook.Unity.HideUnityDelegate,Facebook.Unity.InitDelegate)')
  - [LogAppEvent(logEvent,valueToSum,parameters)](#M-Facebook-Unity-FB-LogAppEvent-System-String,System-Nullable{System-Single},System-Collections-Generic-Dictionary{System-String,System-Object}- 'Facebook.Unity.FB.LogAppEvent(System.String,System.Nullable{System.Single},System.Collections.Generic.Dictionary{System.String,System.Object})')
  - [LogInWithPublishPermissions(permissions,callback)](#M-Facebook-Unity-FB-LogInWithPublishPermissions-System-Collections-Generic-IEnumerable{System-String},Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}- 'Facebook.Unity.FB.LogInWithPublishPermissions(System.Collections.Generic.IEnumerable{System.String},Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult})')
  - [LogInWithReadPermissions(permissions,callback)](#M-Facebook-Unity-FB-LogInWithReadPermissions-System-Collections-Generic-IEnumerable{System-String},Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}- 'Facebook.Unity.FB.LogInWithReadPermissions(System.Collections.Generic.IEnumerable{System.String},Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult})')
  - [LogOut()](#M-Facebook-Unity-FB-LogOut 'Facebook.Unity.FB.LogOut')
  - [LogPurchase(logPurchase,currency,parameters)](#M-Facebook-Unity-FB-LogPurchase-System-Decimal,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}- 'Facebook.Unity.FB.LogPurchase(System.Decimal,System.String,System.Collections.Generic.Dictionary{System.String,System.Object})')
  - [LogPurchase(logPurchase,currency,parameters)](#M-Facebook-Unity-FB-LogPurchase-System-Single,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}- 'Facebook.Unity.FB.LogPurchase(System.Single,System.String,System.Collections.Generic.Dictionary{System.String,System.Object})')
  - [ShareLink(contentURL,contentTitle,contentDescription,photoURL,callback)](#M-Facebook-Unity-FB-ShareLink-System-Uri,System-String,System-String,System-Uri,Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult}- 'Facebook.Unity.FB.ShareLink(System.Uri,System.String,System.String,System.Uri,Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult})')
- [FBGamingServices](#T-Facebook-Unity-FBGamingServices 'Facebook.Unity.FBGamingServices')
  - [OpenFriendFinderDialog(callback)](#M-Facebook-Unity-FBGamingServices-OpenFriendFinderDialog-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGamingServicesFriendFinderResult}- 'Facebook.Unity.FBGamingServices.OpenFriendFinderDialog(Facebook.Unity.FacebookDelegate{Facebook.Unity.IGamingServicesFriendFinderResult})')
  - [PerformCustomUpdate(content,callback)](#M-Facebook-Unity-FBGamingServices-PerformCustomUpdate-Facebook-Unity-CustomUpdateContent,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult}- 'Facebook.Unity.FBGamingServices.PerformCustomUpdate(Facebook.Unity.CustomUpdateContent,Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult})')
  - [UploadImageToMediaLibrary(caption,imageUri,shouldLaunchMediaDialog,callback)](#M-Facebook-Unity-FBGamingServices-UploadImageToMediaLibrary-System-String,System-Uri,System-Boolean,Facebook-Unity-FacebookDelegate{Facebook-Unity-IMediaUploadResult}- 'Facebook.Unity.FBGamingServices.UploadImageToMediaLibrary(System.String,System.Uri,System.Boolean,Facebook.Unity.FacebookDelegate{Facebook.Unity.IMediaUploadResult})')
  - [UploadVideoToMediaLibrary(caption,videoUri,callback)](#M-Facebook-Unity-FBGamingServices-UploadVideoToMediaLibrary-System-String,System-Uri,System-Boolean,Facebook-Unity-FacebookDelegate{Facebook-Unity-IMediaUploadResult}- 'Facebook.Unity.FBGamingServices.UploadVideoToMediaLibrary(System.String,System.Uri,System.Boolean,Facebook.Unity.FacebookDelegate{Facebook.Unity.IMediaUploadResult})')
- [FBInsightsFlushBehavior](#T-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior 'Facebook.Unity.Mobile.IOS.IOSFacebook.FBInsightsFlushBehavior')
  - [FBInsightsFlushBehaviorAuto](#F-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior-FBInsightsFlushBehaviorAuto 'Facebook.Unity.Mobile.IOS.IOSFacebook.FBInsightsFlushBehavior.FBInsightsFlushBehaviorAuto')
  - [FBInsightsFlushBehaviorExplicitOnly](#F-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior-FBInsightsFlushBehaviorExplicitOnly 'Facebook.Unity.Mobile.IOS.IOSFacebook.FBInsightsFlushBehavior.FBInsightsFlushBehaviorExplicitOnly')
- [FBLocation](#T-Facebook-Unity-FBLocation 'Facebook.Unity.FBLocation')
  - [ID](#P-Facebook-Unity-FBLocation-ID 'Facebook.Unity.FBLocation.ID')
  - [Name](#P-Facebook-Unity-FBLocation-Name 'Facebook.Unity.FBLocation.Name')
  - [ToString()](#M-Facebook-Unity-FBLocation-ToString 'Facebook.Unity.FBLocation.ToString')
- [FacebookDelegate\`1](#T-Facebook-Unity-FacebookDelegate`1 'Facebook.Unity.FacebookDelegate`1')
- [FacebookSdkVersion](#T-Facebook-Unity-FacebookSdkVersion 'Facebook.Unity.FacebookSdkVersion')
  - [Build](#P-Facebook-Unity-FacebookSdkVersion-Build 'Facebook.Unity.FacebookSdkVersion.Build')
- [FriendFinderInviation](#T-Facebook-Unity-FriendFinderInviation 'Facebook.Unity.FriendFinderInviation')
- [HideUnityDelegate](#T-Facebook-Unity-HideUnityDelegate 'Facebook.Unity.HideUnityDelegate')
- [HttpMethod](#T-Facebook-Unity-HttpMethod 'Facebook.Unity.HttpMethod')
  - [DELETE](#F-Facebook-Unity-HttpMethod-DELETE 'Facebook.Unity.HttpMethod.DELETE')
  - [GET](#F-Facebook-Unity-HttpMethod-GET 'Facebook.Unity.HttpMethod.GET')
  - [POST](#F-Facebook-Unity-HttpMethod-POST 'Facebook.Unity.HttpMethod.POST')
- [IAccessTokenRefreshResult](#T-Facebook-Unity-IAccessTokenRefreshResult 'Facebook.Unity.IAccessTokenRefreshResult')
  - [AccessToken](#P-Facebook-Unity-IAccessTokenRefreshResult-AccessToken 'Facebook.Unity.IAccessTokenRefreshResult.AccessToken')
- [IAppLinkResult](#T-Facebook-Unity-IAppLinkResult 'Facebook.Unity.IAppLinkResult')
  - [Extras](#P-Facebook-Unity-IAppLinkResult-Extras 'Facebook.Unity.IAppLinkResult.Extras')
  - [Ref](#P-Facebook-Unity-IAppLinkResult-Ref 'Facebook.Unity.IAppLinkResult.Ref')
  - [TargetUrl](#P-Facebook-Unity-IAppLinkResult-TargetUrl 'Facebook.Unity.IAppLinkResult.TargetUrl')
  - [Url](#P-Facebook-Unity-IAppLinkResult-Url 'Facebook.Unity.IAppLinkResult.Url')
- [IAppRequestResult](#T-Facebook-Unity-IAppRequestResult 'Facebook.Unity.IAppRequestResult')
  - [RequestID](#P-Facebook-Unity-IAppRequestResult-RequestID 'Facebook.Unity.IAppRequestResult.RequestID')
  - [To](#P-Facebook-Unity-IAppRequestResult-To 'Facebook.Unity.IAppRequestResult.To')
- [ICatalogResult](#T-Facebook-Unity-ICatalogResult 'Facebook.Unity.ICatalogResult')
  - [Products](#P-Facebook-Unity-ICatalogResult-Products 'Facebook.Unity.ICatalogResult.Products')
- [IChooseGamingContextResult](#T-Facebook-Unity-IChooseGamingContextResult 'Facebook.Unity.IChooseGamingContextResult')
- [ICreateGamingContextResult](#T-Facebook-Unity-ICreateGamingContextResult 'Facebook.Unity.ICreateGamingContextResult')
- [IGamingServicesFriendFinderResult](#T-Facebook-Unity-IGamingServicesFriendFinderResult 'Facebook.Unity.IGamingServicesFriendFinderResult')
- [IGetCurrentGamingContextResult](#T-Facebook-Unity-IGetCurrentGamingContextResult 'Facebook.Unity.IGetCurrentGamingContextResult')
  - [ContextId](#P-Facebook-Unity-IGetCurrentGamingContextResult-ContextId 'Facebook.Unity.IGetCurrentGamingContextResult.ContextId')
- [IGraphResult](#T-Facebook-Unity-IGraphResult 'Facebook.Unity.IGraphResult')
  - [ResultList](#P-Facebook-Unity-IGraphResult-ResultList 'Facebook.Unity.IGraphResult.ResultList')
  - [Texture](#P-Facebook-Unity-IGraphResult-Texture 'Facebook.Unity.IGraphResult.Texture')
- [IGroupCreateResult](#T-Facebook-Unity-IGroupCreateResult 'Facebook.Unity.IGroupCreateResult')
  - [GroupId](#P-Facebook-Unity-IGroupCreateResult-GroupId 'Facebook.Unity.IGroupCreateResult.GroupId')
- [IGroupJoinResult](#T-Facebook-Unity-IGroupJoinResult 'Facebook.Unity.IGroupJoinResult')
- [IHasLicenseResult](#T-Facebook-Unity-IHasLicenseResult 'Facebook.Unity.IHasLicenseResult')
  - [HasLicense](#P-Facebook-Unity-IHasLicenseResult-HasLicense 'Facebook.Unity.IHasLicenseResult.HasLicense')
- [IInternalResult](#T-Facebook-Unity-IInternalResult 'Facebook.Unity.IInternalResult')
  - [CallbackId](#P-Facebook-Unity-IInternalResult-CallbackId 'Facebook.Unity.IInternalResult.CallbackId')
- [ILoginResult](#T-Facebook-Unity-ILoginResult 'Facebook.Unity.ILoginResult')
  - [AccessToken](#P-Facebook-Unity-ILoginResult-AccessToken 'Facebook.Unity.ILoginResult.AccessToken')
  - [AuthenticationToken](#P-Facebook-Unity-ILoginResult-AuthenticationToken 'Facebook.Unity.ILoginResult.AuthenticationToken')
- [ILoginStatusResult](#T-Facebook-Unity-ILoginStatusResult 'Facebook.Unity.ILoginStatusResult')
  - [Failed](#P-Facebook-Unity-ILoginStatusResult-Failed 'Facebook.Unity.ILoginStatusResult.Failed')
- [IMediaUploadResult](#T-Facebook-Unity-IMediaUploadResult 'Facebook.Unity.IMediaUploadResult')
  - [MediaId](#P-Facebook-Unity-IMediaUploadResult-MediaId 'Facebook.Unity.IMediaUploadResult.MediaId')
- [IPayResult](#T-Facebook-Unity-IPayResult 'Facebook.Unity.IPayResult')
  - [ErrorCode](#P-Facebook-Unity-IPayResult-ErrorCode 'Facebook.Unity.IPayResult.ErrorCode')
- [IPayloadResult](#T-Facebook-Unity-IPayloadResult 'Facebook.Unity.IPayloadResult')
  - [Payload](#P-Facebook-Unity-IPayloadResult-Payload 'Facebook.Unity.IPayloadResult.Payload')
- [IProfileResult](#T-Facebook-Unity-IProfileResult 'Facebook.Unity.IProfileResult')
  - [CurrentProfile](#P-Facebook-Unity-IProfileResult-CurrentProfile 'Facebook.Unity.IProfileResult.CurrentProfile')
- [IPurchaseResult](#T-Facebook-Unity-IPurchaseResult 'Facebook.Unity.IPurchaseResult')
  - [Purchase](#P-Facebook-Unity-IPurchaseResult-Purchase 'Facebook.Unity.IPurchaseResult.Purchase')
- [IPurchasesResult](#T-Facebook-Unity-IPurchasesResult 'Facebook.Unity.IPurchasesResult')
  - [Purchases](#P-Facebook-Unity-IPurchasesResult-Purchases 'Facebook.Unity.IPurchasesResult.Purchases')
- [IResult](#T-Facebook-Unity-IResult 'Facebook.Unity.IResult')
  - [Cancelled](#P-Facebook-Unity-IResult-Cancelled 'Facebook.Unity.IResult.Cancelled')
  - [Error](#P-Facebook-Unity-IResult-Error 'Facebook.Unity.IResult.Error')
  - [ErrorDictionary](#P-Facebook-Unity-IResult-ErrorDictionary 'Facebook.Unity.IResult.ErrorDictionary')
  - [RawResult](#P-Facebook-Unity-IResult-RawResult 'Facebook.Unity.IResult.RawResult')
  - [ResultDictionary](#P-Facebook-Unity-IResult-ResultDictionary 'Facebook.Unity.IResult.ResultDictionary')
- [ISessionScoreResult](#T-Facebook-Unity-ISessionScoreResult 'Facebook.Unity.ISessionScoreResult')
- [IShareResult](#T-Facebook-Unity-IShareResult 'Facebook.Unity.IShareResult')
  - [PostId](#P-Facebook-Unity-IShareResult-PostId 'Facebook.Unity.IShareResult.PostId')
- [ISwitchGamingContextResult](#T-Facebook-Unity-ISwitchGamingContextResult 'Facebook.Unity.ISwitchGamingContextResult')
- [ITournamentResult](#T-Facebook-Unity-ITournamentResult 'Facebook.Unity.ITournamentResult')
- [ITournamentScoreResult](#T-Facebook-Unity-ITournamentScoreResult 'Facebook.Unity.ITournamentScoreResult')
- [InitDelegate](#T-Facebook-Unity-InitDelegate 'Facebook.Unity.InitDelegate')
- [Json](#T-Facebook-MiniJSON-Json 'Facebook.MiniJSON.Json')
  - [Deserialize(json)](#M-Facebook-MiniJSON-Json-Deserialize-System-String- 'Facebook.MiniJSON.Json.Deserialize(System.String)')
  - [Serialize(obj)](#M-Facebook-MiniJSON-Json-Serialize-System-Object- 'Facebook.MiniJSON.Json.Serialize(System.Object)')
- [Mobile](#T-Facebook-Unity-FB-Mobile 'Facebook.Unity.FB.Mobile')
  - [ShareDialogMode](#P-Facebook-Unity-FB-Mobile-ShareDialogMode 'Facebook.Unity.FB.Mobile.ShareDialogMode')
  - [UserID](#P-Facebook-Unity-FB-Mobile-UserID 'Facebook.Unity.FB.Mobile.UserID')
  - [CurrentAuthenticationToken()](#M-Facebook-Unity-FB-Mobile-CurrentAuthenticationToken 'Facebook.Unity.FB.Mobile.CurrentAuthenticationToken')
  - [CurrentProfile()](#M-Facebook-Unity-FB-Mobile-CurrentProfile 'Facebook.Unity.FB.Mobile.CurrentProfile')
  - [CurrentProfile()](#M-Facebook-Unity-FB-Mobile-CurrentProfile-Facebook-Unity-FacebookDelegate{Facebook-Unity-IProfileResult}- 'Facebook.Unity.FB.Mobile.CurrentProfile(Facebook.Unity.FacebookDelegate{Facebook.Unity.IProfileResult})')
  - [EnableProfileUpdatesOnAccessTokenChange()](#M-Facebook-Unity-FB-Mobile-EnableProfileUpdatesOnAccessTokenChange-System-Boolean- 'Facebook.Unity.FB.Mobile.EnableProfileUpdatesOnAccessTokenChange(System.Boolean)')
  - [FetchDeferredAppLinkData(callback)](#M-Facebook-Unity-FB-Mobile-FetchDeferredAppLinkData-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult}- 'Facebook.Unity.FB.Mobile.FetchDeferredAppLinkData(Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult})')
  - [IsImplicitPurchaseLoggingEnabled()](#M-Facebook-Unity-FB-Mobile-IsImplicitPurchaseLoggingEnabled 'Facebook.Unity.FB.Mobile.IsImplicitPurchaseLoggingEnabled')
  - [LoginWithTrackingPreference(loginTracking,permissions,nonce,callback)](#M-Facebook-Unity-FB-Mobile-LoginWithTrackingPreference-Facebook-Unity-LoginTracking,System-Collections-Generic-IEnumerable{System-String},System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}- 'Facebook.Unity.FB.Mobile.LoginWithTrackingPreference(Facebook.Unity.LoginTracking,System.Collections.Generic.IEnumerable{System.String},System.String,Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult})')
  - [RefreshCurrentAccessToken(callback)](#M-Facebook-Unity-FB-Mobile-RefreshCurrentAccessToken-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAccessTokenRefreshResult}- 'Facebook.Unity.FB.Mobile.RefreshCurrentAccessToken(Facebook.Unity.FacebookDelegate{Facebook.Unity.IAccessTokenRefreshResult})')
  - [SetAdvertiserIDCollectionEnabled(advertiserIDCollectionEnabled)](#M-Facebook-Unity-FB-Mobile-SetAdvertiserIDCollectionEnabled-System-Boolean- 'Facebook.Unity.FB.Mobile.SetAdvertiserIDCollectionEnabled(System.Boolean)')
  - [SetAdvertiserTrackingEnabled(advertiserTrackingEnabled)](#M-Facebook-Unity-FB-Mobile-SetAdvertiserTrackingEnabled-System-Boolean- 'Facebook.Unity.FB.Mobile.SetAdvertiserTrackingEnabled(System.Boolean)')
  - [SetAutoLogAppEventsEnabled(autoLogAppEventsEnabled)](#M-Facebook-Unity-FB-Mobile-SetAutoLogAppEventsEnabled-System-Boolean- 'Facebook.Unity.FB.Mobile.SetAutoLogAppEventsEnabled(System.Boolean)')
  - [SetPushNotificationsDeviceTokenString(token)](#M-Facebook-Unity-FB-Mobile-SetPushNotificationsDeviceTokenString-System-String- 'Facebook.Unity.FB.Mobile.SetPushNotificationsDeviceTokenString(System.String)')
- [MobileFacebook](#T-Facebook-Unity-Mobile-MobileFacebook 'Facebook.Unity.Mobile.MobileFacebook')
  - [ShareDialogMode](#P-Facebook-Unity-Mobile-MobileFacebook-ShareDialogMode 'Facebook.Unity.Mobile.MobileFacebook.ShareDialogMode')
- [OGActionType](#T-Facebook-Unity-OGActionType 'Facebook.Unity.OGActionType')
  - [ASKFOR](#F-Facebook-Unity-OGActionType-ASKFOR 'Facebook.Unity.OGActionType.ASKFOR')
  - [SEND](#F-Facebook-Unity-OGActionType-SEND 'Facebook.Unity.OGActionType.SEND')
  - [TURN](#F-Facebook-Unity-OGActionType-TURN 'Facebook.Unity.OGActionType.TURN')
- [Product](#T-Facebook-Unity-Product 'Facebook.Unity.Product')
  - [#ctor(title,productID,description,imageURI,price,priceAmount,priceCurrencyCode)](#M-Facebook-Unity-Product-#ctor-System-String,System-String,System-String,System-String,System-String,System-Nullable{System-Double},System-String- 'Facebook.Unity.Product.#ctor(System.String,System.String,System.String,System.String,System.String,System.Nullable{System.Double},System.String)')
  - [Description](#P-Facebook-Unity-Product-Description 'Facebook.Unity.Product.Description')
  - [ImageURI](#P-Facebook-Unity-Product-ImageURI 'Facebook.Unity.Product.ImageURI')
  - [Price](#P-Facebook-Unity-Product-Price 'Facebook.Unity.Product.Price')
  - [PriceAmount](#P-Facebook-Unity-Product-PriceAmount 'Facebook.Unity.Product.PriceAmount')
  - [PriceCurrencyCode](#P-Facebook-Unity-Product-PriceCurrencyCode 'Facebook.Unity.Product.PriceCurrencyCode')
  - [ProductID](#P-Facebook-Unity-Product-ProductID 'Facebook.Unity.Product.ProductID')
  - [Title](#P-Facebook-Unity-Product-Title 'Facebook.Unity.Product.Title')
  - [ToString()](#M-Facebook-Unity-Product-ToString 'Facebook.Unity.Product.ToString')
- [Profile](#T-Facebook-Unity-Profile 'Facebook.Unity.Profile')
  - [#ctor(userID,firstName,middleName,lastName,name,email,imageURL,linkURL,friendIDs,birthday,ageRange,hometown,location,gender)](#M-Facebook-Unity-Profile-#ctor-System-String,System-String,System-String,System-String,System-String,System-String,System-String,System-String,System-String[],System-String,Facebook-Unity-UserAgeRange,Facebook-Unity-FBLocation,Facebook-Unity-FBLocation,System-String- 'Facebook.Unity.Profile.#ctor(System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String[],System.String,Facebook.Unity.UserAgeRange,Facebook.Unity.FBLocation,Facebook.Unity.FBLocation,System.String)')
  - [AgeRange](#P-Facebook-Unity-Profile-AgeRange 'Facebook.Unity.Profile.AgeRange')
  - [Birthday](#P-Facebook-Unity-Profile-Birthday 'Facebook.Unity.Profile.Birthday')
  - [Email](#P-Facebook-Unity-Profile-Email 'Facebook.Unity.Profile.Email')
  - [FirstName](#P-Facebook-Unity-Profile-FirstName 'Facebook.Unity.Profile.FirstName')
  - [FriendIDs](#P-Facebook-Unity-Profile-FriendIDs 'Facebook.Unity.Profile.FriendIDs')
  - [Gender](#P-Facebook-Unity-Profile-Gender 'Facebook.Unity.Profile.Gender')
  - [Hometown](#P-Facebook-Unity-Profile-Hometown 'Facebook.Unity.Profile.Hometown')
  - [ImageURL](#P-Facebook-Unity-Profile-ImageURL 'Facebook.Unity.Profile.ImageURL')
  - [LastName](#P-Facebook-Unity-Profile-LastName 'Facebook.Unity.Profile.LastName')
  - [LinkURL](#P-Facebook-Unity-Profile-LinkURL 'Facebook.Unity.Profile.LinkURL')
  - [Location](#P-Facebook-Unity-Profile-Location 'Facebook.Unity.Profile.Location')
  - [MiddleName](#P-Facebook-Unity-Profile-MiddleName 'Facebook.Unity.Profile.MiddleName')
  - [Name](#P-Facebook-Unity-Profile-Name 'Facebook.Unity.Profile.Name')
  - [UserID](#P-Facebook-Unity-Profile-UserID 'Facebook.Unity.Profile.UserID')
  - [ToString()](#M-Facebook-Unity-Profile-ToString 'Facebook.Unity.Profile.ToString')
- [Purchase](#T-Facebook-Unity-Purchase 'Facebook.Unity.Purchase')
  - [#ctor(developerPayload,isConsumed,paymentActionType,paymentID,productID,purchasePlatform,purchasePrice,purchaseTime,purchaseToken,signedRequest)](#M-Facebook-Unity-Purchase-#ctor-System-String,System-Boolean,System-String,System-String,System-String,System-String,System-Collections-Generic-IDictionary{System-String,System-Object},System-Int64,System-String,System-String- 'Facebook.Unity.Purchase.#ctor(System.String,System.Boolean,System.String,System.String,System.String,System.String,System.Collections.Generic.IDictionary{System.String,System.Object},System.Int64,System.String,System.String)')
  - [DeveloperPayload](#P-Facebook-Unity-Purchase-DeveloperPayload 'Facebook.Unity.Purchase.DeveloperPayload')
  - [IsConsumed](#P-Facebook-Unity-Purchase-IsConsumed 'Facebook.Unity.Purchase.IsConsumed')
  - [PaymentActionType](#P-Facebook-Unity-Purchase-PaymentActionType 'Facebook.Unity.Purchase.PaymentActionType')
  - [PaymentID](#P-Facebook-Unity-Purchase-PaymentID 'Facebook.Unity.Purchase.PaymentID')
  - [ProductID](#P-Facebook-Unity-Purchase-ProductID 'Facebook.Unity.Purchase.ProductID')
  - [PurchasePlatform](#P-Facebook-Unity-Purchase-PurchasePlatform 'Facebook.Unity.Purchase.PurchasePlatform')
  - [PurchasePrice](#P-Facebook-Unity-Purchase-PurchasePrice 'Facebook.Unity.Purchase.PurchasePrice')
  - [PurchaseTime](#P-Facebook-Unity-Purchase-PurchaseTime 'Facebook.Unity.Purchase.PurchaseTime')
  - [PurchaseToken](#P-Facebook-Unity-Purchase-PurchaseToken 'Facebook.Unity.Purchase.PurchaseToken')
  - [SignedRequest](#P-Facebook-Unity-Purchase-SignedRequest 'Facebook.Unity.Purchase.SignedRequest')
  - [ToString()](#M-Facebook-Unity-Purchase-ToString 'Facebook.Unity.Purchase.ToString')
- [ShareDialogMode](#T-Facebook-Unity-ShareDialogMode 'Facebook.Unity.ShareDialogMode')
  - [AUTOMATIC](#F-Facebook-Unity-ShareDialogMode-AUTOMATIC 'Facebook.Unity.ShareDialogMode.AUTOMATIC')
  - [FEED](#F-Facebook-Unity-ShareDialogMode-FEED 'Facebook.Unity.ShareDialogMode.FEED')
  - [NATIVE](#F-Facebook-Unity-ShareDialogMode-NATIVE 'Facebook.Unity.ShareDialogMode.NATIVE')
  - [WEB](#F-Facebook-Unity-ShareDialogMode-WEB 'Facebook.Unity.ShareDialogMode.WEB')
- [UserAgeRange](#T-Facebook-Unity-UserAgeRange 'Facebook.Unity.UserAgeRange')
  - [Max](#P-Facebook-Unity-UserAgeRange-Max 'Facebook.Unity.UserAgeRange.Max')
  - [Min](#P-Facebook-Unity-UserAgeRange-Min 'Facebook.Unity.UserAgeRange.Min')
  - [ToString()](#M-Facebook-Unity-UserAgeRange-ToString 'Facebook.Unity.UserAgeRange.ToString')

<a name='T-Facebook-Unity-AccessToken'></a>
## AccessToken `type`

##### Namespace

Facebook.Unity

##### Summary

Contains the access token and related information.

<a name='M-Facebook-Unity-AccessToken-#ctor-System-String,System-String,System-DateTime,System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-DateTime},System-String-'></a>
### #ctor(tokenString,userId,expirationTime,permissions,lastRefresh) `constructor`

##### Summary

Initializes a new instance of the [AccessToken](#T-Facebook-Unity-AccessToken 'Facebook.Unity.AccessToken') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Token string of the token. |
| userId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | User identifier of the token. |
| expirationTime | [System.DateTime](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.DateTime 'System.DateTime') | Expiration time of the token. |
| permissions | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | Permissions of the token. |
| lastRefresh | [System.Nullable{System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.DateTime}') | Last Refresh time of token. |

<a name='P-Facebook-Unity-AccessToken-CurrentAccessToken'></a>
### CurrentAccessToken `property`

##### Summary

Gets the current access token.

<a name='P-Facebook-Unity-AccessToken-ExpirationTime'></a>
### ExpirationTime `property`

##### Summary

Gets the expiration time.

<a name='P-Facebook-Unity-AccessToken-GraphDomain'></a>
### GraphDomain `property`

##### Summary

Gets the domain this access token is valid for.

<a name='P-Facebook-Unity-AccessToken-LastRefresh'></a>
### LastRefresh `property`

##### Summary

Gets the last refresh.

<a name='P-Facebook-Unity-AccessToken-Permissions'></a>
### Permissions `property`

##### Summary

Gets the list of permissions.

<a name='P-Facebook-Unity-AccessToken-TokenString'></a>
### TokenString `property`

##### Summary

Gets the token string.

<a name='P-Facebook-Unity-AccessToken-UserId'></a>
### UserId `property`

##### Summary

Gets the user identifier.

<a name='M-Facebook-Unity-AccessToken-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [AccessToken](#T-Facebook-Unity-AccessToken 'Facebook.Unity.AccessToken').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [AccessToken](#T-Facebook-Unity-AccessToken 'Facebook.Unity.AccessToken').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-FB-Android'></a>
## Android `type`

##### Namespace

Facebook.Unity.FB

##### Summary

Contains code specific to the Android Platform.

<a name='P-Facebook-Unity-FB-Android-KeyHash'></a>
### KeyHash `property`

##### Summary

Gets the key hash.

<a name='M-Facebook-Unity-FB-Android-RetrieveLoginStatus-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginStatusResult}-'></a>
### RetrieveLoginStatus(callback) `method`

##### Summary

Retrieves the login status for the user. This will return an access token for the app if a user
is logged into the Facebook for Android app on the same device and that user had previously
logged into the app.If an access token was retrieved then a toast will be shown telling the
user that they have been logged in.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginStatusResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginStatusResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginStatusResult}') | The callback to be called when the request completes |

<a name='T-Facebook-Unity-AppEventName'></a>
## AppEventName `type`

##### Namespace

Facebook.Unity

##### Summary

Contains the names used for standard App Events.

<a name='F-Facebook-Unity-AppEventName-AchievedLevel'></a>
### AchievedLevel `constants`

##### Summary

App Event for achieved level.

<a name='F-Facebook-Unity-AppEventName-ActivatedApp'></a>
### ActivatedApp `constants`

##### Summary

App Event for  activated app.

<a name='F-Facebook-Unity-AppEventName-AddedPaymentInfo'></a>
### AddedPaymentInfo `constants`

##### Summary

App Event for  added payment info.

<a name='F-Facebook-Unity-AppEventName-AddedToCart'></a>
### AddedToCart `constants`

##### Summary

App Event for  added to cart.

<a name='F-Facebook-Unity-AppEventName-AddedToWishlist'></a>
### AddedToWishlist `constants`

##### Summary

App Event for  added to wishlist.

<a name='F-Facebook-Unity-AppEventName-CompletedRegistration'></a>
### CompletedRegistration `constants`

##### Summary

App Event for  completed registration.

<a name='F-Facebook-Unity-AppEventName-CompletedTutorial'></a>
### CompletedTutorial `constants`

##### Summary

App Event for  completed tutorial.

<a name='F-Facebook-Unity-AppEventName-InitiatedCheckout'></a>
### InitiatedCheckout `constants`

##### Summary

App Event for  initiated checkout.

<a name='F-Facebook-Unity-AppEventName-Purchased'></a>
### Purchased `constants`

##### Summary

App Event for  purchased.

<a name='F-Facebook-Unity-AppEventName-Rated'></a>
### Rated `constants`

##### Summary

App Event for  rated.

<a name='F-Facebook-Unity-AppEventName-Searched'></a>
### Searched `constants`

##### Summary

App Event for  searched.

<a name='F-Facebook-Unity-AppEventName-SpentCredits'></a>
### SpentCredits `constants`

##### Summary

App Event for  spent credits.

<a name='F-Facebook-Unity-AppEventName-UnlockedAchievement'></a>
### UnlockedAchievement `constants`

##### Summary

App Event for  unlocked achievement.

<a name='F-Facebook-Unity-AppEventName-ViewedContent'></a>
### ViewedContent `constants`

##### Summary

App Event for  content of the viewed.

<a name='T-Facebook-Unity-AppEventParameterName'></a>
## AppEventParameterName `type`

##### Namespace

Facebook.Unity

##### Summary

Contains the parameter names used for standard App Events.

<a name='F-Facebook-Unity-AppEventParameterName-ContentID'></a>
### ContentID `constants`

##### Summary

App Event parameter for content ID.

<a name='F-Facebook-Unity-AppEventParameterName-ContentType'></a>
### ContentType `constants`

##### Summary

App Event parameter for type of the content.

<a name='F-Facebook-Unity-AppEventParameterName-Currency'></a>
### Currency `constants`

##### Summary

App Event parameter for currency.

<a name='F-Facebook-Unity-AppEventParameterName-Description'></a>
### Description `constants`

##### Summary

App Event parameter for description.

<a name='F-Facebook-Unity-AppEventParameterName-Level'></a>
### Level `constants`

##### Summary

App Event parameter for level.

<a name='F-Facebook-Unity-AppEventParameterName-MaxRatingValue'></a>
### MaxRatingValue `constants`

##### Summary

App Event parameter for max rating value.

<a name='F-Facebook-Unity-AppEventParameterName-NumItems'></a>
### NumItems `constants`

##### Summary

App Event parameter for number items.

<a name='F-Facebook-Unity-AppEventParameterName-PaymentInfoAvailable'></a>
### PaymentInfoAvailable `constants`

##### Summary

App Event parameter for payment info available.

<a name='F-Facebook-Unity-AppEventParameterName-RegistrationMethod'></a>
### RegistrationMethod `constants`

##### Summary

App Event parameter for registration method.

<a name='F-Facebook-Unity-AppEventParameterName-SearchString'></a>
### SearchString `constants`

##### Summary

App Event parameter for search string.

<a name='F-Facebook-Unity-AppEventParameterName-Success'></a>
### Success `constants`

##### Summary

App Event parameter for success.

<a name='T-Facebook-Unity-AuthenticationToken'></a>
## AuthenticationToken `type`

##### Namespace

Facebook.Unity

<a name='M-Facebook-Unity-AuthenticationToken-#ctor-System-String,System-String-'></a>
### #ctor(tokenString,nonce) `constructor`

##### Summary

Initializes a new instance of the [AuthenticationToken](#T-Facebook-Unity-AuthenticationToken 'Facebook.Unity.AuthenticationToken') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tokenString | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Token string of the token. |
| nonce | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Nonce of the token. |

<a name='P-Facebook-Unity-AuthenticationToken-Nonce'></a>
### Nonce `property`

##### Summary

Gets the nonce string.

<a name='P-Facebook-Unity-AuthenticationToken-TokenString'></a>
### TokenString `property`

##### Summary

Gets the token string.

<a name='M-Facebook-Unity-AuthenticationToken-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [AuthenticationToken](#T-Facebook-Unity-AuthenticationToken 'Facebook.Unity.AuthenticationToken').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [AuthenticationToken](#T-Facebook-Unity-AuthenticationToken 'Facebook.Unity.AuthenticationToken').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-FB-Canvas'></a>
## Canvas `type`

##### Namespace

Facebook.Unity.FB

##### Summary

Contains methods specific to the Facebook Games Canvas platform.

<a name='M-Facebook-Unity-FB-Canvas-Pay-System-String,System-String,System-Int32,System-Nullable{System-Int32},System-Nullable{System-Int32},System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}-'></a>
### Pay(product,action,quantity,quantityMin,quantityMax,requestId,pricepointId,testCurrency,callback) `method`

##### Summary

Pay the specified product, action, quantity, quantityMin, quantityMax, requestId, pricepointId,
testCurrency and callback.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| product | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The URL of your og:product object that the user is looking to purchase. |
| action | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Should always be purchaseitem. |
| quantity | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The amount of this item the user is looking to purchase - typically used when implementing a virtual currency purchase. |
| quantityMin | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The minimum quantity of the item the user is able to purchase.
This parameter is important when handling price jumping to maximize the efficiency of the transaction. |
| quantityMax | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The maximum quantity of the item the user is able to purchase.
This parameter is important when handling price jumping to maximize the efficiency of the transaction. |
| requestId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The developer defined unique identifier for this transaction, which becomes
attached to the payment within the Graph API. |
| pricepointId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Used to shortcut a mobile payer directly to the
mobile purchase flow at a given price point. |
| testCurrency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | This parameter can be used during debugging and testing your implementation to force the dialog to
use a specific currency rather than the current user's preferred currency. This allows you to
rapidly prototype your payment experience for different currencies without having to repeatedly
change your personal currency preference settings. This parameter is only available for admins,
developers and testers associated with the app, in order to minimize the security risk of a
malicious JavaScript injection. Provide the 3 letter currency code of the intended forced currency. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}') | The callback to use upon completion. |

<a name='M-Facebook-Unity-FB-Canvas-PayWithProductId-System-String,System-String,System-Int32,System-Nullable{System-Int32},System-Nullable{System-Int32},System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}-'></a>
### PayWithProductId(productId,action,quantity,quantityMin,quantityMax,requestId,pricepointId,testCurrency,callback) `method`

##### Summary

Pay the specified productId, action, quantity, quantityMin, quantityMax, requestId, pricepointId,
testCurrency and callback.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| productId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The product Id referencing the product managed by Facebook. |
| action | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Should always be purchaseiap. |
| quantity | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The amount of this item the user is looking to purchase - typically used when implementing a virtual currency purchase. |
| quantityMin | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The minimum quantity of the item the user is able to purchase.
This parameter is important when handling price jumping to maximize the efficiency of the transaction. |
| quantityMax | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The maximum quantity of the item the user is able to purchase.
This parameter is important when handling price jumping to maximize the efficiency of the transaction. |
| requestId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The developer defined unique identifier for this transaction, which becomes
attached to the payment within the Graph API. |
| pricepointId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Used to shortcut a mobile payer directly to the
mobile purchase flow at a given price point. |
| testCurrency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | This parameter can be used during debugging and testing your implementation to force the dialog to
use a specific currency rather than the current user's preferred currency. This allows you to
rapidly prototype your payment experience for different currencies without having to repeatedly
change your personal currency preference settings. This parameter is only available for admins,
developers and testers associated with the app, in order to minimize the security risk of a
malicious JavaScript injection. Provide the 3 letter currency code of the intended forced currency. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}') | The callback to use upon completion. |

<a name='M-Facebook-Unity-FB-Canvas-PayWithProductId-System-String,System-String,System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult}-'></a>
### PayWithProductId(productId,action,developerPayload,testCurrency,callback) `method`

##### Summary

Pay the specified productId, action, developerPayload, testCurrency and callback.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| productId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The product Id referencing the product managed by Facebook. |
| action | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Should always be purchaseiap. |
| developerPayload | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A string that can be used to provide supplemental information about an order. It can be
used to uniquely identify the purchase request. |
| testCurrency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | This parameter can be used during debugging and testing your implementation to force the dialog to
use a specific currency rather than the current user's preferred currency. This allows you to
rapidly prototype your payment experience for different currencies without having to repeatedly
change your personal currency preference settings. This parameter is only available for admins,
developers and testers associated with the app, in order to minimize the security risk of a
malicious JavaScript injection. Provide the 3 letter currency code of the intended forced currency. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IPayResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IPayResult}') | The callback to use upon completion. |

<a name='T-Facebook-Unity-ComponentFactory'></a>
## ComponentFactory `type`

##### Namespace

Facebook.Unity

<a name='M-Facebook-Unity-ComponentFactory-AddComponent``1'></a>
### AddComponent\`\`1() `method`

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-ComponentFactory-GetComponent``1-Facebook-Unity-ComponentFactory-IfNotExist-'></a>
### GetComponent\`\`1() `method`

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-Constants'></a>
## Constants `type`

##### Namespace

Facebook.Unity

<a name='P-Facebook-Unity-Constants-GraphUrl'></a>
### GraphUrl `property`

##### Summary

Gets the graph URL.

<a name='P-Facebook-Unity-Constants-UnitySDKUserAgent'></a>
### UnitySDKUserAgent `property`

##### Summary

Gets the Unity SDK user agent.

<a name='P-Facebook-Unity-Constants-UnitySDKUserAgentSuffixLegacy'></a>
### UnitySDKUserAgentSuffixLegacy `property`

##### Summary

Gets the legacy user agent suffix that gets
appended to graph requests on ios and android.

<a name='T-Facebook-Unity-CurrencyAmount'></a>
## CurrencyAmount `type`

##### Namespace

Facebook.Unity

##### Summary

Contains an amount and currency associated with a purchase or transaction.

<a name='M-Facebook-Unity-CurrencyAmount-#ctor-System-String,System-String-'></a>
### #ctor(amount,currency) `constructor`

##### Summary

Initializes a new instance of the [CurrencyAmount](#T-Facebook-Unity-CurrencyAmount 'Facebook.Unity.CurrencyAmount') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The associated amount. |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The associated currency. |

<a name='P-Facebook-Unity-CurrencyAmount-Amount'></a>
### Amount `property`

##### Summary

Gets the amount, eg "0.99".

<a name='P-Facebook-Unity-CurrencyAmount-Currency'></a>
### Currency `property`

##### Summary

Gets the currency, represented by the ISO currency code, eg "USD".

<a name='M-Facebook-Unity-CurrencyAmount-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [CurrencyAmount](#T-Facebook-Unity-CurrencyAmount 'Facebook.Unity.CurrencyAmount').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [CurrencyAmount](#T-Facebook-Unity-CurrencyAmount 'Facebook.Unity.CurrencyAmount').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-CustomUpdateContent'></a>
## CustomUpdateContent `type`

##### Namespace

Facebook.Unity

##### Summary

Static class to hold Custom Update Content for FBGamingServices.PerformCustomUpdate.

<a name='T-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder'></a>
## CustomUpdateContentBuilder `type`

##### Namespace

Facebook.Unity.CustomUpdateContent

<a name='M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-#ctor-System-String,Facebook-Unity-CustomUpdateLocalizedText,UnityEngine-Texture2D-'></a>
### #ctor(contextTokenId,text,image) `constructor`

##### Summary

Creates a CustomUpdateContent Builder

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| contextTokenId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A valid GamingContext to send the update to |
| text | [Facebook.Unity.CustomUpdateLocalizedText](#T-Facebook-Unity-CustomUpdateLocalizedText 'Facebook.Unity.CustomUpdateLocalizedText') | The text that will be included in the update |
| image | [UnityEngine.Texture2D](#T-UnityEngine-Texture2D 'UnityEngine.Texture2D') | An image that will be included in the update |

<a name='M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-#ctor-System-String,Facebook-Unity-CustomUpdateLocalizedText,Facebook-Unity-CustomUpdateMedia-'></a>
### #ctor(contextTokenId,text,media) `constructor`

##### Summary

Creates a CustomUpdateContent Builder

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| contextTokenId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A valid GamingContext to send the update to |
| text | [Facebook.Unity.CustomUpdateLocalizedText](#T-Facebook-Unity-CustomUpdateLocalizedText 'Facebook.Unity.CustomUpdateLocalizedText') | The text that will be included in the update |
| media | [Facebook.Unity.CustomUpdateMedia](#T-Facebook-Unity-CustomUpdateMedia 'Facebook.Unity.CustomUpdateMedia') | A gif or video that will be included in the update |

<a name='M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-build'></a>
### build() `method`

##### Summary

Returns a CustomUpdateContent with the values defined in this builder

##### Returns

CustomUpdateContent instance to pass to FBGamingServices.PerformCustomUpdate()

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-setCTA-Facebook-Unity-CustomUpdateLocalizedText-'></a>
### setCTA(cta) `method`

##### Summary

Sets the CTA (Call to Action) text in the update message

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cta | [Facebook.Unity.CustomUpdateLocalizedText](#T-Facebook-Unity-CustomUpdateLocalizedText 'Facebook.Unity.CustomUpdateLocalizedText') | Custom CTA to use. If none is provided a localized version of 'play' is used. |

<a name='M-Facebook-Unity-CustomUpdateContent-CustomUpdateContentBuilder-setData-System-String-'></a>
### setData(data) `method`

##### Summary

Sets a Data that will be sent back to the game when a user clicks on the message. When the
 game is launched from a Custom Update message the data here will be forwarded as a Payload.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A String that will be sent back to the game |

<a name='T-Facebook-Unity-CustomUpdateLocalizedText'></a>
## CustomUpdateLocalizedText `type`

##### Namespace

Facebook.Unity

##### Summary

Represents a text string that can have different Locale values provided.

<a name='M-Facebook-Unity-CustomUpdateLocalizedText-#ctor-System-String,System-Collections-Generic-IDictionary{System-String,System-String}-'></a>
### #ctor(defaultText,localizations) `constructor`

##### Summary

Creates a CustomUpdateLocalizedText instance

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| defaultText | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Text that will be used if no matching locale is found |
| localizations | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | Optional key-value Dictionary of Locale_Code: Locale String Value for this text.
For a list of valid locale codes see:
https://lookaside.facebook.com/developers/resources/?id=FacebookLocales.xml |

<a name='T-Facebook-Unity-CustomUpdateMedia'></a>
## CustomUpdateMedia `type`

##### Namespace

Facebook.Unity

##### Summary

Represents a media that will be included in a Custom Update Message

<a name='M-Facebook-Unity-CustomUpdateMedia-#ctor-Facebook-Unity-CustomUpdateMediaInfo,Facebook-Unity-CustomUpdateMediaInfo-'></a>
### #ctor(gif,video) `constructor`

##### Summary

Creates a CustomUpdateMedia instance. Note that gif and video are mutually exclusive

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| gif | [Facebook.Unity.CustomUpdateMediaInfo](#T-Facebook-Unity-CustomUpdateMediaInfo 'Facebook.Unity.CustomUpdateMediaInfo') | Gif that will be included in the Update Message |
| video | [Facebook.Unity.CustomUpdateMediaInfo](#T-Facebook-Unity-CustomUpdateMediaInfo 'Facebook.Unity.CustomUpdateMediaInfo') | Video that will be included in the Update Message. Currently this is not yet
supported but will be in a server side update so it is already included in the SDK. This
disclaimer will be removed as soon as it is. |

<a name='T-Facebook-Unity-CustomUpdateMediaInfo'></a>
## CustomUpdateMediaInfo `type`

##### Namespace

Facebook.Unity

##### Summary

Stores Information about a Media that will be part of a Custom Update

<a name='T-Facebook-Unity-FB'></a>
## FB `type`

##### Namespace

Facebook.Unity

##### Summary

Static class for exposing the facebook integration.

<a name='P-Facebook-Unity-FB-AppId'></a>
### AppId `property`

##### Summary

Gets the app identifier. AppId might be different from FBSettings.AppId
if using the programmatic version of FB.Init().

<a name='P-Facebook-Unity-FB-ClientToken'></a>
### ClientToken `property`

##### Summary

Gets the app client token. ClientToken might be different from FBSettings.ClientToken
if using the programmatic version of FB.Init().

<a name='P-Facebook-Unity-FB-GraphApiVersion'></a>
### GraphApiVersion `property`

##### Summary

Gets or sets the graph API version.
The Unity sdk is by default pegged to the lastest version of the graph api
at the time of the SDKs release. To override this value to use a different
version set this value.

<a name='P-Facebook-Unity-FB-IsInitialized'></a>
### IsInitialized `property`

##### Summary

Gets a value indicating whether is the SDK is initialized.

<a name='P-Facebook-Unity-FB-IsLoggedIn'></a>
### IsLoggedIn `property`

##### Summary

Gets a value indicating whether a user logged in.

<a name='P-Facebook-Unity-FB-LimitAppEventUsage'></a>
### LimitAppEventUsage `property`

##### Summary

Gets or sets a value indicating whether this [FB](#T-Facebook-Unity-FB 'Facebook.Unity.FB') limit app event usage.
If the player has set the limitEventUsage flag to YES, your app will continue
to send this data to Facebook, but Facebook will not use the data to serve
targeted ads. Facebook may continue to use the information for other purposes,
including frequency capping, conversion events, estimating the number of unique
users, security and fraud detection, and debugging.

<a name='M-Facebook-Unity-FB-API-System-String,Facebook-Unity-HttpMethod,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult},System-Collections-Generic-IDictionary{System-String,System-String}-'></a>
### API(query,method,callback,formData) `method`

##### Summary

Makes a call to the Facebook Graph API.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| query | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The Graph API endpoint to call.
You may prefix this with a version string to call a particular version of the API. |
| method | [Facebook.Unity.HttpMethod](#T-Facebook-Unity-HttpMethod 'Facebook.Unity.HttpMethod') | The HTTP method to use in the call. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}') | The callback to use upon completion. |
| formData | [System.Collections.Generic.IDictionary{System.String,System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.String}') | The key/value pairs to be passed to the endpoint as arguments. |

<a name='M-Facebook-Unity-FB-API-System-String,Facebook-Unity-HttpMethod,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult},UnityEngine-WWWForm-'></a>
### API(query,method,callback,formData) `method`

##### Summary

Makes a call to the Facebook Graph API.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| query | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The Graph API endpoint to call.
You may prefix this with a version string to call a particular version of the API. |
| method | [Facebook.Unity.HttpMethod](#T-Facebook-Unity-HttpMethod 'Facebook.Unity.HttpMethod') | The HTTP method to use in the call. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}') | The callback to use upon completion. |
| formData | [UnityEngine.WWWForm](#T-UnityEngine-WWWForm 'UnityEngine.WWWForm') | Form data for the request. |

<a name='M-Facebook-Unity-FB-ActivateApp'></a>
### ActivateApp() `method`

##### Summary

Sends an app activation event to Facebook when your app is activated.

 On iOS and Android, this event is logged automatically if you turn on
 AutoLogAppEventsEnabled flag. You may still need to call this event if
 you are running on web.

 On iOS the activate event is fired when the app becomes active
 On Android the activate event is fired during FB.Init

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-AppRequest-System-String,Facebook-Unity-OGActionType,System-String,System-Collections-Generic-IEnumerable{System-String},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}-'></a>
### AppRequest(message,actionType,objectId,to,data,title,callback) `method`

##### Summary

Apps the request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The request string the recipient will see, maximum length 60 characters. |
| actionType | [Facebook.Unity.OGActionType](#T-Facebook-Unity-OGActionType 'Facebook.Unity.OGActionType') | Request action type for structured request. |
| objectId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Open Graph object ID for structured request.
Note the type of object should belong to this app. |
| to | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of Facebook IDs to which to send the request. |
| data | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Additional data stored with the request on Facebook,
and handed back to the app when it reads the request back out.
Maximum length 255 characters. |
| title | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The title for the platform multi-friend selector dialog. Max length 50 characters.. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}') | A callback for when the request completes. |

<a name='M-Facebook-Unity-FB-AppRequest-System-String,Facebook-Unity-OGActionType,System-String,System-Collections-Generic-IEnumerable{System-Object},System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-Int32},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}-'></a>
### AppRequest(message,actionType,objectId,filters,excludeIds,maxRecipients,data,title,callback) `method`

##### Summary

Apps the request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The request string the recipient will see, maximum length 60 characters. |
| actionType | [Facebook.Unity.OGActionType](#T-Facebook-Unity-OGActionType 'Facebook.Unity.OGActionType') | Request action type for structured request. |
| objectId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Open Graph object ID for structured request.
Note the type of object should belong to this app. |
| filters | [System.Collections.Generic.IEnumerable{System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.Object}') | The configuration of the platform multi-friend selector.
It should be a List of filter strings. |
| excludeIds | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of Facebook IDs to exclude from the platform multi-friend selector dialog.
This list is currently not supported for mobile devices. |
| maxRecipients | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Platform-dependent The maximum number of recipients the sender should be able to
choose in the platform multi-friend selector dialog.
Only guaranteed to work in Unity Web Player app. |
| data | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Additional data stored with the request on Facebook, and handed
back to the app when it reads the request back out.
Maximum length 255 characters. |
| title | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The title for the platform multi-friend selector dialog. Max length 50 characters. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}') | A callback for when the request completes. |

<a name='M-Facebook-Unity-FB-AppRequest-System-String,System-Collections-Generic-IEnumerable{System-String},System-Collections-Generic-IEnumerable{System-Object},System-Collections-Generic-IEnumerable{System-String},System-Nullable{System-Int32},System-String,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult}-'></a>
### AppRequest(message,to,filters,excludeIds,maxRecipients,data,title,callback) `method`

##### Summary

Apps the request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The request string the recipient will see, maximum length 60 characters. |
| to | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of Facebook IDs to which to send the request. |
| filters | [System.Collections.Generic.IEnumerable{System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.Object}') | The configuration of the platform multi-friend selector.
It should be a List of filter strings. |
| excludeIds | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of Facebook IDs to exclude from the platform multi-friend selector dialog.
This list is currently not supported for mobile devices. |
| maxRecipients | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Platform-dependent The maximum number of recipients the sender should be able to
choose in the platform multi-friend selector dialog.
Only guaranteed to work in Unity Web Player app. |
| data | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Additional data stored with the request on Facebook, and handed
back to the app when it reads the request back out.
Maximum length 255 characters. |
| title | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The title for the platform multi-friend selector dialog. Max length 50 characters. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppRequestResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppRequestResult}') | A callback for when the request completes. |

<a name='M-Facebook-Unity-FB-ClearAppLink'></a>
### ClearAppLink() `method`

##### Summary

Clear app link.

 Clear app link when app link has been handled, only works for
 Android, this function will do nothing in other platforms.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-FeedShare-System-String,System-Uri,System-String,System-String,System-String,System-Uri,System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult}-'></a>
### FeedShare(toId,link,linkName,linkCaption,linkDescription,picture,mediaSource,callback) `method`

##### Summary

Legacy feed share. Only use this dialog if you need the legacy parameters otherwiese use
[](#!-FB-ShareLink-System-String, System-String, System-String, System-String, Facebook-FacebookDelegate- 'FB.ShareLink(System.String, System.String, System.String, System.String, Facebook.FacebookDelegate)').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| toId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the profile that this story will be published to.
    If this is unspecified, it defaults to the value of from.
    The ID must be a friend who also uses your app. |
| link | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | The link attached to this post. |
| linkName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the link attachment. |
| linkCaption | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The caption of the link (appears beneath the link name).
    If not specified, this field is automatically populated
    with the URL of the link. |
| linkDescription | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The description of the link (appears beneath the link caption).
    If not specified, this field is automatically populated by information
    scraped from the link, typically the title of the page. |
| picture | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | The URL of a picture attached to this post.
    The picture must be at least 200px by 200px.
    See our documentation on sharing best practices for more information on sizes. |
| mediaSource | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The URL of a media file (either SWF or MP3) attached to this post.
    If SWF, you must also specify picture to provide a thumbnail for the video. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult}') | The callback to use upon completion. |

<a name='M-Facebook-Unity-FB-GetAppLink-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult}-'></a>
### GetAppLink(callback) `method`

##### Summary

Gets the deep link if available.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult}') | The callback to use upon completion. |

<a name='M-Facebook-Unity-FB-Init-Facebook-Unity-InitDelegate,Facebook-Unity-HideUnityDelegate,System-String-'></a>
### Init(onInitComplete,onHideUnity,authResponse) `method`

##### Summary

This is the preferred way to call FB.Init(). It will take the facebook app id specified in your "Facebook"
=> "Edit Settings" menu when it is called.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| onInitComplete | [Facebook.Unity.InitDelegate](#T-Facebook-Unity-InitDelegate 'Facebook.Unity.InitDelegate') | Delegate is called when FB.Init() finished initializing everything. By passing in a delegate you can find
out when you can safely call the other methods. |
| onHideUnity | [Facebook.Unity.HideUnityDelegate](#T-Facebook-Unity-HideUnityDelegate 'Facebook.Unity.HideUnityDelegate') | A delegate to invoke when unity is hidden. |
| authResponse | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Auth response. |

<a name='M-Facebook-Unity-FB-Init-System-String,System-String,System-Boolean,System-Boolean,System-Boolean,System-Boolean,System-Boolean,System-String,System-String,Facebook-Unity-HideUnityDelegate,Facebook-Unity-InitDelegate-'></a>
### Init(appId,clientToken,cookie,logging,status,xfbml,frictionlessRequests,authResponse,javascriptSDKLocale,onHideUnity,onInitComplete) `method`

##### Summary

If you need a more programmatic way to set the facebook app id and other setting call this function.
Useful for a build pipeline that requires no human input.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| appId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | App identifier. |
| clientToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | App client token. |
| cookie | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If set to `true` cookie. |
| logging | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If set to `true` logging. |
| status | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If set to `true` status. |
| xfbml | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If set to `true` xfbml. |
| frictionlessRequests | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If set to `true` frictionless requests. |
| authResponse | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Auth response. |
| javascriptSDKLocale | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The locale of the js sdk used see
https://developers.facebook.com/docs/internationalization#plugins. |
| onHideUnity | [Facebook.Unity.HideUnityDelegate](#T-Facebook-Unity-HideUnityDelegate 'Facebook.Unity.HideUnityDelegate') | A delegate to invoke when unity is hidden. |
| onInitComplete | [Facebook.Unity.InitDelegate](#T-Facebook-Unity-InitDelegate 'Facebook.Unity.InitDelegate') | Delegate is called when FB.Init() finished initializing everything. By passing in a delegate you can find
out when you can safely call the other methods. |

<a name='M-Facebook-Unity-FB-LogAppEvent-System-String,System-Nullable{System-Single},System-Collections-Generic-Dictionary{System-String,System-Object}-'></a>
### LogAppEvent(logEvent,valueToSum,parameters) `method`

##### Summary

Logs an app event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logEvent | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the event to log. |
| valueToSum | [System.Nullable{System.Single}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Single}') | A number representing some value to be summed when reported. |
| parameters | [System.Collections.Generic.Dictionary{System.String,System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}') | Any parameters needed to describe the event. |

<a name='M-Facebook-Unity-FB-LogInWithPublishPermissions-System-Collections-Generic-IEnumerable{System-String},Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}-'></a>
### LogInWithPublishPermissions(permissions,callback) `method`

##### Summary

Logs the user in with the requested publish permissions.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| permissions | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of requested permissions. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}') | Callback to be called when request completes. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.NotSupportedException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.NotSupportedException 'System.NotSupportedException') | Thrown when called on a TV device. |

<a name='M-Facebook-Unity-FB-LogInWithReadPermissions-System-Collections-Generic-IEnumerable{System-String},Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}-'></a>
### LogInWithReadPermissions(permissions,callback) `method`

##### Summary

Logs the user in with the requested read permissions.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| permissions | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of requested permissions. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}') | Callback to be called when request completes. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.NotSupportedException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.NotSupportedException 'System.NotSupportedException') | Thrown when called on a TV device. |

<a name='M-Facebook-Unity-FB-LogOut'></a>
### LogOut() `method`

##### Summary

Logs out the current user.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-LogPurchase-System-Decimal,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}-'></a>
### LogPurchase(logPurchase,currency,parameters) `method`

##### Summary

Logs the purchase.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logPurchase | [System.Decimal](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Decimal 'System.Decimal') | The amount of currency the user spent. |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The 3-letter ISO currency code. |
| parameters | [System.Collections.Generic.Dictionary{System.String,System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}') | Any parameters needed to describe the event.
Elements included in this dictionary can't be null. |

<a name='M-Facebook-Unity-FB-LogPurchase-System-Single,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}-'></a>
### LogPurchase(logPurchase,currency,parameters) `method`

##### Summary

Logs the purchase.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logPurchase | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') | The amount of currency the user spent. |
| currency | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The 3-letter ISO currency code. |
| parameters | [System.Collections.Generic.Dictionary{System.String,System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}') | Any parameters needed to describe the event.
Elements included in this dictionary can't be null. |

<a name='M-Facebook-Unity-FB-ShareLink-System-Uri,System-String,System-String,System-Uri,Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult}-'></a>
### ShareLink(contentURL,contentTitle,contentDescription,photoURL,callback) `method`

##### Summary

Opens a share dialog for sharing a link.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| contentURL | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | The URL or the link to share. |
| contentTitle | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The title to display for this link.. |
| contentDescription | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The description of the link.  If not specified, this field is automatically populated by
information scraped from the link, typically the title of the page. |
| photoURL | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | The URL of a picture to attach to this content. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IShareResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IShareResult}') | A callback for when the request completes. |

<a name='T-Facebook-Unity-FBGamingServices'></a>
## FBGamingServices `type`

##### Namespace

Facebook.Unity

##### Summary

Static class for exposing the Facebook GamingServices Integration.

<a name='M-Facebook-Unity-FBGamingServices-OpenFriendFinderDialog-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGamingServicesFriendFinderResult}-'></a>
### OpenFriendFinderDialog(callback) `method`

##### Summary

Opens the Friend Finder Dialog

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IGamingServicesFriendFinderResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGamingServicesFriendFinderResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IGamingServicesFriendFinderResult}') | A callback for when the Dialog is closed. |

<a name='M-Facebook-Unity-FBGamingServices-PerformCustomUpdate-Facebook-Unity-CustomUpdateContent,Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult}-'></a>
### PerformCustomUpdate(content,callback) `method`

##### Summary

Informs facebook that the player has taken an action and will notify other players in the same GamingContext

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [Facebook.Unity.CustomUpdateContent](#T-Facebook-Unity-CustomUpdateContent 'Facebook.Unity.CustomUpdateContent') | Please check CustomUpdateContent.Builder for details on all the fields that
allow customizing the update. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IGraphResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IGraphResult}') | The callback to use upon completion. |

<a name='M-Facebook-Unity-FBGamingServices-UploadImageToMediaLibrary-System-String,System-Uri,System-Boolean,Facebook-Unity-FacebookDelegate{Facebook-Unity-IMediaUploadResult}-'></a>
### UploadImageToMediaLibrary(caption,imageUri,shouldLaunchMediaDialog,callback) `method`

##### Summary

Uploads an Image to the player's Gaming Media Library

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| caption | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Title for this image in the Media Library |
| imageUri | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | Path to the image file in the local filesystem. On Android
this can also be a content:// URI |
| shouldLaunchMediaDialog | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | If we should open the Media Dialog to allow
the player to Share this image right away. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IMediaUploadResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IMediaUploadResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IMediaUploadResult}') | A callback for when the image upload is complete. |

<a name='M-Facebook-Unity-FBGamingServices-UploadVideoToMediaLibrary-System-String,System-Uri,System-Boolean,Facebook-Unity-FacebookDelegate{Facebook-Unity-IMediaUploadResult}-'></a>
### UploadVideoToMediaLibrary(caption,videoUri,callback) `method`

##### Summary

Uploads a video to the player's Gaming Media Library

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| caption | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Title for this video in the Media Library |
| videoUri | [System.Uri](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Uri 'System.Uri') | Path to the video file in the local filesystem. On Android
this can also be a content:// URI |
| callback | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | A callback for when the video upload is complete. |

##### Remarks

Note that when the callback is fired, the video will still need to be
encoded before it is available in the Media Library.

<a name='T-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior'></a>
## FBInsightsFlushBehavior `type`

##### Namespace

Facebook.Unity.Mobile.IOS.IOSFacebook

<a name='F-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior-FBInsightsFlushBehaviorAuto'></a>
### FBInsightsFlushBehaviorAuto `constants`

##### Summary

The FB insights flush behavior auto.

<a name='F-Facebook-Unity-Mobile-IOS-IOSFacebook-FBInsightsFlushBehavior-FBInsightsFlushBehaviorExplicitOnly'></a>
### FBInsightsFlushBehaviorExplicitOnly `constants`

##### Summary

The FB insights flush behavior explicit only.

<a name='T-Facebook-Unity-FBLocation'></a>
## FBLocation `type`

##### Namespace

Facebook.Unity

<a name='P-Facebook-Unity-FBLocation-ID'></a>
### ID `property`

##### Summary

Gets the location's unique identifier

<a name='P-Facebook-Unity-FBLocation-Name'></a>
### Name `property`

##### Summary

Gets the location's name.

<a name='M-Facebook-Unity-FBLocation-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [FBLocation](#T-Facebook-Unity-FBLocation 'Facebook.Unity.FBLocation').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [FBLocation](#T-Facebook-Unity-FBLocation 'Facebook.Unity.FBLocation').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-FacebookDelegate`1'></a>
## FacebookDelegate\`1 `type`

##### Namespace

Facebook.Unity

##### Summary

Facebook delegate.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| result | [T:Facebook.Unity.FacebookDelegate\`1](#T-T-Facebook-Unity-FacebookDelegate`1 'T:Facebook.Unity.FacebookDelegate`1') | The result. |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The result type. |

<a name='T-Facebook-Unity-FacebookSdkVersion'></a>
## FacebookSdkVersion `type`

##### Namespace

Facebook.Unity

##### Summary

Facebook sdk version.

<a name='P-Facebook-Unity-FacebookSdkVersion-Build'></a>
### Build `property`

##### Summary

Gets the SDK build version.

<a name='T-Facebook-Unity-FriendFinderInviation'></a>
## FriendFinderInviation `type`

##### Namespace

Facebook.Unity

##### Summary

Contains a Inviation from Friend Finder.

<a name='T-Facebook-Unity-HideUnityDelegate'></a>
## HideUnityDelegate `type`

##### Namespace

Facebook.Unity

##### Summary

Hide unity delegate.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| isUnityShown | [T:Facebook.Unity.HideUnityDelegate](#T-T-Facebook-Unity-HideUnityDelegate 'T:Facebook.Unity.HideUnityDelegate') | When called with its sole argument set to false,
your game should pause and prepare to lose focus. If it's called with its
argument set to true, your game should prepare to regain focus and resume
play. Your game should check whether it is in fullscreen mode when it resumes,
and offer the player a chance to go to fullscreen mode if appropriate. |

<a name='T-Facebook-Unity-HttpMethod'></a>
## HttpMethod `type`

##### Namespace

Facebook.Unity

##### Summary

Http method.

<a name='F-Facebook-Unity-HttpMethod-DELETE'></a>
### DELETE `constants`

##### Summary

Indicates that the request is a DELETE request.

<a name='F-Facebook-Unity-HttpMethod-GET'></a>
### GET `constants`

##### Summary

Indicates that the request is a GET request.

<a name='F-Facebook-Unity-HttpMethod-POST'></a>
### POST `constants`

##### Summary

Indicates that the request is a POST request.

<a name='T-Facebook-Unity-IAccessTokenRefreshResult'></a>
## IAccessTokenRefreshResult `type`

##### Namespace

Facebook.Unity

##### Summary

The access token refresh result.

<a name='P-Facebook-Unity-IAccessTokenRefreshResult-AccessToken'></a>
### AccessToken `property`

##### Summary

Gets the refreshed access token.

<a name='T-Facebook-Unity-IAppLinkResult'></a>
## IAppLinkResult `type`

##### Namespace

Facebook.Unity

##### Summary

A result containing an app link.

<a name='P-Facebook-Unity-IAppLinkResult-Extras'></a>
### Extras `property`

##### Summary

Gets the extras.

<a name='P-Facebook-Unity-IAppLinkResult-Ref'></a>
### Ref `property`

##### Summary

Gets the ref.

<a name='P-Facebook-Unity-IAppLinkResult-TargetUrl'></a>
### TargetUrl `property`

##### Summary

Gets the target URI.

<a name='P-Facebook-Unity-IAppLinkResult-Url'></a>
### Url `property`

##### Summary

Gets the URL. This is the url that was used to open the app on iOS
or on Android the intent's data string. When handling deffered app
links on Android this may not be available.

<a name='T-Facebook-Unity-IAppRequestResult'></a>
## IAppRequestResult `type`

##### Namespace

Facebook.Unity

##### Summary

App request result.

<a name='P-Facebook-Unity-IAppRequestResult-RequestID'></a>
### RequestID `property`

##### Summary

Gets RequestID.

<a name='P-Facebook-Unity-IAppRequestResult-To'></a>
### To `property`

##### Summary

Gets the list of users who the request was sent to.

<a name='T-Facebook-Unity-ICatalogResult'></a>
## ICatalogResult `type`

##### Namespace

Facebook.Unity

##### Summary

The catalog result.

<a name='P-Facebook-Unity-ICatalogResult-Products'></a>
### Products `property`

##### Summary

Gets a list of products available for purchase.

<a name='T-Facebook-Unity-IChooseGamingContextResult'></a>
## IChooseGamingContextResult `type`

##### Namespace

Facebook.Unity

##### Summary

The Choose Gaming Context API result.

<a name='T-Facebook-Unity-ICreateGamingContextResult'></a>
## ICreateGamingContextResult `type`

##### Namespace

Facebook.Unity

##### Summary

The Create Gaming Context API result.

<a name='T-Facebook-Unity-IGamingServicesFriendFinderResult'></a>
## IGamingServicesFriendFinderResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a a FriendFinder Dialog. There is no data returned
to the client.

<a name='T-Facebook-Unity-IGetCurrentGamingContextResult'></a>
## IGetCurrentGamingContextResult `type`

##### Namespace

Facebook.Unity

##### Summary

The Get Current Gaming Context API result.

<a name='P-Facebook-Unity-IGetCurrentGamingContextResult-ContextId'></a>
### ContextId `property`

##### Summary

Gets Gaming Context ID.

<a name='T-Facebook-Unity-IGraphResult'></a>
## IGraphResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a graph api call.

<a name='P-Facebook-Unity-IGraphResult-ResultList'></a>
### ResultList `property`

##### Summary

Gets the result.

<a name='P-Facebook-Unity-IGraphResult-Texture'></a>
### Texture `property`

##### Summary

Gets the Texture.

##### Remarks

The Graph API does not return textures directly, but a few endpoints can
    redirect to images when no 'redirect=false' parameter is specified. Ex: '/me/picture'.

<a name='T-Facebook-Unity-IGroupCreateResult'></a>
## IGroupCreateResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a group creation.

<a name='P-Facebook-Unity-IGroupCreateResult-GroupId'></a>
### GroupId `property`

##### Summary

Gets the group identifier.

<a name='T-Facebook-Unity-IGroupJoinResult'></a>
## IGroupJoinResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a group join.

<a name='T-Facebook-Unity-IHasLicenseResult'></a>
## IHasLicenseResult `type`

##### Namespace

Facebook.Unity

##### Summary

The license check for premium games result.

<a name='P-Facebook-Unity-IHasLicenseResult-HasLicense'></a>
### HasLicense `property`

##### Summary

Gets if the user has a license to play the game.

<a name='T-Facebook-Unity-IInternalResult'></a>
## IInternalResult `type`

##### Namespace

Facebook.Unity

<a name='P-Facebook-Unity-IInternalResult-CallbackId'></a>
### CallbackId `property`

##### Summary

Gets the callback identifier.

<a name='T-Facebook-Unity-ILoginResult'></a>
## ILoginResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a login request.

<a name='P-Facebook-Unity-ILoginResult-AccessToken'></a>
### AccessToken `property`

##### Summary

Gets the access token.

<a name='P-Facebook-Unity-ILoginResult-AuthenticationToken'></a>
### AuthenticationToken `property`

##### Summary

Gets the authentication token.

<a name='T-Facebook-Unity-ILoginStatusResult'></a>
## ILoginStatusResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result for getting the Login Status of a user.

<a name='P-Facebook-Unity-ILoginStatusResult-Failed'></a>
### Failed `property`

##### Summary

Gets a value indicating whether the access token retrieval has failed

<a name='T-Facebook-Unity-IMediaUploadResult'></a>
## IMediaUploadResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a Media Upload

<a name='P-Facebook-Unity-IMediaUploadResult-MediaId'></a>
### MediaId `property`

##### Summary

Gets the Media Identifier.

<a name='T-Facebook-Unity-IPayResult'></a>
## IPayResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a pay request.

<a name='P-Facebook-Unity-IPayResult-ErrorCode'></a>
### ErrorCode `property`

##### Summary

Gets the error code.

<a name='T-Facebook-Unity-IPayloadResult'></a>
## IPayloadResult `type`

##### Namespace

Facebook.Unity

##### Summary

The payload result.

<a name='P-Facebook-Unity-IPayloadResult-Payload'></a>
### Payload `property`

##### Summary

Gets a dictionary of the payload items.

<a name='T-Facebook-Unity-IProfileResult'></a>
## IProfileResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a user profile request.

<a name='P-Facebook-Unity-IProfileResult-CurrentProfile'></a>
### CurrentProfile `property`

##### Summary

Gets the current user profile.

<a name='T-Facebook-Unity-IPurchaseResult'></a>
## IPurchaseResult `type`

##### Namespace

Facebook.Unity

##### Summary

The purchase result.

<a name='P-Facebook-Unity-IPurchaseResult-Purchase'></a>
### Purchase `property`

##### Summary

Gets the purchase information of the item that was purchased.

<a name='T-Facebook-Unity-IPurchasesResult'></a>
## IPurchasesResult `type`

##### Namespace

Facebook.Unity

##### Summary

The access token refresh result.

<a name='P-Facebook-Unity-IPurchasesResult-Purchases'></a>
### Purchases `property`

##### Summary

Gets a list of products available for purchase.

<a name='T-Facebook-Unity-IResult'></a>
## IResult `type`

##### Namespace

Facebook.Unity

##### Summary

A class contiaing the result data.

<a name='P-Facebook-Unity-IResult-Cancelled'></a>
### Cancelled `property`

##### Summary

Gets a value indicating whether this instance cancelled.

<a name='P-Facebook-Unity-IResult-Error'></a>
### Error `property`

##### Summary

Gets the error.

<a name='P-Facebook-Unity-IResult-ErrorDictionary'></a>
### ErrorDictionary `property`

##### Summary

Gets the error, as a Dictionary.

<a name='P-Facebook-Unity-IResult-RawResult'></a>
### RawResult `property`

##### Summary

Gets the raw result.

<a name='P-Facebook-Unity-IResult-ResultDictionary'></a>
### ResultDictionary `property`

##### Summary

Gets the result.

<a name='T-Facebook-Unity-ISessionScoreResult'></a>
## ISessionScoreResult `type`

##### Namespace

Facebook.Unity

##### Summary

The post session score result.

<a name='T-Facebook-Unity-IShareResult'></a>
## IShareResult `type`

##### Namespace

Facebook.Unity

##### Summary

The result of a share.

<a name='P-Facebook-Unity-IShareResult-PostId'></a>
### PostId `property`

##### Summary

Gets the post identifier.

<a name='T-Facebook-Unity-ISwitchGamingContextResult'></a>
## ISwitchGamingContextResult `type`

##### Namespace

Facebook.Unity

##### Summary

The Switch Gaming Context API result.

<a name='T-Facebook-Unity-ITournamentResult'></a>
## ITournamentResult `type`

##### Namespace

Facebook.Unity

##### Summary

The instant tournament result.

<a name='T-Facebook-Unity-ITournamentScoreResult'></a>
## ITournamentScoreResult `type`

##### Namespace

Facebook.Unity

##### Summary

The instant tournament score result.

<a name='T-Facebook-Unity-InitDelegate'></a>
## InitDelegate `type`

##### Namespace

Facebook.Unity

##### Summary

Init delegate.

<a name='T-Facebook-MiniJSON-Json'></a>
## Json `type`

##### Namespace

Facebook.MiniJSON

##### Summary

This class encodes and decodes JSON strings.
Spec. details, see http://www.json.org/
JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
All numbers are parsed to doubles.

<a name='M-Facebook-MiniJSON-Json-Deserialize-System-String-'></a>
### Deserialize(json) `method`

##### Summary

Parses the string json into a value.

##### Returns

An List<object>, a Dictionary<string, object>, a double, an integer,a string, null, true, or false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| json | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A JSON string. |

<a name='M-Facebook-MiniJSON-Json-Serialize-System-Object-'></a>
### Serialize(obj) `method`

##### Summary

Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string.

##### Returns

A JSON encoded string, or null if object 'json' is not serializable.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| obj | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | A Dictionary<string, object> / List<object>. |

<a name='T-Facebook-Unity-FB-Mobile'></a>
## Mobile `type`

##### Namespace

Facebook.Unity.FB

##### Summary

A class containing the settings specific to the supported mobile platforms.

<a name='P-Facebook-Unity-FB-Mobile-ShareDialogMode'></a>
### ShareDialogMode `property`

##### Summary

Gets or sets the share dialog mode.

<a name='P-Facebook-Unity-FB-Mobile-UserID'></a>
### UserID `property`

##### Summary

Gets or sets the user ID.

<a name='M-Facebook-Unity-FB-Mobile-CurrentAuthenticationToken'></a>
### CurrentAuthenticationToken() `method`

##### Summary

Current Authentication Token.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-Mobile-CurrentProfile'></a>
### CurrentProfile() `method`

##### Summary

Current Profile.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-Mobile-CurrentProfile-Facebook-Unity-FacebookDelegate{Facebook-Unity-IProfileResult}-'></a>
### CurrentProfile() `method`

##### Summary

Current Profile via vallback.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-Mobile-EnableProfileUpdatesOnAccessTokenChange-System-Boolean-'></a>
### EnableProfileUpdatesOnAccessTokenChange() `method`

##### Summary

Call this function so that Profile will be automatically updated based on the changes to the access token.

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-Mobile-FetchDeferredAppLinkData-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult}-'></a>
### FetchDeferredAppLinkData(callback) `method`

##### Summary

Fetchs the deferred app link data.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAppLinkResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAppLinkResult}') | A callback for when the call is complete. |

<a name='M-Facebook-Unity-FB-Mobile-IsImplicitPurchaseLoggingEnabled'></a>
### IsImplicitPurchaseLoggingEnabled() `method`

##### Summary

Returns the setting for Automatic Purchase Logging

##### Parameters

This method has no parameters.

<a name='M-Facebook-Unity-FB-Mobile-LoginWithTrackingPreference-Facebook-Unity-LoginTracking,System-Collections-Generic-IEnumerable{System-String},System-String,Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult}-'></a>
### LoginWithTrackingPreference(loginTracking,permissions,nonce,callback) `method`

##### Summary

Login with tracking experience.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loginTracking | [Facebook.Unity.LoginTracking](#T-Facebook-Unity-LoginTracking 'Facebook.Unity.LoginTracking') | The option for login tracking preference, "enabled" or "limited". |
| permissions | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | A list of permissions. |
| nonce | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | An optional nonce to use for the login attempt. |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-ILoginResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.ILoginResult}') | A callback for when the call is complete. |

<a name='M-Facebook-Unity-FB-Mobile-RefreshCurrentAccessToken-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAccessTokenRefreshResult}-'></a>
### RefreshCurrentAccessToken(callback) `method`

##### Summary

Refreshs the current access to get a new access token if possible.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| callback | [Facebook.Unity.FacebookDelegate{Facebook.Unity.IAccessTokenRefreshResult}](#T-Facebook-Unity-FacebookDelegate{Facebook-Unity-IAccessTokenRefreshResult} 'Facebook.Unity.FacebookDelegate{Facebook.Unity.IAccessTokenRefreshResult}') | A on refresh access token compelte callback. |

<a name='M-Facebook-Unity-FB-Mobile-SetAdvertiserIDCollectionEnabled-System-Boolean-'></a>
### SetAdvertiserIDCollectionEnabled(advertiserIDCollectionEnabled) `method`

##### Summary

Sets the setting for Advertiser ID collection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| advertiserIDCollectionEnabled | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | The setting for Advertiser ID collection |

<a name='M-Facebook-Unity-FB-Mobile-SetAdvertiserTrackingEnabled-System-Boolean-'></a>
### SetAdvertiserTrackingEnabled(advertiserTrackingEnabled) `method`

##### Summary

Sets the setting for Advertiser Tracking Enabled.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| advertiserTrackingEnabled | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | The setting for Advertiser Tracking Enabled |

<a name='M-Facebook-Unity-FB-Mobile-SetAutoLogAppEventsEnabled-System-Boolean-'></a>
### SetAutoLogAppEventsEnabled(autoLogAppEventsEnabled) `method`

##### Summary

Sets the setting for Automatic App Events Logging.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| autoLogAppEventsEnabled | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | The setting for Automatic App Events Logging |

<a name='M-Facebook-Unity-FB-Mobile-SetPushNotificationsDeviceTokenString-System-String-'></a>
### SetPushNotificationsDeviceTokenString(token) `method`

##### Summary

Sets device token in the purpose of uninstall tracking.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| token | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The device token from APNs |

<a name='T-Facebook-Unity-Mobile-MobileFacebook'></a>
## MobileFacebook `type`

##### Namespace

Facebook.Unity.Mobile

##### Summary

Classes defined on the mobile sdks.

<a name='P-Facebook-Unity-Mobile-MobileFacebook-ShareDialogMode'></a>
### ShareDialogMode `property`

##### Summary

Gets or sets the dialog mode.

<a name='T-Facebook-Unity-OGActionType'></a>
## OGActionType `type`

##### Namespace

Facebook.Unity

##### Summary

OG action type.

<a name='F-Facebook-Unity-OGActionType-ASKFOR'></a>
### ASKFOR `constants`

##### Summary

ASKFOR Action.

<a name='F-Facebook-Unity-OGActionType-SEND'></a>
### SEND `constants`

##### Summary

SEND Action.

<a name='F-Facebook-Unity-OGActionType-TURN'></a>
### TURN `constants`

##### Summary

TURN Action.

<a name='T-Facebook-Unity-Product'></a>
## Product `type`

##### Namespace

Facebook.Unity

##### Summary

Contains a Instant Game Product.

<a name='M-Facebook-Unity-Product-#ctor-System-String,System-String,System-String,System-String,System-String,System-Nullable{System-Double},System-String-'></a>
### #ctor(title,productID,description,imageURI,price,priceAmount,priceCurrencyCode) `constructor`

##### Summary

Initializes a new instance of the [Product](#T-Facebook-Unity-Product 'Facebook.Unity.Product') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| title | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The title of the product. |
| productID | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The product's game-specified identifier. |
| description | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The product description. |
| imageURI | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A link to the product's associated image. |
| price | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The price of the product. |
| priceAmount | [System.Nullable{System.Double}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Double}') | The numeric price of a product. |
| priceCurrencyCode | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The currency code for the product. |

<a name='P-Facebook-Unity-Product-Description'></a>
### Description `property`

##### Summary

Gets the description.

<a name='P-Facebook-Unity-Product-ImageURI'></a>
### ImageURI `property`

##### Summary

Gets the image uniform resource identifier.

<a name='P-Facebook-Unity-Product-Price'></a>
### Price `property`

##### Summary

Gets the price.

<a name='P-Facebook-Unity-Product-PriceAmount'></a>
### PriceAmount `property`

##### Summary

Gets the price amount.

<a name='P-Facebook-Unity-Product-PriceCurrencyCode'></a>
### PriceCurrencyCode `property`

##### Summary

Gets the price currency code.

<a name='P-Facebook-Unity-Product-ProductID'></a>
### ProductID `property`

##### Summary

Gets the product identifier.

<a name='P-Facebook-Unity-Product-Title'></a>
### Title `property`

##### Summary

Gets the title.

<a name='M-Facebook-Unity-Product-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [Product](#T-Facebook-Unity-Product 'Facebook.Unity.Product').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [Product](#T-Facebook-Unity-Product 'Facebook.Unity.Product').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-Profile'></a>
## Profile `type`

##### Namespace

Facebook.Unity

<a name='M-Facebook-Unity-Profile-#ctor-System-String,System-String,System-String,System-String,System-String,System-String,System-String,System-String,System-String[],System-String,Facebook-Unity-UserAgeRange,Facebook-Unity-FBLocation,Facebook-Unity-FBLocation,System-String-'></a>
### #ctor(userID,firstName,middleName,lastName,name,email,imageURL,linkURL,friendIDs,birthday,ageRange,hometown,location,gender) `constructor`

##### Summary

Initializes a new instance of the [Profile](#T-Facebook-Unity-Profile 'Facebook.Unity.Profile') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userID | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | User ID. |
| firstName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | First Name. |
| middleName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Middle Name. |
| lastName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Last Name. |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name. |
| email | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Email. |
| imageURL | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Image URL. |
| linkURL | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Link URL. |
| friendIDs | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | A list of identifiers for the user's friends. |
| birthday | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | User's birthday |
| ageRange | [Facebook.Unity.UserAgeRange](#T-Facebook-Unity-UserAgeRange 'Facebook.Unity.UserAgeRange') | Age Range for the User |
| hometown | [Facebook.Unity.FBLocation](#T-Facebook-Unity-FBLocation 'Facebook.Unity.FBLocation') | Home Town |
| location | [Facebook.Unity.FBLocation](#T-Facebook-Unity-FBLocation 'Facebook.Unity.FBLocation') | Location |
| gender | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Gender |

<a name='P-Facebook-Unity-Profile-AgeRange'></a>
### AgeRange `property`

##### Summary

Gets the user's age range.

<a name='P-Facebook-Unity-Profile-Birthday'></a>
### Birthday `property`

##### Summary

Gets the user's birthday.

<a name='P-Facebook-Unity-Profile-Email'></a>
### Email `property`

##### Summary

Gets the email.

<a name='P-Facebook-Unity-Profile-FirstName'></a>
### FirstName `property`

##### Summary

Gets the fist name.

<a name='P-Facebook-Unity-Profile-FriendIDs'></a>
### FriendIDs `property`

##### Summary

Gets the list of identifiers for the user's friends.

<a name='P-Facebook-Unity-Profile-Gender'></a>
### Gender `property`

##### Summary

Gets the user's gender

<a name='P-Facebook-Unity-Profile-Hometown'></a>
### Hometown `property`

##### Summary

Gets the user's hometown

<a name='P-Facebook-Unity-Profile-ImageURL'></a>
### ImageURL `property`

##### Summary

Gets the image URL.

<a name='P-Facebook-Unity-Profile-LastName'></a>
### LastName `property`

##### Summary

Gets the last name.

<a name='P-Facebook-Unity-Profile-LinkURL'></a>
### LinkURL `property`

##### Summary

Gets the link URL.

<a name='P-Facebook-Unity-Profile-Location'></a>
### Location `property`

##### Summary

Gets the user's location

<a name='P-Facebook-Unity-Profile-MiddleName'></a>
### MiddleName `property`

##### Summary

Gets the middle name.

<a name='P-Facebook-Unity-Profile-Name'></a>
### Name `property`

##### Summary

Gets the name.

<a name='P-Facebook-Unity-Profile-UserID'></a>
### UserID `property`

##### Summary

Gets the user ID.

<a name='M-Facebook-Unity-Profile-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [Profile](#T-Facebook-Unity-Profile 'Facebook.Unity.Profile').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [Profile](#T-Facebook-Unity-Profile 'Facebook.Unity.Profile').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-Purchase'></a>
## Purchase `type`

##### Namespace

Facebook.Unity

##### Summary

Contains a Instant Game Purchase.

<a name='M-Facebook-Unity-Purchase-#ctor-System-String,System-Boolean,System-String,System-String,System-String,System-String,System-Collections-Generic-IDictionary{System-String,System-Object},System-Int64,System-String,System-String-'></a>
### #ctor(developerPayload,isConsumed,paymentActionType,paymentID,productID,purchasePlatform,purchasePrice,purchaseTime,purchaseToken,signedRequest) `constructor`

##### Summary

Initializes a new instance of the [Purchase](#T-Facebook-Unity-Purchase 'Facebook.Unity.Purchase') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| developerPayload | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A developer-specified string, provided during the purchase of the product. |
| isConsumed | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Whether or not the purchase has been consumed. |
| paymentActionType | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The current status of the purchase. |
| paymentID | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The identifier for the purchase transaction. |
| productID | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The product's game-specified identifier. |
| purchasePlatform | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The purchase platform, such as "GOOGLE" or "FB". |
| purchasePrice | [System.Collections.Generic.IDictionary{System.String,System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IDictionary 'System.Collections.Generic.IDictionary{System.String,System.Object}') | Contains the local amount and currency associated with the purchased item. |
| purchaseTime | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | Unix timestamp of when the purchase occurred. |
| purchaseToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A token representing the purchase that may be used to consume the purchase. |
| signedRequest | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Server-signed encoding of the purchase request. |

<a name='P-Facebook-Unity-Purchase-DeveloperPayload'></a>
### DeveloperPayload `property`

##### Summary

Gets the developer payload string.

<a name='P-Facebook-Unity-Purchase-IsConsumed'></a>
### IsConsumed `property`

##### Summary

Gets whether or not the purchase has been consumed.

<a name='P-Facebook-Unity-Purchase-PaymentActionType'></a>
### PaymentActionType `property`

##### Summary

Gets the purchase status.

<a name='P-Facebook-Unity-Purchase-PaymentID'></a>
### PaymentID `property`

##### Summary

Gets the purchase identifier.

<a name='P-Facebook-Unity-Purchase-ProductID'></a>
### ProductID `property`

##### Summary

Gets the product identifier.

<a name='P-Facebook-Unity-Purchase-PurchasePlatform'></a>
### PurchasePlatform `property`

##### Summary

Gets the platform associated with the purchase.

<a name='P-Facebook-Unity-Purchase-PurchasePrice'></a>
### PurchasePrice `property`

##### Summary

Gets the amount and currency fields associated with the purchase.

<a name='P-Facebook-Unity-Purchase-PurchaseTime'></a>
### PurchaseTime `property`

##### Summary

Gets the purchase time.

<a name='P-Facebook-Unity-Purchase-PurchaseToken'></a>
### PurchaseToken `property`

##### Summary

Gets the price.

<a name='P-Facebook-Unity-Purchase-SignedRequest'></a>
### SignedRequest `property`

##### Summary

Gets the price currency code.

<a name='M-Facebook-Unity-Purchase-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [Purchase](#T-Facebook-Unity-Purchase 'Facebook.Unity.Purchase').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents a [Purchase](#T-Facebook-Unity-Purchase 'Facebook.Unity.Purchase').

##### Parameters

This method has no parameters.

<a name='T-Facebook-Unity-ShareDialogMode'></a>
## ShareDialogMode `type`

##### Namespace

Facebook.Unity

##### Summary

Share dialog mode.

<a name='F-Facebook-Unity-ShareDialogMode-AUTOMATIC'></a>
### AUTOMATIC `constants`

##### Summary

The sdk will choose which type of dialog to show
See the Facebook SDKs for ios and android for specific details.

<a name='F-Facebook-Unity-ShareDialogMode-FEED'></a>
### FEED `constants`

##### Summary

Uses the feed dialog.

<a name='F-Facebook-Unity-ShareDialogMode-NATIVE'></a>
### NATIVE `constants`

##### Summary

Uses the dialog inside the native facebook applications. Note this will fail if the
native applications are not installed.

<a name='F-Facebook-Unity-ShareDialogMode-WEB'></a>
### WEB `constants`

##### Summary

Opens the facebook dialog in a webview.

<a name='T-Facebook-Unity-UserAgeRange'></a>
## UserAgeRange `type`

##### Namespace

Facebook.Unity

<a name='P-Facebook-Unity-UserAgeRange-Max'></a>
### Max `property`

##### Summary

Gets the user's maximun age, -1 if unspecified.

<a name='P-Facebook-Unity-UserAgeRange-Min'></a>
### Min `property`

##### Summary

Gets the user's minimun age, -1 if unspecified.

<a name='M-Facebook-Unity-UserAgeRange-ToString'></a>
### ToString() `method`

##### Summary

Returns a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [UserAgeRange](#T-Facebook-Unity-UserAgeRange 'Facebook.Unity.UserAgeRange').

##### Returns

A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') that represents the current [UserAgeRange](#T-Facebook-Unity-UserAgeRange 'Facebook.Unity.UserAgeRange').

##### Parameters

This method has no parameters.
