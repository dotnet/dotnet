// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Represents a loading strategy
/// </summary>
[Flags]
internal enum TestAdapterLoadingStrategy
{
    /// <summary>
    /// A strategy not defined, Test Platform will load adapters normally.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Test Platform will only load adapters specified by /TestAdapterPath (or RunConfiguration.TestAdaptersPaths node).
    /// If a specific adapter path is provided, adapter will be loaded; if a directory path is provided adapters directly in that folder will be loaded.
    /// If no adapter path is specified, test run will fail.
    /// This will imply /InIsolation switch and force the tests to be run in an isolated process.
    /// </summary>
    Explicit = 1,

    /// <summary>
    /// Load adapters next to source.
    /// </summary>
    NextToSource = 2,

    /// <summary>
    /// Default runtime providers inside Extensions folder will be included.
    /// </summary>
    DefaultRuntimeProviders = 4,

    /// <summary>
    /// Load adapters inside Extensions folder.
    /// </summary>
    ExtensionsDirectory = 8,

    /// <summary>
    /// Directory wide searches will be recursive, this is required to be used with <see cref="NextToSource" /> or <see cref="Explicit" />.
    /// </summary>
    Recursive = 16,
}
