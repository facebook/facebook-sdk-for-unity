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
    /// Contains a Instant Game Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="title">The title of the product.</param>
        /// <param name="productID">The product's game-specified identifier.</param>
        /// <param name="description">The product description.</param>
        /// <param name="imageURI">A link to the product's associated image.</param>
        /// <param name="price">The price of the product.</param>
        /// <param name="priceCurrencyCode">The currency code for the product.</param>
        internal Product(
            string title,
            string productID,
            string description,
            string imageURI,
            string price,
            string priceCurrencyCode)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            if (string.IsNullOrEmpty(productID))
            {
                throw new ArgumentNullException("productID");
            }

            if (string.IsNullOrEmpty(price))
            {
                throw new ArgumentException("price");
            }

            if (string.IsNullOrEmpty(priceCurrencyCode))
            {
                throw new ArgumentNullException("priceCurrencyCode");
            }

            this.Title = title;
            this.ProductID = productID;
            this.Description = description;
            this.ImageURI = imageURI;
            this.Price = price;
            this.PriceCurrencyCode = priceCurrencyCode;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductID { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the image uniform resource identifier.
        /// </summary>
        /// <value>The image uniform resource identifier.</value>
        public string ImageURI { get; private set; }

        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>The price.</value>
        public string Price { get; private set; }


        /// <summary>
        /// Gets the price currency code.
        /// </summary>
        /// <value>The price currency code.</value>
        public string PriceCurrencyCode { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Product"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents a <see cref="Facebook.Unity.Product"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "Title", this.Title },
                    { "ProductID", this.ProductID },
                    { "Description", this.Description.ToStringNullOk() },
                    { "ImageURI", this.ImageURI.ToStringNullOk() },
                    { "Price", this.Price },
                    { "PriceCurrencyCode", this.PriceCurrencyCode },
                });
        }
    }
}
