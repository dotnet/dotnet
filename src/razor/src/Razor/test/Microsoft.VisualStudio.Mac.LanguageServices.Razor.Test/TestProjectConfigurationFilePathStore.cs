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

    public override IReadOnlyDictionary<string, string> GetMappings()
    {
        throw new NotImplementedException();
    }

    public override void Remove(string projectFilePath)
    {
        Changed?.Invoke(this, new ProjectConfigurationFilePathChangedEventArgs(projectFilePath, configurationFilePath: null));
    }

    public override void Set(string projectFilePath, string configurationFilePath)
    {
        Changed?.Invoke(this, new ProjectConfigurationFilePathChangedEventArgs(projectFilePath, configurationFilePath));
    }

    public override bool TryGet(string projectFilePath, [NotNullWhen(returnValue: true)] out string? configurationFilePath)
    {
        configurationFilePath = null;
        return false;
    }
}
