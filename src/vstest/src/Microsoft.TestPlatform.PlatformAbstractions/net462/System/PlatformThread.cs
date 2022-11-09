// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System;
using System.Runtime.ExceptionServices;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

public class PlatformThread : IThread
{
    /// <inheritdoc/>
    public void Run(Action? action, PlatformApartmentState apartmentState, bool waitForCompletion)
    {
        if (action == null)
        {
            return;
        }

        Exception? exThrown = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                exThrown = e;
            }
        });

        ApartmentState state = Enum.TryParse(apartmentState.ToString(), out state) ? state : ApartmentState.MTA;
        thread.SetApartmentState(state);
        thread.IsBackground = true;
        thread.Start();
        if (waitForCompletion)
        {
            thread.Join();
            if (exThrown != null)
            {
                // Preserve the stacktrace when re-throwing the exception.
                ExceptionDispatchInfo.Capture(exThrown).Throw();
            }
        }
    }
}

#endif
