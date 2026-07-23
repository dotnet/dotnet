---
name: nullable-enablement
description: Enable C# nullable reference types on files that still have `#nullable disable` in the NuGet.Client codebase. Use this skill whenever the user asks to enable nullable, migrate nullable, remove `#nullable disable`, annotate nullability, or fix nullable warnings for any NuGet project or file. Also trigger when the user mentions a GitHub issue about nullable enablement, references PublicAPI.Shipped.txt annotation updates, or says things like "let's do nullable on X" or "enable nullable for these files."
---

# Nullable Enablement Skill

This skill guides the process of enabling C# nullable reference types on files in the NuGet.Client repository that currently opt out with `#nullable disable`. The codebase has nullable globally enabled via `build/common.project.props`, so individual files opt *out* with `#nullable disable` at the top. Migration means removing that directive, annotating types correctly, and updating the public API surface files.

The work is inherently incremental — don't try to migrate an entire project at once. Work in batches of related files, fix cascading warnings, build, and repeat.

## Coding Guidelines for Nullable Migration

Nullable migration is a rare chance to modernize internal types. Beyond just annotating, apply the immutability and non-null design principles from [`docs/coding-guidelines.md`](../../coding-guidelines.md#prefer-immutability-and-non-null-types) — prefer `required init` over `set`, get-only collections, and `readonly` fields. The migration touchpoint is the natural moment to tighten up these types.

### Fix Stale Contracts, Don't Paper Over Them

During nullable annotation you'll sometimes discover that a method's actual behavior doesn't match its documented contract — e.g., a method documented as "returns null if not found" that actually returns an empty object, or dead null checks in callers. Don't just annotate what the code does today. If the internal method's contract is wrong, fix it:

- If a method is documented to return null but never does, decide whether to make it return null (honest contract) or update the docs (honest documentation).
- Prefer the semantically correct option — returning null for "not found" is usually cleaner than returning an empty shell object with uninitialized properties.
- Since internal methods have no public API surface risk, you can change their contracts freely.

## Annotation Philosophy: Prefer Non-Null

The default mindset when annotating is **non-null unless proven otherwise**. Before marking something `?`, ask: "Can I make this non-null instead?" Sometimes a small, non-breaking code change can eliminate nullability — and that's preferable to marking something nullable. But be careful not to replace null problems with empty-value problems.

**Good non-null opportunities:**
- **Use `required`** on internal/private types so the constructor enforces initialization. The `required` keyword works on net472 via the shared polyfill `RequiredModifierAttributes.cs` (and `IsExternalInit.cs` for `init`). If the target project doesn't include them, add `<Compile Include="$(SharedDirectory)\RequiredModifierAttributes.cs" />` and `<Compile Include="$(SharedDirectory)\IsExternalInit.cs" />` to the csproj under the `<ItemGroup Label="NuGet Shared">` section. Check `NuGet.Packaging.csproj` as a reference.
- **Set a property in the constructor** instead of relying on callers to set it later.
- **Use `??` or `?? throw`** at assignment sites to guarantee non-null storage.
- **Initialize a field** to a sensible default — but only when the consuming code already handles that default gracefully. Don't initialize to `string.Empty` or `Array.Empty<T>()` if downstream code would silently misbehave with an empty value instead of correctly handling null. Empty should not become the new null.
- **Use `null!` initializer** when `required` can't work (e.g., the property is set by external code after construction, not in an initializer). Add a comment explaining why.

**When `?` is the right answer:**
- The value is genuinely optional — configuration, cache misses, "not found" semantics.
- The only non-null alternative is a sentinel value (empty string, empty collection) that downstream code doesn't expect and would silently mishandle.
- The type represents something that can legitimately be absent at runtime.

The goal is to shrink the nullable surface area, but honestly — not by hiding nullability behind empty values that shift the bug downstream.

## High-Level Workflow

1. **Identify target files** — find files with `#nullable disable` in the target project.
2. **Pick a batch** — group related files (e.g., a class and its immediate dependencies). Small batches (1–5 files) are easier to review.
3. **For each file:**
   a. Remove `#nullable disable`
   b. Annotate types (parameters, return types, properties, fields)
   c. Update `PublicAPI.Shipped.txt` for both TFMs
   d. Fix cascading warnings in dependent files
4. **Build** to verify zero warnings/errors.
5. **Repeat** with the next batch.

## Step-by-Step: Migrating a Single File

### 1. Remove the directive

Delete the `#nullable disable` line (and any blank line it leaves behind).

### 2. Annotate types

Work through every public and internal member with a **non-null bias** — look for opportunities to keep or make things non-null before reaching for `?`:

- **Parameters** → default to non-null. Only mark `?` when the parameter genuinely accepts null by design. If a parameter was oblivious and callers never pass null in practice, annotate it as non-null and add a null check.
- **Properties/fields** → prefer non-null. Can you initialize in the constructor, use a default value, or use `required`? Do that instead of marking `?`.
- **Return types** → prefer non-null. Can the method return an empty collection, `string.Empty`, or a sentinel instead of null? If so, make the return non-null.
- **Only mark `?` when the value is genuinely, unavoidably absent** — optional configuration, cache misses, "not found" semantics, etc.
- **`IEnumerable<T>` and other generics** → annotate the element type too: `IEnumerable<PackageIdentity!>!` in the API file means both the collection and its elements are non-null

### 3. Risk Assessment for Null Checks

When a parameter is annotated as non-null, the **default** is to add a runtime `ArgumentNullException` check if one doesn't already exist. Most of the time, this is the right thing to do:

**Add the check (the common case):**
- The parameter already had a null check — keep it
- The parameter is new, internal, or has no history of null being passed
- The parameter is used in a way that would throw `NullReferenceException` anyway — `ArgumentNullException` is more informative and fail-fast

**`NULL_INC` — rare, last-resort escape hatch:**

`NULL_INC` exists for the narrow case where a shipped public API parameter is practically never null, but you can't be 100% certain no caller passes null, and adding a throw would be a risky behavioral change. This should be **very few** instances across the entire codebase — if you're reaching for `NULL_INC` more than once or twice per PR, you're probably using it too liberally.

The criteria are strict — ALL of these must be true:
- The parameter is on a **shipped public API** (not internal)
- The parameter **never** had a null check
- The value is **practically never null** in real-world usage (it's not an "optional" parameter — callers almost certainly always provide a value)
- But you lack telemetry to be **absolutely certain** no caller passes null

Place the `NULL_INC` as an XML `<remarks>` on the relevant property or as a comment on the constructor, using this format:
```xml
/// <remarks>
/// NULL_INC: Annotated as non-null but no runtime check is enforced in the constructor
/// to avoid introducing a new throw in a previously-permissive code path.
/// Revisit with telemetry to confirm callers never pass null.
/// </remarks>
```

If in doubt between adding a null check and using `NULL_INC`, **add the null check**. The bar for `NULL_INC` is intentionally high.

**Never suppress nullability with `!` (the forgiveness operator) when the value genuinely can be null.** Make the type honest and let callers handle it.

### 4. Common Annotation Patterns

**Constructor-initialized properties:**
```csharp
// Non-null — set in constructor, guaranteed by null check
public PackageIdentity Identity { get; }

// Nullable — no guarantee
public string? Description { get; set; }
```

**`required` keyword for internal types:**
```csharp
// Prefer `required` over `null!` initializer for internal/private types
internal class Options
{
    public required string Path { get; init; }
}
```

**TryCreate / TryGet patterns:**
```csharp
public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
```
- Out parameters need `?`
- Add `[NotNullWhen(true)]` only when it's actually guaranteed non-null on success for ALL code paths
- Callers use `!` after the success guard

**`Debug.Assert(x != null)` + `x!`:**
When the parameter type is non-null and all callers are nullable-enabled, you can remove both the assert and the `!` — they're redundant.

**Covariant return nullability:**
A subclass method returning `byte[]` can override a base returning `byte[]?` — this is valid in C# 9+. Use it when the subclass guarantees non-null.

**IEquatable and Equals:**
```csharp
public bool Equals(MyType? other)  // parameter is nullable
public override bool Equals(object? obj)  // always nullable
```

## PublicAPI.Shipped.txt Updates

This is the most precision-sensitive part of the migration. Every public type and member has an entry in `PublicAPI.Shipped.txt`. Each project has **two** TFM directories (e.g., `net8.0/` and `net472/`). Both must be updated identically.

### The `~` (oblivious) prefix

Lines starting with `~` represent nullable-oblivious signatures. When you annotate a type, **replace the `~` line in place** with the correctly annotated line. Do NOT add to `PublicAPI.Unshipped.txt`.

### Annotation syntax

| C# type | PublicAPI notation |
|---|---|
| `string` (non-null) | `string!` |
| `string?` | `string?` |
| `byte[]` (non-null) | `byte[]!` |
| `byte[]?` | `byte[]?` |
| `string[]` (non-null array of non-null strings) | `string![]!` |
| `IEnumerable<PackageIdentity>` (non-null) | `System.Collections.Generic.IEnumerable<...PackageIdentity!>!` |
| `Task<bool>` (non-null) | `System.Threading.Tasks.Task<bool>!` |
| Unconstrained generic `T` | `T` (NO `!` — generics don't get annotated) |
| `Func<T>` (non-null) | `Func<T>!` (the `Func` gets `!`, not `T`) |

### Rules

- **Match existing format precisely** — look at other annotated entries in the same file for guidance.
- **Internal types don't need Shipped.txt updates** — only public API surfaces.
- **Never add to Unshipped.txt** for nullable annotation changes — always replace in-place in Shipped.txt.
- **Both TFMs must match** — update `net8.0/PublicAPI.Shipped.txt` and `net472/PublicAPI.Shipped.txt` (or whatever TFMs the project targets) identically.

### Example transformation

Before (oblivious):
```
~NuGet.Protocol.Core.Types.PackageDownloadContext.PackageDownloadContext(NuGet.Protocol.Core.Types.SourceCacheContext sourceCacheContext, string directDownloadDirectory, bool directDownload) -> void
```

After (annotated):
```
NuGet.Protocol.Core.Types.PackageDownloadContext.PackageDownloadContext(NuGet.Protocol.Core.Types.SourceCacheContext! sourceCacheContext, string? directDownloadDirectory, bool directDownload) -> void
```

## Building and Verifying

After each batch of changes, build the affected project to ensure zero warnings/errors:

```
dotnet msbuild src\NuGet.Core\<ProjectName>\<ProjectName>.csproj -v:q
```

Also build the test projects to catch cascading warnings:

```
dotnet msbuild test\NuGet.Core.Tests\<ProjectName>.Tests\<ProjectName>.Tests.csproj -v:q
dotnet msbuild test\NuGet.Core.FuncTests\<ProjectName>.FuncTest\<ProjectName>.FuncTest.csproj -v:q
```

`dotnet msbuild` builds all TFMs without needing a restore step, unlike `dotnet build` which may fail on some TFMs with NETSDK1005 (missing restore for net8.0). Use `dotnet msbuild` as the default build command.

Common build errors after nullable migration:

| Error | Fix |
|---|---|
| **RS0016** "Symbol not in PublicAPI" | You added a new annotated entry but didn't remove the `~` line — replace, don't add |
| **RS0017** "Symbol removed" | You removed the `~` line but the replacement doesn't match — check annotation syntax |
| **CS8600–CS8605** nullable warnings | Annotate the type correctly, or add a null check |
| **CS8618** non-nullable field not initialized | Use `required`, add a constructor init, or make the field nullable |

## XLF Files

**Never edit `.xlf` files directly.** They are generated from `.resx` files. If you need to change localized strings, edit the `.resx` file and build — the xlf files will be regenerated automatically.

## Checklist Before Submitting

- [ ] All `#nullable disable` directives removed from target files
- [ ] Types annotated correctly with a non-null bias (prefer non-null, only `?` when genuinely absent)
- [ ] `NULL_INC` remarks added only for the rare cases meeting all criteria (shipped API, practically never null, no telemetry)
- [ ] `PublicAPI.Shipped.txt` updated for ALL TFMs (`~` lines replaced in place)
- [ ] No entries added to `PublicAPI.Unshipped.txt`
- [ ] Project builds with zero warnings/errors
- [ ] No `.xlf` files edited manually
