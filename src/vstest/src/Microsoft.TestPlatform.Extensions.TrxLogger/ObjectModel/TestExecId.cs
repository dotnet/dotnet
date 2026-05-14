// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Class identifying test execution id.
/// Execution ID is assigned to test at run creation time and is guaranteed to be unique within that run.
/// </summary>
internal sealed class TestExecId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecId"/> class.
    /// </summary>
    public TestExecId()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestExecId"/> class.
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    public TestExecId(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets an object of <see cref="TestExecId"/> class which empty GUID
    /// </summary>
    public static TestExecId Empty { get; } = new TestExecId(Guid.Empty);

    /// <summary>
    /// Gets the id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Override function of Equals.
    /// </summary>
    /// <param name="obj">
    /// The object to compare.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is TestExecId id && Id.Equals(id.Id);
    }

    /// <summary>
    /// Override function of GetHashCode
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Override function of ToString.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string ToString()
    {
        string s = Id.ToString("B");
        return string.Format(CultureInfo.InvariantCulture, s);
    }
}
