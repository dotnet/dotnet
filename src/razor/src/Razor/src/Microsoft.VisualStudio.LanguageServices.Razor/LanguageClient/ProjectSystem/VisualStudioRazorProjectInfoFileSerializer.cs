// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Serialization;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

[Export(typeof(IRazorProjectInfoFileSerializer))]
[method: ImportingConstructor]
internal sealed class VisualStudioRazorProjectInfoFileSerializer(ILoggerFactory loggerFactory)
    : RazorProjectInfoFileSerializer(loggerFactory)
{
}
