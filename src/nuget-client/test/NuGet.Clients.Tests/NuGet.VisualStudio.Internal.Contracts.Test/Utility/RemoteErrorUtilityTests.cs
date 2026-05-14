// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class RemoteErrorUtilityTests
    {
        [Fact]
        public void ToRemoteError_WhenExceptionIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                () => RemoteErrorUtility.ToRemoteError(exception: null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Equal("exception", exception.ParamName);
        }

        [Fact]
        public void ToRemoteError_WhenExceptionIsOtherType_SetsProjectContextLogMessage()
        {
            var exception = new DivideByZeroException();
            RemoteError remoteError = RemoteErrorUtility.ToRemoteError(exception);

            Assert.Equal(exception.ToString(), remoteError.ProjectContextLogMessage);
        }
    }
}
