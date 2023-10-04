// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudio.Editor.Razor.Logging;

internal interface IOutputWindowLogger : ILogger
{
    void SetTestLogger(ILogger? testOutputLogger);
}
