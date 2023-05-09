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

namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the purchase of a subscription.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="deactivationTime">The Unix timestamp of when the subscription entitlement will no longer be active,
        /// if subscription is not renewed or is canceled. Otherwise, null </param>
        /// <param name="isEntitlementActive">Whether or not the user is an active subscriber and should receive entitlement
        /// to the subscription benefits.</param>
        /// <param name="periodStartTime">The current start Unix timestamp of the latest billing cycle.</param>
        /// <param name="periodEndTime">The current end Unix timestamp of the latest billing cycle.</param>
        /// <param name="productID">The corresponding subscribable product's game-specified identifier.</param>
        /// <param name="purchasePlatform">The platform associated with the purchase, such as "FB" for Facebook and "GOOGLE" for Google.</param>
        /// <param name="purchasePrice">Contains the local amount and currency.</param>
        /// <param name="purchaseTime">Unix timestamp of when the purchase occurred.</param>
        /// <param name="purchaseToken">A token representing the purchase that may be used to cancel the subscription.</param>
        /// <param name="signedRequest">Server-signed encoding of the purchase request.</param>
        /// <param name="status">The status of the subscription, such as CANCELED.</param>
        /// <param name="subscriptionTerm">The billing cycle of a subscription.</param>
        internal Subscription(
            long deactivationTime,
            bool isEntitlementActive,
            long periodStartTime,
            long periodEndTime,
            string productID,
            string purchasePlatform,
            IDictionary<string, object> purchasePrice,
            long purchaseTime,
            string purchaseToken,
            string signedRequest,
            string status,
            string subscriptionTerm)
        {
            int deactivationTimeInt;
            try {
                deactivationTimeInt = Convert.ToInt32(deactivationTime);
            } catch (OverflowException) {
                throw new ArgumentException("purchaseTime");
            }

            int periodStartTimeInt;
            try {
                periodStartTimeInt = Convert.ToInt32(periodStartTime);
            } catch (OverflowException) {
                throw new ArgumentException("periodStartTime");
            }

            int periodEndTimeInt;
            try {
                periodEndTimeInt = Convert.ToInt32(periodEndTime);
            } catch (OverflowException) {
                throw new ArgumentException("periodEndTime");
            }

            if (string.IsNullOrEmpty(productID))
            {
                throw new ArgumentNullException("productID");
            }

            if (string.IsNullOrEmpty(purchasePlatform))
            {
                throw new ArgumentNullException("purchasePlatform");
            }

            int purchaseTimeInt;
            try {
                purchaseTimeInt = Convert.ToInt32(purchaseTime);
            } catch (OverflowException) {
                throw new ArgumentException("purchaseTime");
            }

            if (string.IsNullOrEmpty(purchaseToken))
            {
                throw new ArgumentNullException("purchaseToken");
            }

            if (string.IsNullOrEmpty(signedRequest))
            {
                throw new ArgumentNullException("signedRequest");
            }

            if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }

            if (string.IsNullOrEmpty(subscriptionTerm))
            {
                throw new ArgumentNullException("subscriptionTerm");
            }

            this.DeactivationTime = Utilities.FromTimestamp(deactivationTimeInt);
            this.IsEntitlementActive = isEntitlementActive;
            this.PeriodStartTime = Utilities.FromTimestamp(periodStartTimeInt);
            this.PeriodEndTime = Utilities.FromTimestamp(periodEndTimeInt);
            this.ProductID = productID;
            this.PurchasePlatform = purchasePlatform;
            this.PurchasePrice = new CurrencyAmount(purchasePrice["currency"].ToStringNullOk(), purchasePrice["amount"].ToStringNullOk());
            this.PurchaseTime = Utilities.FromTimestamp(purchaseTimeInt);
            this.PurchaseToken = purchaseToken;
            this.SignedRequest = signedRequest;
            this.Status = status;
            this.SubscriptionTerm = subscriptionTerm;
        }

        /// <summary>
        /// Gets the deactivation time.
        /// </summary>
        /// <value>The deactivation time.</value>
        public DateTime DeactivationTime { get; private set; }

        /// <summary>
        /// Gets whether or not the entitlement is active.
        /// </summary>
        /// <value>The entitlement status.</value>
        public bool IsEntitlementActive { get; private set; }

        /// <summary>
        /// Gets the period start time.
        /// </summary>
        /// <value>The period start time.</value>
        public DateTime PeriodStartTime { get; private set; }

        /// <summary>
        /// Gets the period end time.
        /// </summary>
        /// <value>The period end time.</value>
        public DateTime PeriodEndTime { get; private set; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductID { get; private set; }

        /// <summary>
        /// Gets the platform associated with the purchase.
        /// </summary>
        /// <value>The purchase platform, such as "GOOGLE" or "FB".</value>
        public string PurchasePlatform { get; private set; }

        /// <summary>
        /// Gets the amount and currency fields associated with the purchase.
        /// </summary>
        /// <value>The amount and currency fields associated with the purchase as a CurrencyAmount</value>
        public CurrencyAmount PurchasePrice { get; private set; }

        /// <summary>
        /// Gets the purchase time.
        /// </summary>
        /// <value>The purchase time.</value>
        public DateTime PurchaseTime { get; private set; }

        /// <summary>
        /// Gets the purchase token.
        /// </summary>
        /// <value>The purchase token.</value>
        public string PurchaseToken { get; private set; }

        /// <summary>
        /// Gets the price currency code.
        /// </summary>
        /// <value>The price currency code.</value>
        public string SignedRequest { get; private set; }

        /// <summary>
        /// Gets the subscription status.
        /// </summary>
        /// <value>The subscription status</value>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the subscription term.
        /// </summary>
        /// <value>The subscription term</value>
        public string SubscriptionTerm { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Subscription"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Subscription"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "DeactivationTime", this.DeactivationTime.TotalSeconds().ToString() },
                    { "IsEntitlementActive", this.IsEntitlementActive.ToStringNullOk() },
                    { "PeriodStartTime", this.PeriodStartTime.TotalSeconds().ToString() },
                    { "PeriodEndTime", this.PeriodEndTime.TotalSeconds().ToString() },
                    { "ProductID", this.ProductID },
                    { "PurchasePlatform", this.PurchasePlatform },
                    { "PurchasePrice", this.PurchasePrice.ToString() },
                    { "PurchaseTime", this.PurchaseTime.TotalSeconds().ToString() },
                    { "PurchaseToken", this.PurchaseToken },
                    { "SignedRequest", this.SignedRequest },
                    { "Status", this.Status },
                    { "SubscriptionTerm", this.SubscriptionTerm },
                });
        }
    }
}
