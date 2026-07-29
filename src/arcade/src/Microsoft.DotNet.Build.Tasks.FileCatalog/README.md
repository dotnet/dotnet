# Microsoft.DotNet.Build.Tasks.FileCatalog

A pure-managed, cross-platform generator for Windows catalog (`.cat`) files,
exposed as the `GenerateFileCatalog` MSBuild task.

A catalog file records the cryptographic hashes of a set of files so those files
can be integrity-verified without modifying them. It is typically used for
customer-modifiable files (such as `.js` scripts) that must ship in a
Visual Studio / signing-compliant product but cannot themselves be
Authenticode-signed.

This component produces **unsigned** catalogs whose byte structure is identical
to what the Windows SDK's `makecat.exe` emits.
The catalog is then Authenticode-signed by the normal Arcade signing infrastructure
(`FileExtensionSignInfo Include=".cat"`), exactly as a `makecat`-produced catalog
would be.

Unlike `makecat.exe`, this generator:

- runs on **any OS** (no Windows SDK / `makecat.exe` dependency),
- produces **deterministic** output (stable `ListIdentifier`/`ctlThisUpdate` and
  members sorted by hash), so identical inputs yield byte-identical catalogs.

## Using the `GenerateFileCatalog` MSBuild task

The task is packaged in `Microsoft.DotNet.Build.Tasks.FileCatalog`; its props
auto-import the `UsingTask`. Author a target that collects the files and calls the
task with an output path:

```xml
<Target Name="GenerateCatalogFiles" AfterTargets="Build">
  <ItemGroup>
    <_FileToCatalog Include="$(OutputPath)**\*.js" />
  </ItemGroup>

  <GenerateFileCatalog Files="@(_FileToCatalog)"
                       OutputPath="$(OutputPath)mypackage.cat" />
</Target>
```

- `Files` (required): the files to include in the catalog.
- `OutputPath` (required): full path of the `.cat` to write. The directory is
  created if needed.
