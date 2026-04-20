namespace FunctionalTests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using AI;
    using FunctionalTests.Utils;
    using Xunit.Abstractions;

    public class TelemetryHttpListenerObservable : HttpListenerObservableBase<Envelope>
    {        
        public bool FailureDetected { get; set; }

        public TelemetryHttpListenerObservable(string url, ITestOutputHelper output) : base(url, output)
        {
        }        

        protected override IEnumerable<Envelope> CreateNewItemsFromContext(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;

                string content = string.Empty;

                if (!string.IsNullOrWhiteSpace(request.Headers["Content-Encoding"]) &&
                    string.Equals("gzip", request.Headers["Content-Encoding"],
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    content = Decompress(request);
                }

                Trace.WriteLine("=>");
                Trace.WriteLine("Item received: " + content);
                Trace.WriteLine("<=");                

                return TelemetryItemFactory.GetTelemetryItems(content);
            }
            finally
            {
                context.Response.Close();
            }
        }

        /// <summary>
        /// Decompresses content in gzip and returns decompressed string
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string Decompress(HttpListenerRequest request)
        {
            var gzipStream = new GZipStream(request.InputStream, CompressionMode.Decompress);
            using (var streamReader = new StreamReader(gzipStream, request.ContentEncoding))
            {
                return streamReader.ReadToEnd();
            }
        }        
    }
}
