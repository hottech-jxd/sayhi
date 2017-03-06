using System.Collections.Specialized;
using System.Diagnostics;

namespace common
{
    public class HttpFormGetRequest
    {
        [DebuggerStepThrough]
        public HttpFormGetRequest()
        {
            Headers = new NameValueCollection();
            Cookies = new NameValueCollection();
        }

        public string Url
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public NameValueCollection Headers
        {
            get;
            set;
        }

        public NameValueCollection Cookies
        {
            get;
            set;
        }
    }
}