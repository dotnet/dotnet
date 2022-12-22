// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET472

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Tools
{
    internal static class AssemblyResolution
    {
        internal static TaskLoggingHelper Log;

        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            if (!name.Name.Equals("System.Collections.Immutable", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fullPath = Path.Combine(Path.GetDirectoryName(typeof(AssemblyResolution).Assembly.Location), "System.Collections.Immutable.dll");

            Assembly sci;
            try
            {
                sci = Assembly.LoadFile(fullPath);
            }
            catch (Exception e)
            {
                Log?.LogWarning($"AssemblyResolve: exception while loading '{fullPath}': {e.Message}");
                return null;
            }

            if (name.Version <= sci.GetName().Version)
            {
                Log?.LogMessage(MessageImportance.Low, $"AssemblyResolve: loaded '{fullPath}' to {AppDomain.CurrentDomain.FriendlyName}");
                return sci;
            }

            return null;
        }
    }
}

#endif
