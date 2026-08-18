// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Resx = NuGet.PackageManagement.UI.Resources;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// A document window pane that displays an exception's message, stack trace, and inner exceptions as
    /// read-only, selectable text so the user can read and copy the details of a failure that occurred
    /// while opening the Package Manager UI.
    /// </summary>
    /// <remarks>
    /// This pane and its content are intentionally built entirely in code with no XAML. Loading XAML
    /// relies on compiled BAML resources, which can themselves fail to load in the experimental instance.
    /// As the last-resort error window for Package Manager open failures, it must not depend on that same
    /// resource-loading machinery, so it keeps working even when BAML loading does not.
    /// </remarks>
    public sealed class ExceptionWindowPane : WindowPane
    {
        /// <summary>
        /// The maximum depth of nested inner exceptions that will be formatted. This is a hard safety
        /// limit that guarantees termination even if an exception graph contains a reference cycle that
        /// somehow escapes the cycle detection below. A StackOverflowException on this last-resort error
        /// path is uncatchable and would crash the entire IDE, so the bound is deliberately conservative.
        /// </summary>
        private const int MaxDepth = 32;

        private readonly TextBox _content;

        public ExceptionWindowPane(Exception exception)
            : base(null)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _content = new TextBox
            {
                IsReadOnly = true,
                IsReadOnlyCaretVisible = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.NoWrap,
                Margin = new Thickness(12),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Text = FormatException(exception),
            };
            _content.SetResourceReference(Control.BackgroundProperty, VsBrushes.WindowKey);
            _content.SetResourceReference(Control.ForegroundProperty, VsBrushes.WindowTextKey);
            _content.SetResourceReference(Control.FontFamilyProperty, VsFonts.EnvironmentFontFamilyKey);
            _content.SetResourceReference(Control.FontSizeProperty, VsFonts.EnvironmentFontSizeKey);

            AutomationProperties.SetName(_content, Resx.Text_ErrorOccurred);
        }

        public override object Content => _content;

        /// <summary>
        /// Formats an exception, including its type, message, stack trace, and all inner exceptions.
        /// </summary>
        internal static string FormatException(Exception exception)
        {
            var builder = new StringBuilder();
            var visited = new HashSet<Exception>(ReferenceComparer.Instance);
            AppendException(builder, exception, depth: 0, visited);
            return builder.ToString();
        }

        private static void AppendException(StringBuilder builder, Exception exception, int depth, HashSet<Exception> visited)
        {
            if (depth > 0)
            {
                builder.AppendLine();
                builder.AppendLine("--- Inner exception ---");
            }

            builder.Append(exception.GetType().FullName);
            builder.Append(": ");
            builder.AppendLine(exception.Message);

            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                builder.AppendLine(exception.StackTrace);
            }

            // Stop if we've already formatted this exception instance (cycle) or reached the depth cap.
            // Either guard prevents unbounded recursion, which on this path could crash Visual Studio.
            if (!visited.Add(exception) || depth >= MaxDepth)
            {
                return;
            }

            if (exception is AggregateException aggregateException)
            {
                IReadOnlyList<Exception> innerExceptions = aggregateException.InnerExceptions;
                for (int i = 0; i < innerExceptions.Count; i++)
                {
                    AppendException(builder, innerExceptions[i], depth + 1, visited);
                }
            }
            else if (exception.InnerException is not null)
            {
                AppendException(builder, exception.InnerException, depth + 1, visited);
            }
        }

        /// <summary>
        /// Compares exceptions by reference identity so that cycle detection is based on the actual
        /// exception instances rather than any overridden <see cref="object.Equals(object)"/> behavior.
        /// </summary>
        private sealed class ReferenceComparer : IEqualityComparer<Exception>
        {
            internal static readonly ReferenceComparer Instance = new ReferenceComparer();

            public bool Equals(Exception? x, Exception? y) => ReferenceEquals(x, y);

            public int GetHashCode(Exception obj) => RuntimeHelpers.GetHashCode(obj);
        }
    }
}
