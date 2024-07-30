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

public class ProductRowPrefab : MonoBehaviour
{
    [SerializeField]
    private RawImage _thumbImg;

    [SerializeField]
    private Text _titleText;

    [SerializeField]
    private Text _productIdText;

    [SerializeField]
    private GameObject _buyBtn;

    private Product _product;
    private LogScroller _logScroller;
    private PurchasePage _parentScript;

    private void Awake()
    {
    }

    public void OnLogBtnClick()
    {
        _logScroller.Log(_product.ToString());
    }
    public void OnPurchaseBtnClick()
    {
        _parentScript.MakePurchase(_product);
    }

    public void Initialize(PurchasePage parentScript, LogScroller logScroller, Product product)
    {
        _parentScript = parentScript;
        _logScroller = logScroller;
        _product = product;
        _titleText.text = product.Title;
        _productIdText.text = product.ProductID;
        _buyBtn.transform.GetChild(0).GetComponent<Text>().text = product.Price;
        SetThumb(product.ProductID, product.ImageURI);
    }

    private void SetThumb(string productId, string url)
    {
        StartCoroutine(Utility.GetTexture(productId, url, (texture) =>
        {
            _thumbImg.texture = texture;
        }));
    }

    private void LogText(string text)
    {
        _logScroller.Log(text);
    }

}
