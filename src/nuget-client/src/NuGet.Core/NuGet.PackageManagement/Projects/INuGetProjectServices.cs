// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Provides an API to a collection of <see cref="NuGetProject"/> scoped services, such as
    /// - project references
    /// - assembly references
    /// - project capabilities
    /// - binding redirects
    /// - script executor
    /// </summary>
    public interface INuGetProjectServices
    {
        /// <summary>
        /// Service to access project's build properties.
        /// </summary>
        [Obsolete]
        IProjectBuildProperties BuildProperties { get; }

        /// <summary>
        /// Service to query project system capabilities.
        /// </summary>
        IProjectSystemCapabilities Capabilities { get; }

        /// <summary>
        /// Service providing read-only access to references.
        /// </summary>
        IProjectSystemReferencesReader ReferencesReader { get; }

        /// <summary>
        /// Service to control references.
        /// </summary>
        IProjectSystemReferencesService References { get; }

        /// <summary>
        /// Service providing project system generic functionality.
        /// </summary>
        IProjectSystemService ProjectSystem { get; }

        /// <summary>
        /// Service to execute package scripts.
        /// </summary>
        IProjectScriptHostService ScriptService { get; }
    }
}
