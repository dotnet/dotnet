// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.UI
{
    // Represents an item in the FileConflictAction combobox.
    public class FileConflictActionOptionItem
    {
        public string Text { get; }

        public FileConflictAction Action { get; private set; }

        public FileConflictActionOptionItem(string text, FileConflictAction action)
        {
            Text = text;
            Action = action;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
