// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Logging;

[MetadataAttribute]
[AttributeUsage(AttributeTargets.Class)]
internal sealed class ExportLoggerProviderAttribute : ExportAttribute
{
    public LogLevel? MinimumLogLevel { get; }

    public ExportLoggerProviderAttribute()
        : base(typeof(ILoggerProvider))
    {
        MinimumLogLevel = null;
    }

    public ExportLoggerProviderAttribute(LogLevel minimumLogLevel)
        : base(typeof(ILoggerProvider))
    {
        MinimumLogLevel = minimumLogLevel;
    }
}
