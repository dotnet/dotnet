// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class X509ChainBuildPolicyFactoryTests
    {
        [Fact]
        public void Create_WhenArgumentIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => X509ChainBuildPolicyFactory.Create(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void Create_WhenArgumentIsValid_IsIdempotent()
        {
            IX509ChainBuildPolicy policy0 = X509ChainBuildPolicyFactory.Create(EnvironmentVariableWrapper.Instance);
            IX509ChainBuildPolicy policy1 = X509ChainBuildPolicyFactory.Create(EnvironmentVariableWrapper.Instance);

            Assert.Same(policy0, policy1);
        }

        [PlatformFact(Platform.Windows)]
        public void CreateWithoutCaching_OnWindowsWhenEnvironmentVariableIsNotDefined_ReturnsRetriablePolicy()
        {
            Mock<IEnvironmentVariableReader> reader = CreateMockEnvironmentVariableReader(variableValue: null);

            IX509ChainBuildPolicy policy = X509ChainBuildPolicyFactory.CreateWithoutCaching(reader.Object);

            Assert.IsType<RetriableX509ChainBuildPolicy>(policy);

            reader.VerifyAll();
        }

        [PlatformFact(Platform.Windows)]
        public void CreateWithoutCaching_OnWindowsWhenEnvironmentVariableIsDisabled_ReturnsDefaultPolicy()
        {
            Mock<IEnvironmentVariableReader> reader = CreateMockEnvironmentVariableReader(variableValue: X509ChainBuildPolicyFactory.DisabledValue);

            IX509ChainBuildPolicy policy = X509ChainBuildPolicyFactory.CreateWithoutCaching(reader.Object);

            Assert.IsType<DefaultX509ChainBuildPolicy>(policy);

            reader.VerifyAll();
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(",")]
        [InlineData("-1,2")]
        [InlineData("0,0")]
        [InlineData("0,1")]
        [InlineData("1,-2")]
        [InlineData("1,2,3")]
        public void CreateWithoutCaching_OnWindowsWhenEnvironmentVariableValueIsInvalid_ReturnsDefaultPolicy(string value)
        {
            Mock<IEnvironmentVariableReader> reader = CreateMockEnvironmentVariableReader(value);

            IX509ChainBuildPolicy policy = X509ChainBuildPolicyFactory.CreateWithoutCaching(reader.Object);

            Assert.IsType<DefaultX509ChainBuildPolicy>(policy);

            reader.VerifyAll();
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("1,0")]
        [InlineData("3,7")]
        [InlineData(" 5 , 9 ")]
        public void CreateWithoutCaching_OnWindowsWhenEnvironmentVariableValueIsValid_ReturnsRetriablePolicy(string value)
        {
            Mock<IEnvironmentVariableReader> reader = CreateMockEnvironmentVariableReader(value);

            IX509ChainBuildPolicy policy = X509ChainBuildPolicyFactory.CreateWithoutCaching(reader.Object);

            Assert.IsType<RetriableX509ChainBuildPolicy>(policy);

            var retryPolicy = (RetriableX509ChainBuildPolicy)policy;

            string[] parts = value.Split(X509ChainBuildPolicyFactory.ValueDelimiter);
            int expectedRetryCount = int.Parse(parts[0]);
            TimeSpan expectedSleepInterval = TimeSpan.FromMilliseconds(int.Parse(parts[1]));

            Assert.Equal(expectedRetryCount, retryPolicy.RetryCount);
            Assert.Equal(expectedSleepInterval, retryPolicy.SleepInterval);

            reader.VerifyAll();
        }

        [PlatformFact(Platform.Linux, Platform.Darwin)]
        public void CreateWithoutCaching_OnNonWindowsAlways_ReturnsDefaultPolicy()
        {
            var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

            IX509ChainBuildPolicy policy = X509ChainBuildPolicyFactory.CreateWithoutCaching(reader.Object);

            Assert.IsType<DefaultX509ChainBuildPolicy>(policy);

            reader.VerifyAll();
        }

        private static Mock<IEnvironmentVariableReader> CreateMockEnvironmentVariableReader(
            string variableValue)
        {
            var reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

            reader.Setup(r => r.GetEnvironmentVariable(X509ChainBuildPolicyFactory.EnvironmentVariableName))
                .Returns(variableValue);

            return reader;
        }
    }
}
