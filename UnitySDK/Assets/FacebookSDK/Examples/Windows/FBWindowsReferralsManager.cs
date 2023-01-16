using Facebook.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FBWindowsReferralsManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public InputField Payload;
    public InputField ReferralLinks;

    private string _referral_example_payload = "{\"worldID\": \"vxrvgq7d8\",\"giftID\": \"sword_gmewvn3qe8\"}";


    private void OnEnable()
    {
        Payload.text = _referral_example_payload;
    }

    public void CreateReferral()
    {
        Logger.DebugLog("Creating Referral link ...");
        FB.Windows.CreateReferral(Payload.text, CallbackReferralsCreate);
    }

    private void CallbackReferralsCreate(IReferralsCreateResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Error Debug print call: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Referral raw result: " + result.Raw);
            Logger.DebugLog("Referral link: " + result.ReferralLink);
            ReferralLinks.text = result.ReferralLink;
        }
    }

    public void GetDataReferral()
    {
        Logger.DebugLog("Getting Referral data ...");
        FB.Windows.GetDataReferral(CallbackReferralsGetData);
    }

    private void CallbackReferralsGetData(IReferralsGetDataResult result)
    {
        if (result.Error != null)
        {
            Logger.DebugErrorLog("Error Debug print call: " + result.RawResult);
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Referral raw result: " + result.Raw);
            Logger.DebugLog("Referral payload: " + result.Payload);
        }
    }
}
