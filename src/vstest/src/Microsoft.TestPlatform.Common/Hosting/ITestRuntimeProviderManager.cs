// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

namespace Microsoft.VisualStudio.TestPlatform.Common.Hosting;

internal interface ITestRuntimeProviderManager
{
    ITestRuntimeProvider? GetTestHostManagerByRunConfiguration(string? runConfiguration, List<string> sources);
    ITestRuntimeProvider? GetTestHostManagerByUri(string hostUri);

    /// <summary>
    /// Runs only the source-aware first-refusal pass for a single source and returns the <see cref="Type"/> of
    /// the runtime provider that would claim it, without instantiating the provider. Returns <see langword="null"/>
    /// when no source-aware provider claims the source (i.e. it will be resolved by a generic, source-blind
    /// provider). This lets callers group sources by which source-aware provider owns them.
    /// </summary>
    Type? GetSourceAwareRuntimeProviderType(string? runConfiguration, string source);
}
