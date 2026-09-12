// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger;

/// <summary>
/// Where the id a test case actually carries came from.
/// </summary>
/// <remarks>
/// The whole point of the report is that this is not a boolean. A test id is not always computed by
/// the platform: an adapter is free to assign one itself, and MSTest v3 and v4 do exactly that
/// through their own id generation strategy, never reaching the platform hashing at all. Reporting
/// only the two computed candidates would tell every MSTest user their ids are about to change when
/// in fact they are not going to move at all.
/// </remarks>
internal enum TestIdSource
{
    /// <summary>
    /// The id equals the SHA1 hash of the seed, so the platform computed it with the legacy
    /// algorithm. This id changes when the default moves to xxHash128.
    /// </summary>
    Sha1,

    /// <summary>
    /// The id equals the xxHash128 hash of the seed, so the platform computed it with the new
    /// algorithm - the run already selected it. This id does not change when the default moves.
    /// </summary>
    XxHash128,

    /// <summary>
    /// The id matches neither candidate, so the adapter assigned it rather than the platform
    /// computing it. This id does not change when the default moves.
    /// </summary>
    SelfAssigned,
}

/// <summary>
/// One reported test: the id it carries, both ids the platform would compute for it, and enough
/// identity to join the row against a consumer's own records.
/// </summary>
internal sealed class TestIdRecord
{
    public TestIdRecord(
        string source,
        string executorUri,
        string fullyQualifiedName,
        string displayName,
        Guid id,
        Guid sha1Id,
        Guid xxHash128Id)
    {
        Source = source;
        ExecutorUri = executorUri;
        FullyQualifiedName = fullyQualifiedName;
        DisplayName = displayName;
        Id = id;
        Sha1Id = sha1Id;
        XxHash128Id = xxHash128Id;
    }

    /// <summary>
    /// The test container the test was found in, as the adapter reported it.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// The executor uri of the adapter that reported the test.
    /// </summary>
    public string ExecutorUri { get; }

    /// <summary>
    /// The fully qualified name of the test, as reported.
    /// </summary>
    public string FullyQualifiedName { get; }

    /// <summary>
    /// The display name of the test. For data driven tests this is usually the only thing that
    /// distinguishes one row from another, which is why it is reported.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// The id the test case actually carries.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The id the SHA1 algorithm computes from this test's seed.
    /// </summary>
    public Guid Sha1Id { get; }

    /// <summary>
    /// The id the xxHash128 algorithm computes from this test's seed.
    /// </summary>
    public Guid XxHash128Id { get; }

    /// <summary>
    /// Where <see cref="Id"/> came from, decided by comparing it against the two candidates.
    /// </summary>
    public TestIdSource IdSource
        => Id == Sha1Id ? TestIdSource.Sha1
            : Id == XxHash128Id ? TestIdSource.XxHash128
            : TestIdSource.SelfAssigned;
}
