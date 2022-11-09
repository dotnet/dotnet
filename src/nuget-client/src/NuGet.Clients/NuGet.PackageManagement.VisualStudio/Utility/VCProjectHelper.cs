// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.VCProjectEngine;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class VCProjectHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddFileToProject(object project, string filePath, string folderPath)
        {
            var vcProject = project as VCProject;
            if (vcProject != null)
            {
                if (String.IsNullOrEmpty(folderPath))
                {
                    vcProject.AddFile(filePath);
                }
                else
                {
                    var filter = GetFilter(vcProject, folderPath, createIfNotExists: true);
                    if (filter != null)
                    {
                        filter.AddFile(filePath);
                    }
                }
            }
        }

        /// <summary>
        /// Get the filter that is represented by the specified path
        /// </summary>
        private static VCFilter GetFilter(VCProject vcProject, string folderPath, bool createIfNotExists)
        {
            Debug.Assert(!String.IsNullOrEmpty(folderPath));

            string[] paths = folderPath.Split(Path.DirectorySeparatorChar);

            // recursively walks the folder path to get the last folder
            dynamic parent = vcProject;
            foreach (string path in paths)
            {
                VCFilter childFilter = null;
                foreach (VCFilter child in parent.Filters)
                {
                    if (child.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                    {
                        childFilter = child;
                        break;
                    }
                }

                if (childFilter == null)
                {
                    if (createIfNotExists)
                    {
                        // if a child folder doesn't already exist, create it
                        childFilter = parent.AddFilter(path);
                    }
                    else
                    {
                        return null;
                    }
                }
                parent = childFilter;
            }

            return (VCFilter)parent;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool RemoveFileFromProject(object project, string filePath, string folderPath)
        {
            var vcProject = project as VCProject;
            if (vcProject != null)
            {
                IEnumerable files = null;

                if (String.IsNullOrEmpty(folderPath))
                {
                    files = vcProject.Files as IEnumerable;
                }
                else
                {
                    var filter = GetFilter(vcProject, folderPath, createIfNotExists: false);
                    if (filter != null)
                    {
                        files = filter.Files as IEnumerable;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (files != null)
                {
                    string fileName = Path.GetFileName(filePath);
                    foreach (VCFile file in files)
                    {
                        if (file.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                        {
                            var parent = file.Parent;
                            file.Remove();
                            DeleteAllParentFilters(parent);
                            break;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete all parent, grand parent, etc. of this file if they are empty, after deleting this file.
        /// </summary>
        private static void DeleteAllParentFilters(object filter)
        {
            VCFilter currentFilter = filter as VCFilter;
            while (currentFilter != null)
            {
                var remainingFiles = currentFilter.Files as IEnumerable;
                var remainingFilters = currentFilter.Filters as IEnumerable;

                // if the current filter is empty, delete it
                if (!remainingFiles.GetEnumerator().MoveNext()
                    && !remainingFilters.GetEnumerator().MoveNext())
                {
                    var parent = currentFilter.Parent;
                    currentFilter.Remove();
                    currentFilter = parent as VCFilter;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
