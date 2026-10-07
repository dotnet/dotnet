# Microsoft.TestPlatform.Hashing.Source

Shared source (not a NuGet package, not a separate assembly). The files here are compiled
directly into `Microsoft.TestPlatform.ObjectModel` and `Microsoft.TestPlatform.AdapterUtilities`
as `internal` types, following the same pattern as `src/Microsoft.TestPlatform.Filter.Source`.
`Microsoft.TestPlatform.CrossPlatEngine`, which has to reproduce the id of a test case from the
runner process on the Microsoft.Testing.Platform path, does **not** compile these files: it uses
ObjectModel's copy through the `InternalsVisibleTo` in
`src/Microsoft.TestPlatform.ObjectModel/Friends.cs`, so there is exactly one `TestIdSeed` and one
`TestCaseIdAlgorithm` on that path. The temporary test id report logger
(`src/Microsoft.TestPlatform.Extensions.TestIdsLogger`) takes the same route for `TestIdSeed` alone -
it reports what each algorithm computes rather than resolving one, so it needs the seed and not
`TestCaseIdAlgorithm`. That grant is removed together with the logger, which goes when the SHA1
implementation it reports on does.

Consumers pick files deliberately rather than taking everything: `AdapterUtilities` must exclude
`TestCaseIdAlgorithm.cs`, because that file reads the CoreUtilities feature flag and `AdapterUtilities`
does not reference CoreUtilities, so compiling it in is an error rather than a silent duplicate. It is
not needed there either - `AdapterUtilities` does not read the flag. Check the `Compile` items when
adding a file, since the two projects that use a `*.cs` glob will otherwise pick it up silently.

## What is here

| File | Origin |
|---|---|
| `XxHash128.cs` | `dotnet/runtime` — `src/libraries/System.IO.Hashing/src/System/IO/Hashing/XxHash128.cs` |
| `XxHashShared.cs` | `dotnet/runtime` — `src/libraries/System.IO.Hashing/src/System/IO/Hashing/XxHashShared.cs` |
| `BitOperations.cs` | polyfill of `System.Numerics.BitOperations` for target frameworks that lack it |
| `TestIdGuid.cs` | vstest-authored — turns a 128-bit hash into an RFC 9562 version 8 UUID |
| `TestIdSeed.cs` | vstest-authored — composes the string a test case id is hashed from |
| `TestCaseIdAlgorithm.cs` | vstest-authored — resolves which algorithm computes a test case id |

`XxHash128.cs` and `XxHashShared.cs` were vendored via [microsoft/testfx][testfx-hashing],
which vendors them from `dotnet/runtime`. Both upstreams are MIT licensed.

## Why vendored instead of `PackageReference Include="System.IO.Hashing"`

`Microsoft.TestPlatform.ObjectModel` and `Microsoft.TestPlatform.AdapterUtilities` ship
`netstandard2.0` and `net462` assets that are loaded by hosts which do **not** have binding
redirects — the Distributed Test Agent (DTA) being the canonical example. Taking
`System.IO.Hashing` would:

1. add a brand new assembly identity (`System.IO.Hashing`) that needs a binding redirect in
   `src/vstest.console/app.config`, `src/testhost.x86/app.config` **and**
   `src/datacollector/app.config` — miss one and net462 hosts get a `FileLoadException`;
2. add a new DLL to every shipped nupkg, changing `eng/expected-nupkg-file-counts.json` and
   `eng/expected-dll-frameworks.json` and landing a new file next to every test adapter;
3. add a new public package dependency to `Microsoft.TestPlatform.ObjectModel`, which is
   consumed extremely widely.

Vendoring avoids all three. It only makes `System.Memory` and
`System.Runtime.CompilerServices.Unsafe` explicit on the .NET Framework / netstandard2.0 legs —
both of which `ObjectModel` already pulled in transitively via `System.Reflection.Metadata`, and
both of which **already** have binding redirects in all three app.configs.

microsoft/testfx reached the same conclusion for the same reason and vendors these exact files.

## Keeping in sync

Keep the vendored files as close to upstream as possible so future syncs stay cheap. The
adaptations applied are listed in a header comment at the top of each file; there are only two
per file (explicit `using` directives, because vstest does not enable `ImplicitUsings`, and a
namespace change). Do not reformat these files and do not "fix" analyzer complaints in them —
they are excluded from repo style enforcement via `.editorconfig`.

That exclusion is scoped to the vendored files **by name**, so that the vstest-authored files in
this folder stay under normal repo style enforcement. When vendoring another file, add it to the
pattern in `.editorconfig`, otherwise it is silently held to repo style and the next sync fights it.

## Consequence of being shared source

Because these are compiled into each consuming assembly rather than referenced from one, a type
here exists once **per assembly**. `ObjectModel` and `AdapterUtilities` therefore each have their
own `TestIdGuid`, `XxHash128` and `TestIdSeed`, and those are different types despite the identical
source. Two assemblies compiling the same file do not share the type, and an assembly that can see
the internals of both (a test project, via `InternalsVisibleTo`) cannot name it without `CS0433`.

Prefer giving a new consumer access to an existing copy over compiling another one. Adding a
consumer is cheap for types that only appear inside a method body, and awkward for types that
appear in a signature a test needs to name — `CrossPlatEngine` takes the `InternalsVisibleTo`
route for exactly that reason.

[testfx-hashing]: https://github.com/microsoft/testfx/tree/main/src/Platform/Microsoft.Testing.Extensions.TrxReport/Hashing
