// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal enum MappingBehavior
{
    Strict,

    /// <summary>
    /// Inclusive mapping behavior will attempt to map overlapping or intersecting generated ranges with a provided projection range.
    ///
    /// Behavior:
    ///     - Overlaps > 1 generated range = No mapping
    ///     - Intersects > 1 generated range = No mapping
    ///     - Overlaps 1 generated range = Will reduce the provided range down to the generated range.
    ///     - Intersects 1 generated range = Will use the generated range mappings
    /// </summary>
    Inclusive,

    /// <summary>
    /// Inferred mapping behavior will attempt to map overlapping, intersecting or inbetween generated ranges with a provided projection range.
    ///
    /// Behavior: Everything `Inclusive` does +
    ///     - No mappings in document = No mapping
    ///     - Inbetween two mappings = Maps inbetween the two generated ranges
    ///     - Inbetween one mapping and end of document = Maps end of mapping to the end of document
    ///     - Inbetween beginning of document and one mapping = No mapping
    ///         o Usually errors flow forward in the document space (not backwards) which is why we don't map this scenario.
    /// </summary>
    Inferred
}
