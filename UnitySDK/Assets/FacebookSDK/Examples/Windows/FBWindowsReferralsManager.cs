/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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
