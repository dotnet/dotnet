// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

internal class TestProjectConfigurationFilePathStore : ProjectConfigurationFilePathStore
{
    public static readonly TestProjectConfigurationFilePathStore Instance = new();

    private TestProjectConfigurationFilePathStore()
    {
    }

    public override event EventHandler<ProjectConfigurationFilePathChangedEventArgs>? Changed;

    public override IReadOnlyDictionary<ProjectKey, string> GetMappings()
    {
        throw new NotImplementedException();
    }

    public override void Remove(ProjectKey projectKey)
    {
        Changed?.Invoke(this, new ProjectConfigurationFilePathChangedEventArgs(projectKey, configurationFilePath: null));
    }

    public override void Set(ProjectKey projectKey, string configurationFilePath)
    {
        Changed?.Invoke(this, new ProjectConfigurationFilePathChangedEventArgs(projectKey, configurationFilePath));
    }

    public override bool TryGet(ProjectKey projectKey, [NotNullWhen(returnValue: true)] out string? configurationFilePath)
    {
        configurationFilePath = null;
        return false;
    }
}
