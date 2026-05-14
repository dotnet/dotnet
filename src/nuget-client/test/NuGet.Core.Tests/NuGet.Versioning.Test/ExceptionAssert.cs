// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Versioning.Test
{
    public static class ExceptionAssert
    {
        public static void Throws<TException>(Action act) where TException : Exception
        {
            Throws<TException>(act, ex => { });
        }

        public static void Throws<TException>(Action act, Action<TException> condition) where TException : Exception
        {
            var ex = Record.Exception(act);
            Assert.NotNull(ex);
            var tex = Assert.IsAssignableFrom<TException>(ex);
            condition(tex);
        }

        public static void Throws<TException>(Action action, string expectedMessage) where TException : Exception
        {
            Throws<TException>(action, ex => Assert.Equal(expectedMessage, ex.Message));
        }

        public static void ThrowsArgNull(Action act, string paramName)
        {
            Throws<ArgumentNullException>(act, ex => Assert.Equal(paramName, ex.ParamName));
        }

        public static void ThrowsArgNullOrEmpty(Action act, string paramName)
        {
            ThrowsArgumentException<ArgumentException>(act, paramName, "Value cannot be null or an empty string.");
        }

        public static void ThrowsArgOutOfRange(Action act, string paramName, object minimum, object maximum, bool equalAllowed)
        {
            ThrowsArgumentException<ArgumentOutOfRangeException>(act, paramName, BuildOutOfRangeMessage(paramName, minimum, maximum, equalAllowed));
        }

        private static string BuildOutOfRangeMessage(string paramName, object minimum, object maximum, bool equalAllowed)
        {
            if (minimum == null)
            {
                return string.Format(equalAllowed ? "Argument_Must_Be_LessThanOrEqualTo" : "Argument_Must_Be_LessThan", maximum);
            }
            else if (maximum == null)
            {
                return string.Format(equalAllowed ? "Argument_Must_Be_GreaterThanOrEqualTo" : "Argument_Must_Be_GreaterThan", minimum);
            }
            else
            {
                return string.Format("Argument_Must_Be_Between", minimum, maximum);
            }
        }

        public static void ThrowsArgumentException(Action act, string message)
        {
            ThrowsArgumentException<ArgumentException>(act, message);
        }

        public static void ThrowsArgumentException<TArgException>(Action act, string message) where TArgException : ArgumentException
        {
            Throws<TArgException>(act, ex => Assert.Equal(message, ex.Message));
        }

        public static void ThrowsArgumentException(Action act, string paramName, string message)
        {
            ThrowsArgumentException<ArgumentException>(act, paramName, message);
        }

        public static void ThrowsArgumentException<TArgException>(Action act, string paramName, string message) where TArgException : ArgumentException
        {
            Throws<TArgException>(act, ex =>
            {
                Assert.Equal(paramName, ex.ParamName);
                Assert.Contains(message, ex.Message);
                //Remove the expected message from the exception message, the rest part should have param info.
                //Background of this change: System.ArgumentException(string message, string paramName) used to generate two lines of message before, but changed to generate one line
                //in PR: https://github.com/dotnet/coreclr/pull/25185/files#diff-0365d5690376ef849bf908dfc225b8e8
                var paramPart = ex.Message.Substring(ex.Message.IndexOf(message) + message.Length);
                Assert.Contains("Parameter", paramPart);
                Assert.Contains(paramName, paramPart);
            });
        }
    }
}
