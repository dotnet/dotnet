// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace Test.Utility
{
    public static class ExceptionUtility
    {
        public static void AssertMicrosoftAssumesException(Exception exception)
        {
            Assert.NotNull(exception);
            Assert.Equal("Microsoft.Assumes+InternalErrorException", exception.GetType().FullName);
            Assert.Equal(0x80131500, (uint)exception.HResult);
            Assert.Equal("Microsoft.VisualStudio.Validation", exception.Source);
        }
    }
}
