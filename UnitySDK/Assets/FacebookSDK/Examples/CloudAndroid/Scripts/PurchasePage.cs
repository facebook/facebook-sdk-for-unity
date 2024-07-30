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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class PurchasePage : MonoBehaviour
{
    [SerializeField]
    private Button _loadProductsBtn;
    [SerializeField]
    private Button _loadPurchasesBtn;
    [SerializeField]
    private GameObject _productRowPrefab;
    [SerializeField]
    private Transform _productScrollTransform;
    [SerializeField]
    private GameObject _purchaseRowPrefab;
    [SerializeField]
    private Transform _purchaseScrollTransform;

    private LogScroller _logScroller;
    private IList<Product> _products = new List<Product>();

    private void Awake()
    {
        _logScroller = transform.root.GetComponent<UIState>().logScroller;
    }

    public void CheckReady()
    {
        LogText("Checking Ready");

        FBGamingServices.OnIAPReady(delegate (IIAPReadyResult result)
        {
            if (result.Error == null && result.ResultDictionary != null)
            {
                _loadProductsBtn.interactable = true;
                _loadPurchasesBtn.interactable = true;
                LogText("IAP Ready");
            }
            else
            {
                LogText("ERR: IAP not ready" + result.Error.ToString());
            }
        });
    }
    public void ConsumePurchase(Purchase purchase, Action<bool> callback)
    {
        LogText("Consuming " + purchase.PurchaseToken + " : " + purchase.ProductID);

        callback(true);

        FBGamingServices.ConsumePurchase(purchase.PurchaseToken, delegate (IConsumePurchaseResult result)
        {
            if (result.Error == null && result.ResultDictionary != null)
            {
                LogText("Consumed purchase " + purchase.PurchaseToken + " : " + purchase.ProductID);
                callback(true);
            }
            else
            {
                LogText(string.Format("ERR: Failed to consume purchase {0} : {1}\n{2}", purchase.PurchaseToken, purchase.ProductID, result.Error.ToString()));
                callback(false);
            }
        });
    }
    public void LoadProducts()
    {
        LogText("Loading Products");

        foreach (Transform child in _productScrollTransform)
        {
            Destroy(child.gameObject);
        }

        FBGamingServices.GetCatalog(delegate (ICatalogResult result)
        {
            if (result.Error == null && result.ResultDictionary != null)
            {
                _products = result.Products; // nullable?
                LogText(string.Format("Got {0} products", result.Products.Count));

                foreach (Product product in _products)
                {
                    GameObject productRow = Instantiate(_productRowPrefab, _productScrollTransform);
                    productRow.GetComponent<ProductRowPrefab>().Initialize(this, _logScroller, product);
                }
            }
            else
            {
                LogText(string.Format("ERR: Failed to get products\n{0}", result.Error));
            }
        });
    }
    public void LoadPurchases()
    {
        LogText("Loading Purchases");

        foreach (Transform child in _purchaseScrollTransform)
        {
            Destroy(child.gameObject);
        }

        FBGamingServices.GetPurchases(delegate (IPurchasesResult result)
        {
            if (result.Error == null && result.ResultDictionary != null)
            {
                LogText(string.Format("Got {0} purchases", result.Purchases.Count));
                foreach (Purchase purchase in result.Purchases)
                {
                    AddPurchase(purchase, false);
                }
            }
            else
            {
                LogText("ERR: Failed to get purchases\n" + result.Error);
            }
        });
    }
    public void MakePurchase(Product product)
    {
        LogText("Purchasing " + product.ProductID);

        FBGamingServices.Purchase(product.ProductID, delegate (IPurchaseResult result)
        {
            if (result.Error == null && result.ResultDictionary != null)
            {
                // ConsumePurchase(result.Purchase);
                LogText(string.Format("Purchased {0} | {1}", result.Purchase.ProductID, result.Purchase.PurchaseToken));
                AddPurchase(result.Purchase, true);
            }
            else if (result.Cancelled)
            {
                LogText("Cancelled purchase");
            }
            else
            {
                LogText(string.Format("ERR: Failed to purchase {0}\n{1}", product.ProductID, result.Error.ToString()));
            }
        });
    }
    public void ToggleShowConsumedClicked(bool show)
    {
        foreach (Transform child in _purchaseScrollTransform)
        {
            child.gameObject.SetActive(show);
        }
    }

    // private
    private void AddPurchase(Purchase purchase, bool setToTop)
    {
        GameObject purchaseRow = Instantiate(_purchaseRowPrefab, _purchaseScrollTransform);
        purchaseRow.GetComponent<PurchaseRowPrefab>().Initialize(this, _logScroller, purchase);
        if (setToTop)
        {
            purchaseRow.transform.SetSiblingIndex(0);
        }
    }

    private void LogText(string text)
    {
        _logScroller.Log(text);
    }
}
