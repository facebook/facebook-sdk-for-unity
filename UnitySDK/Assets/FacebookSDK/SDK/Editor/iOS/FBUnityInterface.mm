// Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
//
// You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
// copy, modify, and distribute this software in source code or binary form for use
// in connection with the web services and APIs provided by Facebook.
//
// As with any software that integrates with the Facebook platform, your use of
// this software is subject to the Facebook Developer Principles and Policies
// [http://developers.facebook.com/policy/]. This copyright notice shall be
// included in all copies or substantial portions of the software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#import <SafariServices/SafariServices.h>
#import <AuthenticationServices/AuthenticationServices.h>
#import <FBSDKCoreKit/FBSDKCoreKit-Swift.h>
#import <FBSDKLoginKit/FBSDKLoginKit-Swift.h>
#import <Foundation/NSJSONSerialization.h>

#include "FBUnityInterface.h"
#include "FBUnitySDKDelegate.h"
#include "FBUnityUtility.h"
#include "FBSDK+Internal.h"

@interface FBUnityInterface()

@property (nonatomic, copy) NSString *openURLString;

@end

@implementation FBUnityInterface

#pragma mark Object Initialization

+ (FBUnityInterface *)sharedInstance
{
  static dispatch_once_t pred;
  static FBUnityInterface *shared = nil;

  dispatch_once(&pred, ^{
    shared = [[FBUnityInterface alloc] init];
    shared.shareDialogMode = ShareDialogMode::AUTOMATIC;
  });

  return shared;
}

+ (void)load
{
  UnityRegisterAppDelegateListener([FBUnityInterface sharedInstance]);
}

#pragma mark - App (Delegate) Lifecycle

// didBecomeActive: and onOpenURL: are called by Unity's AppController
// because we implement <AppDelegateListener> and registered via UnityRegisterAppDelegateListener(...) above.

- (void)didFinishLaunching:(NSNotification *)notification
{
  [[FBSDKApplicationDelegate sharedInstance] application:[UIApplication sharedApplication]
                           didFinishLaunchingWithOptions:notification.userInfo];
}

- (void)didBecomeActive:(NSNotification *)notification
{

}

- (void)onOpenURL:(NSNotification *)notification
{
  NSURL *url = notification.userInfo[@"url"];
  BOOL isHandledByFBSDK = [[FBSDKApplicationDelegate sharedInstance] application:[UIApplication sharedApplication]
                                                                         openURL:url
                                                               sourceApplication:notification.userInfo[@"sourceApplication"]
                                                                      annotation:notification.userInfo[@"annotation"]];
  if (!isHandledByFBSDK) {
    [FBUnityInterface sharedInstance].openURLString = [url absoluteString];
  }
}

#pragma mark - Implementation

- (void)configureAppId:(const char *)appId
  frictionlessRequests:(bool)frictionlessRequests
             urlSuffix:(const char *)urlSuffix
{
  self.useFrictionlessRequests = frictionlessRequests;

  if(appId) {
    [FBSDKSettings.sharedSettings setAppID:[FBUnityUtility stringFromCString:appId]];
  }

  if(urlSuffix && strlen(urlSuffix) > 0) {
    [FBSDKSettings.sharedSettings setAppURLSchemeSuffix:[FBUnityUtility stringFromCString:urlSuffix]];
  }

  NSDictionary *userData = [self getAccessTokenUserData] ?: @{};

  [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnInitComplete userData:userData requestId:0];
}

- (void)logInWithPublishPermissions:(int) requestId
                             scope:(const char *)scope
{
  [self startLogin:requestId scope:scope isPublishPermLogin:YES];
}

- (void)logInWithReadPermissions:(int) requestId
                           scope:(const char *)scope
{
  [self startLogin:requestId scope:scope isPublishPermLogin:NO];
}

- (void)startLogin:(int) requestId
             scope:(const char *)scope
