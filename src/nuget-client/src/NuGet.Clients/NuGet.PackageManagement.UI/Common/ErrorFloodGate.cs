using System;
using System.Collections.Concurrent;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Sliding in time error threshold evaluator.
    /// Verifies the number of fatal errors within last hour not bigger than reasonable threshold.
    /// </summary>
    internal class ErrorFloodGate
    {
        // If we fail at least this high (failures/attempts), we'll shut off image loads.
        // TODO: Should we allow this to be overridden in nuget.config.
        private const double StopLoadingThreshold = 0.50;
        private const int SlidingExpirationInMinutes = 60;
        private const int MinFailuresCount = 5;
        private const int SecondsInOneTick = 5;
        private readonly DateTimeOffset _origin = DateTimeOffset.Now;
        private readonly ConcurrentQueue<int> _attempts = new ConcurrentQueue<int>();
        private readonly ConcurrentQueue<int> _failures = new ConcurrentQueue<int>();

        private DateTimeOffset _lastEvaluate = DateTimeOffset.Now;
        private bool _hasTooManyNetworkErrors = false;

        public bool HasTooManyNetworkErrors
        {
            get
            {
                if (GetTicks(_lastEvaluate) > 1)
                {
                    var discardOlderThan1Hour = GetTicks(DateTimeOffset.Now.AddMinutes(-SlidingExpirationInMinutes));

                    ExpireOlderValues(_attempts, discardOlderThan1Hour);
                    ExpireOlderValues(_failures, discardOlderThan1Hour);

                    var attemptsCount = _attempts.Count;
                    var failuresCount = _failures.Count;
                    _hasTooManyNetworkErrors = attemptsCount > 0 && failuresCount > MinFailuresCount && ((double)failuresCount / attemptsCount) > StopLoadingThreshold;
                    _lastEvaluate = DateTimeOffset.Now;
                }
                return _hasTooManyNetworkErrors;
            }
        }

        private static void ExpireOlderValues(ConcurrentQueue<int> q, int expirationOffsetInTicks)
        {
            lock (q) //locking for TryPeek and TryDequeue
            {
                while (q.Count > 0)
                {
                    int result;
                    bool peekSucceeded = q.TryPeek(out result);
                    if (!peekSucceeded)
                    {
                        continue;
                    }

                    if (result < expirationOffsetInTicks)
                    {
                        q.TryDequeue(out result);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        public void ReportAttempt()
        {
            int ticks = GetTicks(_origin);
            _attempts.Enqueue(ticks);
        }

        public void ReportBadNetworkError()
        {
            int ticks = GetTicks(_origin);
            _failures.Enqueue(ticks);
        }

        // Ticks here are of 5sec long
        private static int GetTicks(DateTimeOffset origin)
        {
            return (int)((DateTimeOffset.Now - origin).TotalSeconds / SecondsInOneTick);
        }
    }
}
