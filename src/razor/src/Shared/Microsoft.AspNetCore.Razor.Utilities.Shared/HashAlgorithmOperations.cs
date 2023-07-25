// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Security.Cryptography;

namespace System;

internal static class HashAlgorithmOperations
{
    public static HashAlgorithm Create()
        => SHA256.Create();

    public static string? GetAlgorithmName()
        => HashAlgorithmName.SHA256.Name;
}
