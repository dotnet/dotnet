# Test id report logger

> [!WARNING]
> **This logger is temporary and will be removed.**
>
> It exists for one purpose: to let you build a mapping from the test case ids you have already
> stored to the ids that will replace them when vstest changes the algorithm it hashes test case ids
> with. It will be deleted at the same time as the SHA1 implementation it reports on, and there will
> be no replacement, because once SHA1 is gone there is nothing left to map from. Do not build a
> permanent pipeline on it.

## What this is for

vstest computes `TestCase.Id` by hashing a seed string. That hash has always been SHA1. A new
algorithm, xxHash128, is now available and will become the default in a later release; until then a
run opts in to it with the `VSTEST_DISABLE_XXHASH128_TESTCASE_ID` feature flag set to `0` - see
[Environment variables](environment-variables.md).

When the default moves, the id of every test whose id the platform computes changes. Anything that
stored those ids - Azure DevOps test case work item associations, a test history database, a flaky
test tracker, a triage spreadsheet - needs to know which old id becomes which new id.

Without this logger the only way to obtain that mapping is to run the whole suite twice, once with
each algorithm selected, and join the two result files on test name. That join is ambiguous in
practice: data driven tests routinely produce several rows that share a fully qualified name and
whose display names do not distinguish them, and there is no reliable key to join those rows on.

This logger removes the join. One run, and every test is reported once with **both** ids on the same
row.

## Enabling it

```shell
vstest.console.exe Tests.dll /logger:testids
```

```shell
dotnet test --logger:testids
```

By default the report is written to `TestIds.csv` in the test results directory, qualified by the
target framework when the platform reports one - a multi targeted project is run once per framework
into the same directory, so its reports are written as `TestIds_net8.0.csv`, `TestIds_net48.csv` and
so on rather than overwriting each other. An existing report is not overwritten under the default
name: the next free `TestIds_net8.0(1).csv`, `TestIds_net8.0(2).csv` is claimed instead, because every
project of a solution run into a shared results directory picks the same default name, and a mapping
quietly replaced by another project's is a mapping lost. **The path actually written is printed at the
end of the run - read it rather than assuming the plain name**, because on any rerun into the same
directory the plain name holds the *previous* run's report.

A different path can be given with `LogFileName`, which may be absolute or relative to the test
results directory. That path is used exactly as given and is overwritten if it already exists, so
pass it when a script needs to know up front where the report will be:

```shell
vstest.console.exe Tests.dll /logger:"testids;LogFileName=ids\mapping.csv"
```

The logger also runs during discovery, so the mapping can be produced without executing anything:

```shell
vstest.console.exe Tests.dll /ListTests /logger:testids
```

Which algorithm the run itself uses does not matter. Both ids are reported either way; the run's
choice - the `VSTEST_DISABLE_XXHASH128_TESTCASE_ID` flag, which defaults to `1` and so today means
SHA1 - only determines which one the test actually carries, and so which value `IdSource` reports.
There is no need to set it to produce the mapping.

## Output format

The report is CSV, quoted per RFC 4180 - a field containing a comma, a quote or a newline is
enclosed in quotes and embedded quotes are doubled. CSV is the format because the report is meant to
be joined against whatever you stored your ids in, and a flat table of scalar columns loads into a
database, a spreadsheet or a script without anything having to be written to parse it.

| Column | Meaning |
| ------ | ------- |
| `Source` | The test container the test was found in, as the adapter reported it. |
| `ExecutorUri` | The executor uri of the adapter that reported the test. |
| `FullyQualifiedName` | The fully qualified name of the test, as reported. |
| `DisplayName` | The display name of the test. For data driven tests this is often the only thing distinguishing one row from another. |
| `Id` | **The id the test actually carries today.** |
| `Sha1Id` | The id the SHA1 algorithm computes from this test's seed. |
| `XxHash128Id` | The id the xxHash128 algorithm computes from this test's seed. |
| `IdSource` | Where `Id` came from: `Sha1`, `XxHash128` or `SelfAssigned`. |

