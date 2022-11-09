// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace NuGet.ProjectManagement
{
    public static class NuGetProjectMetadataKeys
    {
        /// <summary>
        /// The name of the project, e.g. "ConsoleApplication1"
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        /// The name of the project, relative to the solution. e.g. "src\ConsoleApplication1"
        /// </summary>
        public const string UniqueName = "UniqueName";

        public const string TargetFramework = "TargetFramework";

        public const string FullPath = "FullPath";

        /// <summary>
        /// Used by Project K projects
        /// </summary>
        public const string SupportedFrameworks = "SupportedFrameworks";

        public const string ProjectId = "ProjectId";
    }
}
