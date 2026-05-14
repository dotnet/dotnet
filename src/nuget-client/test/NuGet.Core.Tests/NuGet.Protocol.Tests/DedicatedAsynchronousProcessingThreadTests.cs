// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DedicatedAsynchronousProcessingThreadTests
    {
        TimeSpan _defaultTimeSpan = TimeSpan.FromMilliseconds(50);

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan))
            {
                thread.Dispose();
                thread.Dispose();
            }
        }

        [Fact]
        public void Start_ThrowsForDisposesObject()
        {
            var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan);
            thread.Dispose();
            Assert.Throws<ObjectDisposedException>(() => thread.Start());
        }

        [Fact]
        public void Push_ThrowsForDisposesObject()
        {
            var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan);
            thread.Start();
            thread.Dispose();

            Assert.Throws<ObjectDisposedException>(() => thread.Enqueue(() => Task.CompletedTask));
        }

        [Fact]
        public void Start_ThrowsForAlreadyStartedObject()
        {
            using (var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan))
            {
                thread.Start();
                var exception = Assert.Throws<InvalidOperationException>(() => thread.Start());
                exception.Message.Should().Be("The processing thread is already started.");
            }
        }

        [Fact]
        public void Enqueue_ThrowsForNotStartedObject()
        {
            using (var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan))
            {
                var exception = Assert.Throws<InvalidOperationException>(() => thread.Enqueue(() => Task.CompletedTask));
                exception.Message.Should().Be("The processing thread is not started yet.");
            }
        }

        [Fact]
        public void Enqueue_ExecutesTaskAsynchronously()
        {
            using (var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan))
            using (var handledEvent = new ManualResetEventSlim(initialState: false))
            {
                thread.Start();
                var executed = false;
                Func<Task> task = () => { executed = true; handledEvent.Set(); return Task.CompletedTask; };
                thread.Enqueue(task);
                handledEvent.Wait();
                Assert.True(executed);
            }
        }

        [Fact]
        public void Enqueue_ExecutesTaskAsynchronouslyInSameOrderAsPush()
        {
            using (var countdownEvent = new CountdownEvent(3))
            using (var thread = new DedicatedAsynchronousProcessingThread(_defaultTimeSpan))
            {
                thread.Start();
                var queue = new Queue<int>();

                Func<Task> task1 = () => { queue.Enqueue(1); countdownEvent.Signal(); return Task.CompletedTask; };
                Func<Task> task2 = () => { queue.Enqueue(2); countdownEvent.Signal(); return Task.CompletedTask; };
                Func<Task> task3 = () => { queue.Enqueue(3); countdownEvent.Signal(); return Task.CompletedTask; };

                thread.Enqueue(task1);
                thread.Enqueue(task2);
                thread.Enqueue(task3);

                countdownEvent.Wait();
                Assert.Equal(1, queue.Dequeue());
                Assert.Equal(2, queue.Dequeue());
                Assert.Equal(3, queue.Dequeue());
                Assert.Empty(queue);
            }
        }
    }
}
