using System;
using System.Collections.Generic;

namespace Facebook.Unity.Windows
{
    public class WindowsResult : IInternalResult,ILoginResult
    {
        public WindowsResult()
        {
        }

        public string CallbackId { get; set; }

        public string Error { get; set; }

        public IDictionary<string, string> ErrorDictionary => throw new NotImplementedException();

        public IDictionary<string, object> ResultDictionary => throw new NotImplementedException();

        public string RawResult { get; set; }

        public bool Cancelled { get; set; }

        public AccessToken AccessToken { get; set; }

        public AuthenticationToken AuthenticationToken { get; set; }
    }
}
