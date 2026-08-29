# Helix test results JSON format

The Helix test reporter writes a portable, language-neutral JSON file
describing the results of a work item. This file is written alongside
the legacy pickle format (`__test_report.json`) and is intended as the
long-term wire format between the in-work-item reporter and any consumer
(the Python Helix client, an AOT/native Helix client, build-side
post-processing, etc.).

## File location

```
$HELIX_WORKITEM_ROOT/__test_report_v2.json
```

If `HELIX_WORKITEM_ROOT` is unset (e.g. ad-hoc local invocation), the
file is written to the current working directory.

The file is written **unconditionally** by `reporter/run.py`, regardless
of whether the legacy `helix-scripts` Python package is installed on the
machine. When `helix-scripts` is present, the legacy pickle file is
also written; behavior is byte-identical to releases prior to the
introduction of this format.

## Producing this file directly

The xUnit / JUnit / TRX parsers shipped with the reporter are
*adapters*: they read a third-party XML format and populate the same
`TestResult` objects that get serialized here. A test runner that does
not produce one of those XML formats can emit this JSON file directly
and skip the parser entirely. The reporter (or any consumer) will
treat directly-emitted JSON identically to JSON produced from XML.

Recommended use: write `__test_report_v2.json` to `$HELIX_WORKITEM_ROOT`
from your test runner's own reporting hook, then either omit
`EnableAzurePipelinesReporter` or let it run — the reporter is a no-op
on a missing/empty results XML when the JSON file already exists.

## Schema

Top level:

```json
{
  "schema_version": 1,
  "azdo": { ... },
  "results": [ { ... }, ... ]
}
```

| Field            | Type   | Required | Description |
|------------------|--------|----------|-------------|
| `schema_version` | int    | yes      | Major version of this format. Consumers must refuse files with an unknown major version. Current value: `1`. |
| `azdo`           | object | yes      | Azure DevOps reporting parameters needed by the consumer to publish results. May contain `null` fields when running outside an AzDO pipeline. |
| `results`        | array  | yes      | Per-test results. May be empty (e.g. work item produced no detectable tests). |

### `azdo` object

| Field            | Type           | Required | Description |
|------------------|----------------|----------|-------------|
| `collection_uri` | string \| null | yes      | AzDO collection URI, e.g. `https://dev.azure.com/dnceng/`. |
| `team_project`   | string \| null | yes      | AzDO project name, e.g. `internal`. |
| `test_run_id`    | string \| null | yes      | AzDO test run ID, as a string. Created by the build before the work item is dispatched. |
| `access_token`   | string \| null | no       | AzDO bearer token used by the consumer to POST results. **Sensitive.** Consumers MUST treat the entire file as secret while this field is populated. May be omitted when the consumer obtains the token by other means (managed identity, env var). |

### `results[]` object

Each entry describes one test case.

| Field              | Type           | Required | Description |
|--------------------|----------------|----------|-------------|
| `name`             | string         | yes      | Human-readable test identifier. Typically `{type}.{method}` plus theory data. Used as the display name in AzDO. |
| `kind`             | string         | yes      | Source format: one of `"xunit"`, `"junit"`, `"trx"`, or any custom value used by a direct producer. Informational only. |
| `type`             | string \| null | yes      | Containing class / module / fixture name. |
| `method`           | string \| null | yes      | Method / function name within `type`. |
| `duration_seconds` | number         | yes      | Wall-clock duration of the test, in seconds. `0` if unknown. |
| `result`           | string         | yes      | Outcome. One of: `"Pass"`, `"Fail"`, `"Skip"`. Other values are reserved. |
| `exception_type`   | string \| null | yes      | Fully-qualified exception type for failed tests, e.g. `"Xunit.Sdk.TrueException"`. `null` for passes/skips or when not available. |
| `failure_message`  | string \| null | yes      | Failure message produced by the test framework. `null` for passes/skips. May contain newlines. |
| `stack_trace`      | string \| null | yes      | Captured stack trace for failed tests. `null` for passes/skips or when not available. May contain newlines. |
| `skip_reason`      | string \| null | yes      | Reason a test was skipped (e.g. xUnit `[Fact(Skip = "...")]`). `null` for non-skipped tests. |
| `ignored`          | bool           | no       | Producer-side hint that the consumer should not report this result (used during local rerun logic). Defaults to `false`. |
| `attachments`      | array          | yes      | Per-test attachments. Empty array if none. |

### `attachments[]` object

| Field   | Type           | Required | Description |
|---------|----------------|----------|-------------|
| `name`  | string         | yes      | Attachment file name as it should appear in AzDO. Should be unique within the test result. |
| `text`  | string         | yes      | Attachment contents as text. Binary attachments must be base64- or otherwise-encoded into a textual form by the producer; this format does not transport binary blobs natively. |

## Example

```json
{
  "schema_version": 1,
  "azdo": {
    "collection_uri": "https://dev.azure.com/dnceng/",
    "team_project": "internal",
    "test_run_id": "12345",
    "access_token": "eyJ0eXAiOiJKV1QiLCJ..."
  },
  "results": [
    {
      "name": "MyTests.Math.Addition",
      "kind": "xunit",
      "type": "MyTests.Math",
      "method": "Addition",
      "duration_seconds": 0.012,
      "result": "Pass",
      "exception_type": null,
      "failure_message": null,
      "stack_trace": null,
      "skip_reason": null,
      "ignored": false,
      "attachments": []
    },
    {
      "name": "MyTests.Math.Division_ByZero",
      "kind": "xunit",
      "type": "MyTests.Math",
      "method": "Division_ByZero",
      "duration_seconds": 0.003,
      "result": "Fail",
      "exception_type": "System.DivideByZeroException",
      "failure_message": "Attempted to divide by zero.",
      "stack_trace": "   at MyTests.Math.Division_ByZero() in ...",
      "skip_reason": null,
      "ignored": false,
      "attachments": [
        { "name": "stdout.txt", "text": "Computing 1 / 0...\n" }
      ]
    },
    {
      "name": "MyTests.Math.SlowAddition",
      "kind": "xunit",
      "type": "MyTests.Math",
      "method": "SlowAddition",
      "duration_seconds": 0,
      "result": "Skip",
      "exception_type": null,
      "failure_message": null,
      "stack_trace": null,
      "skip_reason": "Disabled pending perf investigation",
      "ignored": false,
      "attachments": []
    }
  ]
}
```

## Versioning

* `schema_version` is a single integer.
* Consumers MUST treat any value other than the version(s) they
  understand as an error, not silently degrade.
* Backwards-compatible additions (new optional fields, new optional
  outcome strings) do **not** bump the version.
* Breaking changes (renamed fields, changed semantics, changed required
  fields) MUST bump the version.

## Security

The `azdo.access_token` field, when populated, is a bearer token with
build-scope authority on Azure DevOps. The producer SHOULD ensure the
file is written with permissions that prevent other users on the
machine from reading it; the consumer SHOULD delete or overwrite the
file once results have been published.
