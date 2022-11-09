// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// The event args of the ActionsExecuted event.
    /// </summary>
    public class ActionsExecutedEventArgs : EventArgs
    {
        public ActionsExecutedEventArgs(IEnumerable<ResolvedAction> actions)
        {
            Actions = actions;
        }

        /// <summary>
        /// The list of actions that are executed.
        /// </summary>
        public IEnumerable<ResolvedAction> Actions { get; }
    }
}
