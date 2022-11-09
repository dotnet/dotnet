// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Versioning;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeAssemblyMetadataProvider : IAssemblyMetadataProvider
{
    public FakeFileHelper FakeFileHelper { get; }

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public FakeAssemblyMetadataProvider(FakeFileHelper fakeFileHelper, FakeErrorAggregator fakeErrorAggregator)
    {
        FakeFileHelper = fakeFileHelper;
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public Architecture GetArchitecture(string filePath)
    {
        var file = FakeFileHelper.GetFakeFile<FakeTestDllFile>(filePath);
        return file.Architecture;
    }

    public FrameworkName GetFrameworkName(string filePath)
    {
        var file = FakeFileHelper.GetFakeFile<FakeTestDllFile>(filePath);
        return file.FrameworkName;
    }
}