isPublishPermLogin:(BOOL)isPublishPermLogin
{
  NSString *scopeStr = [FBUnityUtility stringFromCString:scope];
  NSArray *permissions = nil;
  if(scope && strlen(scope) > 0) {
    permissions = [scopeStr componentsSeparatedByString:@","];
  }

  void (^loginHandler)(FBSDKLoginManagerLoginResult *,NSError *) = ^(FBSDKLoginManagerLoginResult *result, NSError *error) {
    if (error) {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnLoginComplete error:error requestId:requestId];
      return;
    } else if (result.isCancelled) {
      [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnLoginComplete requestId:requestId];
      return;
    }

    if ([self tryCompleteLoginWithRequestId:requestId]) {
      return;
    } else {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnLoginComplete errorMessage:@"Unknown login error" requestId:requestId];
    }
  };

  FBSDKLoginManager *login = [[FBSDKLoginManager alloc] init];
  [login logInWithPermissions:permissions
           fromViewController:nil
                      handler:loginHandler];
}

- (void)loginWithTrackingPreference:(int)requestId
                              scope:(const char *)scope
                           tracking:(const char *)tracking
                              nonce:(const char *)nonce
{
  NSString *scopeStr = [FBUnityUtility stringFromCString:scope];
  NSArray *permissions = nil;
  if(scope && strlen(scope) > 0) {
    permissions = [scopeStr componentsSeparatedByString:@","];
  }

  NSString *trackingStr = [FBUnityUtility stringFromCString:tracking];
  NSString *nonceStr = nil;
  if (nonce) {
    nonceStr = [FBUnityUtility stringFromCString:nonce];
  }
  FBSDKLoginConfiguration *config;
  if (nonce) {
    config = [[FBSDKLoginConfiguration alloc] initWithPermissions:permissions tracking:([trackingStr isEqualToString:@"enabled"] ? FBSDKLoginTrackingEnabled : FBSDKLoginTrackingLimited) nonce:nonceStr];
  } else {
    config = [[FBSDKLoginConfiguration alloc] initWithPermissions:permissions tracking:([trackingStr isEqualToString:@"enabled"] ? FBSDKLoginTrackingEnabled : FBSDKLoginTrackingLimited)];
  }

  void (^loginHandler)(FBSDKLoginManagerLoginResult *,NSError *) = ^(FBSDKLoginManagerLoginResult *result, NSError *error) {
    if (error) {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnLoginComplete error:error requestId:requestId];
      return;
    } else if (result.isCancelled) {
      [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnLoginComplete requestId:requestId];
      return;
    }

    if ([self tryCompleteLoginWithRequestId:requestId]) {
      return;
    } else {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnLoginComplete errorMessage:@"Unknown login error" requestId:requestId];
    }
  };

  FBSDKLoginManager *login = [[FBSDKLoginManager alloc] init];
  [login logInFromViewController:nil configuration:config completion:loginHandler];
}

- (void)logOut
{
  FBSDKLoginManager *login = [[FBSDKLoginManager alloc] init];
  [login logOut];
  [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnLogoutComplete userData:@{} requestId:0];
}

- (void)appRequestWithRequestId:(int)requestId
                        message:(const char *)message
                     actionType:(const char *)actionType
                       objectId:(const char *)objectId
                             to:(const char **)to
                       toLength:(int)toLength
                        filters:(const char *)filters
                           data:(const char *)data
                          title:(const char *)title
{
  FBSDKGameRequestContent *content = [[FBSDKGameRequestContent alloc] init];
  content.message = [FBUnityUtility stringFromCString:message];
  content.actionType = [FBUnityUtility gameRequestActionTypeFromString:[FBUnityUtility stringFromCString:actionType]];
  content.objectID = [FBUnityUtility stringFromCString:objectId];
  if(to && toLength) {
    NSMutableArray *toArray = [NSMutableArray array];
    for(int i = 0; i < toLength; i++) {
      [toArray addObject:[FBUnityUtility stringFromCString:to[i]]];
    }
    content.recipients = toArray;
  }
  content.filters = [FBUnityUtility gameRequestFilterFromString:[FBUnityUtility stringFromCString:filters]];
  content.data = [FBUnityUtility stringFromCString:data];
  content.title = [FBUnityUtility stringFromCString:title];

  FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestId];
  NSError *error;
  FBSDKGameRequestDialog *dialog = [[FBSDKGameRequestDialog alloc] initWithContent:content delegate:delegate];
  dialog.isFrictionlessRequestsEnabled = self.useFrictionlessRequests;

  if (![dialog validateWithError:&error]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnAppRequestsComplete error:error requestId:requestId];
  }
  if (![dialog show]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnAppRequestsComplete errorMessage:@"Failed to show request dialog" requestId:requestId];
  }
}

