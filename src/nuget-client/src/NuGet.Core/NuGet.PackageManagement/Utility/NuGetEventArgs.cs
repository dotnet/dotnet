// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Generic EventArg class for NuGet.
    /// </summary>
    /// <typeparam name="T">Class type of the argument Arg.</typeparam>
    public sealed class NuGetEventArgs<T> : EventArgs where T : class
    {
        /// <summary>
        /// Argument data of the EventArg.
        /// </summary>
        public T Arg { get; }

        /// <summary>
        /// Constructor for creating NuGetEventArgs object.
        /// </summary>
        /// <param name="arg"> Argument to NuGetEventArgs of type T.</param>
        public NuGetEventArgs(T arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            Arg = arg;
        }
    }
}
