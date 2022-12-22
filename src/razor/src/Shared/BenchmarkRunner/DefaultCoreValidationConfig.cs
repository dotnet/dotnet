// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace BenchmarkDotNet.Attributes;

internal class DefaultCoreValidationConfig : ManualConfig
{
    public DefaultCoreValidationConfig()
    {
        AddLogger(ConsoleLogger.Default);

        AddJob(Job.Dry.WithToolchain(InProcessNoEmitToolchain.Instance));
    }
}
