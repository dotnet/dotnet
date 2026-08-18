// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens.Experimental;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ValidationErrorTests
    {
        [Fact]
        public void ExceptionCreatedFromValidationError_ContainsTheRightStackTrace()
        {
            var validationError = new ValidationErrorReturningClass().FirstMethod();
            Assert.NotNull(validationError);
            Assert.NotNull(validationError.StackFrames);
            Assert.Equal(3, validationError.StackFrames.Count);
            Assert.NotNull(validationError.GetException());
            Exception exception = validationError.GetException();
            Assert.NotNull(validationError.GetException().StackTrace);
            Assert.Equal("ThirdMethod", validationError.StackFrames[0].GetMethod().Name);
            Assert.Equal("SecondMethod", validationError.StackFrames[1].GetMethod().Name);
            Assert.Equal("FirstMethod", validationError.StackFrames[2].GetMethod().Name);
        }

        class ValidationErrorReturningClass
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public ValidationError FirstMethod()
            {
                return SecondMethod().AddCurrentStackFrame();
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public ValidationError SecondMethod()
            {
                return ThirdMethod().AddCurrentStackFrame();
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public ValidationError ThirdMethod()
            {
                return new ValidationError(
                    new MessageDetail("This is a test error"),
                    ValidationFailureType.SecurityTokenNotExpectedType,
                    ValidationError.GetCurrentStackFrame());
            }
        }
    }
}
