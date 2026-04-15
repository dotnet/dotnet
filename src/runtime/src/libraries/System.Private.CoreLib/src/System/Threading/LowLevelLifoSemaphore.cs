// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Internal;

namespace System.Threading
{
    /// <summary>
    /// A LIFO semaphore.
    /// Waits on this semaphore are uninterruptible.
    /// </summary>
    internal sealed partial class LowLevelLifoSemaphore
    {
        // Spinning in the threadpool semaphore is not always useful and benefits vary greatly by scenario.
        //
        // Example1: An app periodically with rough time span T runs a task and waits for task's completion.
        //           The app would benefit if a threadpool worker spins for longer than T as worker would not need to be woken up.
        //
        // Example2: The new workitems may be produced by non-pool threads and could only arrive if pool threads start blocking.
        //           For this scenario, once pool is out of work, we benefit from promptly releasing cores.
        //
        // Intuitively, when a threadpool has a lot of active threads, they can absorb an occasional extra task, thus benefits from
        // spinning could be less, while danger of starving non-threadpool threads is higher.
        //
        // Based on the above we use the following heuristic (certainly open to improvements):
        // * We will limit spinning to roughly 256 spinwaits, each taking ~35-40ns. That should be under 10 usec total.
        //    For reference the wakeup latency of a futex/event with threads queued up is in 4-20 usec range. (year 2026)
        // * We will dial spin count according to the number of available cores. (i.e. proc_num - active_workers).
        //                                               |    _ |
        // * We will use a "hard sigmoid" function like: |   /  | that will map "available cores" to spin count.
        //                                               | _/   |
        //    - when threadpool threads use more than 3/4 cores, we do not spin
        //    - when threadpool occupies 1/4 cores or less we spin to the max,
        //    - in between we have a linear gain.
        //    all should be smoothed somewhat by the randomness of individual spin iterations.

        private const int DefaultSemaphoreSpinCountLimit = 256;

        private CacheLineSeparatedCounts _separated;

        private readonly int _maximumSignalCount;
        private readonly int _maxSpinCount;
        private readonly Action _onWait;
        private readonly int _procCount;

        // When we need to block threads we use a linked list of thread blockers.
        // When we need to wake a worker, we pop the topmost blocker and release it.
        private sealed class LifoWaitNode : LowLevelThreadBlocker
        {
            internal LifoWaitNode? _next;
        }

        private readonly LowLevelLock _stackLock = new LowLevelLock();
        private LifoWaitNode? _blockerStack;

        // Sometimes due to races we may see nonzero waiter count, but no blockers to wake.
        // That happens if a thread that added itself to waiter count, has not yet blocked itself.
        // In such case we increment _pendingSignals and the waiter will simply
        // decrement the counter and return without blocking.
        private int _pendingSignals;

        [ThreadStatic]
        private static LifoWaitNode? t_blocker;

        public LowLevelLifoSemaphore(int maximumSignalCount, Action onWait)
        {
            Debug.Assert(maximumSignalCount > 0);
            Debug.Assert(maximumSignalCount <= short.MaxValue);

            _separated = default;
            _maximumSignalCount = maximumSignalCount;
            _onWait = onWait;
            _procCount = Environment.ProcessorCount;

            _maxSpinCount = AppContextConfigHelper.GetInt32ComPlusOrDotNetConfig(
                "System.Threading.ThreadPool.UnfairSemaphoreSpinLimit",
                "ThreadPool_UnfairSemaphoreSpinLimit",
                DefaultSemaphoreSpinCountLimit,
                false);

            // Do not accept unreasonably huge _maxSpinCount value to prevent overflows.
            // Also, 1+ minute spins do not make sense.
            if (_maxSpinCount > 1000000)
                _maxSpinCount = DefaultSemaphoreSpinCountLimit;
        }

        public bool Wait(int timeoutMs, short tpThreadCount)
        {
            Debug.Assert(timeoutMs >= -1);

            // Try one-shot acquire first
            Counts counts = _separated._counts;
            if (counts.SignalCount != 0)
            {
                Counts newCounts = counts;
                newCounts.DecrementSignalCount();
                Counts countsBeforeUpdate = _separated._counts.InterlockedCompareExchange(newCounts, counts);
                if (countsBeforeUpdate == counts)
                {
                    // we've consumed a signal
                    return true;
                }
            }

            RuntimeFeature.ThrowIfMultithreadingIsNotSupported();

            return WaitSlow(timeoutMs, tpThreadCount);
        }

