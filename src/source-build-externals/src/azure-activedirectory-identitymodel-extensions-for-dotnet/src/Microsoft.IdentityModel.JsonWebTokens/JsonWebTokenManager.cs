//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Globalization;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    // Feature is off pending additional memory tests.
    /*
    internal static class JsonWebTokenManager
    {
        internal static ConcurrentDictionary<string, string> KeyToHeaderCache = new ConcurrentDictionary<string, string>();
        internal static ConcurrentDictionary<string, JObject> RawHeaderToJObjectCache = new ConcurrentDictionary<string, JObject>();

        internal static string GetHeaderCacheKey(SecurityKey securityKey, string algorithm)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}", securityKey.GetType(), securityKey.KeyId, algorithm);
        }

        internal static string GetHeaderCacheKey(SigningCredentials signingCredentials)
        {
            if (signingCredentials == null)
                throw LogHelper.LogArgumentNullException(nameof(signingCredentials));

            return GetHeaderCacheKey(signingCredentials.Key, signingCredentials.Algorithm);
        }
    }
    */
}