Rows are sorted by source, then fully qualified name, then executor uri, then id - every part of the
deduplication key - so two reports of the same suite are byte identical and can be diffed against
each other.

The path the report was written to is printed at the end of the run. If it could not be written the
failure is reported as an error rather than passed over silently, because a missing report is
otherwise indistinguishable from a suite with no tests in it.

If the run was aborted or cancelled the report is still written, but a warning says so. Such a report
covers only the tests that were reported before the run stopped, and must not be used to migrate
stored ids: the tests that were never reached are missing from it for a reason that has nothing to do
with whether they still exist.

### Deduplication

Each distinct test is reported once, keyed on source, executor uri, fully qualified name and the id
the test carries.

- A test that is **retried**, or that otherwise reports several results, carries the same id every
  time and is reported once.
- **Data driven rows that carry distinct ids** are reported as distinct rows, because each id needs
  its own mapping. This is precisely the case a two-run join cannot resolve.
- Data driven rows that share a single id collapse into one row - one id maps to one id, no matter
  how many ways it was rendered. Where that happens the ordinally first `DisplayName` is kept, so
  which one survives does not depend on which parallel worker reported it first.

## The important part: not every id is computed by the platform

`Id` is deliberately reported separately from the two computed candidates, because an id is not
always computed by the platform at all. An adapter may assign one itself, and **MSTest v3 and v4 do
exactly that** through their own `TestIdGenerationStrategy`; those ids never go through the platform
hashing.

Such an id matches neither candidate. It is reported with `IdSource=SelfAssigned`, and it means:

> **This id will not change when the default algorithm moves. There is nothing to migrate for this
> test.**

A report that showed only `Sha1Id` and `XxHash128Id` would tell every MSTest user their ids are
about to change when in fact none of them are. Read `IdSource` before doing anything with a row:

| `IdSource` | What it means |
| ---------- | ------------- |
| `Sha1` | The carried id equals the SHA1 candidate, so the platform computed it with the legacy algorithm. **This id changes** to `XxHash128Id` when the default moves. |
| `XxHash128` | The carried id equals the xxHash128 candidate - this run already selected it. The id does not change, but anything you stored before opting in is still a SHA1 id, and `Sha1Id` on this row is what it maps from. |
| `SelfAssigned` | The carried id matches neither candidate, so the adapter assigned it. **Nothing changes.** Ignore the two computed columns for this row. |

`IdSource` is inferred by comparing the id a test carries against the two candidates, which is all
the report can do - an adapter does not say where it got an id from. An adapter that assigns an id
which happens to equal one of the candidates is therefore reported as though the platform had
computed it. In practice adapters either let the platform compute the id or assign an unrelated one,
so this is a caveat rather than a live hazard, but it is why the column is named for where the id
matches rather than asserting where it came from.

## Worked example

Running a suite that mixes an adapter using platform computed ids with an MSTest project:

```shell
vstest.console.exe Sample.Tests.dll /logger:testids
```

produces `TestResults\TestIds_net8.0.csv` - the name of the *first* run into that directory; a later
run claims `TestIds_net8.0(1).csv` and prints that instead:

```csv
Source,ExecutorUri,FullyQualifiedName,DisplayName,Id,Sha1Id,XxHash128Id,IdSource
c:\src\Sample.Tests.dll,executor://sample/v1,Sample.Tests.Calculator.Adds,Adds,4ea1b0b6-0b17-3b06-a1c1-6a0a8ad0b6fd,4ea1b0b6-0b17-3b06-a1c1-6a0a8ad0b6fd,1f0a5b7c-1d22-8f31-9c44-2b7e6c0d1a55,Sha1
c:\src\Sample.Tests.dll,executor://sample/v1,Sample.Tests.Calculator.Divides,"Divides (1,0)",b21f7c30-9d51-3a0e-88b2-71c5a3e0f912,b21f7c30-9d51-3a0e-88b2-71c5a3e0f912,17d3e881-4a60-8b19-a2f3-55e9d7c14b08,Sha1
c:\src\Sample.Tests.dll,executor://mstestadapter/v4,Sample.Tests.MSTestSuite.Works,Works,11111111-2222-3333-4444-555555555555,d4c2a919-77b3-3e2c-9d81-0f3a5b6c7d8e,139e1c22-8a70-8d45-b6c1-9e2d4a5f8b03,SelfAssigned
```

