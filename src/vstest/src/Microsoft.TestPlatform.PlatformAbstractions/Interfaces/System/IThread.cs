// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

public interface IThread
{
    /// <summary>
    /// Runs the action in a thread with given apartment state.
    /// </summary>
    /// <param name="action">The Action to be called. No-Op if action is null.</param>
    /// <param name="apartmentState">The apartment state.</param>
    /// <param name="waitForCompletion"> True for running in Sync, False for running in Async</param>
    /// <exception cref="ThreadApartmentStateNotSupportedException"> When thread apartment state not supported for platform. </exception>>
    void Run(Action? action, PlatformApartmentState apartmentState, bool waitForCompletion);
}
