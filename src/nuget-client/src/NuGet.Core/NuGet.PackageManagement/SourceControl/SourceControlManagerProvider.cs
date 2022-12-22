// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectManagement
{
    // Helpers ensure that the singleton SourceControlManager is used across the codebase
    public interface ISourceControlManagerProvider
    {
        SourceControlManager GetSourceControlManager();
    }
}