Three things to read off this:

- Row 1 and 2 are platform computed. Their ids move from `Id` to `XxHash128Id`.
- Row 2 shows the quoting: the display name `Divides (1,0)` contains a comma, so it is quoted.
- Row 3 is MSTest. `Id` matches neither computed column, `IdSource` says `SelfAssigned`, and this id
  is not going anywhere.

## What the report covers

The report contains the tests **this invocation reported**, and nothing else. That is not the same
as every test you have ids for:

- A run narrowed by `/TestCaseFilter`, `/Tests` or a subset of sources reports only the tests it
  selected.
- A run that was aborted or cancelled reports only what it reached, and says so.
- A test that never produces a result - because the run stopped first - is not in the report.

So produce the report from an **unfiltered** invocation over **all** your test containers, and prefer
`/ListTests`, which reaches every test without executing anything. Otherwise a row missing from the
report says nothing about whether the test still exists.

### Tests run from a package

For a package based run - UWP, where the source given on the command line is an `.appx` or
`.appxrecipe` rather than the assembly - the platform rewrites `TestCase.Source` to the package
*after* the id was computed from the assembly. The report recomputes both candidates from what the
test case now carries, so neither matches, and such rows are reported as `SelfAssigned` even though
the platform did compute the id. Produce the mapping for those tests from a discovery over the test
assemblies themselves.

## Building an old to new mapping

Every row that the platform computed carries both candidates, so the mapping is `Sha1Id` to
`XxHash128Id` regardless of which algorithm the reporting run happened to use.

Load **the report the run actually printed**. Under the default name a rerun into the same results
directory writes `TestIds_net8.0(1).csv` and leaves the older file in place, so reading the plain name
out of habit builds the mapping from a stale run. Either pass `LogFileName` so the path is yours and
fixed:

```shell
vstest.console.exe Tests.dll /logger:"testids;LogFileName=ids\mapping.csv"
```

or take the most recent matching report:

```powershell
$report = Get-ChildItem TestResults\TestIds_net8.0*.csv | Sort-Object LastWriteTime | Select-Object -Last 1
```

Then select the rows to rewrite:

```sql
-- Self assigned ids are not computed from the seed and never move, so they are the only rows
-- excluded. Rows already carrying the new id still map, because the ids you stored are the old ones.
SELECT Sha1Id AS OldId, XxHash128Id AS NewId, FullyQualifiedName, DisplayName, Source
FROM TestIds
WHERE IdSource <> 'SelfAssigned';
```

Then join that against your own records on `OldId` and rewrite them to `NewId`. Rows with
`IdSource = 'SelfAssigned'` should be left exactly as they are.

Do not filter on `IdSource = 'Sha1'` and map from `Id`. That works only when the reporting run used
SHA1: run the report with `VSTEST_DISABLE_XXHASH128_TESTCASE_ID=0` set and every computed row comes
back as `XxHash128`, so such a query returns nothing at all even though every one of those rows still
holds the mapping you need.

The same thing in PowerShell:

```powershell
Import-Csv $report |
    Where-Object IdSource -ne 'SelfAssigned' |
    Select-Object @{ n = 'OldId'; e = { $_.Sha1Id } }, @{ n = 'NewId'; e = { $_.XxHash128Id } }, FullyQualifiedName |
    Export-Csv mapping.csv -NoTypeInformation
```

If a test in your records does not appear in the report at all, check first that the report covers
what you think it does - see [What the report covers](#what-the-report-covers). For an unfiltered
report from a run that completed, a missing test was not discovered, the mapping cannot be produced
for a test that no longer exists, and such records need deciding on separately.

## Related

- [Environment variables](environment-variables.md) - `VSTEST_DISABLE_XXHASH128_TESTCASE_ID`
- [Reporting test results](report.md) - test loggers in general
