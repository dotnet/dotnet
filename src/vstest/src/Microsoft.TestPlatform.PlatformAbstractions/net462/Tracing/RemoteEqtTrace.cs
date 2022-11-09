// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// A class used to expose EqtTrace functionality across AppDomains.
/// </summary>
public sealed class RemoteEqtTrace : MarshalByRefObject
{
    /// <summary>
    /// Gets or sets the trace level.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public TraceLevel TraceLevel
    {
        get
        {
            return PlatformEqtTrace.TraceLevel;
        }

        set
        {
            PlatformEqtTrace.TraceLevel = value;
        }
    }

    /// <summary>
    /// Register listeners from parent domain in current domain.
    /// </summary>
    /// <param name="listener">Trace listener instance.</param>
    internal static void SetupRemoteListeners(TraceListener? listener)
    {
        PlatformEqtTrace.SetupRemoteListeners(listener);
    }
}

#endif
