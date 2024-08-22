// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Shell;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace NuGet.VisualStudio
{
    public static class EnvDteExtensions
    {
        public static string GetSKU(this EnvDTE.DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var sku = dte.Edition;
            if (sku.Equals("Ultimate", StringComparison.OrdinalIgnoreCase)
                ||
                sku.Equals("Premium", StringComparison.OrdinalIgnoreCase)
                ||
                sku.Equals("Professional", StringComparison.OrdinalIgnoreCase))
            {
                sku = "Pro";
            }

            return sku;
        }

        public static string GetFullVsVersionString(this EnvDTE.DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return dte.Edition + "/" + dte.Version;
        }
    }
}
