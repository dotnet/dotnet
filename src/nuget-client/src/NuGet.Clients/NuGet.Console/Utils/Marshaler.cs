// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.VisualStudio;

namespace NuGetConsole
{
    internal class Marshaler<T>
    {
        protected readonly T _impl;

        protected Marshaler(T impl)
        {
            _impl = impl;
        }

        /// <summary>
        /// Invoke an action on the main UI thread.
        /// </summary>
        protected static void Invoke(Action action)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    action();
                });
        }

        /// <summary>
        /// Invoke a function on the main UI thread.
        /// </summary>
        protected static TResult Invoke<TResult>(Func<TResult> func)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return func();
                });
        }
    }
}
