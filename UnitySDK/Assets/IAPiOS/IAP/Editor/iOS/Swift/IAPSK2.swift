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

import Foundation
import StoreKit

@objc
@available(iOS 15.0, *)
public final class IAPSK2: NSObject {
    private var isInitialized: Bool
    private var updateListenerTask: Task<Void, Error>?
    private var products: [Product] = []
    private var productOfferings: [String: Product] = [:]

    @objc public static let shared = IAPSK2()

    @objc public func initialize() {
        if isInitialized || products.count > 0 {
            return
        }
        print("Initializing IAP SK2...")
        Task {
            await fetchProducts(productIdentifiers: ["com.fbsdk.unity.consumable", "com.fbsdk.unity.nonconsumable", "com.fbsdk.unity.subscription"])
            isInitialized = true
        }
    }

    @objc public func purchaseConsumable() {
        guard let product = productOfferings["com.fbsdk.unity.consumable"] else {
            return
        }
        makePurchase(product: product)
    }

    @objc public func purchaseNonConsumable() {
        guard let product = productOfferings["com.fbsdk.unity.nonconsumable"] else {
            return
        }
        makePurchase(product: product)
    }

    @objc public func purchaseSubscription() {
        guard let product = productOfferings["com.fbsdk.unity.subscription"] else {
            return
        }
        makePurchase(product: product)
    }

    private override init() {
        isInitialized = false
        super.init()
        updateListenerTask = listenForTransactionUpdates()
    }
}

@available(iOS 15.0, *)
public extension IAPSK2 {
    private func makePurchase(product: Product) {
        Task {
            guard let purchaseResult = try? await product.purchase() else {
                return
            }
            await handlePurchaseResult(purchaseResult: purchaseResult)
        }
    }

    private func handlePurchaseResult(purchaseResult: Product.PurchaseResult) async {
        switch purchaseResult {
        case let .success(verificationResult):
            switch verificationResult {
            case .unverified:
                return
            case let .verified(transaction):
                await transaction.finish()
                print("Successfully purchased \(transaction.productID) with id \(transaction.id)")
                return
            }
        case .userCancelled: return
        case .pending: return
        default: return
        }
    }

    private func fetchProducts(productIdentifiers: [String]) async {
        guard let fetchedProducts = try? await Product.products(for: productIdentifiers) else {
            return
        }
        products = fetchedProducts
        for product in fetchedProducts {
            productOfferings[product.id] = product
        }
        print("Fetched Product Offerings: \(self.productOfferings)")
    }

    private func listenForTransactionUpdates() -> Task<Void, Error> {
      return Task.detached {
        for await verificationResult in Transaction.updates {
          switch verificationResult {
          case .unverified: continue
          case let .verified(transaction):
            await transaction.finish()
          }
        }
      }
    }
}
