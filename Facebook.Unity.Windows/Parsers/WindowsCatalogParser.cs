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
                    products.Add(Utilities.ParseProductFromCatalogResult(product, true));
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
