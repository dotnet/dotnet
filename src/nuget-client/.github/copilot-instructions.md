# Instructions

## General Guidelines

- When creating pull requests, always follow the [PR template](PULL_REQUEST_TEMPLATE.md).
- Always format before submitting a pull request.

## Coding Standards

- Use the following coding guidelines: https://github.com/NuGet/NuGet.Client/blob/dev/docs/coding-guidelines.md
- Never use reflection.
- When using value tuples, never use `var` (e.g., `var result = Method()`), but always use decomposed names (e.g., `(var name, var value) = Method()`).

## Project-Specific Rules

- All files in the repository are nullable by default (project-level nullable enable). No need to add `#nullable enable` directives to individual files.

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
