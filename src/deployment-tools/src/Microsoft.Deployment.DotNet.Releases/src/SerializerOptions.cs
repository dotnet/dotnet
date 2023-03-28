// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Options used to deserialize the release JSON files.
    /// </summary>
    internal static class SerializerOptions
    {
        /// <summary>
        /// The default deserialization options.
        /// </summary>
        public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
        {
            Converters =
            {
                new ReleaseVersionConverter(),
                new ReleaseTypeConverter(),
                new SupportPhaseConverter(),
                new VersionConverter()
            }
        };
    }
}
