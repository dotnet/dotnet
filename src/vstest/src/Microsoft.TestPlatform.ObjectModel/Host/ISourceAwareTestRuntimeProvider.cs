// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

/// <summary>
/// Internal extension of <see cref="ITestRuntimeProvider"/> that lets a runtime provider decide whether it
/// can host a run based on the actual <b>test sources</b>, not just the runsettings XML.
/// </summary>
/// <remarks>
/// The base <see cref="ITestRuntimeProvider.CanExecuteCurrentRunConfiguration(string?)"/> hook is
/// source-blind: it only receives the runsettings XML, so a provider cannot inspect the source assembly to
/// decide whether it owns it (for example, sniffing the Microsoft.Testing.Platform marker of an MTP app).
/// <para>
/// A provider that implements this interface is consulted <b>first</b> by
/// <c>TestRuntimeProviderManager.GetTestHostManagerByRunConfiguration</c>, before the source-blind providers.
/// This lets a more specific provider claim a source by its shape and be selected ahead of the generic
/// providers that match only by target framework — without any global ordering/priority scheme and without
/// requiring the other providers to decline. Providers that do not implement this interface keep their
/// existing source-blind behavior unchanged.
/// </para>
/// <para>
/// This is intentionally an <see langword="internal"/> interface (detected via a type check) so it adds no
/// public API surface; in-box providers reach it through <c>InternalsVisibleTo</c>.
/// </para>
/// </remarks>
internal interface ISourceAwareTestRuntimeProvider : ITestRuntimeProvider
{
    /// <summary>
    /// Determines whether this provider can host the given run for the specified test sources.
    /// </summary>
    /// <param name="runsettingsXml">The run configuration (runsettings XML).</param>
    /// <param name="sources">The test sources (assembly/executable paths) that will be run.</param>
    /// <returns>
    /// <see langword="true"/> if this provider should host the given sources; otherwise <see langword="false"/>.
    /// </returns>
    bool CanExecuteCurrentRunConfiguration(string? runsettingsXml, IEnumerable<string> sources);
}
