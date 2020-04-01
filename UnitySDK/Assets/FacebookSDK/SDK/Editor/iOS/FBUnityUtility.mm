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

#import "FBUnityUtility.h"

#include <string>
#import <Foundation/Foundation.h>

#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import <FBSDKLoginKit/FBSDKLoginKit.h>
#import <FBSDKShareKit/FBSDKShareKit.h>

const char* const FB_OBJECT_NAME = "UnityFacebookSDKPlugin";

// Helper method to create C string copy
static char* FBUnityMakeStringCopy (const char* string)
{
  if (string == NULL)
    return NULL;

  char* res = (char*)malloc(strlen(string) + 1);
  strcpy(res, string);
  return res;
}

@implementation FBUnityUtility

+ (void) sendCancelToUnity:(NSString *)unityMessage
                 requestId:(int)requestId
{
  [self sendMessageToUnity:unityMessage
                  userData:@{ @"cancelled" : @"true" }
                 requestId:requestId];
}

+ (void) triggerUploadViewHierarchy
{
  [self sendMessageToUnity:@"CaptureViewHierarchy"
                  userData:nil
                 requestId:0];
}

+ (void) triggerUpdateBindings:(NSString *)json
{
    [self sendMessageToUnity:@"OnReceiveMapping"
                    message:json
                   requestId:0];
}

+ (void)sendErrorToUnity:(NSString *)unityMessage
                   error:(NSError *)error
               requestId:(int)requestId
{
  NSString *errorMessage =
    error.userInfo[FBSDKErrorLocalizedDescriptionKey] ?:
    error.userInfo[FBSDKErrorDeveloperMessageKey] ?:
    error.localizedDescription;
  [self sendErrorToUnity:unityMessage
            errorMessage:errorMessage
               requestId:requestId];
}

+ (void)sendErrorToUnity:(NSString *)unityMessage
            errorMessage:(NSString *)errorMessage
               requestId:(int)requestId
{
  [self sendMessageToUnity:unityMessage
                  userData:@{ @"error" : errorMessage }
                 requestId:requestId];
}

+ (void)sendMessageToUnity:(NSString *)unityMessage
                  userData:(NSDictionary *)userData
                 requestId:(int)requestId
{
  NSMutableDictionary *resultDictionary = [ @{ @"callback_id": [@(requestId) stringValue] } mutableCopy];
  [resultDictionary addEntriesFromDictionary:userData];

  if (![NSJSONSerialization isValidJSONObject:resultDictionary]) {
    [self sendErrorToUnity:unityMessage errorMessage:@"Result cannot be converted to json" requestId:requestId];
    return;
  }

  NSError *serializationError = nil;
  NSData *jsonData = [NSJSONSerialization dataWithJSONObject:resultDictionary options:0 error:&serializationError];
  if (serializationError) {
    [self sendErrorToUnity:unityMessage error:serializationError requestId:requestId];
    return;
  }

  NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
  if (!jsonString) {
    [self sendErrorToUnity:unityMessage errorMessage:@"Failed to generate response string" requestId:requestId];
    return;
  }

  const char *cString = [jsonString UTF8String];
  UnitySendMessage(FB_OBJECT_NAME, [unityMessage cStringUsingEncoding:NSASCIIStringEncoding], FBUnityMakeStringCopy(cString));
}

+ (void)sendMessageToUnity:(NSString *)unityMessage
                   message:(NSString *)message
                 requestId:(int)requestId
{
    const char *cString = [message UTF8String];
    UnitySendMessage(FB_OBJECT_NAME, [unityMessage cStringUsingEncoding:NSASCIIStringEncoding], FBUnityMakeStringCopy(cString));
}

+ (NSString *)stringFromCString:(const char *)string {
  if (string && string[0] != 0) {
    return [NSString stringWithUTF8String:string];
  }

  return nil;
}

+ (NSDictionary *)dictionaryFromKeys:(const char **)keys
                              values:(const char **)vals
                              length:(int)length
{
  NSMutableDictionary *params = nil;
  if(length > 0 && keys && vals) {
    params = [NSMutableDictionary dictionaryWithCapacity:length];
    for(int i = 0; i < length; i++) {
      if (vals[i] && vals[i] != 0 && keys[i] && keys[i] != 0) {
        params[[NSString stringWithUTF8String:keys[i]]] = [NSString stringWithUTF8String:vals[i]];
      }
    }
  }

  return params;
}

+ (FBSDKGameRequestFilter) gameRequestFilterFromString:(NSString *)filter {
  if (filter.length == 0 || [filter isEqualToString:@"none"]) {
    return FBSDKGameRequestFilterNone;
  } else if ([filter isEqualToString:@"app_users"]) {
    return FBSDKGameRequestFilterAppUsers;
  } else if ([filter isEqualToString:@"app_non_users"]) {
    return FBSDKGameRequestFilterAppNonUsers;
  }

  NSLog(@"Unexpected filter type: %@", filter);
  return FBSDKGameRequestFilterNone;
}

+ (FBSDKGameRequestActionType) gameRequestActionTypeFromString:(NSString *)actionType {
  NSString *actionUpper = [actionType uppercaseString];
  if (actionUpper.length == 0 || [actionUpper isEqualToString:@"NONE"]) {
    return FBSDKGameRequestActionTypeNone;
  } else if ([actionUpper isEqualToString:@"SEND"]) {
    return FBSDKGameRequestActionTypeSend;
  } else if ([actionUpper isEqualToString:@"ASKFOR"]) {
    return FBSDKGameRequestActionTypeAskFor;
  } else if ([actionUpper isEqualToString:@"TURN"]) {
    return FBSDKGameRequestActionTypeTurn;
  }

  NSLog(@"Unexpected action type: %@", actionType);
  return FBSDKGameRequestActionTypeNone;
}

+ (NSDictionary *)appLinkDataFromUrl:(NSURL *)url
{
  NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
  if (url) {
    [dict setObject:url.absoluteString forKey:@"url"];
    FBSDKURL *parsedUrl = [FBSDKURL URLWithInboundURL:url sourceApplication:nil];
    if (parsedUrl) {
      if (parsedUrl.appLinkExtras) {
        [dict setObject:parsedUrl.appLinkExtras forKey:@"extras"];

        // TODO - Try to parse ref param out and pass back
      }

      if (parsedUrl.targetURL) {
        [dict setObject:parsedUrl.targetURL.absoluteString forKey:@"target_url"];
      }
    }
  } else {
    [dict setObject:@true forKey:@"did_complete"];
  }
  return dict;
}

+ (NSDictionary *)getUserDataFromAccessToken:(FBSDKAccessToken *)token
{
  if (token) {
    if (token.tokenString &&
        token.expirationDate &&
        token.userID &&
        token.permissions &&
        token.declinedPermissions) {
      NSInteger expiration = token.expirationDate.timeIntervalSince1970;
      NSInteger lastRefreshDate = token.refreshDate ? token.refreshDate.timeIntervalSince1970 : 0;
      return @{
               @"opened" : @"true",
               @"access_token" : token.tokenString,
               @"expiration_timestamp" : [@(expiration) stringValue],
               @"user_id" : token.userID,
               @"permissions" : [token.permissions allObjects],
               @"granted_permissions" : [token.permissions allObjects],
               @"declined_permissions" : [token.declinedPermissions allObjects],
               @"last_refresh" : [@(lastRefreshDate) stringValue],
               @"graph_domain" : token.graphDomain ? : @"facebook",
               };
    }
  }

  return nil;
}

@end
