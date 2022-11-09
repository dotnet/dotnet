// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net;

namespace Test.Utility.Signing
{
    public abstract class HttpResponder : IHttpResponder
    {
        public abstract Uri Url { get; }

#if IS_SIGNING_SUPPORTED
        public abstract void Respond(HttpListenerContext context);

        protected static bool IsGet(HttpListenerRequest request)
        {
            return string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase);
        }

        protected static bool IsPost(HttpListenerRequest request)
        {
            return string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase);
        }

        protected static byte[] ReadRequestBody(HttpListenerRequest request)
        {
            using (var reader = new BinaryReader(request.InputStream))
            {
                return reader.ReadBytes((int)request.ContentLength64);
            }
        }

        protected static void WriteResponseBody(HttpListenerResponse response, byte[] bytes)
        {
            response.ContentLength64 = bytes.Length;

            using (var writer = new BinaryWriter(response.OutputStream))
            {
                writer.Write(bytes);
            }
        }
#endif
    }
}