        private bool WaitSlow(int timeoutMs, short tpThreadCount)
        {
            // Now spin briefly with exponential backoff.
            // We estimate availability of CPU resources and limit spin count accordingly.
            // See comments on DefaultSemaphoreSpinCountLimit for more details.
            int active = tpThreadCount - _separated._counts.WaiterCount;
            int available = _procCount - active;
            int spinStep = _maxSpinCount * 2 / _procCount;
            // With activeThreadCount arbitrarily large and _procCount arbitrarily small
            // we can, in theory, overflow int, so just use long here.
            long spinsRemainingLong = (available - _procCount / 4) * (long)spinStep;

            // clamp to [0, _maxSpinCount] range.
            int spinsRemaining = (int)Math.Clamp(spinsRemainingLong, 0, _maxSpinCount);

            uint iteration = 0;
            while (spinsRemaining > 0)
            {
                spinsRemaining -= Backoff.Exponential(iteration++);

                Counts counts = _separated._counts;
                if (counts.SignalCount != 0)
                {
                    Counts newCounts = counts;
                    newCounts.DecrementSignalCount();
                    Counts countsBeforeUpdate = _separated._counts.InterlockedCompareExchange(newCounts, counts);
                    if (countsBeforeUpdate == counts)
                    {
                        // we've consumed a signal
                        return true;
                    }
                }
            }

            // Now we will try registering as a waiter and wait.
            // If signaled before that, we have to acquire as this can be the last thread that could take that signal.
            // The difference with spinning above is that we are not waiting for a signal. We should typically
            // immediately succeed unless a lot of threads are trying to update the counts.
            uint collisionCount = 0;
            while (true)
            {
                Counts counts = _separated._counts;
                Counts newCounts = counts;
                if (counts.SignalCount != 0)
                {
                    newCounts.DecrementSignalCount();
                }
                else
                {
                    newCounts.IncrementWaiterCount();
                }

                Counts countsBeforeUpdate = _separated._counts.InterlockedCompareExchange(newCounts, counts);
                if (countsBeforeUpdate == counts)
                {
                    return counts.SignalCount != 0 || WaitAsWaiter(timeoutMs, allowFastWake: false);
                }

                Backoff.Exponential(collisionCount++);
            }
        }

        public bool WaitNoSpin(int timeoutMs)
        {
            Counts counts = _separated._counts.InterlockedIncrementWaiterCount();

            // If there are pending signals, we may end in a condition that requires
            // waking a waiter.
            // Perhaps the current thread will be such waiter, but we should still
            // go through wait/wake routine (vs. just claiming the signal) as the
            // caller wants to park the thread.
            MaybeWakeWaiters(counts);

            return WaitAsWaiter(timeoutMs, allowFastWake: false);
        }

        private void MaybeWakeWaiters(Counts counts)
        {
            // Check if waiters need to be woken
            uint collisionCount = 0;
            while (true)
            {
                // Determine how many waiters to wake.
                // The number of wakes should not be more than the signal count, not more than waiter count and discount any pending wakes.
                int countOfWaitersToWake = (int)Math.Min(counts.SignalCount, counts.WaiterCount) - counts.CountOfWaitersSignaledToWake;
                if (countOfWaitersToWake <= 0)
                {
                    // No waiters to wake. This is the most common case.
                    break;
                }

                Counts newCounts = counts;
                newCounts.AddCountOfWaitersSignaledToWake((uint)countOfWaitersToWake);
                Counts countsBeforeUpdate = _separated._counts.InterlockedCompareExchange(newCounts, counts);
                if (countsBeforeUpdate == counts)
                {
                    Debug.Assert(_maximumSignalCount - counts.SignalCount >= 1);
                    if (countOfWaitersToWake > 0)
                    {
                        ReleaseCore(countOfWaitersToWake);
                    }

                    break;
                }

                // collision, try again.
                Backoff.Exponential(collisionCount++);

                counts = _separated._counts;
            }
        }

