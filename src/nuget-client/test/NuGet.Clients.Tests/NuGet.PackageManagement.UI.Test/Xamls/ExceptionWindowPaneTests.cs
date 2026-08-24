// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class ExceptionWindowPaneTests
    {
        [Fact]
        public void FormatException_SimpleException_IncludesTypeAndMessage()
        {
            var exception = new InvalidOperationException("something went wrong");

            string result = ExceptionWindowPane.FormatException(exception);

            Assert.Contains(typeof(InvalidOperationException).FullName, result);
            Assert.Contains("something went wrong", result);
        }

        [Fact]
        public void FormatException_ThrownException_IncludesStackTrace()
        {
            Exception caught = ThrowAndCatch(() => throw new InvalidOperationException("boom"));

            string result = ExceptionWindowPane.FormatException(caught);

            Assert.Contains(nameof(ThrowAndCatch), result);
        }

        [Fact]
        public void FormatException_WithInnerException_IncludesInnerException()
        {
            var inner = new ArgumentNullException("param", "inner message");
            var outer = new InvalidOperationException("outer message", inner);

            string result = ExceptionWindowPane.FormatException(outer);

            Assert.Contains("outer message", result);
            Assert.Contains("inner message", result);
            Assert.Contains(typeof(ArgumentNullException).FullName, result);
        }

        [Fact]
        public void FormatException_AggregateException_IncludesAllInnerExceptions()
        {
            var first = new InvalidOperationException("first failure");
            var second = new FormatException("second failure");
            var aggregate = new AggregateException(first, second);

            string result = ExceptionWindowPane.FormatException(aggregate);

            Assert.Contains("first failure", result);
            Assert.Contains("second failure", result);
        }

        [Fact]
        public void FormatException_DeeplyNestedInnerExceptions_TerminatesAndTruncates()
        {
            // Build a chain far deeper than the formatter's depth cap to prove it terminates instead
            // of recursing unboundedly (which on the real error path could crash Visual Studio).
            Exception exception = new InvalidOperationException("innermost failure");
            for (int i = 0; i < 100; i++)
            {
                exception = new InvalidOperationException("level " + i, exception);
            }

            string result = ExceptionWindowPane.FormatException(exception);

            // The outermost levels are formatted, but the chain is truncated before the innermost one.
            Assert.Contains("level 99", result);
            Assert.DoesNotContain("innermost failure", result);
        }

        private static Exception ThrowAndCatch(Action action)
        {
            try
            {
                action();
                throw new InvalidOperationException("The action was expected to throw.");
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
