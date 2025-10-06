namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.IO;

    internal class StubStream : MemoryStream
    {
        public Action<bool> OnDispose;

        public StubStream()
        {
            this.OnDispose = disposing => { };
        }

        protected override void Dispose(bool disposing)
        {
            this.OnDispose(disposing);
        }
    }
}
