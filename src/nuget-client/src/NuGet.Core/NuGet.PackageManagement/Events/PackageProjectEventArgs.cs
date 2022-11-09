// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Event arguments for nuget batch events.
    /// </summary>
    public class PackageProjectEventArgs : EventArgs
    {
        public PackageProjectEventArgs(string id, string name, string projectPath)
            : this(id, name)
        {
            ProjectPath = projectPath;
        }

        public PackageProjectEventArgs(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// A unique ID for the operation.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The project name. Usually matches the project file name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The path for the project getting updated. May be null in the scenarios where there's no project file.
        /// </summary>
        public string ProjectPath { get; }
    }
}
