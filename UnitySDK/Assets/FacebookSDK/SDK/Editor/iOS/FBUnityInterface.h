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

#import <UIKit/UIKit.h>

#import "AppDelegateListener.h"
#include "RegisterMonoModules.h"

//if we are on a version of unity that has the version number defined use it, otherwise we have added it ourselves in the post build step
#if HAS_UNITY_VERSION_DEF
#include "UnityTrampolineConfigure.h"
#endif

@interface FBUnityInterface : NSObject <AppDelegateListener>
{
  //If you make changes in here make the same changes in Assets/Facebook/Scripts/NativeDialogModes.cs
  enum ShareDialogMode
  {
    AUTOMATIC = 0,
    NATIVE = 1,
    WEB = 2,
    FEED = 3,
  };
}

@property (assign, nonatomic) BOOL useFrictionlessRequests;
@property (nonatomic) ShareDialogMode shareDialogMode;

+ (FBUnityInterface *)sharedInstance;
@end
