// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Class to categorize the tests.
/// </summary>
internal sealed class TestListCategoryId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestListCategoryId"/> class.
    /// </summary>
    public TestListCategoryId()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestListCategoryId"/> class.
    /// </summary>
    /// <param name="id">
    /// The id (GUID).
    /// </param>
    public TestListCategoryId(Guid id)
    {
        Id = id;
    }


    /// <summary>
    /// Gets the Id of very root category - parent of all categories (fake, not real category).
    /// </summary>
    public static TestListCategoryId Root { get; } = new TestListCategoryId(Guid.Empty);

    /// <summary>
    /// Gets an object of <see cref="TestListCategoryId"/> class with empty GUID.
    /// </summary>
    public static TestListCategoryId Empty
    {
        get { return Root; }
    }

    /// <summary>
    /// Gets an object of <see cref="TestListCategoryId"/> class with GUID which represent uncategorized.
    /// </summary>
    public static TestListCategoryId Uncategorized { get; } = new(new Guid("8C84FA94-04C1-424b-9868-57A2D4851A1D"));

    /// <summary>
    /// Gets an object of <see cref="TestListCategoryId"/> class with GUID which represent categorize.
    /// </summary>
    public static TestListCategoryId Categories { get; } = new TestListCategoryId(new Guid("8C43106B-9DC1-4907-A29F-AA66A61BF5B6"));

    /// <summary>
    /// Gets the id.
    /// </summary>
    public Guid Id { get; }

    public static TestListCategoryId AllItems { get; } = new(new Guid("19431567-8539-422a-85D7-44EE4E166BDA"));


    /// <summary>
    /// Override function for Equals.
    /// </summary>
    /// <param name="other">
    /// The object to compare.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? other)
    {
        return other is TestListCategoryId testListCategoryId && Id.Equals(testListCategoryId.Id);
    }

    /// <summary>
    /// Override function for GetHashCode.
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Override function for ToString.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string ToString()
    {
        // "B" adds curly braces around guid
        string s = Id.ToString("B");
        return string.Format(CultureInfo.InvariantCulture, s);
    }
}
