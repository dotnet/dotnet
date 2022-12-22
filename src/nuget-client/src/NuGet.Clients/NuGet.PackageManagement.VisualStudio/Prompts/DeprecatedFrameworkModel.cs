// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using NuGet.Frameworks;

namespace NuGet.PackageManagement.VisualStudio
{
    public class DeprecatedFrameworkModel
    {
        public DeprecatedFrameworkModel(NuGetFramework deprecated, string migrationUrl, IReadOnlyList<string> projects)
        {
            TextBeforeLink = string.Format(
                CultureInfo.CurrentCulture,
                Strings.Text_DeprecatedFramework_DocumentLink_Before,
                deprecated.DotNetFrameworkName,
                deprecated.GetShortFolderName());
            LinkText = Strings.Text_DeprecatedFramework_DocumentLink;
            TextAfterLink = Strings.Text_DeprecatedFramework_DocumentLink_After;
            ProjectListText = Strings.Text_DeprecatedFramework_ProjectList;

            MigrationUrl = migrationUrl;
            Projects = projects;
        }

        public string TextBeforeLink { get; }
        public string LinkText { get; }
        public string TextAfterLink { get; }
        public string ProjectListText { get; }
        public string MigrationUrl { get; }
        public IReadOnlyList<string> Projects { get; }
    }
}