- (void)shareLinkWithRequestId:(int)requestId
                    contentURL:(const char *)contentURL
                  contentTitle:(const char *)contentTitle
            contentDescription:(const char *)contentDescription
                      photoURL:(const char *)photoURL
{
  FBSDKShareLinkContent *linkContent = [[FBSDKShareLinkContent alloc] init];

  NSString *contentUrlStr = [FBUnityUtility stringFromCString:contentURL];
  if (contentUrlStr) {
    linkContent.contentURL = [NSURL URLWithString:contentUrlStr];
  }

  [self shareContentWithRequestId:requestId
                     shareContent:linkContent
                       dialogMode:[self getDialogMode]];
}

- (void)shareFeedWithRequestId:(int)requestId
                          toId:(const char *)toID
                          link:(const char *)link
                      linkName:(const char *)linkName
                   linkCaption:(const char *)linkCaption
               linkDescription:(const char *)linkDescription
                       picture:(const char *)picture
                   mediaSource:(const char *)mediaSource
{
  FBSDKShareLinkContent *linkContent = [[FBSDKShareLinkContent alloc] init];
  NSString *contentUrlStr = [FBUnityUtility stringFromCString:link];
  if (contentUrlStr) {
    linkContent.contentURL = [NSURL URLWithString:contentUrlStr];
  }

  NSMutableDictionary *feedParameters = [[NSMutableDictionary alloc] init];
  NSString *toStr = [FBUnityUtility stringFromCString:toID];
  if (toStr) {
    [feedParameters setObject:toStr forKey:@"to"];
  }

  NSString *captionStr = [FBUnityUtility stringFromCString:linkCaption];
  if (captionStr) {
    [feedParameters setObject:captionStr forKey:@"caption"];
  }

  NSString *sourceStr = [FBUnityUtility stringFromCString:mediaSource];
  if (sourceStr) {
    [feedParameters setObject:sourceStr forKey:@"source"];
  }

  [linkContent addParameters:feedParameters bridgeOptions:FBSDKShareBridgeOptionsDefault];
  [self shareContentWithRequestId:requestId
                     shareContent:linkContent
                       dialogMode:FBSDKShareDialogModeFeedWeb];
}

- (void)shareContentWithRequestId:(int)requestId
                     shareContent:(FBSDKShareLinkContent *)linkContent
                       dialogMode:(FBSDKShareDialogMode)dialogMode
{
  FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestId];
  FBSDKShareDialog *dialog = [[FBSDKShareDialog alloc] initWithViewController:nil
                                                                      content:linkContent
                                                                     delegate:delegate];
  dialog.mode = dialogMode;

  NSError *error;
  if (![dialog validateWithError:&error]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnShareLinkComplete error:error requestId:requestId];
  }
  if (![dialog show]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnShareLinkComplete errorMessage:@"Failed to show share dialog" requestId:requestId];
  }
}

- (FBSDKShareDialogMode)getDialogMode
{
  switch (self.shareDialogMode) {
    case ShareDialogMode::AUTOMATIC:
      return FBSDKShareDialogModeAutomatic;
    case ShareDialogMode::NATIVE:
      return FBSDKShareDialogModeNative;
    case ShareDialogMode::WEB:
      return FBSDKShareDialogModeWeb;
    case ShareDialogMode::FEED:
      return FBSDKShareDialogModeFeedWeb;
    default:
      NSLog(@"Unexpected dialog mode: %@", [NSNumber numberWithInt:self.shareDialogMode]);
      return FBSDKShareDialogModeAutomatic;
  }
}

- (BOOL)tryCompleteLoginWithRequestId:(int) requestId
{
  NSMutableDictionary *userData = [[NSMutableDictionary alloc] init];
  NSDictionary *accessTokenUserData = [self getAccessTokenUserData];
  NSDictionary *authenticationTokenUserData = [self getAuthenticationTokenUserData];
  if (accessTokenUserData) {
    [userData addEntriesFromDictionary:accessTokenUserData];
  }
  if (authenticationTokenUserData) {
    [userData addEntriesFromDictionary:authenticationTokenUserData];
  }
  if (userData) {
    [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnLoginComplete
                              userData:[userData copy]
                             requestId:requestId];
    return YES;
  } else {
    return NO;
  }
}

