// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.DotNet.Releases
{
    internal static class JsonExtensions
    {
        internal static bool IsNullOrEmpty(this JToken token)
        {
            return token is null || token is { Type: JTokenType.Null } || !token.HasValues;
        }
    }
}
