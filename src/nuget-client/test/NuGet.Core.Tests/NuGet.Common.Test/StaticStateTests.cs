// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Common.Test
{
    public class StaticStateTests
    {
        // StaticState exposes a process-global event that product types subscribe to from their static constructors.
        // There is no way to clear those subscriptions, so each test subscribes a private marker handler, asserts
        // only on its own local counter (product/earlier-test handlers may also run but cannot affect it), and
        // unsubscribes in a finally so it does not pollute later tests.

        [Fact]
        public void RaiseBuildEnded_NoSubscriber_DoesNotThrow()
        {
            StaticState.RaiseBuildEnded();
        }

        [Fact]
        public void RaiseBuildEnded_InvokesSubscribedHandler()
        {
            int ran = 0;
            Action handler = () => ran++;
            StaticState.BuildEnded += handler;
            try
            {
                StaticState.RaiseBuildEnded();
            }
            finally
            {
                StaticState.BuildEnded -= handler;
            }

            Assert.Equal(1, ran);
        }

        [Fact]
        public void RaiseBuildEnded_InvokesEverySubscribedHandler()
        {
            int firstRan = 0;
            int secondRan = 0;
            Action first = () => firstRan++;
            Action second = () => secondRan++;
            StaticState.BuildEnded += first;
            StaticState.BuildEnded += second;
            try
            {
                StaticState.RaiseBuildEnded();
            }
            finally
            {
                StaticState.BuildEnded -= first;
                StaticState.BuildEnded -= second;
            }

            Assert.Equal(1, firstRan);
            Assert.Equal(1, secondRan);
        }

        [Fact]
        public void RaiseBuildEnded_HandlerThrows_PropagatesAndDoesNotSwallow()
        {
            Action handler = () => throw new InvalidOperationException("boom");
            StaticState.BuildEnded += handler;
            try
            {
                Assert.Throws<InvalidOperationException>(() => StaticState.RaiseBuildEnded());
            }
            finally
            {
                StaticState.BuildEnded -= handler;
            }
        }
    }
}
