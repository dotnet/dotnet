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

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Net.Http;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Provides a way to add and remove telemetry data.
    /// </summary>
    public static class IdentityModelTelemetryUtil
    {
        internal const string skuTelemetry = "x-client-SKU";
        internal const string versionTelemetry = "x-client-Ver";
        internal static readonly List<string> defaultTelemetryValues = new List<string> { skuTelemetry, versionTelemetry };
        internal static readonly ConcurrentDictionary<string, string> telemetryData = new ConcurrentDictionary<string, string>()
        {
            [skuTelemetry] = ClientSku,
            [versionTelemetry] = ClientVer
        };

        /// <summary>
        /// Get the string that represents the client SKU.
        /// </summary>
        public static string ClientSku =>
#if NET45
            "ID_NET45";
#elif NET461
            "ID_NET461";
#elif NET472
            "ID_NET472";
#elif NETSTANDARD2_0
            "ID_NETSTANDARD2_0";
#elif NET6_0
            "ID_NET6_0";
#endif

        /// <summary>
        /// Get the string that represents the client version.
        /// </summary>
        public static string ClientVer => typeof(IdentityModelTelemetryUtil).GetTypeInfo().Assembly.GetName().Version.ToString();

        /// <summary>
        /// Adds a key and its value to the collection of telemetry data.
        /// </summary>
        /// <param name="key"> The name of the telemetry.</param>
        /// <param name="value"> The value of the telemetry.</param>
        /// <returns> true if the key is successfully added; otherwise, false.</returns>
        public static bool AddTelemetryData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogHelper.LogArgumentNullException(nameof(key));
                return false;
            }

            if (string.IsNullOrEmpty(value))
            {
                LogHelper.LogArgumentNullException(nameof(value));
                return false;
            }

            if (defaultTelemetryValues.Contains(key))
            {
                LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.MIML10003));
                return false;
            }

            telemetryData[key] = value;
            return true;
        }

        /// <summary>
        /// Removes a key and its value from the collection of telemetry data.
        /// </summary>
        /// <param name="key"> The name of the telemetry.</param>
        /// <returns> true if the key is successfully removed; otherwise, false.</returns>
        public static bool RemoveTelemetryData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogHelper.LogArgumentNullException(nameof(key));
                return false;
            }

            if (defaultTelemetryValues.Contains(key))
            {
                LogHelper.LogExceptionMessage(new ArgumentException(LogMessages.MIML10003));
                return false;
            }

            return telemetryData.TryRemove(key, out _);
        }

        internal static void SetTelemetryData(HttpRequestMessage request, IDictionary<string, string> additionalHeaders)
        {
            if (request == null)
                return;

            foreach (var parameter in telemetryData)
            {
                // remove this header if it already exists.
                // we don't want to add an additional value in case when a telemetry header already exists, but to overwrite it.
                request.Headers.Remove(parameter.Key);
                request.Headers.Add(parameter.Key, parameter.Value);
            }

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                    request.Headers.Add(header.Key, header.Value);
            }
        }

        internal static bool UpdateDefaultTelemetryData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogHelper.LogArgumentNullException(nameof(key));
                return false;
            }

            if (string.IsNullOrEmpty(value))
            {
                LogHelper.LogArgumentNullException(nameof(value));
                return false;
            }

            telemetryData[key] = value;
            return true;
        }
    }
}
