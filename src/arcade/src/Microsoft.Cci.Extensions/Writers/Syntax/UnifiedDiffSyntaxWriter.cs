// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.Cci.Writers.Syntax
{
    // This writer produces an output similar to this:
    //
    // ---assembly-location\before\System.Collections.Immutable.dll
    // +++assembly-location\after-extract\System.Collections.Immutable.dll
    //  namespace System.Collections.Immutable {
    //    public struct ImmutableArray<T>
    //      public sealed class Builder
    // +      public int Capacity { get; }
    //        ^ <Author Name>: Add a setter. Should have the same behavior as List<T>.
    //        | <Author Name>:
    //        | <Author Name>: Second line
    //        public int Count { get; set;}
    //      }
    //    }
    //  }
    //
    // For more details, take a look at the Wikipedia article on the unified diff format:
    // http://en.wikipedia.org/wiki/Diff_utility#Unified_format
    public class UnifiedDiffSyntaxWriter : IndentionSyntaxWriter, IStyleSyntaxWriter, IReviewCommentWriter
    {
        private bool _needsMarker;
        private int _numberOfLines;
        private SyntaxStyle? _currentStyle;

        public UnifiedDiffSyntaxWriter(TextWriter writer)
            : base(writer)
        {
            _needsMarker = true;
        }

        public void Dispose()
        {
        }

        private void WriteLineMarker()
        {
            if (_needsMarker)
                _needsMarker = false;
            else
                return;

            switch (_currentStyle)
            {
                case SyntaxStyle.Added:
                    WriteLineMarker('+');
                    break;
                case SyntaxStyle.Removed:
                    WriteLineMarker('-');
                    break;
                default:
                    WriteLineMarker(' ');
                    break;
            }
        }

        private void WriteLineMarker(char marker)
        {
            // The first two lines in a diff format use three pluses and three minuses, e.g.
            //
            // ---assembly-location\before\System.Collections.Immutable.dll
            // +++assembly-location\after-extract\System.Collections.Immutable.dll
            //  namespace System.Collections.Immutable {
            // ...
            //
            // Subsequent line markers use a single plus and single minus.

            var isHeader = _numberOfLines++ < 2;
            var count = isHeader ? 3 : 1;
            var markerStr = new string(marker, count);

            var remainingSpaces = (IndentLevel * SpacesInIndent) - 1;

            using (DisableIndenting())
            {
                WriteCore(markerStr);

                if (remainingSpaces > 0)
                    WriteCore(new string(' ', remainingSpaces));
            }
        }

        private IDisposable DisableIndenting()
        {
            var indent = IndentLevel;
            IndentLevel = 0;
            return new DisposeAction(() => IndentLevel = indent);
        }

        public IDisposable StartStyle(SyntaxStyle style, object context)
        {
            _currentStyle = style;
            return new DisposeAction(() => { });
        }

        public void Write(string str)
        {
            WriteLineMarker();
            WriteCore(str);
        }

        public void WriteSymbol(string symbol)
        {
            WriteLineMarker();
            WriteCore(symbol);
        }

        public void WriteIdentifier(string id)
        {
            WriteLineMarker();
            WriteCore(id);
        }

        public void WriteKeyword(string keyword)
        {
            WriteLineMarker();
            WriteCore(keyword);
        }

        public void WriteTypeName(string typeName)
        {
            WriteLineMarker();
            WriteCore(typeName);
        }

        public void WriteReviewComment(string author, string text)
        {
            // We want to write the comment as individual lines. This method is called after the
            // API being commented is already written.
            //
            // To make this a bit more visually clear, e'll emit the hat character (^) to 'point'
            // to the API the comment is associated with. Subsequent lines will use the pipe (|)
            // to make it clear that these are comments.
            //
            // This will roughly look like this:
            //
            // ---assembly-location\before\System.Collections.Immutable.dll
            // +++assembly-location\after-extract\System.Collections.Immutable.dll
            //  namespace System.Collections.Immutable {
            //    public struct ImmutableArray<T>
            //      public sealed class Builder
            // +      public int Capacity { get; }
            //        ^ <Author Name>: Add a setter.
            //        | <Author Name>:
            //        | <Author Name>: Should have the same behavior as List<T>.
            //        ^ Microsoft Bob: This is very
            //        | Microsoft Bob: relevant.
            //        ^ Capt. Jack Sparrow: Why is the rum gone?
            //        ^ Microsoft Bob: Because the hobbits are taken to Isengard.
            //        public int Count { get; set;}
            //      }
            //    }
            //  }
            //
            // Using the hat character also allows searching for the start of each comment.

            using (var reader = new StringReader(text))
            {
                var first = true;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string marker;

                    if (first)
                    {
                        first = false;
                        marker = "^";
                    }
                    else
                    {
                        WriteLine();
                        marker = "|";
                    }

                    WriteLineMarker();
                    WriteCore("{0} {1}: {2}", marker, author, line);
                }
            }
        }

        public override void WriteLine()
        {
            _needsMarker = true;
            _currentStyle = null;
            base.WriteLine();
        }
    }
}
