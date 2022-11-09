// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// metadata/contentFiles/files entry from a nuspec
    /// </summary>
    public class ContentFilesEntry
    {
        /// <summary>
        /// Included files
        /// </summary>
        /// <remarks>Required</remarks>
        public string Include { get; }

        /// <summary>
        /// Excluded files
        /// </summary>
        public string Exclude { get; }

        /// <summary>
        /// Build action
        /// </summary>
        public string BuildAction { get; }

        /// <summary>
        /// If true the item will be copied to the output folder.
        /// </summary>
        public bool? CopyToOutput { get; }

        /// <summary>
        /// If true the content items will keep the same folder structure in the output
        /// folder.
        /// </summary>
        public bool? Flatten { get; }

        public ContentFilesEntry(
            string include,
            string exclude,
            string buildAction,
            bool? copyToOutput,
            bool? flatten)
        {
            if (include == null)
            {
                throw new ArgumentNullException(nameof(include));
            }

            Include = include;
            Exclude = exclude;
            BuildAction = buildAction;
            CopyToOutput = copyToOutput;
            Flatten = flatten;
        }
    }
}
