// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class representing the context in which data collection occurs.
/// </summary>
[DataContract]
public class DataCollectionContext
{
    // NOTE: These constructors are protected internal to allow 3rd parties to
    //       do unit testing of their data collectors.
    //
    //       We do not want to make the constructors of this class public as it
    //       would lead to a great deal of user error when they start creating
    //       their own data collection context instances to log errors/warnings
    //       or send files with.  The potential for this type of error still
    //       exists by having the protected constructor, but it is less likely
    //       and we have added safeguards in our DataCollectinLogger and
    //       DataCollectionDataSink to safeguard against derived types being
    //       passed to us.
    //
    //       In order to create mock instances of the DataCollectionContext for
    //       unit testing purposes, 3rd parties can derive from this class and
    //       have public constructors.  This will allow them to instantiate their
    //       class and pass to us for creating data collection events.

    /// <summary>
    /// Constructs DataCollection Context for in process data collectors
    /// </summary>
    /// <param name="testCase">test case to identify the context</param>
    public DataCollectionContext(TestCase? testCase)
    {
        TestCase = testCase;
        // TODO: Comment says this ctor should never have been made public but it was added.
        // This leaves a path where SessionId is null but the rest of the class doesn't handle it.
        SessionId = null!;
    }

    /// <summary>
    /// Constructs a DataCollectionContext indicating that there is a session,
    /// but no executing test, in context.
    /// </summary>
    /// <param name="sessionId">The session under which the data collection occurs.  Cannot be null.</param>
    protected internal DataCollectionContext(SessionId sessionId)
        : this(sessionId, (TestExecId?)null)
    {
    }

    /// <summary>
    /// Constructs a DataCollectionContext indicating that there is a session and an executing test,
    /// but no test step, in context.
    /// </summary>
    /// <param name="sessionId">The session under which the data collection occurs.  Cannot be null.</param>
    /// <param name="testExecId">The test execution under which the data collection occurs,
    /// or null if no executing test case is in context</param>
    protected internal DataCollectionContext(SessionId sessionId, TestExecId? testExecId)
    {
        //TODO
        //EqtAssert.ParameterNotNull(sessionId, "sessionId");

        SessionId = sessionId;
        TestExecId = testExecId;
        _hashCode = ComputeHashCode();
    }

    protected internal DataCollectionContext(SessionId sessionId, TestCase testCase)
        : this(sessionId, new TestExecId(testCase.Id))
    {
        TestCase = testCase;

    }
    /// <summary>
    /// Gets test case.
    /// </summary>
    [DataMember]
    public TestCase? TestCase { get; private set; }

    /// <summary>
    /// Identifies the session under which the data collection occurs.  Will not be null.
    /// </summary>
    [DataMember]
    public SessionId SessionId { get; }

    /// <summary>
    /// Identifies the test execution under which the data collection occurs,
    /// or null if no such test exists.
    /// </summary>
    [DataMember]
    public TestExecId? TestExecId { get; }

    /// <summary>
    /// Returns true if there is an executing test case associated with this context.
    /// </summary>
    [DataMember]
    [MemberNotNullWhen(true, nameof(TestExecId))]
    public bool HasTestCase
    {
        get { return TestExecId != null; }
    }

    public static bool operator ==(DataCollectionContext? context1, DataCollectionContext? context2)
    {
        return Equals(context1, context2);
    }

    public static bool operator !=(DataCollectionContext? context1, DataCollectionContext? context2)
    {
        return !(context1 == context2);
    }

    public override bool Equals(object? obj)
    {
        DataCollectionContext? other = obj as DataCollectionContext;

        return other != null
               && SessionId.Equals(other.SessionId)
               && (TestExecId == null ? other.TestExecId == null : TestExecId.Equals(other.TestExecId));
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    private int ComputeHashCode()
    {
        int hashCode = 17;

        hashCode = 31 * hashCode + SessionId.GetHashCode();

        if (TestExecId != null)
        {
            hashCode = 31 * hashCode + TestExecId.GetHashCode();
        }

        return hashCode;
    }

    private readonly int _hashCode;

}
