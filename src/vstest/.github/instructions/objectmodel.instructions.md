---
applyTo: "src/Microsoft.TestPlatform.ObjectModel/**"
---

# ObjectModel — Public API Surface Rules

This assembly is shipped via NuGet and consumed by all test adapters. Every public change is a permanent commitment.

## Public API Protection

- New public types/members must be intentional, minimal, and declared in `PublicAPI.Unshipped.txt`.
- Never change existing public API signatures without an `[Obsolete]` migration path.
- Do not leak serializer-specific attributes or internal dependencies into public types.
- Prefer new interfaces over adding methods to existing interfaces (external implementers exist).

## Binary Compatibility

- API changes must not break callers built against older ObjectModel versions.
- When extending callbacks with framework-specific data, use expandable versioned contracts.
- Bulk analyzer cleanups must exclude refactorings that alter public contracts or binary compatibility.

## Cross-TFM Considerations

- Ensure APIs work on all targeted TFMs (net462, net8.0+).
- Use `#if` guards or polyfills for APIs unavailable on older frameworks.
- netstandard2.0 dependencies must not pull in assemblies absent from net462 hosts.

## Key Checks

- Run `dotnet build` and verify no PublicAPI analyzer warnings before submitting.
- Any signature change here affects the entire test adapter ecosystem — get explicit approval.
- Validate nullable annotations are correct; this is a boundary assembly.
