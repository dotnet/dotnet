// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Packaging.Signing
{
    public static class KeyPairFileUtility
    {
        /// <summary>
        /// Throw if the expected value does not exist.
        /// </summary>
        public static string GetValueOrThrow(Dictionary<string, string> values, string key)
        {
            if (values.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new SignatureException($"Missing expected key: {key}");
        }
    }
}
