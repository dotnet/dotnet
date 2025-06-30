// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.VisualStudio.Razor.Documents;

internal interface IFileChangeTracker
{
    string FilePath { get; }

    void StartListening();
    void StopListening();

    event EventHandler<FileChangeEventArgs> Changed;
}
