// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    public delegate void SolutionRestoreCompletedEventHandler(SolutionRestoredEventArgs args);

    /// <summary>
    /// The interface for listening to events concerning restore operations.
    /// </summary>
    public interface IRestoreEvents
    {
        event SolutionRestoreCompletedEventHandler SolutionRestoreCompleted;
    }
}
