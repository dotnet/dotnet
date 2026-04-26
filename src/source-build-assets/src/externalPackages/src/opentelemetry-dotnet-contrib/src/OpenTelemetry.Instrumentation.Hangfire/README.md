# Hangfire Instrumentation for OpenTelemetry .NET

| Status | |
| ------ | --- |
| Stability | [Beta](../../README.md#beta) |
| Code Owners | [@fred2u](https://github.com/fred2u) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.Instrumentation.Hangfire)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Hangfire)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.Instrumentation.Hangfire)](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Hangfire)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-Instrumentation.Hangfire)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-Instrumentation.Hangfire)

This is an
[Instrumentation Library](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/glossary.md#instrumentation-library),
which instruments
[Hangfire](https://www.nuget.org/packages/Hangfire/)
and collects traces and metrics about BackgroundJob executions.

<!--
  Following statement is required by the CNCF Governing Board.
  See https://github.com/cncf/foundation/issues/1065#issuecomment-3563771634
  for details.
-->
> [!IMPORTANT]
> Please note that installing the OpenTelemetry Hangfire instrumentation
> will retrieve from NuGet and install Hangfire.Core, which is provided
> by its licensor under a choice of LGPL-3.0 or a commercial license.

## Steps to enable OpenTelemetry.Instrumentation.Hangfire

> [!NOTE]
> The following steps show how to enable **tracing**. For metrics, see the
> [Metrics](#metrics) section below.

### Step 1: Install and configure Hangfire

[Getting Started](https://docs.hangfire.io/en/latest/getting-started/index.html)

### Step 2: Install Hangfire instrumentation Package

Add a reference to the
[`OpenTelemetry.Instrumentation.Hangfire`](https://www.nuget.org/packages/OpenTelemetry.Instrumentation.Hangfire)
package. Also, add any other instrumentations & exporters you will need.

```shell
dotnet add package OpenTelemetry.Instrumentation.Hangfire --prerelease
```

### Step 3: Enable Hangfire Instrumentation at application startup

Hangfire instrumentation must be enabled at application startup.

The following example demonstrates adding Hangfire instrumentation to a
console application. This example also sets up the OpenTelemetry Console
exporter, which requires adding the package
[`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

```csharp
using OpenTelemetry.Trace;

public class Program
{
    public static void Main(string[] args)
    {
        using var tracerProvider = Sdk
            .CreateTracerProviderBuilder()
            .AddHangfireInstrumentation()
            .AddConsoleExporter()
            .Build();
    }
}
```

For an ASP.NET Core application, adding instrumentation is typically done in
the `ConfigureServices` of your `Startup` class. Refer to [example](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/examples/AspNetCore/Program.cs).

For an ASP.NET application, adding instrumentation is typically done in the
`Global.asax.cs`. Refer to [example](../../examples/AspNet/Global.asax.cs).

## Advanced configuration

This instrumentation can be configured to change the default behavior by using
`HangfireInstrumentationOptions`.

```csharp
using var tracerProvider = Sdk
    .CreateTracerProviderBuilder()
    .AddHangfireInstrumentation(options =>
    {
        options.DisplayNameFunc = job => $"JOB {job.Id}";
        options.Filter = job => job.Id == "Filter this job";
        options.RecordException = true;
    })
    .AddConsoleExporter()
    .Build();
```

When used with
[`OpenTelemetry.Extensions.Hosting`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Extensions.Hosting/README.md),
all configurations to `HangfireInstrumentationOptions`
can be done in the `ConfigureServices` method of your applications `Startup`
class as shown below.

```csharp
// Configure
services.Configure<HangfireInstrumentationOptions>(options =>
{
    options.DisplayNameFunc = job => $"JOB {job.Id}";
    options.Filter = job => job.Id == "Filter this job";
    options.RecordException = true;
});

services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddHangfireInstrumentation()
        .AddConsoleExporter());
```

### RecordException

Configures a value indicating whether the exception will be recorded as
ActivityEvent or not. See
[Semantic Conventions for Exceptions on Spans](https://github.com/open-telemetry/semantic-conventions/blob/main/docs/exceptions/exceptions-spans.md)

```csharp
using var tracerProvider = Sdk
    .CreateTracerProviderBuilder()
    .AddHangfireInstrumentation(options =>
    {
        options.RecordException = true;
    })
    .AddConsoleExporter()
    .Build();
```

### DisplayNameFunc

This option allows changing activity display name.

```C#
using var tracerProvider = Sdk
    .CreateTracerProviderBuilder()
    .AddHangfireInstrumentation(options =>
    {
        options.DisplayNameFunc = job => $"JOB {job.Id}";
    })
    .AddConsoleExporter()
    .Build();
```

If not configured the default is

```C#
$"JOB {BackgroundJob.Job.Type.Name}.{BackgroundJob.Job.Method.Name}"
```

### Filter

This option can be used to filter out activities based on the `BackgroundJob`
being executed. The `Filter` function should return `true` if the telemetry is
to be collected, and `false` if it should not.

The following code snippet shows how to use `Filter` to filter out traces for
job with a specified job id.

```csharp
using var tracerProvider = Sdk
    .CreateTracerProviderBuilder()
    .AddHangfireInstrumentation(options =>
    {
        options.Filter = job => job.Id == "Filter this job";
    })
    .AddConsoleExporter()
    .Build();
```

## Metrics

This instrumentation library collects metrics following a POC/draft definition
of workflow metrics defined as part of
[semantic-conventions/#1688](https://github.com/open-telemetry/semantic-conventions/issues/1688).
As those definitions evolve, changes including breaking ones will flow back
to this implementation.

In Hangfire, the workflow semantic conventions are applied as follows:

- Each background job execution is modeled as a **workflow task execution**
- Workflow-level metrics track the complete lifecycle (including scheduled jobs)
- Execution-level metrics track the execution pipeline (enqueued jobs and later)

This approach provides comprehensive visibility into both job
creation/scheduling and actual execution performance.

### Enabling Metrics

Metrics are enabled by adding Hangfire instrumentation to the `MeterProviderBuilder`:

```csharp
using OpenTelemetry.Metrics;

var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddHangfireInstrumentation()
    .AddConsoleExporter()
    .Build();
```

The above example also sets up the OpenTelemetry Console exporter, which requires
adding the package
[`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

### Enabling both Traces and Metrics

To collect both traces and metrics from Hangfire:

```csharp
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddHangfireInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddHangfireInstrumentation()
        .AddOtlpExporter());
```

### Metrics Configuration

Metrics behavior is configured through the
`HangfireMetricsInstrumentationOptions` class in the
`OpenTelemetry.Metrics` namespace.

#### RecordQueueLatency

By default, the instrumentation records only the execution phase duration
(time spent actually running the job). To also track the pending phase
duration (time spent waiting in the queue before execution), enable the
`RecordQueueLatency` option:

```csharp
var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddHangfireInstrumentation(options =>
    {
        options.RecordQueueLatency = true;
    })
    .AddConsoleExporter()
    .Build();
```

When enabled, `workflow.execution.duration` is recorded with
`workflow.execution.state="pending"` representing queue wait time, in addition
to `workflow.execution.state="executing"` for actual execution time.

> [!WARNING]
> Enabling `RecordQueueLatency` requires an additional database query per job
> execution to retrieve the enqueue timestamp. In high-throughput scenarios,
> this may impact performance.
>
> [!NOTE]
> Add `using OpenTelemetry.Metrics;` to access
> `HangfireMetricsInstrumentationOptions`.

### Available Metrics

The following metrics are emitted by this instrumentation:

**Execution-level metrics** track jobs that have entered the execution
pipeline:

- `workflow.execution.outcome` - Counter for completed executions
- `workflow.execution.duration` - Histogram for execution duration (pending
  and executing phases)
- `workflow.execution.status` - UpDownCounter for current execution state
- `workflow.execution.errors` - Counter for execution errors

**Workflow-level metrics** track the complete job lifecycle:

- `workflow.outcome` - Counter for completed workflows
- `workflow.status` - UpDownCounter for current workflow state (including
  scheduled jobs)

---

#### workflow.execution.outcome

The number of task executions which have been initiated.

| Units          | Instrument Type | Value Type |
| -------------- | --------------- | ---------- |
| `{executions}` | Counter         | `Int64`    |

> [!NOTE]
> This metric does NOT include the `workflow.execution.state` attribute, as
> state differentiation is not needed for counting completed executions.

**Attributes:**

| Attribute                   | Type   | Description                            | Requirement Level         | Values                |
| --------------------------- | ------ | -------------------------------------- | ------------------------- | --------------------- |
| `workflow.task.name`        | string | Name of the task (Hangfire job method) | Required                  | e.g., `MyJob.Execute` |
| `workflow.execution.result` | string | The result of executing the task       | Required                  | `success`, `failure`  |
| `workflow.platform.name`    | string | The workflow platform being used       | Recommended               | `hangfire`            |
| `error.type`                | string | The type of error that occurred        | Conditionally Required[1] | Exception type name   |

[1]: Required if and only if the task execution failed.

#### workflow.execution.duration

Duration of an execution grouped by task, state, and result. Records duration
for different execution phases:

- **state=pending**: Time spent waiting in queue (only when
  `RecordQueueLatency` is enabled)
- **state=executing**: Time spent in actual execution

| Units | Instrument Type | Value Type |
| ----- | --------------- | ---------- |
| `s`   | Histogram       | `Double`   |

> [!NOTE]
> The `workflow.execution.state` attribute is an extension to the current
> semantic conventions to support measuring different execution phases
> (pending vs executing). This attribute is being proposed for inclusion in
> the official semantic conventions.

**Attributes:**

| Attribute                   | Type   | Description                            | Requirement Level         | Values                 |
| --------------------------- | ------ | -------------------------------------- | ------------------------- | ---------------------- |
| `workflow.task.name`        | string | Name of the task (Hangfire job method) | Required                  | e.g., `MyJob.Execute`  |
| `workflow.execution.result` | string | The result of executing the task       | Required                  | `success`, `failure`   |
| `workflow.execution.state`  | string | The execution phase being measured     | Required                  | `pending`, `executing` |
| `workflow.platform.name`    | string | The workflow platform being used       | Recommended               | `hangfire`             |
| `error.type`                | string | The type of error that occurred        | Conditionally Required[1] | Exception type name    |

[1]: Required if and only if the task execution failed.

#### workflow.execution.status

The number of actively running tasks grouped by task and the current state.

| Units          | Instrument Type | Value Type |
| -------------- | --------------- | ---------- |
| `{executions}` | UpDownCounter   | `Int64`    |

> [!NOTE]
> This metric tracks the current state of job executions. When a job
> transitions to a new state, the previous state is decremented and the new
> state is incremented.

**Attributes:**

| Attribute                  | Type   | Description                            | Requirement Level         | Values                              |
| -------------------------- | ------ | -------------------------------------- | ------------------------- | ----------------------------------- |
| `workflow.task.name`       | string | Name of the task (Hangfire job method) | Required                  | e.g., `MyJob.Execute`               |
| `workflow.execution.state` | string | Current state of the execution         | Required                  | `pending`, `executing`, `completed` |
| `workflow.platform.name`   | string | The workflow platform being used       | Recommended               | `hangfire`                          |
| `error.type`               | string | The type of error that occurred        | Conditionally Required[1] | Exception type name                 |

[1]: Required if and only if the task execution failed.

**Hangfire State Mapping:**

Hangfire job states are mapped to workflow semantic convention states as follows:

| Hangfire State                | Workflow State |
| ----------------------------- | -------------- |
| Scheduled, Enqueued, Awaiting | `pending`      |
| Processing                    | `executing`    |
| Succeeded, Failed, Deleted    | `completed`    |

#### workflow.execution.errors

The number of errors encountered in task executions.

| Units     | Instrument Type | Value Type |
|-----------|-----------------|------------|
| `{error}` | Counter         | `Int64`    |

**Attributes:**

| Attribute                | Type   | Description                            | Requirement Level | Values                |
| ------------------------ | ------ | -------------------------------------- | ----------------- | --------------------- |
| `error.type`             | string | The type of error that occurred        | Required          | Exception type name   |
| `workflow.task.name`     | string | Name of the task (Hangfire job method) | Required          | e.g., `MyJob.Execute` |
| `workflow.platform.name` | string | The workflow platform being used       | Recommended       | `hangfire`            |

#### workflow.outcome

The number of workflow instances which have been initiated. In Hangfire, this tracks
individual job completions.

| Units         | Instrument Type | Value Type |
| ------------- | --------------- | ---------- |
| `{workflows}` | Counter         | `Int64`    |

**Attributes:**

| Attribute                  | Type   | Description                                 | Requirement Level         | Values                    |
| -------------------------- | ------ | ------------------------------------------- | ------------------------- | ------------------------- |
| `workflow.definition.name` | string | Name of the workflow (Hangfire job method)  | Required                  | e.g., `MyJob.Execute`     |
| `workflow.result`          | string | The result of the workflow                  | Required                  | `success`, `failure`      |
| `workflow.trigger.type`    | string | Type of trigger that initiated the workflow | Required                  | `api`, `cron`, `schedule` |
| `workflow.platform.name`   | string | The workflow platform being used            | Recommended               | `hangfire`                |
| `error.type`               | string | The type of error that occurred             | Conditionally Required[1] | Exception type name       |

[1]: Required if and only if the workflow execution failed.

#### workflow.status

The number of actively running workflows grouped by definition and the current state.

| Units         | Instrument Type | Value Type |
| ------------- | --------------- | ---------- |
| `{workflows}` | UpDownCounter   | `Int64`    |

> [!NOTE]
> This metric tracks the workflow lifecycle including jobs that haven't
> entered the execution pipeline yet (e.g., scheduled jobs waiting for their
> trigger time). When a workflow transitions to a new state, the previous
> state is decremented and the new state is incremented.

**Attributes:**

| Attribute                  | Type   | Description                                 | Requirement Level         | Values                              |
| -------------------------- | ------ | ------------------------------------------- | ------------------------- | ----------------------------------- |
| `workflow.definition.name` | string | Name of the workflow (Hangfire job method)  | Required                  | e.g., `MyJob.Execute`               |
| `workflow.state`           | string | Current state of the workflow               | Required                  | `pending`, `executing`, `completed` |
| `workflow.trigger.type`    | string | Type of trigger that initiated the workflow | Required                  | `api`, `cron`, `schedule`           |
| `workflow.platform.name`   | string | The workflow platform being used            | Recommended               | `hangfire`                          |
| `error.type`               | string | The type of error that occurred             | Conditionally Required[1] | Exception type name                 |

[1]: Required if and only if the workflow execution failed.

**Hangfire State Mapping:**

Hangfire job states are mapped to workflow semantic convention states as follows:

| Hangfire State                       | Workflow State | Workflow Trigger Type           |
| ------------------------------------ | -------------- | ------------------------------- |
| Scheduled                            | `pending`      | `schedule`                      |
| Enqueued, Awaiting (from cron)       | `pending`      | `cron`                          |
| Enqueued, Awaiting (fire-and-forget) | `pending`      | `api`                           |
| Processing                           | `executing`    | (inherited from previous state) |
| Succeeded, Failed, Deleted           | `completed`    | (inherited from previous state) |

> [!IMPORTANT]
> **Difference between `workflow.status` and `workflow.execution.status`:**
>
> - **`workflow.status`**: Tracks the complete workflow lifecycle, including
>   jobs that haven't started execution yet (e.g., scheduled jobs waiting for
>   their trigger time). This provides visibility into jobs across all states.
>
> - **`workflow.execution.status`**: Tracks only jobs that have entered the
>   execution pipeline (enqueued or later). Scheduled jobs do NOT appear here
>   until they become enqueued.
>
> For example, a scheduled job appears in
> `workflow.status{state=pending, trigger.type=schedule}` but NOT in
> `workflow.execution.status` until it becomes enqueued. Once enqueued, it
> appears in both metrics.

### Semantic Conventions

This instrumentation follows the OpenTelemetry semantic conventions for workflows:

#### Attribute Usage

- **workflow.platform.name**: Always set to `"hangfire"`
- **workflow.task.name** / **workflow.definition.name**: Derived from the
  Hangfire job method (e.g., `ClassName.MethodName`)
  - `workflow.task.name` is used for execution-level metrics
    (`workflow.execution.*`)
  - `workflow.definition.name` is used for workflow-level metrics
    (`workflow.outcome`, `workflow.status`)
- **workflow.trigger.type**: Identifies how the job was initiated
  - `"cron"` for recurring jobs
  - `"schedule"` for delayed jobs (scheduled for future execution)
  - `"api"` for fire-and-forget and continuation jobs
  - Only included on workflow-level metrics (`workflow.outcome`,
    `workflow.status`), not on execution-level metrics
    (`workflow.execution.*`)
- **workflow.execution.result** / **workflow.result**: Set to `"success"` when
  jobs complete without errors, `"failure"` when an exception is thrown
- **workflow.execution.state** / **workflow.state**: Maps Hangfire states to
  semantic convention states
  - `workflow.execution.state` tracks execution pipeline states (used in
    `workflow.execution.status`, `workflow.execution.duration`)
  - `workflow.state` tracks complete workflow lifecycle (used in
    `workflow.status`)
  - See state mapping tables above for details
- **error.type**: Set to the full type name of the exception when a job fails

#### Workflow vs Execution Metrics

The instrumentation distinguishes between **workflow-level** and
**execution-level** metrics:

**Workflow-level metrics** (`workflow.outcome`, `workflow.status`):

- Track the complete lifecycle of jobs, including pre-execution states (e.g.,
  scheduled)
- Use `workflow.definition.name`, `workflow.state`, and
  `workflow.trigger.type`
- Provide visibility into all jobs regardless of execution status

**Execution-level metrics** (`workflow.execution.*`):

- Track only jobs that have entered the execution pipeline (enqueued or later)
- Use `workflow.task.name` and `workflow.execution.state`
- Do NOT include `workflow.trigger.type` (trigger is a workflow-level concept)
- Provide detailed execution performance metrics

## References

- [OpenTelemetry Project](https://opentelemetry.io/)
- [Hangfire Project](https://www.hangfire.io/)
