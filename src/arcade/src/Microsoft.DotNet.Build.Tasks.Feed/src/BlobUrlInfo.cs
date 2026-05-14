// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class BlobUrlInfo
    {
        private const string AccountNameAndEndpointRegex = @"^(?<accountname>[a-z0-9]+)\.(?<endpoint>.+?)";
        private const string ContainerAndBlobRegex = @"^/(?<containername>[^\/]+)/(?<blobpath>.*)";

        public string AccountName { get; set; }

        public string Endpoint { get; set; }

        public string ContainerName { get; set; }

        public string BlobPath { get; set; }

        public bool HasToken { get; set; }

        public Uri Uri { get; private set; }

        public BlobUrlInfo(string url)
            : this(new Uri(url))
        { }

        public BlobUrlInfo(Uri uri)
        {
            this.Uri = uri;

            // Account name is the first element of the hostname.
            string hostName = uri.Host;
            Match hostNameMatch = Regex.Match(hostName, AccountNameAndEndpointRegex);

            if (hostNameMatch.Success)
            {
                AccountName = hostNameMatch.Groups["accountname"].Value;
                Endpoint = hostNameMatch.Groups["endpoint"].Value;
            }
            else
            {
                throw new ArgumentException(string.Format("Blob URL host name {0} should be of the form <account name>.<endpoint>", hostName));
            }

            String path = uri.AbsolutePath;
            Match containerAndBlobMatch = Regex.Match(path, ContainerAndBlobRegex);

            if (containerAndBlobMatch.Success)
            {
                ContainerName = containerAndBlobMatch.Groups["containername"].Value;
                BlobPath = containerAndBlobMatch.Groups["blobpath"].Value;
            }
            else
            {
                throw new ArgumentException(string.Format("Blob URL path {0} should have a container and blob path", path));
            }

            // TODO, for authenticated NuGet feeds using traditional query strings, we should change this
            // to support recognition of the SAS token
            if (!String.IsNullOrEmpty(uri.Query))
            {
                HasToken = true;
                throw new NotImplementedException("Authenticated SAS token blob URIs is not yet implemented");
            }
        }

        public BlobUrlInfo(string accountName, string containerName, string blobPath, string endpoint = "blob.core.windows.net")
        {
            AccountName = accountName;
            ContainerName = containerName;
            BlobPath = blobPath;
            Endpoint = endpoint;
            // Set Uri
            Uri = new Uri($"https://{accountName}.{endpoint}/{containerName}/{blobPath}");
        }
    }
}
