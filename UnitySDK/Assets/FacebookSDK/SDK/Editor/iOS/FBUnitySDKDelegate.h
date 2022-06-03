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

#import <AVFoundation/AVFoundation.h>
#import <FBSDKShareKit/FBSDKShareKit-Swift.h>
#import <FBSDKGamingServicesKit/FBSDKGamingServicesKit-Swift.h>
#import <UnityFramework/UnityFramework-Swift.h>

extern NSString *const FBUnityMessageName_OnAppRequestsComplete;
extern NSString *const FBUnityMessageName_OnFriendFinderComplete;
extern NSString *const FBUnityMessageName_OnGetAppLinkComplete;
extern NSString *const FBUnityMessageName_OnGroupCreateComplete;
extern NSString *const FBUnityMessageName_OnGroupJoinComplete;
extern NSString *const FBUnityMessageName_OnInitComplete;
extern NSString *const FBUnityMessageName_OnLoginComplete;
extern NSString *const FBUnityMessageName_OnLogoutComplete;
extern NSString *const FBUnityMessageName_OnShareLinkComplete;
extern NSString *const FBUnityMessageName_OnFetchDeferredAppLinkComplete;
extern NSString *const FBUnityMessageName_OnRefreshCurrentAccessTokenComplete;
extern NSString *const FBUnityMessageName_OnUploadImageToMediaLibraryComplete;
extern NSString *const FBUnityMessageName_OnUploadVideoToMediaLibraryComplete;
extern NSString *const FBUnityMessageName_OnCreateGamingContextComplete;
extern NSString *const FBUnityMessageName_OnSwitchGamingContextComplete;
extern NSString *const FBUnityMessageName_OnChooseGamingContextComplete;
extern NSString *const FBUnityMessageName_OnGetCurrentGamingContextComplete;
extern NSString *const FBUnityMessageName_OnGetTournamentsComplete;
extern NSString *const FBUnityMessageName_OnUpdateTournamentComplete;
extern NSString *const FBUnityMessageName_OnTournamentDialogSuccess;
extern NSString *const FBUnityMessageName_OnTournamentDialogCancel;
extern NSString *const FBUnityMessageName_OnTournamentDialogError;

/*!
 @abstract A helper class that implements various FBSDK delegates in order to send
 messages back to Unity.
 */
@interface FBUnitySDKDelegate : NSObject<
  FBSDKGameRequestDialogDelegate,
  FBSDKSharingDelegate,
  FBSDKContextDialogDelegate,
  FBSDKShareTournamentDialogDelegate>

/*
 @abstract returns a self retaining instance that is released once it receives a
 delegate message from FBSDK.
 */
+ (instancetype)instanceWithRequestID:(int)requestID;

@end
