// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

public static class CancellationTokenExtensions
{
    /// <summary>
    /// Extension method used to throw TestPlatformException in case operation is canceled.
    /// </summary>
    /// <param name="token">CancellationToken on which cancel is requested</param>
    public static void ThrowTestPlatformExceptionIfCancellationRequested(this CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            throw new TestPlatformException(Resources.Resources.CancellationRequested);
        }
    }
}
