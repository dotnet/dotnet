// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class AutomaticProgressReporterTests
    {
        [Fact]
        public void Create_ThrowsForNullConnection()
        {
            using (var test = new AutomaticProgressReporterTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => AutomaticProgressReporter.Create(
                        connection: null,
                        request: test.Request,
                        interval: test.Interval,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("connection", exception.ParamName);
            }
        }

        [Fact]
        public void Create_ThrowsForNullRequest()
        {
            using (var test = new AutomaticProgressReporterTest())
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => AutomaticProgressReporter.Create(
                        test.Connection.Object,
                        request: null,
                        interval: test.Interval,
                        cancellationToken: CancellationToken.None));

                Assert.Equal("request", exception.ParamName);
            }
        }

        [Fact]
        public void Create_ThrowsForTimeSpanZero()
        {
            VerifyInvalidInterval(TimeSpan.Zero);
        }

        [Fact]
        public void Create_ThrowsForNegativeTimeSpan()
        {
            VerifyInvalidInterval(TimeSpan.FromSeconds(-1));
        }

        [Fact]
        public void Create_ThrowsForTooLargeTimeSpan()
        {
            var milliseconds = int.MaxValue + 1L;

            VerifyInvalidInterval(TimeSpan.FromMilliseconds(milliseconds));
        }

        [Fact]
        public void Create_ThrowsIfCancelled()
        {
            using (var test = new AutomaticProgressReporterTest())
            {
                Assert.Throws<OperationCanceledException>(
                    () => AutomaticProgressReporter.Create(
                        test.Connection.Object,
                        test.Request,
                        test.Interval,
                        new CancellationToken(canceled: true)));
            }
        }

        [Fact]
        public void Dispose_DisposesDisposables()
        {
            using (var test = new AutomaticProgressReporterTest())
            {
            }
        }

        [Fact]
        public void Dispose_DoesNotDisposeConnection()
        {
            var connection = new Mock<IConnection>(MockBehavior.Strict);
            var request = MessageUtilities.Create(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.GetServiceIndex,
                    payload: new GetServiceIndexRequest(packageSourceRepository: "https://unit.test"));

            using (var reporter = AutomaticProgressReporter.Create(
                connection.Object,
                request,
                TimeSpan.FromHours(1),
                CancellationToken.None))
            {
            }

            connection.Verify();
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var test = new AutomaticProgressReporterTest())
            {
                test.Reporter.Dispose();
                test.Reporter.Dispose();
            }
        }

        [Fact]
        public void Progress_FiredOnInterval()
        {
            using (var test = new AutomaticProgressReporterTest(
                TimeSpan.FromMilliseconds(50),
                expectedSentCount: 3))
            {
                test.SentEvent.Wait();
            }
        }

        private static void VerifyInvalidInterval(TimeSpan interval)
        {
            using (var test = new AutomaticProgressReporterTest())
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => AutomaticProgressReporter.Create(
                        test.Connection.Object,
                        test.Request,
                        interval,
                        CancellationToken.None));

                Assert.Equal("interval", exception.ParamName);
            }
        }

        private sealed class AutomaticProgressReporterTest : IDisposable
        {
            private int _actualSentCount;
            private readonly int _expectedSentCount;
            private bool _isDisposed;

            internal CancellationTokenSource CancellationTokenSource { get; }
            internal Mock<IConnection> Connection { get; }
            internal TimeSpan Interval { get; }
            internal AutomaticProgressReporter Reporter { get; }
            internal Message Request { get; }
            internal ManualResetEventSlim SentEvent { get; }

            internal AutomaticProgressReporterTest(TimeSpan? interval = null, int expectedSentCount = 0)
            {
                _expectedSentCount = expectedSentCount;

                var payload = new HandshakeRequest(
                    ProtocolConstants.CurrentVersion,
                    ProtocolConstants.CurrentVersion);

                CancellationTokenSource = new CancellationTokenSource();
                Interval = interval.HasValue ? interval.Value : ProtocolConstants.MaxTimeout;
                SentEvent = new ManualResetEventSlim(initialState: false);

                Connection = new Mock<IConnection>(MockBehavior.Strict);

                Connection.Setup(x => x.SendAsync(
                        It.IsNotNull<Message>(),
                        It.IsAny<CancellationToken>()))
                    .Callback<Message, CancellationToken>(
                    (message, cancellationToken) =>
                    {
                        ++_actualSentCount;

                        if (_actualSentCount >= expectedSentCount)
                        {
                            SentEvent.Set();
                        }
                    })
                    .Returns(Task.CompletedTask);

                Request = MessageUtilities.Create(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Handshake,
                    payload: payload);
                Reporter = AutomaticProgressReporter.Create(
                    Connection.Object,
                    Request,
                    Interval,
                    CancellationTokenSource.Token);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    try
                    {
                        using (CancellationTokenSource)
                        {
                            CancellationTokenSource.Cancel();
                        }
                    }
                    catch (Exception)
                    {
                    }

                    Reporter.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;

                    var connectionTimes = _expectedSentCount == 0 ? Times.Never() : Times.AtLeast(_expectedSentCount);

                    Connection.Verify(x => x.SendAsync(
                        It.IsNotNull<Message>(),
                        It.IsAny<CancellationToken>()), connectionTimes);
                }
            }
        }
    }
}
