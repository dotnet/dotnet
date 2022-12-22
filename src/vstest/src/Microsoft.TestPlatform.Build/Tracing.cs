// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Build.Trace;

public static class Tracing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Part of the public API.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Part of the public API.")]
    public static bool traceEnabled = false;

    public static void Trace(string message)
    {
        if (traceEnabled)
        {
            Console.WriteLine(message);
        }
    }
}
