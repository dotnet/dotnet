// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestUtilities;

/// <summary>Wraps another test case that should be skipped.</summary>
internal sealed class SkippedTestCase : XunitTestCase
{
    private string? _skipReason;

    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public SkippedTestCase() : base()
    {
    }

    public SkippedTestCase(
        string skipReason,
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod,
        object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
        _skipReason = skipReason;
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
        => _skipReason ?? base.GetSkipReason(factAttribute);

    public override void Deserialize(IXunitSerializationInfo data)
    {
        _skipReason = data.GetValue<string>(nameof(_skipReason));

        // we need to call base after reading our value, because Deserialize will call
        // into GetSkipReason.
        base.Deserialize(data);
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
        base.Serialize(data);
        data.AddValue(nameof(_skipReason), _skipReason);
    }
}