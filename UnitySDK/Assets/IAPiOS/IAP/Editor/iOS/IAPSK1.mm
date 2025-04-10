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

#import <StoreKit/StoreKit.h>
#import "IAPSK1.h"

@interface IAPSK1() <SKRequestDelegate, SKProductsRequestDelegate, SKPaymentTransactionObserver>

@property (nonatomic, strong) NSArray<SKProduct *> *products;
@property (nonatomic, strong) NSMutableDictionary<NSString *, SKProduct *> *productOfferings;

@end

@implementation IAPSK1

#pragma mark - Public APIs

+ (IAPSK1 *)sharedInstance
{
  static dispatch_once_t pred;
  static IAPSK1 *shared = nil;

  dispatch_once(&pred, ^{
    shared = [[IAPSK1 alloc] init];
  });
  return shared;
}

- (void)initialize
{
  static dispatch_once_t pred;
  dispatch_once(&pred, ^{
    NSLog(@"Initializing SK1 IAP...");
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    self.productOfferings = [[NSMutableDictionary alloc] initWithDictionary:@{}];
    [self fetchProducts:@[@"com.fbsdk.unity.consumable", @"com.fbsdk.unity.nonconsumable", @"com.fbsdk.unity.subscription"]];
  });
}

- (void)purchaseConsumable
{
  [self purchaseProductWithID:@"com.fbsdk.unity.consumable"];
}

- (void)purchaseNonConsumable
{
  [self purchaseProductWithID:@"com.fbsdk.unity.nonconsumable"];
}

- (void)purchaseSubscription
{
  [self purchaseProductWithID:@"com.fbsdk.unity.subscription"];
}

#pragma mark - Internal APIs

- (void)purchaseProductWithID:(NSString *)identifier
{
  if (self.products.count == 0) {
    return;
  }
  SKProduct *product = [self.productOfferings valueForKey:identifier];
  if (product == nil) {
    return;
  }
  SKMutablePayment *payment = [SKMutablePayment paymentWithProduct:product];
  [[SKPaymentQueue defaultQueue] addPayment:payment];
}

- (void)fetchProducts:(NSArray *)productIds
{
  SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:[NSSet setWithArray:productIds]];
  request.delegate = self;
  [request start];
}

#pragma mark - SKProductsRequestDelegate

- (void)productsRequest:(nonnull SKProductsRequest *)request didReceiveResponse:(nonnull SKProductsResponse *)response {
  self.products = [[NSArray alloc] initWithArray:response.products];
  for (SKProduct* product in self.products) {
    [self.productOfferings setValue:product forKey:product.productIdentifier];
  }
  NSLog(@"Fetched Product Offerings: %@", self.productOfferings);
}

#pragma mark - SKPaymentTransactionObserver

- (void)paymentQueue:(nonnull SKPaymentQueue *)queue updatedTransactions:(nonnull NSArray<SKPaymentTransaction *> *)transactions {
  for (SKPaymentTransaction* transaction in transactions) {
    switch (transaction.transactionState) {
      case SKPaymentTransactionStatePurchasing:
        break;
      case SKPaymentTransactionStateDeferred:
        break;
      case SKPaymentTransactionStatePurchased:
        NSLog(@"Successfully purchased %@ with id %@", transaction.payment.productIdentifier, transaction.transactionIdentifier);
        [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
        break;
      case SKPaymentTransactionStateRestored:
        [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
        break;
      case SKPaymentTransactionStateFailed:
        [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
        break;
    }
  }
}

@end
