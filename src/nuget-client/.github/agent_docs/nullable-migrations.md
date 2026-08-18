# Nullable migrations

Follow these rules when migrating existing code to nullable reference types:

- Match existing base-class entries when updating `PublicAPI.Shipped.txt`. Array annotations must be precise, for example `string![]!` rather than `string![]`, and `byte[]?` rather than `byte?[]`.
- Replace a `~`-prefixed oblivious entry in place with its annotated form in `PublicAPI.Shipped.txt`; do not add it to `PublicAPI.Unshipped.txt`.
- Update shipped API files only for public API surfaces. Internal types do not require entries.
- Do not use `!` when a value can genuinely be null. Make the type nullable and require callers to handle it.
- A `byte[]` override of a `byte[]?` base return is valid when the override guarantees a non-null result.
- Remove both `Debug.Assert(x != null)` and `x!` when the parameter is non-nullable and every caller is nullable-enabled.
- Prefer `required` members on private and internal types over `null!` field initializers.
- For Try-pattern methods, use nullable out parameters and annotate them with `[NotNullWhen(true)]` when every successful path returns a non-null value. Use `!` at the caller only when the called API cannot be annotated and its successful result is known to be non-null.
- Migrate related files in batches: update the source, fix cascading warnings, and build before moving to the next batch. Use multiple pull requests when necessary.

For public API policy and analyzer-file conventions, see the [NuGet SDK guidance](../../docs/nuget-sdk.md).
