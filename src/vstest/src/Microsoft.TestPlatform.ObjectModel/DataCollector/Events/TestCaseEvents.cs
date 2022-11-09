// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Base class for all test case event arguments.
/// </summary>
[DataContract]
public abstract class TestCaseEventArgs : DataCollectionEventArgs
{
    /// <summary>
    /// Default constructor added for serialization/deserialization.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TestCaseEventArgs()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // TODO: Make private
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testCaseId">
    /// The test case ID
    /// </param>
    /// <param name="testCaseName">
    /// The test case name
    /// </param>
    /// <param name="isChildTestCase">
    /// True if this is a child test case, false if this is a top-level test case.
    /// </param>
    protected TestCaseEventArgs(
        DataCollectionContext context,
        Guid testCaseId,
        string? testCaseName,
        bool isChildTestCase)
        : base(context)
    {
        TestCaseId = testCaseId;
        TestCaseName = testCaseName ?? string.Empty;
        IsChildTestCase = isChildTestCase;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testElement">
    /// The test element of the test that this event is for.
    /// </param>
    protected TestCaseEventArgs(
        DataCollectionContext context,
        TestCase testElement)
        : this(context, Guid.Empty, null, false)
    {
        // NOTE: ONLY USE FOR UNIT TESTING!
        //  This overload is only here for 3rd parties to use for unit testing
        //  their data collectors.  Internally we should not be passing the test element
        //  around in the events as this is extra information that needs to be serialized
        //  and the Execution Plugin Manager will fill this in for us before the event
        //  is sent to the data collector when running in a production environment.

        // TODO
        // EqtAssert.ParameterNotNull(testElement, "testElement");

        TestElement = testElement;
        TestCaseId = testElement.Id;
        TestCaseName = testElement.DisplayName;
        // IsChildTestCase = testElement != null &&
        // !testElement.ParentExecId.Equals(TestExecId.Empty);
    }

    /// <summary>
    /// Gets the test case ID
    /// </summary>
    [DataMember]
    public Guid TestCaseId
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the test case name
    /// </summary>
    [DataMember]
    public string TestCaseName
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether this is a child test case, false if this is a top-level test case
    /// </summary>
    [DataMember]
    public bool IsChildTestCase
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets test element of the test this event is for.
    /// </summary>
    [DataMember]
    public TestCase? TestElement
    {
        get;
        internal set;
    }

}

/// <summary>
/// Test Case Start event arguments.
/// </summary>
[DataContract]
public sealed class TestCaseStartEventArgs : TestCaseEventArgs
{
    /// <summary>
    /// Default constructor for serialization/deserialization
    /// </summary>
    public TestCaseStartEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseStartEventArgs"/> class with default datacollection context.
    /// </summary>
    /// <param name="testElement">
    /// The test element.
    /// </param>
    public TestCaseStartEventArgs(TestCase testElement) : this(new DataCollectionContext(new SessionId(Guid.Empty)), testElement)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseStartEventArgs"/> class.
    /// Initializes the instance by storing the given information.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testElement">
    /// The test element of the test that this event is for.
    /// </param>
    public TestCaseStartEventArgs(
        DataCollectionContext context,
        TestCase testElement)
        : base(context, testElement)
    {
        // NOTE: ONLY USE FOR UNIT TESTING!
        //  This overload is only here for 3rd parties to use for unit testing
        //  their data collectors.  Internally we should not be passing the test element
        //  around in the events as this is extra information that needs to be serialized
        //  and the Execution Plugin Manager will fill this in for us before the event
        //  is sent to the data collector when running in a production environment.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseStartEventArgs"/> class.
    /// Initializes the instance by storing the given information
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testCaseId">
    /// The test case ID
    /// </param>
    /// <param name="testCaseName">
    /// The test case name
    /// </param>
    /// <param name="isChildTestCase">
    /// True if this is a child test case, false if this is a top-level test case
    /// </param>
    internal TestCaseStartEventArgs(
        DataCollectionContext context,
        Guid testCaseId,
        string testCaseName,
        bool isChildTestCase)
        : base(context, testCaseId, testCaseName, isChildTestCase)
    {
        TPDebug.Assert(context.HasTestCase, "Context is not for a test case");
    }

}

/// <summary>
/// Test Case End event arguments.
/// </summary>
[DataContract]
public sealed class TestCaseEndEventArgs : TestCaseEventArgs
{
    /// <summary>
    /// Default constructor added for serialization/deserialization.
    /// </summary>
    public TestCaseEndEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEndEventArgs"/> class with default data collection context.
    /// </summary>
    /// <param name="testElement">
    /// The test element.
    /// </param>
    /// <param name="testOutcome">
    /// The test outcome.
    /// </param>
    /// <remarks>
    /// Default constructor with default DataCollectionContext.
    /// DataCollectionContext with empty session signifies that is it irrelevant in the current context.
    /// </remarks>
    public TestCaseEndEventArgs(TestCase testElement, TestOutcome testOutcome) : this(new DataCollectionContext(new SessionId(Guid.Empty)), testElement, testOutcome)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEndEventArgs"/> class.
    /// Initializes the instance by storing the given information.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testElement">
    /// The test element of the test that this event is for.
    /// </param>
    /// <param name="testOutcome">
    /// The outcome of the test case.
    /// </param>
    public TestCaseEndEventArgs(
        DataCollectionContext context,
        TestCase testElement,
        TestOutcome testOutcome)
        : base(context, testElement)
    {
        // NOTE: ONLY USE FOR UNIT TESTING!
        //  This overload is only here for 3rd parties to use for unit testing
        //  their data collectors.  Internally we should not be passing the test element
        //  around in the events as this is extra information that needs to be serialized
        //  and the Execution Plugin Manager will fill this in for us before the event
        //  is sent to the data collector when running in a production environment.
        TestOutcome = testOutcome;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEndEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testCaseId">
    /// The test case ID
    /// </param>
    /// <param name="testCaseName">
    /// The test case name
    /// </param>
    /// <param name="isChildTestCase">
    /// True if this is a child test case, false if this is a top-level test case
    /// </param>
    internal TestCaseEndEventArgs(
        DataCollectionContext context,
        Guid testCaseId,
        string testCaseName,
        bool isChildTestCase)
        : this(context, testCaseId, testCaseName, isChildTestCase, TestOutcome.Failed)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseEndEventArgs"/> class.
    /// Initializes the instance by storing the given information
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testCaseId">
    /// The test case ID
    /// </param>
    /// <param name="testCaseName">
    /// The test case name
    /// </param>
    /// <param name="isChildTestCase">
    /// True if this is a child test case, false if this is a top-level test case
    /// </param>
    /// <param name="testOutcome">
    /// The outcome of the test case.
    /// </param>
    internal TestCaseEndEventArgs(
        DataCollectionContext context,
        Guid testCaseId,
        string testCaseName,
        bool isChildTestCase,
        TestOutcome testOutcome)
        : base(context, testCaseId, testCaseName, isChildTestCase)
    {
        TPDebug.Assert(context.HasTestCase, "Context is not for a test case");
        TestOutcome = testOutcome;
    }

    /// <summary>
    /// Gets the outcome of the test.
    /// </summary>
    [DataMember]
    public TestOutcome TestOutcome
    {
        get;
        private set;
    }
}

/// <summary>
/// Test Case Result event arguments.
/// </summary>
[DataContract]
public sealed class TestResultEventArgs : TestCaseEventArgs
{
    /// <summary>
    /// Default constructor for serialization/deserialization
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TestResultEventArgs()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // TODO: Make private
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResultEventArgs"/> class with default data collection context.
    /// </summary>
    /// <param name="testResult">
    /// The test result.
    /// </param>
    /// <remarks>
    /// Default constructor with default DataCollectionContext.
    /// DataCollectionContext with empty session signifies that is it irrelevant in the current context.
    /// </remarks>
    public TestResultEventArgs(TestResult testResult)
        : this(new DataCollectionContext(new SessionId(Guid.Empty)), testResult)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResultEventArgs"/> class.
    /// Initializes the instance by storing the given information.
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testResult">
    /// The test Result.
    /// </param>
    public TestResultEventArgs(
        DataCollectionContext context,
        TestResult testResult)
        : base(context, testResult.TestCase)
    {
        // NOTE: ONLY USE FOR UNIT TESTING!
        //  This overload is only here for 3rd parties to use for unit testing
        //  their data collectors.  Internally we should not be passing the test element
        //  around in the events as this is extra information that needs to be serialized
        //  and the Execution Plugin Manager will fill this in for us before the event
        //  is sent to the data collector when running in a production environment.
        TestResult = testResult;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResultEventArgs"/> class.
    /// Initializes the instance by storing the given information
    /// </summary>
    /// <param name="context">
    /// Context information for the test case
    /// </param>
    /// <param name="testCaseId">
    /// The test case ID
    /// </param>
    /// <param name="testCaseName">
    /// The test case name
    /// </param>
    /// <param name="isChildTestCase">
    /// True if this is a child test case, false if this is a top-level test case
    /// </param>
    /// <param name="testResult">
    /// The test Result.
    /// </param>
    internal TestResultEventArgs(
        DataCollectionContext context,
        Guid testCaseId,
        string testCaseName,
        bool isChildTestCase,
        TestResult testResult)
        : base(context, testCaseId, testCaseName, isChildTestCase)
    {
        TPDebug.Assert(context.HasTestCase, "Context is not for a test case");
        TestResult = testResult;
    }

    /// <summary>
    /// Gets the outcome of the test.
    /// </summary>
    [DataMember]
    public TestResult TestResult
    {
        get;
        private set;
    }
}
