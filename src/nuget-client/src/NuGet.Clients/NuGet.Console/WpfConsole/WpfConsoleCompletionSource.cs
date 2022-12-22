// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using NuGet.VisualStudio;

namespace NuGetConsole.Implementation.Console
{
    internal class WpfConsoleCompletionSource : ObjectWithFactory<WpfConsoleService>, ICompletionSource
    {
        private ITextBuffer TextBuffer { get; set; }

        public WpfConsoleCompletionSource(WpfConsoleService factory, ITextBuffer textBuffer)
            : base(factory)
        {
            UtilityMethods.ThrowIfArgumentNull(textBuffer);
            this.TextBuffer = textBuffer;
        }

        private WpfConsole _console;

        private WpfConsole Console
        {
            get
            {
                if (_console == null)
                {
                    TextBuffer.Properties.TryGetProperty<WpfConsole>(typeof(IConsole), out _console);
                    Debug.Assert(_console != null);
                }

                return _console;
            }
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (Console == null
                || Console.InputLineStart == null)
            {
                return;
            }

            SimpleExpansion simpleExpansion;
            if (session.Properties.TryGetProperty("TabExpansion", out simpleExpansion))
            {
                List<Completion> completions = new List<Completion>();
                foreach (string s in simpleExpansion.Expansions)
                {
                    completions.Add(new Completion(s, s, null, null, null));
                }

                SnapshotPoint inputStart = Console.InputLineStart.Value;
                ITrackingSpan span = inputStart.Snapshot.CreateTrackingSpan(
                    new SnapshotSpan(inputStart + simpleExpansion.Start, simpleExpansion.Length),
                    SpanTrackingMode.EdgeInclusive);

                completionSets.Add(new CompletionSet(
                    Console.ContentTypeName, Console.ContentTypeName, span, completions, null));
            }
        }

        #region IDispose

        public void Dispose()
        {
            // Nothing to do.
        }

        #endregion
    }
}
