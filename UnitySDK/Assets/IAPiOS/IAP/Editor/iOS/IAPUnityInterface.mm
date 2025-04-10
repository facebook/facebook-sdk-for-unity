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

#import "IAPUnityInterface.h"
#import "IAPSK1.h"

typedef enum {
    SK1=1,
    SK2=2
} SKIntegration;

@interface IAPUnityInterface()

@property (nonatomic, assign) SKIntegration skIntegration;

@end

@implementation IAPUnityInterface

#pragma mark - Public APIs

+ (IAPUnityInterface *)sharedInstance
{
  static dispatch_once_t pred;
  static IAPUnityInterface *shared = nil;

  dispatch_once(&pred, ^{
    shared = [[IAPUnityInterface alloc] init];
  });
  return shared;
}

- (void)initializeSK1
{
  self.skIntegration = SK1;
  [[IAPSK1 sharedInstance] initialize];
}

- (void)initializeSK2
{
  self.skIntegration = SK2;
  if (@available(iOS 15.0, *)) {
    [[IAPSK2 shared] initialize];
  }
}

- (void)purchaseConsumable
{
  NSLog(@"Purchasing Consumable...");
  if (self.skIntegration == SK1) {
    [[IAPSK1 sharedInstance] purchaseConsumable];
  } else if (self.skIntegration == SK2) {
    if (@available(iOS 15.0, *)) {
      [[IAPSK2 shared] purchaseConsumable];
    }
  }
}

- (void)purchaseNonConsumable
{
  NSLog(@"Purchasing Non-Consumable...");
  if (self.skIntegration == SK1) {
    [[IAPSK1 sharedInstance] purchaseNonConsumable];
  } else if (self.skIntegration == SK2) {
    if (@available(iOS 15.0, *)) {
      [[IAPSK2 shared] purchaseNonConsumable];
    }
  }
}

- (void)purchaseSubscription
{
  NSLog(@"Purchasing Subscription...");
  if (self.skIntegration == SK1) {
    [[IAPSK1 sharedInstance] purchaseSubscription];
  } else if (self.skIntegration == SK2) {
    if (@available(iOS 15.0, *)) {
      [[IAPSK2 shared] purchaseSubscription];
    }
  }
}

@end

#pragma mark - Unity C# interface (extern C)

extern "C" {

  void IAPInitializeSK1()
  {
    [[IAPUnityInterface sharedInstance] initializeSK1];
  }

  void IAPInitializeSK2()
  {
    [[IAPUnityInterface sharedInstance] initializeSK2];
  }

  void IAPPurchaseConsumable()
  {
    [[IAPUnityInterface sharedInstance] purchaseConsumable];
  }

  void IAPPurchaseNonConsumable()
  {
    [[IAPUnityInterface sharedInstance] purchaseNonConsumable];
  }

  void IAPPurchaseSubscription()
  {
    [[IAPUnityInterface sharedInstance] purchaseSubscription];
  }
}