        private bool WaitAsWaiter(int timeoutMs, bool allowFastWake)
        {
            Debug.Assert(timeoutMs >= -1);

            while (true)
            {
                long blockingStart = allowFastWake ? 0 : Stopwatch.GetTimestamp();
                if (timeoutMs == 0 || !Block(timeoutMs))
                {
                    // Unregister the waiter, but do not decrement wake count, the thread did not observe a wake.
                    _separated._counts.InterlockedDecrementWaiterCount();
                    return false;
                }

                if (!allowFastWake)
                {
                    // The caller wants that the thread spends some time waiting as a matter of rate limiting
                    // thus we will require a 16 usec cooldown before reintroducing the thread.
                    // The sleep/wake transition typically takes care of the wait, but the blocker has fast
                    // wake paths and the underlying OS API may have fast/trivial wake paths as well,
                    // thus fast wakeups can happen and are hard to avoid completely.
                    // So, if a fast wake happened when parking was desired, we hold up the thread a bit
                    // before releasing.
                    long cooldown = Stopwatch.Frequency * 10 / 1000000;
                    while (Stopwatch.GetTimestamp() - blockingStart < cooldown)
                    {
                        Thread.UninterruptibleSleep0();
                        Thread.SpinWait(1);
                    }
                }

                uint collisionCount = 0;
                while (true)
                {
                    Counts counts = _separated._counts;
                    Counts newCounts = counts;

                    Debug.Assert(counts.WaiterCount != 0);

                    // we consumed a wake, decrement the count
                    Debug.Assert(counts.CountOfWaitersSignaledToWake != 0);
                    newCounts.DecrementCountOfWaitersSignaledToWake();

                    // If there is a signal, try claiming it and stop waiting.
                    if (newCounts.SignalCount != 0)
                    {
                        newCounts.DecrementSignalCount();
                        newCounts.DecrementWaiterCount();
                    }

                    Counts countsBeforeUpdate = _separated._counts.InterlockedCompareExchange(newCounts, counts);
                    if (countsBeforeUpdate == counts)
                    {
                        if (counts.SignalCount != 0)
                        {
                            // success
                            return true;
                        }

                        // We've consumed a wake, but there was no signal.
                        // The semaphore is unfair and spurious/stolen wakes can happen.
                        // We will have to wait again.
                        break;
                    }

                    // CAS collision, try again.
                    Backoff.Exponential(collisionCount++);
                }
            }
        }

        public void Signal()
        {
            // Increment signal count. This enables one-shot acquire.
            Counts counts = _separated._counts.InterlockedIncrementSignalCount();
            MaybeWakeWaiters(counts);
        }

        private bool Block(int timeoutMs)
        {
            Debug.Assert(timeoutMs >= -1);

            LifoWaitNode? blocker = t_blocker;
            if (blocker == null)
            {
                t_blocker = blocker = new LifoWaitNode();
            }

            _stackLock.Acquire();
            if (_pendingSignals != 0)
            {
                Debug.Assert(_blockerStack == null);
                Debug.Assert(_pendingSignals > 0);
                _pendingSignals--;
                blocker = null;
            }
            else
            {
                blocker._next = _blockerStack;
                _blockerStack = blocker;
            }

            _stackLock.Release();

            // lock release has a full fence thus ordinary read of _pendingWakes is ok
            if (_pendingWakes > 0)
                WakeOneCore();

            if (blocker != null)
            {
                _onWait();
                while (!blocker.TimedWait(timeoutMs))
                {
                    if (TryRemove(blocker))
                    {
                        return false;
                    }

                    // We timed out, but our waiter is already popped. Someone is waking us.
                    // We can't leave or the wake could be lost, let's wait again.
                    // Give it some extra time.
                    timeoutMs = 10;
                }
            }

            return true;
        }

        private void ReleaseCore(int count)
        {
            Debug.Assert(count > 0);

            for (int i = 0; i < count; i++)
            {
                WakeOne();
            }
        }

        private int _pendingWakes;

        private void WakeOne()
        {
            Interlocked.Increment(ref _pendingWakes);
            WakeOneCore();
        }

        private void WakeOneCore()
        {
            while (true)
            {
                if (!_stackLock.TryAcquire())
                    return; // lock holder will pick up _pendingWakes on their exit

                if (Interlocked.Decrement(ref _pendingWakes) < 0)
                {
                    // No work claimed - restore and bail
                    Interlocked.Increment(ref _pendingWakes);
                    _stackLock.Release();
                    return;
                }

                LifoWaitNode? top = _blockerStack;
                if (top != null)
                {
                    _blockerStack = top._next;
                    top._next = null;
                }
                else
                {
                    _pendingSignals++;
                    Debug.Assert(_pendingSignals != ushort.MaxValue);
                }

                _stackLock.Release();
                top?.WakeOne(); // LowLevelThreadBlocker.WakeOne()

                // lock release has a full fence thus ordinary read of _pendingWakes is ok
                if (_pendingWakes <= 0)
                    return;

                // Loop: handle any wakes that arrived while we were working
            }
        }

