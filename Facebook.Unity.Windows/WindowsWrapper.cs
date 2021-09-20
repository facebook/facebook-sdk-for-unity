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

namespace Facebook.Unity.Windows
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class WindowsWrapper : IWindowsWrapper
    {

        private fbg.InitResult result;

        public WindowsWrapper()
        {
        }

        public bool Init(string appId,string clientToken)
        {
            WindowsOptions options = new WindowsOptions(clientToken);
            this.result = fbg.Globals.init(appId, JsonUtility.ToJson(options));
            return result == fbg.InitResult.Success;
        }

        public void LogInWithScopes(IEnumerable<string> scope, string callbackId, CallbackManager callbackManager)
        {
            Dictionary<string, object> result = new Dictionary<string, object>() { { Constants.CallbackIdKey, callbackId } };

            try
            {
                List<fbg.LoginScope> loginScopes = this.ConvertToFbgLoginScope(scope);

                fbg.Globals.loginWithScopes(loginScopes.ToArray(), (accessToken) =>
                {
                    var perms = accessToken.Permissions.ConvertAll<string>(value => value.ToString());
                    var dataTime = Utilities.FromTimestamp((int)accessToken.Expiration);
                    result["WindowsCurrentAccessToken"] = new AccessToken(accessToken.Token, accessToken.UserID.ToString(), dataTime, perms, null, "fb.gg");

                    callbackManager.OnFacebookResponse(new LoginResult((new ResultContainer(result))));
                }, (error) =>
                {
                    string msg = "ERROR: " + error.Message + ",";
                        msg += "InnerErrorCode: " + error.InnerErrorCode.ToString() + ",";
                        msg += "InnerErrorMessage: " + error.InnerErrorMessage + ",";
                        msg += "InnerErrorSubcode: " + error.InnerErrorSubcode.ToString() + ",";
                        msg += "InnerErrorTraceId: " + error.InnerErrorTraceId;

                    result[Constants.ErrorKey] = msg;
                    callbackManager.OnFacebookResponse(new LoginResult((new ResultContainer(result))));
                });
            }
            catch (Exception e)
            {
                result[Constants.ErrorKey] = e.Message;
                callbackManager.OnFacebookResponse(new LoginResult((new ResultContainer(result))));
            }
        }

        public bool IsLoggedIn()
        {
            return fbg.Globals.isLoggedIn();
        }

        public void LogOut()
        {
            fbg.Globals.logout((success) => { Debug.Log("Logged out"); }, (error) => { Debug.LogError(error.Message); });
        }

        public void Tick()
        {
            fbg.Globals.tick();
        }

        public void Deinit()
        {
            this.result = fbg.Globals.deinit();
            Debug.Log("Deinitialized Facebook SDK: " + this.result);
        }

        private List<fbg.LoginScope> ConvertToFbgLoginScope(IEnumerable<string> scope)
        {
            List<fbg.LoginScope> fbgLoginScope = new List<fbg.LoginScope>();
            foreach (string str in scope){
                string result = "";
                string[] subs = str.Split('_');
                foreach (string sub in subs)
                {
                    if (sub.Length == 1)
                    {
                        result += char.ToUpper(sub[0]).ToString();
                    }
                    else
                    {
                        result += (char.ToUpper(sub[0]) + sub.Substring(1)).ToString();
                    }
                }
                if (result != "")
                {
                    fbgLoginScope.Add((fbg.LoginScope)Enum.Parse(typeof(fbg.LoginScope), result));
                }
            }
            return fbgLoginScope;
        }

        public void GetCatalog(string callbackId, CallbackManager callbackManager)
        {
            fbg.Catalog.getCatalog(fbg.PagingType.None, "", 0, (catalogResult) =>
            {
                CatalogResult result = new CatalogResult(WindowsCatalogParser.Parse(catalogResult, callbackId));
                callbackManager.OnFacebookResponse(result);

            }, (error) =>
            {
                PurchaseResult result = new PurchaseResult(WindowsCatalogParser.SetError(error, callbackId));
                callbackManager.OnFacebookResponse(result);
            });
        }

        public void GetPurchases(string callbackId, CallbackManager callbackManager)
        {
            fbg.Purchases.getPurchases(fbg.PagingType.None, "", 0, (purchasesResult) =>
            {
                PurchasesResult result = new PurchasesResult(WindowsPurchaseParser.Parse(purchasesResult, callbackId));
                callbackManager.OnFacebookResponse(result);

            }, (error) =>
            {
                PurchasesResult result = new PurchasesResult(WindowsPurchaseParser.SetError(error, callbackId));
                callbackManager.OnFacebookResponse(result);
            });
        }

        public void Purchase(string newproductID, string newdeveloperPayload, string callbackId, CallbackManager callbackManager)
        {
            fbg.Purchases.purchase(newproductID, newdeveloperPayload, (purchaseResult) => {
                PurchaseResult result = new PurchaseResult(WindowsPurchaseParser.Parse(purchaseResult, callbackId,true));
                callbackManager.OnFacebookResponse(result);
            }, (error) =>
            {
                PurchaseResult result = new PurchaseResult(WindowsPurchaseParser.SetError(error, callbackId));
                callbackManager.OnFacebookResponse(result);
            });
        }

        public void ConsumePurchase(string productToken, string callbackId, CallbackManager callbackManager)
        {
            fbg.Purchases.consume(productToken, (success) => {
                ConsumePurchaseResult result = new ConsumePurchaseResult(new ResultContainer(new Dictionary<string, object>() {
                    {Constants.CallbackIdKey,callbackId }
                }
                ));
                callbackManager.OnFacebookResponse(result);
            }, (error) => {
                ConsumePurchaseResult result = new ConsumePurchaseResult(WindowsPurchaseParser.SetError(error, callbackId));
                callbackManager.OnFacebookResponse(result);
            });
        }
    }
}
