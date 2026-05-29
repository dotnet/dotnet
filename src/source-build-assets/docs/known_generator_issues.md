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
    <Compile Include="$(CommonSrc)IsExternalInit.cs" />
  </ItemGroup>
</Project>
```

### Available common files

| File                            | Resolves                                                                                                    |
|---------------------------------|-------------------------------------------------------------------------------------------------------------|
| `IsExternalInit.cs`             | `error CS0518: Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported` |
| `RequiredModifierAttributes.cs` | `error CS0656: Missing compiler required member 'System.Runtime.CompilerServices.RequiredMemberAttribute..ctor'` and related `CompilerFeatureRequiredAttribute` / `SetsRequiredMembersAttribute` errors |

## The explicit declaration of `TupleElementNamesAttribute` attribute

```text
error CS8138: Cannot reference 'System.Runtime.CompilerServices.TupleElementNamesAttribute' explicitly. Use the tuple syntax to define tuple names.
```

Workaround: update the declaration using tuple syntax to define tuple names. Or comment/remove this attribute.
