// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal partial class BackgroundDocumentGenerator
{
    private sealed class Comparer : IEqualityComparer<(ProjectSnapshot, DocumentSnapshot)>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals((ProjectSnapshot, DocumentSnapshot) x, (ProjectSnapshot, DocumentSnapshot) y)
        {
            var (projectX, documentX) = x;
            var (projectY, documentY) = y;

            var documentKeyX = new DocumentKey(projectX.Key, documentX.FilePath);
            var documentKeyY = new DocumentKey(projectY.Key, documentY.FilePath);

            return documentKeyX.Equals(documentKeyY);
        }

        public int GetHashCode((ProjectSnapshot, DocumentSnapshot) obj)
        {
            var (project, document) = obj;
            var documentKey = new DocumentKey(project.Key, document.FilePath);

            return documentKey.GetHashCode();
        }
    }
}
