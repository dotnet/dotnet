# Known issues

## Unsupported `netcoreapp3.1` TFM

Tracking [issue](https://github.com/dotnet/source-build/issues/3251)

Reference packages for TFM `netcoreapp3.1` are not supported. You may get numerous compilation issues like:

```text
*/lib/netcoreapp3.1/*: error CS0246: The type or namespace name 'DebuggableAttribute' could not be found
*/lib/netcoreapp3.1/*: error CS0518: Predefined type 'System.String' is not defined or imported
```

Workaround: update the `*.csproj` file:

* remove the `netcoreapp3.1` TFM from the `<TargetFrameworks>` list.
* remove `<PropertyGroup>` and `<ItemGroup>` with the `netcoreapp3.1` condition.
* remove the `lib\netcoreapp3.1` folder.

## The explicit declaration of `TupleElementNamesAttribute` attribute

```text
error CS8138: Cannot reference 'System.Runtime.CompilerServices.TupleElementNamesAttribute' explicitly. Use the tuple syntax to define tuple names.
```

Workaround: update the declaration using tuple syntax to define tuple names. Or comment/remove this attribute.
