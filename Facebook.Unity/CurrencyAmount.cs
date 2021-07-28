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
    /// Contains an amount and currency associated with a purchase or transaction.
    /// </summary>
    public class CurrencyAmount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyAmount"/> class.
        /// </summary>
        /// <param name="amount">The associated amount.</param>
        /// <param name="currency">The associated currency.</param>
        internal CurrencyAmount(
            string amount,
            string currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        /// <summary>
        /// Gets the amount, eg "0.99".
        /// </summary>
        /// <value>The amount string.</value>
        public string Amount { get; private set; }

        /// <summary>
        /// Gets the currency, represented by the ISO currency code, eg "USD".
        /// </summary>
        /// <value>The currency string.</value>
        public string Currency { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents a <see cref="Facebook.Unity.CurrencyAmount"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents a <see cref="Facebook.Unity.CurrencyAmount"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "Amount", this.Amount },
                    { "Currency", this.Currency },
                });
        }
    }
}
