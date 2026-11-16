// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.TestPlatform.Hashing;

/// <summary>
/// Builds the string a test case id is hashed from.
/// </summary>
/// <remarks>
/// Shared source, compiled into <c>Microsoft.TestPlatform.ObjectModel</c>, where <c>TestCase</c>
/// computes its own id. <c>Microsoft.TestPlatform.CrossPlatEngine</c>, where the
/// Microsoft.Testing.Platform path has to compute the id in the runner process, uses that same
/// definition through <c>InternalsVisibleTo</c>, as does the temporary test id report logger
/// (<c>Microsoft.VisualStudio.TestPlatform.Extensions.TestIds.TestLogger</c>), which reports what
/// each algorithm computes and so has to hash exactly these bytes; that grant goes away with it.
/// All must derive the id from exactly the same bytes, so the composition lives in one place rather
/// than being written out several times and silently drifting apart.
/// </remarks>
internal static class TestIdSeed
{
    /// <summary>
    /// Composes the seed for a test case id: executor uri + source file name + fully qualified name.
    /// </summary>
    /// <param name="executorUri">The executor uri, as text.</param>
    /// <param name="source">The test container the test was found in.</param>
    /// <param name="fullyQualifiedName">
    /// The fully qualified name to use. This is the managed type and method based name when the test
    /// case carries those properties, and the plain fully qualified name otherwise; the caller
    /// resolves which, because only it knows.
    /// </param>
    /// <remarks>
    /// The parameters are nullable because this reproduces a concatenation of possibly-null values:
    /// a test case built through the serialization constructor has not had them assigned yet, and
    /// asking such an instance for its id used to yield a seed with the missing parts empty rather
    /// than throwing. Null and empty must therefore stay indistinguishable here.
    /// </remarks>
    public static string Compose(string? executorUri, string? source, string? fullyQualifiedName)
    {
        // If source is a file name then just use the filename for the identifier since the
        // file might have moved between discovery and execution (in appx mode for example)
        // This is not elegant because the Source contents should be a black box to the framework.
        // For example in the database adapter case this is not a file path.
        // As discussed with team, we found no scenario for netcore, & fullclr where the Source is not present where ID is generated,
        // which means we would always use FileName to generate ID. In cases where somehow Source Path contained garbage character the API Path.GetFileName()
        // we are simply returning original input.
        // For UWP where source during discovery, & during execution can be on different machine, in such case we should always use Path.GetFileName()
        string? fileNameOrSource = source;
        try
        {
            // If source name is malformed, GetFileName API will throw exception, so use same input malformed string to generate ID
            fileNameOrSource = Path.GetFileName(source);
        }
        catch
        {
            // do nothing
        }

        return executorUri + fileNameOrSource + fullyQualifiedName;
    }
}
