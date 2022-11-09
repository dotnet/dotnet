// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio.Common
{
    /// <summary> Argument verification helpers. </summary>
    internal static class Verify
    {
        /// <summary> Throws <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null. </summary>
        /// <typeparam name="T"> Type of argument. </typeparam>
        /// <param name="argument"> Argument to verify. </param>
        /// <param name="argumentName"> Argument name. </param>
        internal static void ArgumentIsNotNull<T>(T argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
