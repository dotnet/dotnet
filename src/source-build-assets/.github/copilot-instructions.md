# .NET Source Build Reference Packages - Copilot Instructions

This file contains AI-specific guidance for working with this repository.
All general documentation is maintained in the [README.md](../README.md) to avoid duplication.

## Critical Safety Rules

### Absolute Prohibitions

- **NEVER** modify files inside `src/externalPackages/src/` (these are git submodules)
- **NEVER** delete or recreate `Customizations.props` or `Customizations.cs` files
- **NEVER** suggest adding preview/RC packages
- **NEVER** ignore build failures in `./build.sh -sb`
- **NEVER** hand-author `.nuspec` files for reference or text-only packages — they are
  no longer present in the source tree. Per-package metadata (description, license,
  projectUrl, releaseNotes, tags, etc.) lives in the generated `.csproj` and is
  produced from the source nuspec by the package source generator. Centralized
  fields (Authors, Copyright, Serviceable, etc.) live in
  `src/referencePackages/Directory.Build.props` and
  `src/textOnlyPackages/Directory.Build.props`.

### Before Making Changes

- **ALWAYS** check if a package has existing customizations before regenerating
- **ALWAYS** validate commands work on Linux (primary platform for source-build)
- **ALWAYS** preserve intentional code comments explaining manual fixes

## Branch Awareness

The instructions in this file and the README apply to the `main` branch.
**Servicing branches (e.g. `release/9.0`) may have different tools, scripts, and
workflows for generating and adding packages.** When working on a servicing branch,
always read the README and copilot instructions from that branch — do not assume
the `main` branch instructions are correct.

## Workflow Patterns

### When User Asks to "Add a Package"

1. Confirm which branch the work targets — if it is a servicing branch, read that
   branch's README for the correct process before proceeding
2. Determine package type first: External, Reference, Targeting, or Text-only
3. For Reference packages: Use `./generate.sh --package name,version`
4. For External packages: Requires submodule + project creation + patches
5. Check README for detailed workflows

### When Regenerating Packages

```bash
# Check for existing customizations first
find src/referencePackages/src/package.name/version -name "Customizations.*"
```

- If customizations exist, preserve them during regeneration
- The generate script will preserve them

### When Build Fails

1. Try building individual package: `./build.sh -sb --projects /full/path/to/package.csproj`
2. Search for [known GenAPI issues](https://github.com/dotnet/sdk/issues?q=is%3Aissue+label%3AArea-GenAPI) and how to workaround them
3. Check for compilation errors that need `Customizations.props` (NoWarn entries)
4. Look for API issues that need `Customizations.cs` (partial class additions)
5. Always add explanatory comments for manual fixes needed in the generated source files outside of Customizations files

## Decision Support

### Customizations.props vs Customizations.cs

- **Customizations.props**: MSBuild properties, NoWarn suppressions, additional source files
- **Customizations.cs**: Additional types, members for partial classes, conditional compilation

### External Package Changes

- Always create patches via `extract-patches.sh` - never suggest direct file edits
- Patches live in `src/externalPackages/patches/<component>/`
- Test patches with `git am` before suggesting
- Follow guidelines from [README.md](../README.md#patches) for patch creation and maintenance

## API Compatibility Validation

Package validation with baseline comparison is enabled for reference packages and targeting
packs. This ensures the public API surface of source-built packages matches the official
NuGet releases.

### When API Compat Fails After Adding/Regenerating a Package

If `./build.sh -sb` produces API compatibility errors (CP0001, CP0002, CP0008, CP0021, etc.):

1. **Determine if the difference is a real API break or a generator limitation.**
   - A real break means the generated reference assembly is missing public API that
     consumers depend on — fix the generated code or add a `Customizations.cs`.
   - A generator limitation means GenAPI/the package source generator cannot perfectly
     reproduce the original assembly metadata (e.g., missing `notnull` constraints,
     missing `[Serializable]` attributes).

2. **For generator limitations, check for an existing tracking issue:**
   - Search [dotnet/sdk issues with `Area-GenAPI` label](https://github.com/dotnet/sdk/labels/Area-GenAPI)
   - If no issue exists, **file one** in dotnet/sdk with the `Area-GenAPI` label describing
     the gap (include the diagnostic ID, affected type/member, and what the generator
     should emit but doesn't).

3. **Add a `CompatibilitySuppressions.xml` file** in the package version directory:
   - Run the build with `/p:GenerateCompatibilitySuppressionFile=true` to auto-generate it,
     or create it manually.
   - Include a code comment or commit message referencing the tracking issue.
   - Only suppress diagnostics that are confirmed generator limitations — never suppress
     to hide a real API gap.

4. **Commit the suppression file** — it is a required part of the package source.

### Known Limitations (common suppressions)

| Diagnostic | Description | Tracking |
|-----------|-------------|----------|
| CP0008 | Missing internal interfaces from friend assemblies | https://github.com/dotnet/sdk/issues/54451 |

## Validation Checklist

After any package changes:

- [ ] `./build.sh -sb` succeeds (includes API compat checks)
- [ ] Check `artifacts/packages/*` for expected output
- [ ] Verify no new files in submodule directories
- [ ] Confirm Customizations files preserved if they existed
- [ ] If new `CompatibilitySuppressions.xml` entries were needed, verify a tracking
      issue exists in dotnet/sdk for the underlying generator gap

## 🔗 References

- All build commands and detailed processes are in the [README.md](../README.md)
