// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Slightly random id that is just good enough for creating disctinct directories for each test.
/// </summary>
public static class RandomId
{
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
    private const string Pool = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 5 character long id from 0-9A-Za-z0, for example fUfko, A6uvM, sOMXa, RY1ei, KvdJZ.
    /// </summary>
    /// <returns></returns>
    public static string Next()
    {
        return Next(5);
    }

    private static string Next(int length)
    {
        var poolLength = Pool.Length;
        var id = new char[length];
        lock (Pool)
        {
            for (var idIndex = 0; idIndex < length; idIndex++)
            {
                var poolIndex = poolLength + 1;
                while (poolIndex >= poolLength)
                {
                    var bytes = new byte[1];
                    Rng.GetNonZeroBytes(bytes);
                    poolIndex = bytes[0];
                }
                id[idIndex] = Pool[poolIndex];
            }
        }

        return new string(id);
    }
}
