using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IAPWrapper : MonoBehaviour
{
#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern void IAPInitializeSK1();

    [DllImport ("__Internal")]
    private static extern void IAPInitializeSK2();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseConsumable();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseNonConsumable();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseSubscription();

    public static void InitializeSK1()
    {
        IAPInitializeSK1();
    }

    public static void InitializeSK2()
    {
        IAPInitializeSK2();
    }
#elif UNITY_ANDROID
    public static void InitializeGPBL()
    {
        IAPAndroidImpl.IAPInitialize();
    }

    private static void IAPPurchaseConsumable()
    {
        IAPAndroidImpl.IAPPurchaseConsumable();
    }

    private static void IAPPurchaseNonConsumable()
    {
        IAPAndroidImpl.IAPPurchaseNonConsumable();
    }

    private static void IAPPurchaseSubscription()
    {
        IAPAndroidImpl.IAPPurchaseSubscription();
    }
#else

    private static void IAPPurchaseConsumable() { }

    private static void IAPPurchaseNonConsumable() { }

    private static void IAPPurchaseSubscription() { }

#endif

    public static void PurchaseConsumable()
    {
        IAPPurchaseConsumable();
    }

    public static void PurchaseNonConsumable()
    {
        IAPPurchaseNonConsumable();
    }

    public static void PurchaseSubscription()
    {
        IAPPurchaseSubscription();
    }
}
