// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Timers;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

internal interface ISystemTimersTimer : IDisposable
{
    event ElapsedEventHandler? Elapsed;

    void Start();
    void Stop();
}
