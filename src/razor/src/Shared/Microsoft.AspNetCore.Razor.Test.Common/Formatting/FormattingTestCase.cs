// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal sealed class FormattingTestCase : XunitTestCase
{
    private bool _shouldFlipLineEndings;
    private bool _forceRuntimeCodeGeneration;
    private bool _useNewFormattingEngine;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public FormattingTestCase() { }

    public FormattingTestCase(bool shouldFlipLineEndings, bool forceRuntimeCodeGeneration, bool useNewFormattingEngine, IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
        _shouldFlipLineEndings = shouldFlipLineEndings;
        _forceRuntimeCodeGeneration = forceRuntimeCodeGeneration;
        _useNewFormattingEngine = useNewFormattingEngine;
    }

    protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
    {
        return base.GetDisplayName(factAttribute, displayName) +
            (_useNewFormattingEngine ? " (NEW)" : "") +
            (_shouldFlipLineEndings ? " (LF)" : " (CRLF)") +
            (_forceRuntimeCodeGeneration ? " (FUSE)" : " "); // A single space here is important here for uniqueness!
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
    {
        if (_shouldFlipLineEndings && factAttribute.GetNamedArgument<bool>(nameof(FormattingTestFactAttribute.SkipFlipLineEnding)))
        {
            return "Some tests fail with LF line endings";
        }

        if (_shouldFlipLineEndings && !_useNewFormattingEngine && factAttribute.GetNamedArgument<bool>(nameof(FormattingTestFactAttribute.SkipFlipLineEndingInOldEngine)))
        {
            return "Some tests fail with LF line endings in the old formatting engine";
        }

        if (!_useNewFormattingEngine && factAttribute.GetNamedArgument<bool>(nameof(FormattingTestFactAttribute.SkipOldFormattingEngine)))
        {
            return "Some tests cover features not supported by the old formatting engine";
        }

        return base.GetSkipReason(factAttribute);
    }

    public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
    {
        Debug.Assert(constructorArguments.Length >= 1 && constructorArguments[0] is FormattingTestContext, $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name} uses a formatting test attribute in a class without a FormattingTestContext parameter?");
        constructorArguments[0] = new FormattingTestContext
        {
            ShouldFlipLineEndings = _shouldFlipLineEndings,
            ForceRuntimeCodeGeneration = _forceRuntimeCodeGeneration,
            UseNewFormattingEngine = _useNewFormattingEngine,
            CreatedByFormattingDiscoverer = true
        };
        return base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
    }

    public override void Deserialize(IXunitSerializationInfo data)
    {
        _shouldFlipLineEndings = data.GetValue<bool>(nameof(_shouldFlipLineEndings));
        _forceRuntimeCodeGeneration = data.GetValue<bool>(nameof(_forceRuntimeCodeGeneration));
        _useNewFormattingEngine = data.GetValue<bool>(nameof(_useNewFormattingEngine));
        base.Deserialize(data);
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
        data.AddValue(nameof(_shouldFlipLineEndings), _shouldFlipLineEndings);
        data.AddValue(nameof(_forceRuntimeCodeGeneration), _forceRuntimeCodeGeneration);
        data.AddValue(nameof(_useNewFormattingEngine), _useNewFormattingEngine);
        base.Serialize(data);
    }

    protected override string GetUniqueID()
    {
        return base.GetUniqueID() +
            (_useNewFormattingEngine ? "new" : "old") +
            (_shouldFlipLineEndings ? "lf" : "crlf") +
            (_forceRuntimeCodeGeneration ? "FUSE" : "NOFUSE");
    }
}
