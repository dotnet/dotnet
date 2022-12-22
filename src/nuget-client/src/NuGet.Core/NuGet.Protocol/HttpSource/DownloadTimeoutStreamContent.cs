// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net.Http;

namespace NuGet.Protocol
{
    /// <summary>
    /// A wrapper around <see cref="StreamContent"/> that applies a <see cref="DownloadTimeoutStream"/>
    /// to the contained stream. When the <see cref="HttpResponseMessage"/> is disposed, this
    /// content is disposed which in turn disposes the <see cref="DownloadTimeoutStream"/>, which
    /// disposes the actual network stream.
    /// </summary>
    public class DownloadTimeoutStreamContent : StreamContent
    {
        public DownloadTimeoutStreamContent(string downloadName, Stream networkStream, TimeSpan timeout)
            : base(new DownloadTimeoutStream(downloadName, networkStream, timeout))
        {
        }
    }
}
