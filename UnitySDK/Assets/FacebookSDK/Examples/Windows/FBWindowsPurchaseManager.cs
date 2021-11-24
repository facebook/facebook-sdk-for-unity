using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FBWindowsPurchaseManager : MonoBehaviour
{
    public FBWindowsLogsManager Logger;
    public GameObject ProductGameObject;
    public Transform CatalogPanelTarnsform;
    public Transform PurchasesPanelTarnsform;

    // IN APP PURCHASES CATALOG FUNCTIONS ----------------------------------------------------------------------------------------------------------
    public void GetCatalogButton()
    {
        if (FB.IsLoggedIn)
        {
            Logger.DebugLog("GetCatalog");
            FB.GetCatalog(ProcessGetCatalog);
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }

    private void ProcessGetCatalog(ICatalogResult result)
    {
        foreach (Transform child in CatalogPanelTarnsform)
        {
            Destroy(child.gameObject);
        }

        if (FB.IsLoggedIn)
        {
            Logger.DebugLog("Processing Catalog");
            if (result.Error != null)
            {
                Logger.DebugErrorLog(result.Error);
            }
            else
            {
                foreach (Product item in result.Products)
                {
                    string itemInfo = item.Title + "\n";
                    itemInfo += item.Description + "\n";
                    itemInfo += item.Price;

                    GameObject newProduct = Instantiate(ProductGameObject, CatalogPanelTarnsform);

                    newProduct.GetComponentInChildren<Text>().text = itemInfo;
                    newProduct.GetComponentInChildren<Button>().onClick.AddListener(() => FB.Purchase(item.ProductID, ProcessPurchase, "FB_PayLoad"));

                    if (item.ImageURI != "")
                    {
                        StartCoroutine(LoadPictureFromUrl(item.ImageURI, newProduct.GetComponentInChildren<RawImage>()));
                    }
                }
            }
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }

    IEnumerator LoadPictureFromUrl(string url, RawImage itemImage)
    {
        Texture2D UserPicture = new Texture2D(32, 32);

        WWW www = new WWW(url);
        yield return www;

        www.LoadImageIntoTexture(UserPicture);
        www.Dispose();
        www = null;

        itemImage.texture = UserPicture;
    }

    private void ProcessPurchase(IPurchaseResult result)
    {
        Logger.DebugLog("Purchase result");
        if (result.Error != null)
        {
            Logger.DebugErrorLog(result.Error);
        }
        else
        {
            Logger.DebugLog("Product Purchased: " + result.Purchase.ProductID);
            Logger.DebugLog("----------------------------------");
            Logger.DebugLog("ProductID: " + result.Purchase.ProductID);
            Logger.DebugLog("IsConsumed: " + result.Purchase.IsConsumed);
            Logger.DebugLog("DeveloperPayload: " + result.Purchase.DeveloperPayload);
            Logger.DebugLog("PaymentID: " + result.Purchase.PaymentID);
            Logger.DebugLog("PaymentActionType: " + result.Purchase.PaymentActionType);
            Logger.DebugLog("PurchasePlatform: " + result.Purchase.PurchasePlatform);
            Logger.DebugLog("PurchasePrice.Amount: " + result.Purchase.PurchasePrice.Amount);
            Logger.DebugLog("PurchasePrice.Currency: " + result.Purchase.PurchasePrice.Currency);
            Logger.DebugLog("PurchaseTime: " + result.Purchase.PurchaseTime);
            Logger.DebugLog("PurchaseToken: " + result.Purchase.PurchaseToken);
            Logger.DebugLog("SignedRequest: " + result.Purchase.SignedRequest);
            Logger.DebugLog("----------------------------------");
        }
        GetPurchases();
    }

    // IN APP PURCHASES PURCHASES FUNCTIONS ----------------------------------------------------------------------------------------------------------
    public void GetPurchases()
    {
        Logger.DebugLog("Getting purchases...");
        FB.GetPurchases(processPurchases);
    }

    private void processPurchases(IPurchasesResult result)
    {
        foreach (Transform child in PurchasesPanelTarnsform)
        {
            Destroy(child.gameObject);
        }

        if (FB.IsLoggedIn)
        {
            Logger.DebugLog("Processing Purchases...");
            if (result.Error != null)
            {
                Logger.DebugErrorLog(result.Error);
            }
            else
            {
                if (result.Purchases != null)
                {
                    foreach (Purchase item in result.Purchases)
                    {
                        string itemInfo = item.ProductID + "\n";
                        itemInfo += item.PurchasePlatform + "\n";
                        itemInfo += item.PurchaseToken;

                        GameObject newProduct = Instantiate(ProductGameObject, PurchasesPanelTarnsform);

                        newProduct.GetComponentInChildren<Text>().text = itemInfo;
                        newProduct.GetComponentInChildren<Button>().onClick.AddListener(() =>
                        {
                            FB.ConsumePurchase(item.PurchaseToken, delegate (IConsumePurchaseResult consumeResult)
                            {
                                if (consumeResult.Error != null)
                                {
                                    Logger.DebugErrorLog(consumeResult.Error);
                                }
                                else
                                {
                                    Logger.DebugLog("Product consumed correctly! ProductID:" + item.ProductID);
                                }
                                GetPurchases();
                            });
                        });
                    }
                }
                else
                {
                    Logger.DebugLog("No purchases");
                }
            }
        }
        else
        {
            Logger.DebugWarningLog("Login First");
        }
    }
}
