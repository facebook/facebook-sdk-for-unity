#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IAPAndroidImpl : MonoBehaviour
{
    private const string IAPJavaClassName = "com.facebook.unity.gpbl.iapunity.IAPUnityInterface";
    private static AndroidJavaClass iapJavaClass;
    private static AndroidJavaObject iapJavaInstance;
    private static AndroidJavaObject unityActivity;

    private static AndroidJavaClass IAPClass {
        get {
            if (iapJavaClass == null) {
                iapJavaClass = new AndroidJavaClass(IAPJavaClassName);
            }
            return iapJavaClass;
        }
    }

    private static AndroidJavaObject IAPInstance {
        get {
            if (iapJavaInstance == null) {
                iapJavaInstance = IAPClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            return iapJavaInstance;
        }
    }

    private static AndroidJavaObject UnityActivity {
        get {
            if (unityActivity == null) {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return unityActivity;
        }
    }

    public static void IAPInitialize() {
        IAPInstance.Call("initialize", UnityActivity);
    }

     public static void IAPPurchaseConsumable() {
        IAPInstance.Call("purchaseConsumable");
    }

     public static void IAPPurchaseNonConsumable() {
        IAPInstance.Call("purchaseNonConsumable");
    }

     public static void IAPPurchaseSubscription() {
        IAPInstance.Call("purchaseSubscription");
    }
}
#endif
