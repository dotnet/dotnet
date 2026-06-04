# Known issues

## Common Source Files

The `src/referencePackages/common/` directory contains shared source files that address
known GenAPI limitations. These files are preferred over hand-editing generated code
because they are preserved across package regeneration when included via `Customizations.props`.

The `$(CommonSrc)` MSBuild property (defined in `src/referencePackages/Directory.Build.props`)
points to this directory. To include a common file, add it to the package's `Customizations.props`:

```xml
<Project>
  <ItemGroup>
    <Compile Include="$(CommonSrc)x.cs" />
  </ItemGroup>
</Project>
```

### Available common files

| File                            | Resolves                                                                                                    |
|---------------------------------|-------------------------------------------------------------------------------------------------------------|
