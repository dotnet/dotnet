// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents an API providing different capabilities
    /// exposed by a project system
    /// </summary>
    public interface IProjectSystemCapabilities
    {
        bool SupportsPackageReferences { get; }

        bool NominatesOnSolutionLoad { get; }
    }
}
