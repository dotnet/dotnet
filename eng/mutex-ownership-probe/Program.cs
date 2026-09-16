using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;

internal static class Program
{
    private static int Main(string[] args)
    {
        int seconds = int.Parse(args[0]);
        bool initiallyOwned = bool.Parse(args[1]);
        var timer = Stopwatch.StartNew();
        var failures = new List<object>();
        string prefix = "Global\\vmr-mutex-probe-" + Guid.NewGuid().ToString("N") + "-";
        int completed = 0;
        int stop = 0;
        int failureCount = 0;
        using var phases = new Barrier(2, phase =>
        {
            if (phase.CurrentPhaseNumber % 3 == 2)
            {
                completed++;
                Volatile.Write(ref stop, failureCount != 0 || timer.Elapsed.TotalSeconds >= seconds ? 1 : 0);
            }
        });

        void Worker()
        {
            for (int iteration = 0; ; iteration++)
            {
                if (!phases.SignalAndWait(TimeSpan.FromSeconds(20)))
                    throw new TimeoutException("Probe start barrier timed out.");

                int acquireThread = Environment.CurrentManagedThreadId;
                using (var mutex = new Mutex(initiallyOwned, prefix + iteration, out bool createdNew))
                {
                    bool owned = initiallyOwned && createdNew;
                    bool waited = false;
                    if (!owned)
                    {
                        waited = true;
                        try
                        {
                            owned = mutex.WaitOne(0);
                        }
                        catch (AbandonedMutexException)
                        {
                            owned = true;
                        }
                    }

                    // Keep a competing acquisition held until both constructors have returned.
                    if (!phases.SignalAndWait(TimeSpan.FromSeconds(20)))
                        throw new TimeoutException("Probe acquisition barrier timed out.");

                    if (owned)
                    {
                        try
                        {
                            mutex.ReleaseMutex();
                        }
                        catch (Exception error) when (error is InvalidOperationException || error is ApplicationException)
                        {
                            Interlocked.Increment(ref failureCount);
                            lock (failures)
                            {
                                failures.Add(new
                                {
                                    iteration,
                                    createdNew,
                                    initiallyOwned,
                                    waited,
                                    acquireThread,
                                    releaseThread = Environment.CurrentManagedThreadId,
                                    exception = error.ToString()
                                });
                            }
                        }
                    }
                }

                if (!phases.SignalAndWait(TimeSpan.FromSeconds(20)))
                    throw new TimeoutException("Probe completion barrier timed out.");
                if (Volatile.Read(ref stop) != 0)
                    return;
            }
        }

        var first = new Thread(Worker) { IsBackground = true };
        var second = new Thread(Worker) { IsBackground = true };
        first.Start();
        second.Start();
        if (!first.Join(TimeSpan.FromSeconds(seconds + 60)) || !second.Join(TimeSpan.FromSeconds(30)))
            throw new TimeoutException("Probe workers did not complete within their time budget.");

        Console.WriteLine(JsonSerializer.Serialize(new
        {
            framework = RuntimeInformation.FrameworkDescription,
            coreLib = typeof(object).Assembly.Location,
            architecture = RuntimeInformation.ProcessArchitecture.ToString(),
            processorCount = Environment.ProcessorCount,
            initiallyOwned,
            elapsedSeconds = timer.Elapsed.TotalSeconds,
            completedIterations = completed,
            ownershipViolations = failureCount,
            failures
        }, new JsonSerializerOptions { WriteIndented = true }));
        return failureCount == 0 ? 0 : 2;
    }
}
