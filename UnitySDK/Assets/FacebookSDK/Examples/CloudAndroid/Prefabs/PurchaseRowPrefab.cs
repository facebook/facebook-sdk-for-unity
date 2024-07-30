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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class PurchaseRowPrefab : MonoBehaviour
{
    [SerializeField]
    private Text _purchaseTokenText;

    [SerializeField]
    private Text _productIdText;

    [SerializeField]
    private Text _purchaseTimeText;

    [SerializeField]
    private GameObject _consumeBtn;

    private string DATE_FORMAT = @"M/d/yyyy hh:mm:ss tt";

    private Purchase _purchase;
    private LogScroller _logScroller;
    private PurchasePage _parentScript;

    public string GetPurchaseToken()
    {
        return _purchase.PurchaseToken;
    }
    public void OnLogBtnClick()
    {
        _logScroller.Log(_purchase.ToString());
    }
    public void OnConsumeBtnClick()
    {
        _parentScript.ConsumePurchase(_purchase, delegate (bool success)
        {
            Destroy(gameObject);
        });
    }
    public void Initialize(PurchasePage parentScript, LogScroller logScroller, Purchase purchase)
    {
        _parentScript = parentScript;
        _logScroller = logScroller;
        _purchase = purchase;
        _purchaseTokenText.text = purchase.PurchaseToken;
        _productIdText.text = purchase.ProductID;
        _purchaseTimeText.text = purchase.PurchaseTime.ToLocalTime().ToString(DATE_FORMAT);
        SetConsumeBtnData(purchase.IsConsumed);
    }

    private void SetConsumeBtnData(bool isConsumed)
    {
        _consumeBtn.GetComponent<Button>().interactable = !isConsumed;
        _consumeBtn.transform.GetChild(0).GetComponent<Text>().text = isConsumed ? "Consumed" : "Consume";
    }
}
