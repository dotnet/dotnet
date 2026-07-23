// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Common.Test
{
    public class StaticStateTests
    {
        // StaticState exposes process-global events that product types subscribe to from their static constructors.
        // There is no way to clear those subscriptions, so each test subscribes a private marker handler, asserts
        // only on its own local counter (product/earlier-test handlers may also run but cannot affect it), and
        // unsubscribes in a finally so it does not pollute later tests.

        [Fact]
        public void RaiseStartMSBuildRestoreTasks_NoSubscriber_DoesNotThrow()
        {
            StaticState.RaiseStartMSBuildRestoreTasks();
            StaticState.RaiseEndMSBuildRestoreTasks();
        }

        [Fact]
        public void RaiseStartMSBuildRestoreTasks_InvokesSubscribedHandler()
        {
            int ran = 0;
            Action handler = () => ran++;
            StaticState.StartMSBuildRestoreTasks += handler;
            try
            {
                StaticState.RaiseStartMSBuildRestoreTasks();
            }
            finally
            {
                StaticState.StartMSBuildRestoreTasks -= handler;
            }

            Assert.Equal(1, ran);
        }

        [Fact]
        public void RaiseStartMSBuildRestoreTasks_InvokesEverySubscribedHandler()
        {
            int firstRan = 0;
            int secondRan = 0;
            Action first = () => firstRan++;
            Action second = () => secondRan++;
            StaticState.StartMSBuildRestoreTasks += first;
            StaticState.StartMSBuildRestoreTasks += second;
            try
            {
                StaticState.RaiseStartMSBuildRestoreTasks();
            }
            finally
            {
                StaticState.StartMSBuildRestoreTasks -= first;
                StaticState.StartMSBuildRestoreTasks -= second;
            }

            Assert.Equal(1, firstRan);
            Assert.Equal(1, secondRan);
        }

        [Fact]
        public void RaiseStartMSBuildRestoreTasks_DoesNotInvokeEndSubscriber()
        {
            int ran = 0;
            Action handler = () => ran++;
            StaticState.EndMSBuildRestoreTasks += handler;
            try
            {
                // Raising the start event must not invoke a handler subscribed to the end event.
                StaticState.RaiseStartMSBuildRestoreTasks();
                Assert.Equal(0, ran);

                // Raising its own event invokes it.
                StaticState.RaiseEndMSBuildRestoreTasks();
                Assert.Equal(1, ran);
            }
            finally
            {
                StaticState.EndMSBuildRestoreTasks -= handler;
            }
        }

        [Fact]
        public void RaiseStartMSBuildRestoreTasks_HandlerThrows_PropagatesAndDoesNotSwallow()
        {
            Action handler = () => throw new InvalidOperationException("boom");
            StaticState.StartMSBuildRestoreTasks += handler;
            try
            {
                Assert.Throws<InvalidOperationException>(() => StaticState.RaiseStartMSBuildRestoreTasks());
            }
            finally
            {
                StaticState.StartMSBuildRestoreTasks -= handler;
            }
        }
    }
}
