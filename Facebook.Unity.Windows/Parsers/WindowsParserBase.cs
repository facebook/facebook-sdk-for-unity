
using System.Collections.Generic;

namespace Facebook.Unity.Windows
{
    class WindowsParserBase
    {
        public static ResultContainer SetError(fbg.Error error, string callbackId)
        {
            string msg = "ERROR: " + error.Message + ",";
            msg += "InnerErrorCode: " + error.InnerErrorCode.ToString() + ",";
            msg += "InnerErrorMessage: " + error.InnerErrorMessage + ",";
            msg += "InnerErrorSubcode: " + error.InnerErrorSubcode.ToString() + ",";
            msg += "InnerErrorTraceId: " + error.InnerErrorTraceId;

            ResultContainer container = new ResultContainer(new Dictionary<string, object>()
                {
                    {Constants.CallbackIdKey,callbackId},
                    {Constants.ErrorKey,msg }
                }
            );
            return container;
        }
    }
}
