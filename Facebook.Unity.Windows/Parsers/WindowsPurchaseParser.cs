using System.Collections;
using System.Collections.Generic;

namespace Facebook.Unity.Windows
{
    class WindowsPurchaseParser : WindowsParserBase
    {
        public static ResultContainer Parse(fbg.Purchases purchases, string callbackId)
        {
            IDictionary<string, object> deserializedPurchaseData = Facebook.MiniJSON.Json.Deserialize(purchases.Raw) as Dictionary<string, object>;
            ResultContainer container;
            if (deserializedPurchaseData.TryGetValue("data", out IList apiData))
            {
                IList<Purchase> purchasesList = new List<Purchase>();

                foreach (IDictionary<string, object> purchase in apiData)
                {
                    purchasesList.Add( Utilities.ParsePurchaseFromDictionary(purchase, true) );
                }

                Dictionary<string, object> result = new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", purchases.Raw}
                };

                if (purchasesList.Count >= 1)
                {
                    result["purchases"] = purchasesList;
                }
                else
                {
                    result[Constants.ErrorKey] = "ERROR: Parsing purchases. No purchase data.";
                }

                container = new ResultContainer(result);
            }
            else
            {
                container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey, callbackId},
                    {"RawResult", purchases.Raw},
                    {Constants.ErrorKey, "ERROR: Parsing purchases. Wrong data format"}
                });
            }
            return container;
        }
    }
}
