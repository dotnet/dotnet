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

internal sealed class FuseTestCase : XunitTestCase
{
    private bool _forceRuntimeCodeGeneration;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public FuseTestCase() { }

    public FuseTestCase(bool forceRuntimeCodeGeneration, IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
        _forceRuntimeCodeGeneration = forceRuntimeCodeGeneration;
    }

    protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
    {
        return base.GetDisplayName(factAttribute, displayName) + (_forceRuntimeCodeGeneration ? " (FUSE)" : "");
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
    {
        if (_forceRuntimeCodeGeneration && TestMethod.TestClass.TestCollection.TestAssembly.Assembly.Name.StartsWith("Microsoft.AspNetCore.Razor.LanguageServer"))
        {
            return "Language server cannot run FUSE tests";
        }

        if (_forceRuntimeCodeGeneration && factAttribute.GetNamedArgument<string>("SkipFuse") is { } skipReason)
        {
            return $"Skipping FUSE run: {skipReason}";
        }

        return base.GetSkipReason(factAttribute);
    }

    public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
    {
        Debug.Assert(constructorArguments.Length >= 1 && constructorArguments[0] is FuseTestContext, $"{TestMethod.TestClass.Class.Name}.{TestMethod.Method.Name} uses a formatting test attribute in a class without a FuseTestContext parameter?");
        constructorArguments[0] = new FuseTestContext
        {
            ForceRuntimeCodeGeneration = _forceRuntimeCodeGeneration
        };
        return base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
    }

    public override void Deserialize(IXunitSerializationInfo data)
    {
        _forceRuntimeCodeGeneration = data.GetValue<bool>(nameof(_forceRuntimeCodeGeneration));
        base.Deserialize(data);
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
        data.AddValue(nameof(_forceRuntimeCodeGeneration), _forceRuntimeCodeGeneration);
        base.Serialize(data);
    }

    protected override string GetUniqueID()
    {
        return base.GetUniqueID() + (_forceRuntimeCodeGeneration ? "FUSE" : "NOFUSE");
    }
}
