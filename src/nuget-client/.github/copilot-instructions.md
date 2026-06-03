# Instructions

## General Guidelines

- When creating pull requests, always follow the [PR template](PULL_REQUEST_TEMPLATE.md).
- Always format before submitting a pull request.
- Before implementing any code changes, read all files in the `docs/` folder. It contains the NuGet development guidelines, including rules for SDKAnalysisLevel gating, public API policies, error handling patterns, and feature design requirements.
- Do not manually edit `.xlf` files. These files are generated from `.resx` files during build and are managed by the OneLocBuild localization pipeline — any manual edits will be overwritten. When adding or modifying localized strings, edit only the `.resx` file, then build. The build will regenerate the `.Designer.cs` and `.xlf` files. Include all three (`.resx`, `.Designer.cs`, `.xlf`) in your pull request.
- Branches should be named as `dev-<user>-<topic>` (e.g., `dev-nkolev92-fixFlakyTest`).

## Coding Standards

- Use the following coding guidelines: https://github.com/NuGet/NuGet.Client/blob/dev/docs/coding-guidelines.md
- Never use reflection.
- When using value tuples, never use `var` (e.g., `var result = Method()`), but always use decomposed names (e.g., `(var name, var value) = Method()`).

## Project-Specific Rules

- All files in the repository are nullable by default (project-level nullable enable). No need to add `#nullable enable` directives to individual files.

## Benchmarking

When asked to benchmark code or measure performance, use the `NuGet.Benchmarks` project at `test/TestExtensions/NuGet.Benchmarks/`. Create a new `.cs` file in that directory (it is git-ignored) with a class that implements `IBenchmark` and annotates benchmark methods with `[Benchmark]`. No changes to `Program.cs` are needed — it auto-discovers all `IBenchmark` implementations. Run with:

```bash
dotnet run -c Release --project test/TestExtensions/NuGet.Benchmarks/NuGet.Benchmarks.csproj
```

See `test/TestExtensions/NuGet.Benchmarks/README.md` for details and an example.

## Nullable Migration Rules

- **Shipped.txt format must be precise** — e.g. `string![]!` not `string![]`, `byte[]?` not `byte?[]`. Always match the format of existing base class entries in the same file.
- **`~` (oblivious) entries get replaced in place** — replace the `~` prefixed line with the annotated line in `PublicAPI.Shipped.txt`. Do not add to `PublicAPI.Unshipped.txt`.
- **Internal types don't need Shipped.txt updates** — only public API surfaces require `PublicAPI.Shipped.txt` changes.
- **Don't suppress nullability with `!`** when the value genuinely can be null — make the type honest, let callers handle it.
- **Covariant return nullability** — `byte[]` override of `byte[]?` base is valid in C# 9+. Use it when a subclass guarantees non-null.
- **`Debug.Assert(x != null)` + `x!` can be replaced** by removing both when the parameter is non-null typed and all callers are nullable-enabled.
- **`required` on private/internal types** is cleaner than `null!` field initializers.
- **TryCreate/TryGet patterns** — out params need `?`, callers use `!` after the success guard. Out parameters that are guaranteed non-null when the method returns true should be annotated with `[NotNullWhen(true)]`. Don't annotate `[NotNullWhen]` unless it's actually true for all code paths.
- **Work in batches** — group related files, fix source, fix cascading, build, repeat. If this means we need multiple pull requests for enabling nullable, that's fine. Don't try to do it all in one go.


