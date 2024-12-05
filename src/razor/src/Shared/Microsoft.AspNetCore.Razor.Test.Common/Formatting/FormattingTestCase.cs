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

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public FormattingTestCase() { }

    public FormattingTestCase(bool shouldFlipLineEndings, bool forceRuntimeCodeGeneration, IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
        _shouldFlipLineEndings = shouldFlipLineEndings;
        _forceRuntimeCodeGeneration = forceRuntimeCodeGeneration;
    }

    protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
    {
        return base.GetDisplayName(factAttribute, displayName) + (_shouldFlipLineEndings ? " (LF)" : " (CRLF)") + (_forceRuntimeCodeGeneration ? " (FUSE)" : "");
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
    {
        if (_shouldFlipLineEndings && factAttribute.GetNamedArgument<bool>(nameof(FormattingTestFactAttribute.SkipFlipLineEnding)))
        {
            return "Some tests fail with LF line endings";
        }

        if (_forceRuntimeCodeGeneration && TestMethod.TestClass.TestCollection.TestAssembly.Assembly.Name.StartsWith("Microsoft.AspNetCore.Razor.LanguageServer"))
        {
            return "Language server cannot run FUSE tests";
        }

        return base.GetSkipReason(factAttribute);
    }

    public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
    {
        Debug.Assert(constructorArguments.Length >= 1 && constructorArguments[0] is FormattingTestContext, $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name} uses a formatting test attribute in a class without a FormattingTestContext parameter?");
        constructorArguments[0] = new FormattingTestContext
        {
            ShouldFlipLineEndings = _shouldFlipLineEndings,
            CreatedByFormattingDiscoverer = true
        };
        return base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
    }

    public override void Deserialize(IXunitSerializationInfo data)
    {
        _shouldFlipLineEndings = data.GetValue<bool>(nameof(_shouldFlipLineEndings));
        _forceRuntimeCodeGeneration = data.GetValue<bool>(nameof(_forceRuntimeCodeGeneration));
        base.Deserialize(data);
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
        data.AddValue(nameof(_shouldFlipLineEndings), _shouldFlipLineEndings);
        data.AddValue(nameof(_forceRuntimeCodeGeneration), _forceRuntimeCodeGeneration);
        base.Serialize(data);
    }

    protected override string GetUniqueID()
    {
        return base.GetUniqueID() + (_shouldFlipLineEndings ? "lf" : "crlf") + (_forceRuntimeCodeGeneration ? "FUSE" : "NOFUSE");
    }
}
