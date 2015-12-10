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

#import <FBSDKShareKit/FBSDKShareKit.h>
#import <FBSDKLoginKit/FBSDKLoginKit.h>

extern const char* const FB_OBJECT_NAME;

@interface FBUnityUtility : NSObject
+ (NSDictionary *)dictionaryFromKeys:(const char **)keys values:(const char **)vals length:(int)length;
+ (FBSDKGameRequestFilter)gameRequestFilterFromString:(NSString *)filter;
+ (FBSDKGameRequestActionType)gameRequestActionTypeFromString:(NSString *)actionType;
+ (void)sendCancelToUnity:(NSString *)unityMessage requestId:(int)requestId;
+ (void)sendErrorToUnity:(NSString *)unityMessage error:(NSError *)error requestId:(int)requestId;
+ (void)sendErrorToUnity:(NSString *)unityMessage errorMessage:(NSString *)errorMessage requestId:(int)requestId;
+ (void)sendMessageToUnity:(NSString *)unityMessage userData:(NSDictionary *)userData requestId:(int)requestId;
+ (NSString *)stringFromCString:(const char *)string;
+ (NSDictionary *)appLinkDataFromUrl:(NSURL *)url;
+ (NSDictionary *)getUserDataFromAccessToken:(FBSDKAccessToken *)token;
@end
