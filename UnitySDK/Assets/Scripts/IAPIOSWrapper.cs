using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IAPIOSWrapper : MonoBehaviour
{
#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern void IAPInitialize();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseConsumable();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseNonConsumable();

    [DllImport ("__Internal")]
    private static extern void IAPPurchaseSubscription();
#else
    private static void IAPInitialize() { }

    private static void IAPPurchaseConsumable() { }

    private static void IAPPurchaseNonConsumable() { }

    private static void IAPPurchaseSubscription() { }
#endif

    public static void Initialize()
    {
        IAPInitialize();
    }

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
