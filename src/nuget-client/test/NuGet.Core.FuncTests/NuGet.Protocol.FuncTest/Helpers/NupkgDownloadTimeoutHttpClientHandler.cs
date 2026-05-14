// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuGet.Protocol.FuncTest.Helpers
{
    /// <summary>
    /// A NuGet feed where every second nupkg download times out, starting from the first attempt.
    /// </summary>
    internal class NupkgDownloadTimeoutHttpClientHandler : MockV3ServerHttpClientHandler
    {
        bool _lastDownloadFailed;

        public NupkgDownloadTimeoutHttpClientHandler(IEnumerable<string> packagePaths)
            : base(packagePaths)
        {
            _lastDownloadFailed = false;
            FailedDownloads = 0;
        }

        public int FailedDownloads { get; private set; }

        protected override async Task<HttpResponseMessage> GetPackageDownloadResponse(string uri)
        {
            HttpResponseMessage response = await base.GetPackageDownloadResponse(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _lastDownloadFailed = !_lastDownloadFailed;
                if (_lastDownloadFailed)
                {
                    response.Content = new StreamContent(new TestTimeoutStream(await response.Content.ReadAsStreamAsync()));
                    FailedDownloads++;
                }
            }

            return response;

        }
    }
}
