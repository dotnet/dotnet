// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IProjectPathProvider))]
[method: ImportingConstructor]
internal sealed class ProjectPathProvider(
    [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
    ITextDocumentFactoryService textDocumentFactoryService,
    [Import(AllowDefault = true)] ILiveShareProjectPathProvider? liveShareProjectPathProvider,
    JoinableTaskContext joinableTaskContext) : IProjectPathProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService = textDocumentFactoryService;
    private readonly ILiveShareProjectPathProvider? _liveShareProjectPathProvider = liveShareProjectPathProvider;
    private readonly JoinableTaskFactory _jtf = joinableTaskContext.Factory;

    public bool TryGetProjectPath(ITextBuffer textBuffer, [NotNullWhen(true)] out string? filePath)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (_liveShareProjectPathProvider is not null &&
            _liveShareProjectPathProvider.TryGetProjectPath(textBuffer, out filePath))
        {
            return true;
        }

        var vsHierarchy = textBuffer.GetVsHierarchy(_textDocumentFactoryService, _serviceProvider, _jtf);
        if (vsHierarchy is null)
        {
            filePath = null;
            return false;
        }

        filePath = vsHierarchy.GetProjectFilePath(_jtf);
        return filePath is not null;
    }
}