        // Used when waiter times out
        private bool TryRemove(LifoWaitNode node)
        {
            bool removed = false;
            _stackLock.Acquire();

            LifoWaitNode? current = _blockerStack;
            if (current == node)
            {
                _blockerStack = node._next;
                node._next = null;
                removed = true;
            }
            else
            {
                while (current != null)
                {
                    if (current._next == node)
                    {
                        current._next = node._next;
                        node._next = null;
                        removed = true;
                        break;
                    }

                    current = current._next;
                }
            }

            _stackLock.Release();

            // lock release has a full fence thus ordinary read of _pendingWakes is ok
            if (_pendingWakes > 0)
                WakeOneCore();

            return removed;
        }

        private struct Counts : IEquatable<Counts>
        {
            private const byte SignalCountShift = 0;
            private const byte WaiterCountShift = 16;
            private const byte CountOfWaitersSignaledToWakeShift = 32;

            private ulong _data;

            private Counts(ulong data) => _data = data;

            private ushort GetUInt16Value(byte shift) => (ushort)(_data >> shift);

            public ushort SignalCount
            {
                get => GetUInt16Value(SignalCountShift);
            }

            public Counts InterlockedIncrementSignalCount()
            {
                var countsAfterUpdate = new Counts(Interlocked.Add(ref _data, 1ul << SignalCountShift));
                Debug.Assert(countsAfterUpdate.SignalCount != ushort.MaxValue); // overflow check
                return countsAfterUpdate;
            }

            public void DecrementSignalCount()
            {
                Debug.Assert(SignalCount != 0);
                _data -= (ulong)1 << SignalCountShift;
            }

            public ushort WaiterCount
            {
                get => GetUInt16Value(WaiterCountShift);
            }

            public void DecrementWaiterCount()
            {
                Debug.Assert(WaiterCount != 0);
                _data -= (ulong)1 << WaiterCountShift;
            }

            public void IncrementWaiterCount()
            {
                _data += (ulong)1 << WaiterCountShift;
                Debug.Assert(WaiterCount != 0);
            }

            public void InterlockedDecrementWaiterCount()
            {
                var countsAfterUpdate = new Counts(Interlocked.Add(ref _data, unchecked((ulong)-1) << WaiterCountShift));
                Debug.Assert(countsAfterUpdate.WaiterCount != ushort.MaxValue); // underflow check
            }

            public Counts InterlockedIncrementWaiterCount()
            {
                var countsAfterUpdate = new Counts(Interlocked.Add(ref _data, unchecked((ulong)1) << WaiterCountShift));
                Debug.Assert(countsAfterUpdate.WaiterCount != ushort.MaxValue); // overflow check
                return countsAfterUpdate;
            }

            public ushort CountOfWaitersSignaledToWake
            {
                get => GetUInt16Value(CountOfWaitersSignaledToWakeShift);
            }

            public void AddCountOfWaitersSignaledToWake(uint value)
            {
                _data += (ulong)value << CountOfWaitersSignaledToWakeShift;
                var countsAfterUpdate = new Counts(_data);
                Debug.Assert(countsAfterUpdate.CountOfWaitersSignaledToWake != ushort.MaxValue); // overflow check
            }

            public void DecrementCountOfWaitersSignaledToWake()
            {
                Debug.Assert(CountOfWaitersSignaledToWake != 0);
                _data -= (ulong)1 << CountOfWaitersSignaledToWakeShift;
            }

            public Counts InterlockedCompareExchange(Counts newCounts, Counts oldCounts) =>
                new Counts(Interlocked.CompareExchange(ref _data, newCounts._data, oldCounts._data));

            public static bool operator ==(Counts lhs, Counts rhs) => lhs.Equals(rhs);
            public static bool operator !=(Counts lhs, Counts rhs) => !lhs.Equals(rhs);

            public override bool Equals([NotNullWhen(true)] object? obj) => obj is Counts other && Equals(other);
            public bool Equals(Counts other) => _data == other._data;
            public override int GetHashCode() => (int)_data + (int)(_data >> 32);
        }

        [StructLayout(LayoutKind.Explicit, Size = 2 * PaddingHelpers.CACHE_LINE_SIZE)]
        private struct CacheLineSeparatedCounts
        {
            [FieldOffset(PaddingHelpers.CACHE_LINE_SIZE)]
            public Counts _counts;
        }
    }
}
