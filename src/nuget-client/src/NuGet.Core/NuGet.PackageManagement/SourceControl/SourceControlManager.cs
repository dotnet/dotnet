// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Configuration;

namespace NuGet.ProjectManagement
{
    public abstract class SourceControlManager
    {
        protected ISettings Settings { get; set; }

        protected SourceControlManager(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            Settings = settings;
        }

        /// <summary>
        /// CreateFile does the following
        /// Marks the file for edit if it already exists
        /// Calls File.Create which creates a file or opens it if already exists
        /// Marks the file for add if it was just created
        /// It will perform necessary operations such as undoing pending changes and so on as appropriate
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="nuGetProjectContext"></param>
        /// <returns></returns>
        public abstract Stream CreateFile(string fullPath, INuGetProjectContext nuGetProjectContext);

        /// <summary>
        /// Marks the files for addition
        /// It will perform necessary operations such as undoing pending changes and so on as appropriate
        /// </summary>
        /// <param name="fullPaths"></param>
        /// <param name="nuGetProjectContext"></param>
        public abstract void PendAddFiles(IEnumerable<string> fullPaths, string root, INuGetProjectContext nuGetProjectContext);

        /// <summary>
        /// Marks the files for deletion
        /// It will perform necessary operations such as undoing pending changes and so on as appropriate
        /// </summary>
        public abstract void PendDeleteFiles(IEnumerable<string> fullPaths, string root, INuGetProjectContext nuGetProjectContext);

        /// <summary>
        /// Determines if the packages folder is bound to SourceControl
        /// If so, files added to packages folder must be checked-in to SourceControl
        /// </summary>
        /// <returns></returns>
        public bool IsPackagesFolderBoundToSourceControl()
        {
            return !SourceControlUtility.IsSourceControlDisabled(Settings);
        }
    }
}
