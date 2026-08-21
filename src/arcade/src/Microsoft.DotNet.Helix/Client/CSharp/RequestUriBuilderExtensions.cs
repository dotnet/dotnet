// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using Azure.Core;

namespace Microsoft.DotNet.Helix.Client
{
    internal static class RequestUriBuilderExtensions
    {
        public static void AppendQuery(this RequestUriBuilder builder, string name, IReadOnlyDictionary<string, string> values)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }

            string query = string.Join("&", values.Select(kvp => $"{WebUtility.UrlEncode(name)}[{WebUtility.UrlEncode(kvp.Key)}]={WebUtility.UrlEncode(kvp.Value)}"));
            if (string.IsNullOrEmpty(builder.Query))
            {
                builder.Query = query;
            }
            else
            {
                builder.Query = builder.Query.TrimStart('?') + "&" + query;
            }
        }
    }
}
