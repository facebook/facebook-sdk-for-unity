package com.facebook.unity.gpbl.iapunity;

import android.util.Log;

public class IAPUnityInterface {
    private static final IAPUnityInterface sharedInstance = new IAPUnityInterface();
    private static final String LOG_TAG = "IAPUnityAndroidWrapper";

    public static IAPUnityInterface getInstance() {
        return sharedInstance;
    }

    private IAPUnityInterface() {
        Log.i(LOG_TAG, "Created IAPUnityInterface");
    }

    public void initialize() {
        Log.i(LOG_TAG, "Initialized IAPUnityInterface");
        // TODO: Implement
    }

    public void purchaseConsumable() {
        Log.i(LOG_TAG, "Purchasing Consumable");
        // TODO: Implement
    }

    public void purchaseNonConsumable() {
        Log.i(LOG_TAG, "Purchasing Non-Consumable");
        // TODO: Implement
    }

    public void purchaseSubscription() {
        Log.i(LOG_TAG, "Purchasing Subscription");
        // TODO: Implement
    }
}
