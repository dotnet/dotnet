// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

internal interface IFileChangeTracker
{
    string FilePath { get; }

    void StartListening();
    void StopListening();

    event EventHandler<FileChangeEventArgs> Changed;
}
