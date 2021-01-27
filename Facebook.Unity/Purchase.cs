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
    /// Contains a Instant Game Purchase.
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="isConsumed">Whether or not the purchase has been consumed.</param>
        /// <param name="developerPayload">A developer-specified string, provided during the purchase of the product.</param>
        /// <param name="paymentID">The identifier for the purchase transaction.</param>
        /// <param name="productID">The product's game-specified identifier.</param>
        /// <param name="purchaseTime">Unix timestamp of when the purchase occurred.</param>
        /// <param name="purchaseToken">A token representing the purchase that may be used to consume the purchase.</param>
        /// <param name="signedRequest">Server-signed encoding of the purchase request.</param>
        internal Purchase(
            bool isConsumed,
            string developerPayload,
            string paymentID,
            string productID,
            long purchaseTime,
            string purchaseToken,
            string signedRequest)
        {
            if (string.IsNullOrEmpty(paymentID))
            {
                throw new ArgumentNullException("paymentID");
            }

            if (string.IsNullOrEmpty(productID))
            {
                throw new ArgumentNullException("productID");
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

            this.DeveloperPayload = developerPayload;
            this.PaymentID = paymentID;
            this.ProductID = productID;
            this.PurchaseTime = Utilities.FromTimestamp(purchaseTimeInt);
            this.PurchaseToken = purchaseToken;
            this.SignedRequest = signedRequest;
        }

        /// <summary>
        /// Gets whether or not the purchase has been consumed.
        /// </summary>
        /// <value>The consumed boolean.</value>
        public bool IsConsumed { get; private set; }

        /// <summary>
        /// Gets the developer payload string.
        /// </summary>
        /// <value>The developer payload string.</value>
        public string DeveloperPayload { get; private set; }

        /// <summary>
        /// Gets the purchase identifier.
        /// </summary>
        /// <value>The purchase identifier.</value>
        public string PaymentID { get; private set; }


        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductID { get; private set; }

        /// <summary>
        /// Gets the purchase time.
        /// </summary>
        /// <value>The purchase time.</value>
        public DateTime PurchaseTime { get; private set; }

        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>The price.</value>
        public string PurchaseToken { get; private set; }


        /// <summary>
        /// Gets the price currency code.
        /// </summary>
        /// <value>The price currency code.</value>
        public string SignedRequest { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Purchase"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Purchase"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "IsConsumed", this.IsConsumed.ToStringNullOk() },
                    { "DeveloperPayload", this.DeveloperPayload.ToStringNullOk() },
                    { "PaymentID", this.PaymentID },
                    { "ProductID", this.ProductID },
                    { "PurchaseTime", this.PurchaseTime.TotalSeconds().ToString() },
                    { "PurchaseToken", this.PurchaseToken },
                    { "SignedRequest", this.SignedRequest },
                });
        }
    }
}
