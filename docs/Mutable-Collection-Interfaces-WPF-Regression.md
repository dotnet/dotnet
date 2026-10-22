# Mutable collection interfaces: downstream regressions

This note accompanies the VMR migration that restores the changes reverted by
[dotnet/runtime#101644](https://github.com/dotnet/runtime/pull/101644).
It records the downstream regressions and compatibility patches for review
when a pull request is opened.

## Reproduction

The original VMR base is `7ac81c6a87ff9c9889e581c189f250923ba59a59`.
Its runtime sources are tracked at
`1093e4f974d796ef4f2776190319cc9de6266403`.
With the inverse of runtime revert commit
`b8fe1d0ccb60926d6e7ef5d42e2398c178c9ee5c` applied under `src/runtime`,
the Windows x64 Release VMR build reaches WPF and fails:

```powershell
.\build.cmd -bl -ci -buildRepoTests
```

The affected project is
`src/wpf/src/Microsoft.DotNet.Wpf/src/DirectWriteForwarder/DirectWriteForwarder.vcxproj`.
The first diagnostics are in
`CPP/DWriteWrapper/LocalizedStrings.h`, originally at lines 156 and 187.

| Diagnostic | Meaning |
| --- | --- |
| C2392 | Managed covariant return types are not supported for the `Keys` and `Values` getters. |
| C3766 | `LocalizedStrings` is missing the inherited `IReadOnlyDictionary.Keys` and `Values` implementations. |
| C3612 | The sealed class cannot contain unresolved abstract interface members. |
| C2259 | Callers cannot instantiate the resulting abstract class. |

The final build reported nine compiler errors, including cascading errors at
construction sites in `Font.cpp` and `FontFamily.cpp`.

## Cause and compatibility patch

`LocalizedStrings` is a C++/CLI class implementing
`IDictionary<CultureInfo, string>`. The migrated interface now also inherits
`IReadOnlyDictionary<CultureInfo, string>`.

The two dictionary interfaces expose properties with the same names but
different return types: `ICollection<T>` on the mutable interface and
`IEnumerable<T>` on the read-only interface. C++/CLI attempts to match the
existing virtual getters to the inherited read-only getters and rejects their
return types. The default interface forwarding bodies added in runtime do not
make this source implementation compile unchanged.

The WPF patch explicitly maps the existing `Keys` and `Values` getters to their
`IDictionary` slots. Separate private property getters explicitly implement the
`IReadOnlyDictionary` slots and forward to the existing getters. Both views use
the same lazily initialized arrays; enumeration, caching, and unsupported
mutation behavior are unchanged.

The `PresentationCore.Tests` regression test constructs the empty
`LocalizedStrings` instance and checks both dictionary interfaces, read-only
count and enumeration, and identity of the key and value views.

## EF Core: dictionary and list indexer reflection ambiguity

Further testing against the modified runtime exposed a second downstream
regression in
`src/efcore/src/EFCore/ChangeTracking/Internal/ShadowValuesFactoryFactory.cs`,
at line 73. Its static initializer locates a dictionary indexer with:

```csharp
typeof(IDictionary<string, object?>)
    .GetRuntimeProperties()
    .Single(p => p.GetIndexParameters().Length > 0);
```

The migrated interface declares both the original `Item` indexer and the
explicit default implementation of `IReadOnlyDictionary<TKey, TValue>.Item`.
Reflection returns both properties, and both satisfy the predicate above.
`Single` therefore throws `InvalidOperationException` with the message
"Sequence contains more than one matching element." This surfaces as a
`TypeInitializationException` for `ShadowValuesFactoryFactory` and cascades
through EF Core query materialization and change-tracking tests.

A minimal reflection probe against the modified runtime reported:

```text
System.Collections.Generic.IDictionary`2[System.String,System.Object]: Item
System.Collections.Generic.IDictionary`2[System.String,System.Object]: System.Collections.Generic.IReadOnlyDictionary<TKey,TValue>.Item
IndexedProperties=2
```

The same issue occurs in
`src/efcore/src/EFCore/Metadata/Internal/ClrIndexedCollectionAccessorFactory.cs`,
originally at line 207. It selects the only indexed property on `IList<T>`, but
the restored interface also declares the explicit `IReadOnlyList<T>.Item`
implementation. Complex collection accessor creation therefore fails before
its indexed getter and setter expressions can be compiled.

The EF Core run produced XML results for 133,424 cases: 96,421 passed,
36,066 failed, and 937 skipped. Representative affected suites include
CrossStore, InMemory, SQLite, SQL Server, and core change-tracking tests.
Case-by-case attribution found 33,384 records with the dictionary initializer
failure and 2,556 with the list accessor failure. A further 74 original failures
had change-dependent outcomes in controlled runs, including secondary duplicate
key and lazy-loading exceptions. Isolating representative affected methods
exposed the dictionary initializer failure underneath these cascades.
Not every EF Core failure belongs to these causes: 26 design-test failures
required an SDK that was absent from the runtime-only testhost, and another
26 original failures were not reproduced in the controlled comparisons.

The initial installed-bootstrap comparison was superseded by a Windows x64
Release runtime built from the exact unmodified VMR base above. Both dictionary
and list selector probes find one public `Item` indexer on that control and two
indexed declarations on the modified runtime. Representative existing EF tests
pass on the exact control and fail with the original reflection exceptions on
the modified runtime. For these comparisons, unchanged EF consumers were built
against the original reference pack and the same binaries were used with both
runtimes; consumers built against the changed reference surface can instead fail
earlier with missing interface members on the old runtime.

**Compatibility patch:** both factories now select the public mutable indexer
with `GetProperty("Item")!`, following the existing indexer-selection pattern in
`PropertyAccessorsFactory`. This excludes the private explicit read-only
forwarders without changing the collection interface relationship or selecting
an arbitrary first match.

The added shadow-snapshot test covers present dictionary values and missing
values with a nondefault sentinel. The existing `IList<T>` accessor test now
checks setters both during normal updates and materialization. All four rows
failed with the original reflection exceptions before the patch and passed
after it.

The post-patch full EF test entrypoint recorded 133,427 cases in 23 result
assemblies: 132,391 passed, 23 failed, and 1,013 skipped. None of the original
36,014 positively attributed failures remains failed: 35,938 pass, including
the accessor case expanded into its two passing theory rows, and 76 reach
their existing skip conditions. Those skips cite existing EF issues,
unsupported inner joins, or foreign-key constraints; no skip was added here.

That run's remaining failures were 22 design tests requiring an SDK under
the runtime-only testhost and one SQL Server execution timeout. Completing the
private test layout subsequently resolved all 22 design prerequisites: the
entire design suite passed all 1,337 cases. The SQL Server HierarchyId suite
also passed all 96 cases in one follow-up run; the original timeout remains
recorded as not reproduced, not as a product bug that was patched.

The four hosted ASP.NET/OData projects now execute with the newly built
`Microsoft.AspNetCore.App` framework and its matching source-built reference
pack. Their complete suites passed 84 InMemory, 80 SQLite, 80 SQL Server, and
17 OData cases, with no failures or skips. The earlier bootstrap ASP.NET
reference assemblies had assembly version 12, whereas this snapshot's built
framework has assembly version 11. Selecting the real matching pack and
rebuilding consumers corrected that test setup without changing consumer code.

The Cosmos functional and native SQLite attempts that report zero cases are
not counted as passing coverage. Historical prerequisite failures and the SQL
timeout are preserved, not suppressed or erased.

## ASP.NET Core: form collection converter selection

The existing form-mapping tests for `ISet<int>`, `IList<int>`, and
`ICollection<int>` returned null instead of the expected `HashSet<int>` or
`List<int>`. The affected selector is
`src/aspnetcore/src/Components/Endpoints/src/FormMapping/Factories/Collections/TypedCollectionConverterFactory.cs`.

Its read-only assignability branches preceded its mutable branches. With the
restored inheritance, a mutable target matched a read-only branch, producing
`FormDataConverter<IReadOnlySet<int>>`, `FormDataConverter<IReadOnlyList<int>>`,
or `FormDataConverter<IReadOnlyCollection<int>>`. These converter classes are
invariant. `FormDataMapperOptions.ResolveConverter<T>` consequently threw an
`InvalidCastException` when casting to the requested mutable converter.
`FormDataMapper.Map<T>` logged the resolution failure and returned default.

**Compatibility patch:** interface converter creation now matches the requested
interface exactly, consistent with the factory's existing `CanConvert`
whitelist. It no longer depends on the inheritance order of the supported
mutable, read-only, immutable, and enumerable interfaces.

The regression matrix checks the converter's requested generic interface for
all 11 supported interface types. Before the patch, the three mutable converter
rows and three existing form-reader tests failed; the eight other converter
rows passed. All 14 rows pass after the patch, and the complete
`Microsoft.AspNetCore.Components.Endpoints.Tests` suite passes all 891 cases.

## ASP.NET Core: shared interface test expectations

Three shared-helper assertions encoded the old interface hierarchy or property
surface. `ClosedGenericMatcher` now finds `IReadOnlyList<int>` on `IList<int>`.
`PropertyHelper` intentionally enumerates the queried interface and inherited
interfaces, so it now also returns inherited read-only declarations: two
`Count` properties for `ICollection<T>`, and additional `Keys` and `Values`
properties for `IDictionary<TKey, TValue>`.

The tests now expect these results on the new target framework while preserving
the downlevel expectations. The guard includes `NET12_0` as well as
`NET12_0_OR_GREATER`: this snapshot's bootstrap SDK emits the exact unknown-TFM
symbol but only knows compatible-framework symbols through .NET 11. The complete
shared suite passes all 1,334 cases. No production property enumeration is
deduplicated or otherwise changed to hide the new interface surface.

## SDK: newly applicable collection analyzer diagnostics

The restored `IList<T>` relationship makes two `Enumerable.Last()` calls in
`src/sdk/src/TemplateEngine/Microsoft.TemplateEngine.Edge/Settings/TemplateCache.cs`
eligible for `CA1826`. They select the winning template from nonempty identity
groups. Both now use the existing list's `Count - 1` index, preserving the
last-template-wins behavior without suppressing the analyzer.

Explicit count-based indexing also preserves the project's .NET Framework
support; from-end indexing would introduce an unavailable `System.Index`
dependency on that target. The repaired project builds for `net472`, `net10.0`,
and `net12.0` with zero warnings or errors.

The full SDK test build exposed three more `CA1826` diagnostics in
`NuGetApiPackageManagerTests`. Those calls selected the first item from the
test's fixed, nonempty single-source list. They now use `[0]`, retaining both
the source selection and downlevel framework compatibility.

The current-TFM GenAPI test `TestGenericClassImplementsGenericInterface` also
encoded the old hierarchy. Its expected expanded interface list now includes
`IReadOnlyCollection<T>` alongside `ICollection<T>`. The generator itself is
unchanged; the assertion reflects the intended new interface surface.

## Validation status

All builds and tests run in a separate checkout containing the same migration.
Build-generated bootstrap rewrites are not included in the reviewed worktree.

- Before the WPF patch, the modified runtime's full build and packaging passed.
  The full VMR build then failed in WPF with the diagnostics above.
- The full runtime library run completed with 1,077,908 cases:
  1,075,605 passed, 106 failed, and 2,197 skipped. The failures span 14 assemblies
  and none was positively attributed to the migration: 98 reproduce on the
  exact baseline, seven were not reproduced on either runtime, and one DNS
  failure is intermittent on both. No test was changed or suppressed to work
  around them.
- The Collections, Concurrent Collections, Immutable Collections, LINQ,
  Expressions, JSON, System.Runtime, and System.Reflection suites reported zero
  failures, including the added type-relationship cases.
- After the patch, the Windows x64 Release WPF build and packaging completed.
  The complete `PresentationCore.Tests` run used the locally built runtime:
  983 cases, 954 passed, zero failed, and 29 skipped. The new
  `LocalizedStringsTests.EmptyDictionary_ReadOnlyViewsShareKeysAndValues`
  case passed.
- All six WPF unit suites passed: 24,596 total, 24,565 passed, zero failed,
  and 31 skipped.
- The SDK library's three target frameworks and both SDK unit-test target
  frameworks compile with the collection analyzer enabled. The corrected
  isolated SDK runner executes all 27 `TemplateCacheTests` cases successfully.
  Its VMR root/SDK discovery changes are validation-only; no fake Git marker
  or repository-wide tooling change is included in the migration.
- The BOM-only Roslyn package imports were stale generated SourceLink files.
  Regenerating and repacking all six affected source packages produced valid
  imports without modifying Roslyn source. Only affected private package-cache
  entries were refreshed; no global cache or package versions were changed.
- The full Windows x64 Release VMR product build and packaging now succeed.
  Four existing NuGet audit warnings remain visible. A single canonical private
  SDK path avoids the isolated task-host handshake failure caused by the
  validation checkout's SDK-local path alias; validation used the supported
  standard MSBuild mode.
- The root VMR validation/scenario entrypoint now completes with 106 cases:
  99 passed, zero failed, and seven skipped. This comprises 88 scenario cases
  (86 passed, two skipped), 15 shared cases (13 passed, two skipped), and three
  reproducibility cases (all skipped). Repository-standard scenario exclusions
  for MultiTFM, net12 targeting-pack requirements, and non-target-RID assets are
  unchanged. The extracted SDK's CoreLib is byte-identical to the migrated
  runtime's CoreLib, not the bootstrap runtime.
- The complete CoreCLR priority-1 Windows x64 Release build and run now finish
  with 24,771 cases: 24,562 passed, zero failed, and 209 skipped. The isolated
  discovery helper selects the actual configured TFM and the real framework
  targeting pack rather than all library references. Seven initial native
  command-launch failures were resolved by adding their specific built output
  directories to the test process's PATH. Executable-search policy is unchanged;
  no compiler warning, native test, or assertion was disabled.
- SDK MSI/workload installation tests require an elevated Hyper-V VM fixture,
  PsExec, and guest configuration. That coverage remains explicitly blocked with
  the user's approval; no host installation or VM settings were changed.
- The five previously unattributed Kestrel certificate-test failures also
  reproduce with an unchanged ASP.NET Core consumer built against the exact
  original reference pack. Both runtimes execute the same six rows with one
  pass and five identical failures, without the `HeaderDictionary` binding
  errors that obstructed the earlier control. No Kestrel fix is included.
- Other baseline-reproduced failures and the original incomplete MSBuild and
  Roslyn crash runs remain recorded separately. No analyzer suppression,
  disabled regression test, dependency-version change, or unrelated baseline rewrite is
  used by these compatibility patches.

### Expanded SDK coverage

The expanded non-admin Windows SDK campaign dispatched 63 modules. Its initial
58 finalized reports contain 34,035 cases: 29,754 passed, 3,889 failed, and
392 skipped. Four modules initially could not start without the source-built
ASP.NET Core framework; the fifth missing report was the interrupted watch
integration leg. All five now have finalized reports in separate follow-ups.
The original reports and failures remain preserved. These results are not an
all-green SDK campaign.

Several initial failures came from the isolated test setup rather than a
shipping source change. The test process inherited build-only NuGet and
MSBuild-path overrides, and fixture discovery selected a nested
`artifacts/bin/artifacts` directory instead of the prepared test root. The
validation-only runner now removes those overrides only for test subprocesses
and uses the supported execution/artifact-directory settings. Project builds
retain the VMR's real generated feeds and CI version context; a non-CI
fast-path build had otherwise created an incomplete, higher-sorting `-dev`
SDK directory. That partial output was preserved separately, not completed
with fabricated files.

The following complete project follow-ups now pass:

| SDK project | Total | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: | ---: |
| ApiSymbolExtensions | 24 | 24 | 0 | 0 |
| PackageValidation | 46 | 46 | 0 | 0 |
| DotNetDeltaApplier | 148 | 148 | 0 | 0 |
| GenAPI | 109 | 107 | 0 | 2 |
| HotReload.Client (`net12.0`) | 154 | 140 | 0 | 14 |
| Watch.BrowserRefresh | 15 | 15 | 0 | 0 |
| HotReload.Watch.Aspire | 58 | 58 | 0 | 0 |

The two long integration projects were rerun without filters after correcting
the fixture setup. `dotnet-new.IntegrationTests` completed all 895 cases:
661 passed, 214 failed, and 20 skipped. All 214 failures were also present in
the initial run; 159 initially failing cases now pass. The remaining families
are 208 stdout approval mismatches, four MCP template restores requesting
unavailable non-target-RID `12.0.0-ci` runtime packages, one missing local
discovery-tool package, and one project-reference post-action that selects
the VMR's `build.proj` instead of the requested generated project.

All 62 retained approval stdout pairs differ only by an extra blank line
after the restore header. No approval baseline was updated. A full template
comparison using the exact original runtime was obstructed by an SDK consumer
compiled against the changed references: `VariableCollection` fails to load
with a missing `get_Keys` implementation. This is not a faithful baseline
reproduction or proof that the approval failures are collection regressions.
A smaller restore-only control runs successfully on both exact runtimes with
the same SDK/host binaries and emits the same MSBuild banner. The approval
and project-selection failures remain unproven migration findings.

The discovery prerequisite was subsequently repaired by supplying the genuine
built `Microsoft.TemplateSearch.TemplateDiscovery.12.0.100-ci.nupkg` at the
test's existing local-source location. Its focused test now passes. No
package version, installation assertion, or test source was changed.

`dotnet-watch.Tests` completed 578 cases: 559 passed, eight failed, and
11 skipped. Seven timeouts were missing-framework launches: two needed the
existing x64 .NET 6 runtime, four needed .NET 9, and one needed the newly built
Windows Desktop framework. Completing only the private test layout resolved
all seven in a focused follow-up. This reused genuine installed .NET 6 files,
installed .NET 9.0.21 only in the private hive, and used the real source-built
Windows Desktop framework. The current host and migrated CoreLib hashes are
unchanged; no machine-wide installation, roll-forward relaxation, or assertion
change was made. The remaining .NET 6 Blazor case requires the unavailable
`Microsoft.NET.Sdk.WebAssembly.Pack` version `12.0.0-ci`.

The full integration reports and focused repairs are recorded separately,
not rewritten into a fictional all-green full run. An additional corrected-
fixture pass finalized 20 of the 21 remaining originally failing SDK modules,
including Build and Publish: 4,635 cases, 4,297 passed, 168 failed, and
170 skipped. On October 22, 2026, the author requested that SDK validation be
deferred after the prolonged run. The remaining `dotnet.Tests` process was
cancelled; its partial execution is not counted as a completed or passing
suite. Finalized reports and partial logs remain preserved.

SDK validation is incomplete and not all green. Missing non-target-RID/
WebAssembly artifacts, remaining unattributed SDK failures, the interrupted
CLI suite, and the approved elevated-VM installation gap prevent an
all-components-green claim. No additional failure has been positively
attributed to the collection change beyond the repaired analyzer diagnostics
and GenAPI expectation described above. No further SDK test changes or
workarounds are included to conceal the outstanding failures.
