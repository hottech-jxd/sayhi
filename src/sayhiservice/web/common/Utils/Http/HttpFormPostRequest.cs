using System.Collections.Specialized;
using System.Diagnostics;

namespace common
{
    public class HttpFormPostRequest : HttpFormGetRequest
    {
        [DebuggerStepThrough]
        public HttpFormPostRequest()
        {
            FormFields = new NameValueCollection();
        }

        public NameValueCollection FormFields
        {
            get;
            set;
        }
    }
}