- (NSDictionary *)getAccessTokenUserData
{
  FBSDKAccessToken *token = [FBSDKAccessToken currentAccessToken];
  if (token) {
    // Old v3 sdk tokens don't always contain a UserID. If the user ID is null
    // treat the token as bad and clear it. These values are all required
    // on c# side for initlizing a token.
    NSDictionary *userData = [FBUnityUtility getUserDataFromAccessToken:token];
    if (userData) {
      return userData;
    } else {
      // The token is missing a required value. Clear the token
      [[[FBSDKLoginManager alloc] init] logOut];
    }
  }

  return nil;
}

- (NSDictionary *)getAuthenticationTokenUserData
{
  FBSDKAuthenticationToken *token = [FBSDKAuthenticationToken currentAuthenticationToken];
  if (token.tokenString && token.nonce) {
    return @{
      @"auth_token_string": token.tokenString,
      @"auth_nonce": token.nonce
    };
  }

  return nil;
}

@end

#pragma mark - Actual Unity C# interface (extern C)

extern "C" {

  void IOSFBSendViewHierarchy(const char *_tree )
  {
    Class FBUnityUtility = NSClassFromString(@"FBSDKCodelessIndexer");
    [FBUnityUtility performSelector:NSSelectorFromString(@"uploadIndexing:")
                           withObject:[NSString stringWithUTF8String:_tree]];
  }

  void IOSFBInit(const char *_appId, bool _frictionlessRequests, const char *_urlSuffix, const char *_userAgentSuffix)
  {
    // Set the user agent before calling init to ensure that calls made during
    // init use the user agent suffix.
    [FBSDKSettings.sharedSettings setUserAgentSuffix:[FBUnityUtility stringFromCString:_userAgentSuffix]];

    [[FBUnityInterface sharedInstance] configureAppId:_appId
                                 frictionlessRequests:_frictionlessRequests
                                            urlSuffix:_urlSuffix];
    [[FBSDKAppEvents shared] setIsUnityInitialized:true];
    [[FBSDKAppEvents shared] sendEventBindingsToUnity];
  }

  void IOSFBEnableProfileUpdatesOnAccessTokenChange(bool enable)
  {
    [FBSDKProfile enableUpdatesOnAccessTokenChange:enable];
  }

  void IOSFBLoginWithTrackingPreference(int requestId, const char *scope, const char *tracking, const char *nonce)
  {
    [[FBUnityInterface sharedInstance] loginWithTrackingPreference:requestId scope:scope
                                                          tracking:tracking
                                                            nonce:nonce];
  }

  void IOSFBLogInWithReadPermissions(int requestId,
                                   const char *scope)
  {
    [[FBUnityInterface sharedInstance] logInWithReadPermissions:requestId scope:scope];
  }

  void IOSFBLogInWithPublishPermissions(int requestId,
                                      const char *scope)
  {
    [[FBUnityInterface sharedInstance] logInWithPublishPermissions:requestId scope:scope];
  }

  void IOSFBLogOut()
  {
    [[FBUnityInterface sharedInstance] logOut];
  }

  char* IOSFBCurrentAuthenticationToken()
  {
    FBSDKAuthenticationToken *token = [FBSDKAuthenticationToken currentAuthenticationToken];
    NSString *str = @"";
    if (token.tokenString && token.nonce) {
      try {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:@{
          @"auth_token_string": token.tokenString,
          @"auth_nonce": token.nonce
        } options:NSJSONWritingPrettyPrinted error:nil];
        if (jsonData) {
          str = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        }
      } catch (NSException *exception) {
        NSLog(@"Fail to parse AuthenticationToken");
      }
    }
    const char* string = [str UTF8String];
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
  }

  char* IOSFBCurrentProfile()
  {
    FBSDKProfile *profile = [FBSDKProfile currentProfile];
    NSString *str = @"";
    NSMutableDictionary<NSString *, id> *data = [NSMutableDictionary new];
    if (profile.userID) {
      data[@"userID"] = profile.userID;
    }
    if (profile.firstName) {
      data[@"firstName"] = profile.firstName;
    }
    if (profile.middleName) {
      data[@"middleName"] = profile.middleName;
    }
    if (profile.lastName) {
      data[@"lastName"] = profile.lastName;
    }
    if (profile.name) {
      data[@"name"] = profile.name;
    }
    if (profile.email) {
      data[@"email"] = profile.email;
    }
    if (profile.imageURL) {
      data[@"imageURL"] = profile.imageURL.absoluteString;
    }
    if (profile.linkURL) {
      data[@"linkURL"] = profile.linkURL.absoluteString;
    }
    if (profile.friendIDs) {
      data[@"friendIDs"] = [profile.friendIDs componentsJoinedByString:@","];
    }
    if (profile.birthday) {
      data[@"birthday"] = [NSString stringWithFormat:@"%@", @((time_t)[profile.birthday timeIntervalSince1970])];
    }
    if (profile.ageRange) {
      if (profile.ageRange.min) {
        data[@"ageMin"] = profile.ageRange.min.stringValue;
      }
      if (profile.ageRange.max) {
        data[@"ageMax"] = profile.ageRange.max.stringValue;
      }
    }

    if (profile.hometown) {
      data[@"hometown_id"] = profile.hometown.id;
      data[@"hometown_name"] = profile.hometown.name;
    }

    if (profile.location) {
      data[@"location_id"] = profile.location.id;
      data[@"location_name"] = profile.location.name;
    }

    if (profile.gender) {
      data[@"gender"] = profile.gender;
    }

    try {
      NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:NSJSONWritingPrettyPrinted error:nil];
      if (jsonData) {
        str = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
      }
    } catch (NSException *exception) {
      NSLog(@"Fail to parse Profile");
    }
    const char* string = [str UTF8String];
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
  }

  void IOSFBSetPushNotificationsDeviceTokenString(const char *token)
  {
    [[FBSDKAppEvents shared] setPushNotificationsDeviceTokenString:[FBUnityUtility stringFromCString:token]];
  }

  void IOSFBSetShareDialogMode(int mode)
  {
    [FBUnityInterface sharedInstance].shareDialogMode = static_cast<ShareDialogMode>(mode);
  }

  void IOSFBAppRequest(int requestId,
                     const char *message,
                     const char *actionType,
                     const char *objectId,
                     const char **to,
                     int toLength,
                     const char *filters,
                     const char **excludeIds, //not supported on mobile
                     int excludeIdsLength, //not supported on mobile
                     bool hasMaxRecipients, //not supported on mobile
                     int maxRecipients, //not supported on mobile
                     const char *data,
                     const char *title)
  {
    [[FBUnityInterface sharedInstance] appRequestWithRequestId: requestId
                                                       message: message
                                                    actionType: actionType
                                                      objectId: objectId
                                                            to: to
                                                      toLength: toLength
                                                       filters: filters
                                                          data: data
                                                         title: title];
  }

  void IOSFBGetAppLink(int requestId)
  {
    NSURL *url = [NSURL URLWithString:[FBUnityInterface sharedInstance].openURLString];
    [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnGetAppLinkComplete
                              userData:[FBUnityUtility appLinkDataFromUrl:url]
                             requestId:requestId];
    [FBUnityInterface sharedInstance].openURLString = nil;
  }

  void IOSFBShareLink(int requestId,
                    const char *contentURL,
                    const char *contentTitle,
                    const char *contentDescription,
                    const char *photoURL)
  {
    [[FBUnityInterface sharedInstance] shareLinkWithRequestId:requestId
                                                   contentURL:contentURL
                                                 contentTitle:contentTitle
                                           contentDescription:contentDescription
                                                     photoURL:photoURL];
  }

  void IOSFBFeedShare(int requestId,
                    const char *toId,
                    const char *link,
                    const char *linkName,
                    const char *linkCaption,
                    const char *linkDescription,
                    const char *picture,
                    const char *mediaSource)
  {
    [[FBUnityInterface sharedInstance] shareFeedWithRequestId:requestId
                                                         toId:toId
                                                         link:link
                                                     linkName:linkName
                                                  linkCaption:linkCaption
                                              linkDescription:linkDescription
                                                      picture:picture
                                                  mediaSource:mediaSource];
  }

  void IOSFBAppEventsActivateApp()
  {
    [FBSDKAppEvents.shared activateApp];
  }

  void IOSFBAppEventsLogEvent(const char *eventName,
                              double valueToSum,
                              int numParams,
                              const char **paramKeys,
                              const char **paramVals)
  {
    NSDictionary *params =  [FBUnityUtility dictionaryFromKeys:paramKeys values:paramVals length:numParams];
    [[FBSDKAppEvents shared] logEvent:[FBUnityUtility stringFromCString:eventName] valueToSum:valueToSum parameters:params];
  }

  void IOSFBAppEventsLogPurchase(double amount,
                                 const char *currency,
                                 int numParams,
                                 const char **paramKeys,
                                 const char **paramVals)
  {
    NSDictionary *params =  [FBUnityUtility dictionaryFromKeys:paramKeys values:paramVals length:numParams];
    [[FBSDKAppEvents shared] logPurchase:amount currency:[FBUnityUtility stringFromCString:currency] parameters:params];
  }

  void IOSFBAppEventsSetLimitEventUsage(BOOL limitEventUsage)
  {
    [FBSDKSettings.sharedSettings setIsEventDataUsageLimited:limitEventUsage];
  }

  void IOSFBAutoLogAppEventsEnabled(BOOL autoLogAppEventsEnabledID)
  {
    [FBSDKSettings.sharedSettings setAutoLogAppEventsEnabled:autoLogAppEventsEnabledID];
  }

  void IOSFBAdvertiserIDCollectionEnabled(BOOL advertiserIDCollectionEnabledID)
  {
    [FBSDKSettings.sharedSettings setAdvertiserIDCollectionEnabled:advertiserIDCollectionEnabledID];
  }

  BOOL IOSFBAdvertiserTrackingEnabled(BOOL advertiserTrackingEnabled)
  {
    [FBSDKSettings.sharedSettings setAdvertiserTrackingEnabled:advertiserTrackingEnabled];
    return [FBSDKSettings.sharedSettings isAdvertiserTrackingEnabled];
  }

  char* IOSFBSdkVersion()
  {
    const char* string = [[FBSDKSettings.sharedSettings sdkVersion] UTF8String];
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
  }

  void IOSFBSetUserID(const char *userID)
  {
    [[FBSDKAppEvents shared] setUserID:[FBUnityUtility stringFromCString:userID]];
  }

  void IOSFBOpenGamingServicesFriendFinder(int requestId)
  {
    [FBSDKFriendFinderDialog
       launchFriendFinderDialogWithCompletionHandler:^(BOOL success, NSError * _Nullable error) {
        if (!success || error) {
            [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnFriendFinderComplete error:error requestId:requestId];
        } else {
           [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnFriendFinderComplete
                                    userData:NULL
                                    requestId:requestId];
        }
      }];
  }

  void IOSFBCreateGamingContext(int requestID, const char *playerID) {
    NSError *error;
    NSString *playerIDString = [FBUnityUtility stringFromCString:playerID];
    FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestID];
    FBSDKCreateContextContent *content = [[FBSDKCreateContextContent alloc] initDialogContentWithPlayerID:playerIDString];
    FBSDKContextDialogPresenter *presenter = [[FBSDKContextDialogPresenter alloc] init];
    [presenter makeAndShowCreateContextDialogWithContent:content delegate:delegate error:&error];
    if (error) {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnGetTournamentsComplete error:error requestId:requestID];
    }
  }

  void IOSFBSwitchGamingContext(
    int requestID,
    const char *contextID) {
    NSError *error;
    NSString *contextIDString = [FBUnityUtility stringFromCString:contextID];
    FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestID];
    FBSDKSwitchContextContent *content = [[FBSDKSwitchContextContent alloc] initDialogContentWithContextID:contextIDString];
    FBSDKContextDialogPresenter *presenter = [[FBSDKContextDialogPresenter alloc] init];
    [presenter makeAndShowSwitchContextDialogWithContent:content delegate:delegate error:&error];
    if (error) {
      [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnGetTournamentsComplete error:error requestId:requestID];
    }
   }

   void IOSFBChooseGamingContext(
     int requestID,
     const char *filter,
     int minSize,
     int maxSize)
   {
     FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestID];
     FBSDKChooseContextContent *chooseContent = [FBSDKChooseContextContent alloc];

     NSString *filterNSString = [NSString stringWithUTF8String:filter];
     if ([filterNSString length] == 0) {
        chooseContent.filter = FBSDKChooseContextFilterNone;
     } else if ([filterNSString isEqualToString:@"NEW_PLAYERS_ONLY"]) {
       chooseContent.filter = FBSDKChooseContextFilterNewPlayersOnly;
     } else if ([filterNSString isEqualToString:@"INCLUDE_EXISTING_CHALLENGES"]) {
       chooseContent.filter = FBSDKChooseContextFilterExistingChallenges;
     } else if ([filterNSString isEqualToString:@"NEW_CONTEXT_ONLY"]) {
       chooseContent.filter = FBSDKChooseContextFilterNewContextOnly;
     }

     if (minSize > 0) {
       chooseContent.minParticipants = minSize;
     }
     if (maxSize > 0) {
       chooseContent.maxParticipants = maxSize;
     }

     FBSDKContextDialogPresenter *presenter = [[FBSDKContextDialogPresenter alloc] init];
     [presenter makeAndShowChooseContextDialogWithContent:chooseContent delegate:delegate];
   }

  void IOSFBGetCurrentGamingContext(int requestID)
  {
      FBSDKGamingContext *currentContext = [FBSDKGamingContext currentContext];
      if (currentContext) {
          [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnGetCurrentGamingContextComplete
            userData:@{@"contextId":[currentContext identifier]}
            requestId:requestID];
      } else {
          [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnGetCurrentGamingContextComplete
            userData:NULL
            requestId:requestID];
      }
  }

  void IOSFBSetDataProcessingOptions(
    const char** options,
    int numOptions,
    int country,
    int state) {
    NSMutableArray<NSString*>* array = [[NSMutableArray alloc] init];
    for (int i = 0; i < numOptions; i++) {
      NSString* option = [FBUnityUtility stringFromCString:options[i]];
      if (option) {
        [array addObject:option];
      }
    }
    [FBSDKSettings.sharedSettings setDataProcessingOptions:array country:country state:state];
  }

  void IOSFBGetTournaments(int requestID)
  {
    FBSDKTournamentFetcher *fetcher = [[FBSDKTournamentFetcher alloc] init];
    [fetcher fetchTournamentsWithCompletionHandler:^(NSArray<FBSDKTournament *> * tournaments, NSError * error) {
      if (error) {
        [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnGetTournamentsComplete error:error requestId:requestID];
      }

      NSMutableDictionary *userData = [NSMutableDictionary new];
      for (FBSDKTournament *tournament in tournaments) {
        userData[tournament.identifier] = [tournament toDictionary];
      }

      [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnGetTournamentsComplete
                                userData:userData
                               requestId:requestID];
    }];
  }

  void IOSFBUpdateTournament(const char *tournamentID, int score, int requestID)
  {
    FBSDKTournamentUpdater *updater = [[FBSDKTournamentUpdater alloc] init];
    [updater updateWithTournamentID:[NSString stringWithUTF8String:tournamentID]
                              score:score
                  completionHandler:^(BOOL success, NSError * _Nullable error) {
      if (!success || error) {
        [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnUpdateTournamentComplete error:error requestId:requestID];
      } else {
        [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnUpdateTournamentComplete
                                  userData:NULL
                                 requestId:requestID];
      }
    }];
  }

  void IOSFBUpdateAndShareTournament(const char *tournamentID, int score, int requestID)
  {
    NSError *error;
    FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestID];
    FBSDKShareTournamentDialog *dialog = [[FBSDKShareTournamentDialog alloc] initWithDelegate: delegate];
    [dialog showWithScore:score tournamentID:[NSString stringWithUTF8String:tournamentID] error:&error];
      if (error) {
        [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnGetTournamentsComplete error:error requestId:requestID];
      }
  }

  void IOSFBCreateAndShareTournament(
    int initialScore,
    const char *title,
    int sortOrder,
    int scoreFormat,
    long endTime,
    const char *payload,
    int requestID)
  {
    NSError *error;
    FBUnitySDKDelegate *delegate = [FBUnitySDKDelegate instanceWithRequestID:requestID];
    FBSDKShareTournamentDialog *dialog = [[FBSDKShareTournamentDialog alloc] initWithDelegate: delegate];
    NSString *payloadString;
    if (payload) {
      payloadString = [NSString stringWithUTF8String:payload];
    }
    [dialog showWithInitialScore:initialScore
                         title:[NSString stringWithUTF8String:title]
                       endTime:[NSDate dateWithTimeIntervalSince1970: endTime]
                     scoreType:scoreFormat
                     sortOrder:sortOrder
                       payload:payloadString
                           error: &error];
      if (error) {
        [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnGetTournamentsComplete error:error requestId:requestID];
      }
  }

  void IOSFBUploadImageToMediaLibrary(
    int requestId,
    const char *caption,
    const char *imageUri,
    bool shouldLaunchMediaDialog)
  {
    NSString *captionString = [FBUnityUtility stringFromCString:caption];
    NSString *imageUriString = [FBUnityUtility stringFromCString:imageUri];
    UIImage *image = [UIImage imageWithContentsOfFile:imageUriString];

    FBSDKGamingImageUploaderConfiguration *config =
    [[FBSDKGamingImageUploaderConfiguration alloc]
      initWithImage:image
      caption:captionString
      shouldLaunchMediaDialog:shouldLaunchMediaDialog ? YES: NO];

    [FBSDKGamingImageUploader
      uploadImageWithConfiguration:config
      andResultCompletion:^(BOOL success, id result, NSError * _Nullable error) {
        if (!success || error) {
          [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnUploadImageToMediaLibraryComplete
            error:error
            requestId:requestId];
        } else {
          [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnUploadImageToMediaLibraryComplete
            userData:@{@"id":result[@"id"]}
            requestId:requestId];
        }
    }];

  }

  void IOSFBUploadVideoToMediaLibrary(int requestId,
                                      const char *caption,
                                      const char *videoUri)
  {
    NSString *captionString = [FBUnityUtility stringFromCString:caption];
    NSString *videoUriString = [FBUnityUtility stringFromCString:videoUri];
    NSURL *videoURL = [NSURL fileURLWithPath:videoUriString];

    FBSDKGamingVideoUploaderConfiguration *config =
    [[FBSDKGamingVideoUploaderConfiguration alloc]
      initWithVideoURL:videoURL
      caption:captionString];

    [FBSDKGamingVideoUploader
      uploadVideoWithConfiguration:config
      andResultCompletion:^(BOOL success, id result, NSError * _Nullable error) {
        if (!success || error) {
          [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnUploadVideoToMediaLibraryComplete
            error:error
            requestId:requestId];
        } else {
          [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnUploadVideoToMediaLibraryComplete
            userData:@{@"id":result[@"id"]}
            requestId:requestId];
        }
    }];

  }

  char* IOSFBGetUserID()
  {
    NSString *userID = [[FBSDKAppEvents shared] userID];
    if (!userID) {
      return NULL;
    }
    const char* string = [userID UTF8String];
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
  }

  void IOSFBFetchDeferredAppLink(int requestId)
  {
    [FBSDKAppLinkUtility fetchDeferredAppLink:^(NSURL *url, NSError *error) {
      if (error) {
        [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnFetchDeferredAppLinkComplete error:error requestId:requestId];
        return;
      }

      [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnFetchDeferredAppLinkComplete
                                userData:[FBUnityUtility appLinkDataFromUrl:url]
                               requestId:requestId];
    }];
  }

  void IOSFBRefreshCurrentAccessToken(int requestId)
  {
    FBSDKGraphRequestCompletion completion = ^(id<FBSDKGraphRequestConnecting> connection, id result, NSError *error) {
          if (error) {
            [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnRefreshCurrentAccessTokenComplete error:error requestId:requestId];
            return;
          }

          [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnRefreshCurrentAccessTokenComplete
                                    userData:[FBUnityUtility getUserDataFromAccessToken:[FBSDKAccessToken currentAccessToken]]
                                   requestId:requestId];
      };
    [FBSDKAccessToken refreshCurrentAccessTokenWithCompletion: completion];
  }
}
