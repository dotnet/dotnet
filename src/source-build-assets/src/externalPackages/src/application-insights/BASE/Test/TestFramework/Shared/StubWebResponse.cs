namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;

    internal class StubWebResponse : WebResponse, IDisposable
    {
        public Func<long> OnGetContentLength = () => default(long);
        public Func<string> OnGetContentType = () => default(string);
        public Func<WebHeaderCollection> OnGetHeaders = () => default(WebHeaderCollection);
        public Func<Uri> OnGetResponseUri = () => default(Uri);
        public Func<bool> OnGetSupportsHeaders = () => default(bool);
        public Action OnDispose = () => { };
        public Func<Stream> OnGetResponseStream = () => default(Stream);

        public override long ContentLength 
        {
            get { return this.OnGetContentLength(); }
        }

        public override string ContentType 
        {
            get { return this.OnGetContentType(); }
        }

        public override WebHeaderCollection Headers 
        {
            get { return this.OnGetHeaders(); }
        }

        public override Uri ResponseUri 
        {
            get { return this.OnGetResponseUri(); }
        }

        public override bool SupportsHeaders 
        {
            get { return this.OnGetSupportsHeaders(); }
        }
#pragma warning disable 0108
        
        public void Dispose()
        {
            this.OnDispose();
        }
#pragma warning restore 0108

        public override Stream GetResponseStream()
        {
            return this.OnGetResponseStream();
        }
    }
}
