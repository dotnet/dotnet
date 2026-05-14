// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public interface IInactivityTimer : IDisposable
{
    /// <summary>
    /// Resets the timer and configures it to fire after inactivityTimespan elapses
    /// </summary>
    /// <param name="inactivityTimespan">Duration after which the timer should fire</param>
    void ResetTimer(TimeSpan inactivityTimespan);
}
