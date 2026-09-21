# State and status

`MonitorState` owns invocation state behind one lock. Collections are exposed
only through immutable snapshots. Upload workers update only narrow methods:
upload lifecycle, uploaded test outcomes, and durable completion.

The runner publishes an immutable latest-poll snapshot. A dedicated periodic
reporting loop consumes that snapshot and the upload pipeline's atomic counters
every five minutes. Reporting therefore continues at a fixed cadence while the
poll loop is active and during final drain, without waiting for Helix file
access or Azure DevOps result uploads.

An initial status is emitted after the first poll. Subsequent status messages
are timer-driven rather than triggered by job-count changes, so completion
bursts do not increase log volume. Normal logging reports semantic transitions
and aggregate counts. Verbose
logging adds queue depth, active worker counts, finalizer depth, and uploaded
result totals. It deliberately does not print every job, work item, file, or
request; verbose mode must remain usable on runs containing thousands of work
items and millions of results.
