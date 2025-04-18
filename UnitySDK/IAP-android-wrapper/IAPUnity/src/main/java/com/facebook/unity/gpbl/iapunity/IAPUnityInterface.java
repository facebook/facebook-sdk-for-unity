package com.facebook.unity.gpbl.iapunity;

import android.app.Activity;
import android.util.Log;

public class IAPUnityInterface {
    private static IAPUnityInterface sharedInstance;
    private IAPBillingManager billingManager;
    public static final String LOG_TAG = "IAPUnityAndroidWrapper";
    private Activity unityActivity;

    public static IAPUnityInterface getInstance() {
        if (sharedInstance == null) {
            sharedInstance = new IAPUnityInterface();
        }
        return sharedInstance;
    }

    private IAPUnityInterface() {
        Log.i(LOG_TAG, "Created IAPUnityInterface");
    }

    private IAPBillingManager getBillingManager() {
        if (billingManager == null) {
            billingManager = IAPBillingManager.getInstance(this.unityActivity);
        }
        return billingManager;
    }

    public void initialize(Activity unityActivity) {
        if (unityActivity == null) {
            return;
        }
        this.unityActivity = unityActivity;
        this.getBillingManager().initialize();
        if (unityActivity.getApplicationContext() == null) {
            return;
        }
        Log.i(LOG_TAG, "Initialized IAPUnityInterface");
    }

    public void purchaseConsumable() {
        Log.i(LOG_TAG, "Purchasing Consumable");
        this.getBillingManager().purchaseConsumable();
    }

    public void purchaseNonConsumable() {
        Log.i(LOG_TAG, "Purchasing Non-Consumable");
        this.getBillingManager().purchaseNonConsumable();
    }

    public void purchaseSubscription() {
        Log.i(LOG_TAG, "Purchasing Subscription");
        this.getBillingManager().purchaseSubscription();
    }
}
