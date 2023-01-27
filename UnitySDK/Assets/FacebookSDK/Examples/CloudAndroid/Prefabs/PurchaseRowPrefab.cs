using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class PurchaseRowPrefab : MonoBehaviour {
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

  public string GetPurchaseToken () {
    return _purchase.PurchaseToken;
  }
  public void OnLogBtnClick () {
    _logScroller.Log(_purchase.ToString());
  }
  public void OnConsumeBtnClick () {
    _parentScript.ConsumePurchase(_purchase, delegate (bool success) {
      Destroy(gameObject);
    });
  }
  public void Initialize (PurchasePage parentScript, LogScroller logScroller, Purchase purchase) {
    _parentScript = parentScript;
    _logScroller = logScroller;
    _purchase = purchase;
    _purchaseTokenText.text = purchase.PurchaseToken;
    _productIdText.text = purchase.ProductID;
    _purchaseTimeText.text = purchase.PurchaseTime.ToLocalTime().ToString(DATE_FORMAT);
    SetConsumeBtnData(purchase.IsConsumed);
  }

  private void SetConsumeBtnData (bool isConsumed) {
    _consumeBtn.GetComponent<Button>().interactable = !isConsumed;
    _consumeBtn.transform.GetChild(0).GetComponent<Text>().text = isConsumed ? "Consumed" : "Consume";
  }
}
