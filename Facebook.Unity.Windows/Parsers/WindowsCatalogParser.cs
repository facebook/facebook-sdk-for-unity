using System.Collections;
using System.Collections.Generic;

namespace Facebook.Unity.Windows
{
    class WindowsCatalogParser: WindowsParserBase
    {
        public static ResultContainer Parse(fbg.Catalog catalog, string callbackId)
        {
            IDictionary<string, object> deserializedCatalogData = Facebook.MiniJSON.Json.Deserialize(catalog.Raw) as Dictionary<string, object>;
            ResultContainer container;
            if (deserializedCatalogData.TryGetValue("data", out IList apiData))
            {
                IList<Product> products = new List<Product>();
                foreach (IDictionary<string, object> product in apiData)
                {
                    string title = product["title"].ToStringNullOk();
                    string productID = product["product_id"].ToStringNullOk();
                    string description = product["description"].ToStringNullOk();
                    string imageURI = product.ContainsKey("image_uri") ? product["image_uri"].ToStringNullOk() : "";
                    string price = product["price"].ToStringNullOk();
                    double? priceAmount = product.ContainsKey("price_amount") ? (double?)product["price_amount"] : null;
                    string priceCurrencyCode = product["price_currency_code"].ToStringNullOk();
                    products.Add(new Product(title, productID, description, imageURI, price, priceAmount, priceCurrencyCode));
                }

                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", catalog.Raw},
                    {"products", products}
                });
            }
            else
            {               
                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", catalog.Raw},
                    {Constants.ErrorKey, "ERROR: Parsing catalog. Wrong data format"}
                });
            }
            return container;
        }
    }
}
