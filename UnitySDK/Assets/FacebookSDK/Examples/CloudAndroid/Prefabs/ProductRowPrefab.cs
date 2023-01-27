using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class ProductRowPrefab : MonoBehaviour {
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

  private void Awake () {
  }

  public void OnLogBtnClick () {
    _logScroller.Log(_product.ToString());
  }
  public void OnPurchaseBtnClick() {
    _parentScript.MakePurchase(_product);
  }

  public void Initialize (PurchasePage parentScript, LogScroller logScroller, Product product) {
    _parentScript = parentScript;
    _logScroller = logScroller;
    _product = product;
    _titleText.text = product.Title;
    _productIdText.text = product.ProductID;
    _buyBtn.transform.GetChild(0).GetComponent<Text>().text = product.Price;
    SetThumb(product.ProductID, product.ImageURI);
  }

  private void SetThumb (string productId, string url) {
    StartCoroutine(Utility.GetTexture(productId, url, (texture) => {
      _thumbImg.texture = texture;
    }));
  }

  private void LogText (string text) {
    _logScroller.Log(text);
  }

}
