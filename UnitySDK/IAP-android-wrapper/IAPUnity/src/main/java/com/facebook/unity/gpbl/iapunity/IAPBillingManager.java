package com.facebook.unity.gpbl.iapunity;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import androidx.annotation.NonNull;

import com.android.billingclient.api.AcknowledgePurchaseParams;
import com.android.billingclient.api.AcknowledgePurchaseResponseListener;
import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingClientStateListener;
import com.android.billingclient.api.BillingFlowParams;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.ConsumeParams;
import com.android.billingclient.api.ConsumeResponseListener;
import com.android.billingclient.api.ProductDetails;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.QueryProductDetailsParams;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class IAPBillingManager {
    private static IAPBillingManager sharedInstance;
    private final BillingClient billingClient;
    private final Map<String, ProductDetails> productDetailsMap = new HashMap<>();
    private final Activity activity;

    private IAPBillingManager(Activity activity) {
        this.activity = activity;
        Log.i(IAPUnityInterface.LOG_TAG, "Initialized IAPBillingManager");
        PurchasesUpdatedListener purchasesUpdatedListener = (billingResult, purchases) -> {
            if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK
                    && purchases != null) {
                for (Purchase purchase : purchases) {
                    processPurchase(purchase);
                }
            }
        };
        this.billingClient = BillingClient.newBuilder(activity.getApplicationContext())
                .setListener(purchasesUpdatedListener)
                .enablePendingPurchases()
                .build();
    }

    private void processPurchase(Purchase purchase) {
        if (purchase.getPurchaseState() != Purchase.PurchaseState.PURCHASED) {
            return;
        }
        List<String> products = purchase.getProducts();
        for (String productID: products) {
            ProductDetails details = productDetailsMap.get(productID);
            if (details == null) {
                continue;
            }
            if (details.getProductId().equals("coffee")) {
                consumePurchase(purchase);
            } else {
                acknowledgePurchase(purchase);
            }
        }
    }

    private void consumePurchase(Purchase purchase) {
        ConsumeParams consumeParams =
                ConsumeParams.newBuilder()
                        .setPurchaseToken(purchase.getPurchaseToken())
                        .build();
        ConsumeResponseListener listener = new ConsumeResponseListener() {
            @Override
            public void onConsumeResponse(BillingResult billingResult, @NonNull String purchaseToken) {
                if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                    Log.i(IAPUnityInterface.LOG_TAG, "Successfully consumed purchase with token " + purchaseToken);
                }
            }
        };
        billingClient.consumeAsync(consumeParams, listener);
    }

    private void fetchAllProducts() {
        fetchInAppProducts();
        fetchSubscriptionProducts();
    }

    private void acknowledgePurchase(Purchase purchase) {
        if (purchase.isAcknowledged()) {
            return;
        }
        AcknowledgePurchaseParams acknowledgePurchaseParams =
                AcknowledgePurchaseParams.newBuilder()
                        .setPurchaseToken(purchase.getPurchaseToken())
                        .build();
        AcknowledgePurchaseResponseListener listener = new AcknowledgePurchaseResponseListener() {
            @Override
            public void onAcknowledgePurchaseResponse(@NonNull BillingResult billingResult) {
                if (billingResult.getResponseCode() == BillingClient.BillingResponseCode.OK) {
                    Log.i(IAPUnityInterface.LOG_TAG, "Successfully acknowledged purchase with token");
                }
            }
        };
        billingClient.acknowledgePurchase(acknowledgePurchaseParams, listener);
    }

    private void fetchInAppProducts() {
        List<QueryProductDetailsParams.Product> inAppProducts = List.of(
                QueryProductDetailsParams.Product.newBuilder()
                        .setProductId("coffee")
                        .setProductType(BillingClient.ProductType.INAPP)
                        .build(),
                QueryProductDetailsParams.Product.newBuilder()
                        .setProductId("unconsumed_coffee")
                        .setProductType(BillingClient.ProductType.INAPP)
                        .build()
        );
        fetchProducts(inAppProducts);
    }

    private void fetchSubscriptionProducts() {
        List<QueryProductDetailsParams.Product> subscriptionProducts = List.of(QueryProductDetailsParams.Product.newBuilder()
                .setProductId("black_tea")
                .setProductType(BillingClient.ProductType.SUBS)
                .build());
        fetchProducts(subscriptionProducts);
    }

    private void fetchProducts(List<QueryProductDetailsParams.Product> products) {
        QueryProductDetailsParams queryProductDetailsParams =
                QueryProductDetailsParams.newBuilder()
                        .setProductList(products)
                        .build();
        this.billingClient.queryProductDetailsAsync(
                queryProductDetailsParams,
                (billingResult, productDetailsList) -> {
                    if (billingResult.getResponseCode() ==  BillingClient.BillingResponseCode.OK) {
                        for (ProductDetails details : productDetailsList) {
                            productDetailsMap.put(details.getProductId(), details);
                        }
                    }
                }
        );
    }

    private void purchaseProductWithID(String productID) {
        if (productDetailsMap.isEmpty()) {
            return;
        }
        ProductDetails details = productDetailsMap.get(productID);
        if (details == null) {
            return;
        }
        BillingFlowParams.ProductDetailsParams.Builder productDetailsParamsBuilder =
                BillingFlowParams.ProductDetailsParams.newBuilder()
                        .setProductDetails(details);
        if (details.getProductType().equals(BillingClient.ProductType.SUBS)) {
            List<ProductDetails.SubscriptionOfferDetails> offerDetailsList = details.getSubscriptionOfferDetails();
            if (offerDetailsList == null || offerDetailsList.isEmpty()) {
                return;
            }
            String offerToken = offerDetailsList.get(0).getOfferToken();
            productDetailsParamsBuilder.setOfferToken(offerToken);
        }
        BillingFlowParams billingFlowParams = BillingFlowParams.newBuilder()
                .setProductDetailsParamsList(List.of(productDetailsParamsBuilder.build())).build();
        billingClient.launchBillingFlow(activity, billingFlowParams);
    }

    public static synchronized IAPBillingManager getInstance(Activity activity) {
        if (sharedInstance == null) {
            sharedInstance = new IAPBillingManager(activity);
        }
        return sharedInstance;
    }

    public void initialize() {
        if (!productDetailsMap.isEmpty() || this.billingClient == null) {
            return;
        }
        final IAPBillingManager self = this;
        this.billingClient.startConnection(new BillingClientStateListener() {
            @Override
            public void onBillingSetupFinished(@NonNull BillingResult billingResult) {
                if (billingResult.getResponseCode() ==  BillingClient.BillingResponseCode.OK) {
                    self.fetchAllProducts();
                }
            }
            @Override
            public void onBillingServiceDisconnected() {}
        });
    }

    public void purchaseConsumable() {
        purchaseProductWithID("coffee");
    }

    public void purchaseNonConsumable() {purchaseProductWithID("unconsumed_coffee");}

    public void purchaseSubscription() {
        purchaseProductWithID("black_tea");
    }
}
