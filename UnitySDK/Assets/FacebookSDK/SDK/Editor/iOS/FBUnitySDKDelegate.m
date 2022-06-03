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

#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import "FBUnitySDKDelegate.h"
#import "FBUnityUtility.h"

NSString *const FBUnityMessageName_OnAppRequestsComplete = @"OnAppRequestsComplete";
NSString *const FBUnityMessageName_OnGetAppLinkComplete = @"OnGetAppLinkComplete";
NSString *const FBUnityMessageName_OnFriendFinderComplete = @"OnFriendFinderComplete";
NSString *const FBUnityMessageName_OnGroupCreateComplete = @"OnGroupCreateComplete";
NSString *const FBUnityMessageName_OnGroupJoinComplete = @"OnGroupJoinComplete";
NSString *const FBUnityMessageName_OnInitComplete = @"OnInitComplete";
NSString *const FBUnityMessageName_OnLoginComplete = @"OnLoginComplete";
NSString *const FBUnityMessageName_OnLogoutComplete = @"OnLogoutComplete";
NSString *const FBUnityMessageName_OnShareLinkComplete = @"OnShareLinkComplete";
NSString *const FBUnityMessageName_OnFetchDeferredAppLinkComplete = @"OnFetchDeferredAppLinkComplete";
NSString *const FBUnityMessageName_OnRefreshCurrentAccessTokenComplete = @"OnRefreshCurrentAccessTokenComplete";
NSString *const FBUnityMessageName_OnUploadImageToMediaLibraryComplete = @"OnUploadImageToMediaLibraryComplete";
NSString *const FBUnityMessageName_OnUploadVideoToMediaLibraryComplete = @"OnUploadVideoToMediaLibraryComplete";
NSString *const FBUnityMessageName_OnCreateGamingContextComplete = @"OnCreateGamingContextComplete";
NSString *const FBUnityMessageName_OnSwitchGamingContextComplete = @"OnSwitchGamingContextComplete";
NSString *const FBUnityMessageName_OnChooseGamingContextComplete = @"OnChooseGamingContextComplete";
NSString *const FBUnityMessageName_OnGetCurrentGamingContextComplete = @"OnGetCurrentGamingContextComplete";
NSString *const FBUnityMessageName_OnGetTournamentsComplete = @"OnGetTournamentsComplete";
NSString *const FBUnityMessageName_OnUpdateTournamentComplete = @"OnUpdateTournamentComplete";
NSString *const FBUnityMessageName_OnTournamentDialogSuccess = @"OnTournamentDialogSuccess";
NSString *const FBUnityMessageName_OnTournamentDialogCancel = @"OnTournamentDialogCancel";
NSString *const FBUnityMessageName_OnTournamentDialogError = @"OnTournamentDialogError";

static NSMutableArray *g_instances;

@implementation FBUnitySDKDelegate {
  int _requestID;
}

+ (void)initialize
{
  if (self == [FBUnitySDKDelegate class]) {
    g_instances = [NSMutableArray array];
  }
}

+ (instancetype)instanceWithRequestID:(int)requestID
{
  FBUnitySDKDelegate *instance = [[FBUnitySDKDelegate alloc] init];
  instance->_requestID = requestID;
  [g_instances addObject:instance];
  return instance;
}

#pragma mark - Private helpers

- (void)complete
{
  [g_instances removeObject:self];
}

#pragma mark - GameRequestDelegate

- (void)gameRequestDialog:(FBSDKGameRequestDialog *)gameRequestDialog didCompleteWithResults:(NSDictionary *)results
{
  [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnAppRequestsComplete userData:results requestId:_requestID];
  [self complete];
}

- (void)gameRequestDialog:(FBSDKGameRequestDialog *)gameRequestDialog didFailWithError:(NSError *)error
{
  [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnAppRequestsComplete error:error requestId:_requestID];
  [self complete];
}

- (void)gameRequestDialogDidCancel:(FBSDKGameRequestDialog *)gameRequestDialog
{
  [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnAppRequestsComplete requestId:_requestID];
  [self complete];
}

#pragma mark - FBSDKSharingDelegate

- (void)sharer:(id<FBSDKSharing>)sharer didCompleteWithResults:(NSDictionary *)results
{
  if (results.count == 0) {
    // We no longer always send back a postId. In cases where the response is empty,
    // stuff in a didComplete so that Unity doesn't treat it as a malformed response.
    results = @{ @"didComplete" : @"1" };
  }
  [FBUnityUtility sendMessageToUnity:FBUnityMessageName_OnShareLinkComplete userData:results requestId:_requestID];
  [self complete];
}

- (void)sharer:(id<FBSDKSharing>)sharer didFailWithError:(NSError *)error
{
  [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnShareLinkComplete error:error requestId:_requestID];
  [self complete];
}

- (void)sharerDidCancel:(id<FBSDKSharing>)sharer
{
  [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnShareLinkComplete requestId:_requestID];
  [self complete];
}

#pragma mark - FBSDKContextDialogDelegate

- (void)contextDialogDidComplete:(NSObject<FBSDKContextDialogDelegate>*)contextDialog;
{
  if ([contextDialog isKindOfClass:[FBSDKChooseContextDialog class]]) {
    [FBUnityUtility sendMessageToUnity: FBUnityMessageName_OnChooseGamingContextComplete userData: NULL requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKSwitchContextDialog class]]) {
    [FBUnityUtility sendMessageToUnity: FBUnityMessageName_OnSwitchGamingContextComplete userData: NULL requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKCreateContextDialog class]]) {
    [FBUnityUtility sendMessageToUnity: FBUnityMessageName_OnCreateGamingContextComplete userData: NULL requestId:_requestID];
  }
  [self complete];
}

- (void)contextDialog:(NSObject<FBSDKContextDialogDelegate>*)contextDialog didFailWithError:(NSError *)error
{
  if ([contextDialog isKindOfClass:[FBSDKChooseContextDialog class]]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnChooseGamingContextComplete error:error requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKSwitchContextDialog class]]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnSwitchGamingContextComplete error:error requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKCreateContextDialog class]]) {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnCreateGamingContextComplete error:error requestId:_requestID];
  }
  [self complete];
}

- (void)contextDialogDidCancel:(NSObject<FBSDKContextDialogDelegate>*)contextDialog
{
  if ([contextDialog isKindOfClass:[FBSDKChooseContextDialog class]]) {
    [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnChooseGamingContextComplete requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKSwitchContextDialog class]]) {
    [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnSwitchGamingContextComplete requestId:_requestID];
  } else if ([contextDialog isKindOfClass:[FBSDKCreateContextDialog class]]) {
    [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnCreateGamingContextComplete requestId:_requestID];
  }

  [self complete];
}

#pragma mark - FBSDKShareTournamentDialogDelegate

- (void)didCancelWithDialog:(FBSDKShareTournamentDialog * _Nonnull)dialog {
    [FBUnityUtility sendCancelToUnity:FBUnityMessageName_OnTournamentDialogCancel requestId:_requestID];
}

- (void)didCompleteWithDialog:(FBSDKShareTournamentDialog * _Nonnull)dialog tournament:(FBSDKTournament * _Nonnull)tournament {
    [FBUnityUtility sendMessageToUnity: FBUnityMessageName_OnTournamentDialogSuccess userData: [tournament toDictionary] requestId:_requestID];
}

- (void)didFailWithError:(NSError * _Nonnull)error dialog:(FBSDKShareTournamentDialog * _Nonnull)dialog {
    [FBUnityUtility sendErrorToUnity:FBUnityMessageName_OnTournamentDialogError error:error requestId:_requestID];
}

@end
