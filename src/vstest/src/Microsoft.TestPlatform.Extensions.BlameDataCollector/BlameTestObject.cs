// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public class BlameTestObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlameTestObject"/> class.
    /// </summary>
    public BlameTestObject()
    {
        // Default constructor
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameTestObject"/> class.
    /// </summary>
    /// <param name="fullyQualifiedName">
    /// Fully qualified name of the test case.
    /// </param>
    /// <param name="executorUri">
    /// The Uri of the executor to use for running this test.
    /// </param>
    /// <param name="source">
    /// Test container source from which the test is discovered.
    /// </param>
    public BlameTestObject(string fullyQualifiedName, Uri executorUri, string source)
    {
        Id = Guid.Empty;
        FullyQualifiedName = ValidateArg.NotNullOrEmpty(fullyQualifiedName, nameof(fullyQualifiedName));
        ExecutorUri = executorUri ?? throw new ArgumentNullException(nameof(executorUri));
        Source = ValidateArg.NotNullOrEmpty(source, nameof(source));
        IsCompleted = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameTestObject"/> class.
    /// </summary>
    /// <param name="testCase">
    /// The test case
    /// </param>
    public BlameTestObject(TestCase testCase)
    {
        Id = testCase.Id;
        FullyQualifiedName = testCase.FullyQualifiedName;
        ExecutorUri = testCase.ExecutorUri;
        Source = testCase.Source;
        DisplayName = testCase.DisplayName;
        IsCompleted = false;
    }

    /// <summary>
    /// Gets or sets the id of the test case.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the fully qualified name of the test case.
    /// </summary>
    public string? FullyQualifiedName { get; set; }

    /// <summary>
    /// Gets or sets the Uri of the Executor to use for running this test.
    /// </summary>
    public Uri? ExecutorUri { get; set; }

    /// <summary>
    /// Gets or sets the test container source from which the test is discovered.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether test case is completed or not.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the display name of the test case
    /// </summary>
    public string? DisplayName { get; set; }

}
