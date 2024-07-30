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

namespace Facebook.Unity.Example
{
    using UnityEngine;

    internal class Subscription : MenuBase
    {
        private string subscriptionProductId = string.Empty;
        private string cancelSubscriptionProductId = string.Empty;

        protected override void GetGui()
        {
            if (this.Button("Call Get Subscribable Catalog"))
            {
                this.CallGetSubscribableCatalog();
            }

            GUILayout.Space(10);


            this.LabelAndTextField("Purchase Subscription Product Id: ", ref this.subscriptionProductId);
            if (this.Button("Call Purchase Subscription"))
            {
                this.CallPurchaseSubscription();
            }

            GUILayout.Space(10);


            if (this.Button("Call Get Subscriptions"))
            {
                this.CallGetSubscriptions();
            }

            GUILayout.Space(10);


            this.LabelAndTextField("Cancel Subscription Id: ", ref this.cancelSubscriptionProductId);
            if (this.Button("Call Cancel Subscription"))
            {
                this.CallCancelSubscription();
            }

            GUILayout.Space(10);
        }


        private void CallGetSubscribableCatalog()
        {
            FB.GetSubscribableCatalog(this.HandleResult);
        }

        private void CallPurchaseSubscription()
        {
            FB.PurchaseSubscription(this.subscriptionProductId, this.HandleResult);
        }

        private void CallGetSubscriptions()
        {
            FB.GetSubscriptions(this.HandleResult);
        }

        private void CallCancelSubscription()
        {
            FB.CancelSubscription(this.cancelSubscriptionProductId, this.HandleResult);
        }
    }
}
