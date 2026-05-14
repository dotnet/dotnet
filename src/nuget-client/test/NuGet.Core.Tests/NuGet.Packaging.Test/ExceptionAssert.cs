// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Packaging.Test
{
    public static class ExceptionAssert
    {
        public static void Throws<TException>(Action act) where TException : Exception
        {
            Throws<TException>(act, ex => { });
        }

        public static void Throws<TException>(Action act, Action<TException> condition) where TException : Exception
        {
            Exception ex = Record.Exception(act);
            Assert.NotNull(ex);
            TException tex = Assert.IsAssignableFrom<TException>(ex);
            condition(tex);
        }

        public static void Throws<TException>(Action action, string expectedMessage) where TException : Exception
        {
            Throws<TException>(action, ex => Assert.Equal(expectedMessage, ex.Message));
        }

        public static void ThrowsArgumentException(Action act, string message)
        {
            ThrowsArgumentException<ArgumentException>(act, message);
        }

        public static void ThrowsArgumentException<TArgException>(Action act, string message) where TArgException : ArgumentException
        {
            Throws<TArgException>(act, ex => Assert.Equal(message, ex.Message));
        }
    }
}
