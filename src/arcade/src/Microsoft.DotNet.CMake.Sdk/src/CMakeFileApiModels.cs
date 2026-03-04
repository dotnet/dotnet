// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.DotNet.CMake.Sdk
{
    /// <summary>
    /// C# object models for the CMake File API.
    /// These types represent a subset of the CMake File API that we use for discovering build artifacts.
    /// 
    /// For the full CMake File API specification, see:
    /// https://cmake.org/cmake/help/latest/manual/cmake-file-api.7.html
    /// 
    /// Specifically, we use:
    /// - Index file: https://cmake.org/cmake/help/latest/manual/cmake-file-api.7.html#index-file
    /// - Codemodel object (v2): https://cmake.org/cmake/help/latest/manual/cmake-file-api.7.html#codemodel-version-2
    /// </summary>

    internal sealed class CMakeFileApiIndex
    {
        public CMakeFileApiIndexReply Reply { get; set; }
    }

    internal sealed class CMakeFileApiIndexReply
    {
        [JsonPropertyName("client-Microsoft.DotNet.CMake.Sdk")]
        public CMakeFileApiClientReply ClientReply { get; set; }
    }

    internal sealed class CMakeFileApiClientReply
    {
        [JsonPropertyName("codemodel-v2")]
        public CMakeFileApiIndexCodeModel CodemodelV2 { get; set; }
    }

    internal sealed class CMakeFileApiIndexCodeModel
    {
        public string JsonFile { get; set; }
    }

    internal sealed class CMakeCodeModel
    {
        public CMakeCodeModelPaths Paths { get; set; }
        public List<CMakeConfiguration> Configurations { get; set; }
    }

    internal sealed class CMakeCodeModelPaths
    {
        public string Source { get; set; }
        public string Build { get; set; }
    }

    internal sealed class CMakeConfiguration
    {
        public string Name { get; set; }
        public List<CMakeDirectory> Directories { get; set; }
        public List<CMakeTarget> Targets { get; set; }
    }

    internal sealed class CMakeDirectory
    {
        public string Source { get; set; }
        public string Build { get; set; }
        public List<int> TargetIndexes { get; set; }
    }

    internal sealed class CMakeTarget
    {
        public string Name { get; set; }
        public string JsonFile { get; set; }
    }

    internal sealed class CMakeTargetDetails
    {
        public List<CMakeArtifact> Artifacts { get; set; }
    }

    internal sealed class CMakeArtifact
    {
        public string Path { get; set; }
    }
}
