// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace NuGetConsole.Implementation.PowerConsole
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType(PowerConsoleWindow.ContentType)]
    [Name("PowerConsoleCompletion")]
    internal class CompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        public IWpfConsoleService WpfConsoleService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return WpfConsoleService.TryCreateCompletionSource(textBuffer) as ICompletionSource;
        }
    }
}
