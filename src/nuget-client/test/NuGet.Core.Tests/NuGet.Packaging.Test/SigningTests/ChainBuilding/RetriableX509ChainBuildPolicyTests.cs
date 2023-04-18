// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class RetriableX509ChainBuildPolicyTests
    {
        private readonly CertificatesFixture _fixture;

        public RetriableX509ChainBuildPolicyTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [PlatformFact(Platform.Windows)]
        public void Constructor_WhenInnerPolicyIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new RetriableX509ChainBuildPolicy(innerPolicy: null, retryCount: 0, TimeSpan.MaxValue));

            Assert.Equal("innerPolicy", exception.ParamName);
        }

        [PlatformFact(Platform.Windows)]
        public void Constructor_WhenRetryCountIsInvalid_Throws()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 0, TimeSpan.MaxValue));

            Assert.Equal("retryCount", exception.ParamName);

            innerPolicy.VerifyAll();
        }

        [PlatformFact(Platform.Windows)]
        public void Constructor_WhenSleepIntervalIsInvalid_Throws()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 1, TimeSpan.FromSeconds(-1)));

            Assert.Equal("sleepInterval", exception.ParamName);

            innerPolicy.VerifyAll();
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(1, 0)]
        [InlineData(3, 700)]
        public void Constructor_WhenArgumentsAreValid_InitializesProperties(
            int expectedRetryCount,
            int expectedMilliseconds)
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            TimeSpan expectedSleepInterval = TimeSpan.FromSeconds(expectedMilliseconds);

            var policy = new RetriableX509ChainBuildPolicy(innerPolicy.Object, expectedRetryCount, expectedSleepInterval);

            Assert.Same(innerPolicy.Object, policy.InnerPolicy);
            Assert.Equal(expectedRetryCount, policy.RetryCount);
            Assert.Equal(expectedSleepInterval, policy.SleepInterval);

            innerPolicy.VerifyAll();
        }

        [PlatformFact(Platform.Windows)]
        public void Build_WhenBuildIsNull_Throws()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            var policy = new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 1, TimeSpan.Zero);

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => policy.Build(chain: null, certificate));

                Assert.Equal("chain", exception.ParamName);

                innerPolicy.VerifyAll();
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Create_WhenCertificateIsNull_Throws()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            var policy = new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 1, TimeSpan.Zero);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => policy.Build(Mock.Of<IX509Chain>(), certificate: null));

            Assert.Equal("certificate", exception.ParamName);

            innerPolicy.VerifyAll();
        }

        [PlatformFact(Platform.Windows)]
        public void Build_WhenArgumentsAreValidAndBuildSucceeds_DoesNotRetry()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            var policy = new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 3, TimeSpan.FromSeconds(10));
            Mock<IX509Chain> chain = new(MockBehavior.Strict);

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                innerPolicy.Setup(
                    x => x.Build(
                        It.Is<IX509Chain>(chainArg => ReferenceEquals(chainArg, chain.Object)),
                        It.Is<X509Certificate2>(certArg => ReferenceEquals(certArg, certificate))))
                    .Returns(true);
                bool actualResult = policy.Build(chain.Object, certificate);

                Assert.True(actualResult);

                chain.VerifyAll();
                innerPolicy.VerifyAll();
            }
        }

        [PlatformFact(Platform.Windows)]
        public void Build_WhenArgumentsAreValidAndBuildFailureIsRetriable_Retries()
        {
            var innerPolicy = new Mock<IX509ChainBuildPolicy>(MockBehavior.Strict);
            var policy = new RetriableX509ChainBuildPolicy(innerPolicy.Object, retryCount: 3, TimeSpan.FromMilliseconds(50));
            Mock<IX509Chain> chain = new(MockBehavior.Strict);

            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                innerPolicy.Setup(
                   x => x.Build(
                       It.Is<IX509Chain>(chainArg => ReferenceEquals(chainArg, chain.Object)),
                       It.Is<X509Certificate2>(certArg => ReferenceEquals(certArg, certificate))))
                   .Returns(false);
                innerPolicy.Setup(
                   x => x.Build(
                       It.Is<IX509Chain>(chainArg => ReferenceEquals(chainArg, chain.Object)),
                       It.Is<X509Certificate2>(certArg => ReferenceEquals(certArg, certificate))))
                   .Returns(false);
                innerPolicy.Setup(
                    x => x.Build(
                        It.Is<IX509Chain>(chainArg => ReferenceEquals(chainArg, chain.Object)),
                        It.Is<X509Certificate2>(certArg => ReferenceEquals(certArg, certificate))))
                    .Returns(true);

                bool actualResult = policy.Build(chain.Object, certificate);

                Assert.True(actualResult);

                chain.VerifyAll();
                innerPolicy.VerifyAll();
            }
        }
    }
}
