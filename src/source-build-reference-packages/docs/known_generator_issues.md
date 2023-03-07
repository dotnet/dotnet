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

## CLSCompliant assembly attributes

Tracking [issue](https://github.com/dotnet/source-build/issues/3252)

By default, it is set to true and it may produce compilation errors like:

```text
error CS3015: ... has no accessible constructors which use only CLS-compliant types
error CS3016: Arrays as attribute arguments is not CLS-compliant
```

Workaround: update the assembly attribute `[assembly: CLSCompliant(true)]` and set value to `false`

## The explicit declaration of `TupleElementNamesAttribute` attribute

```text
error CS8138: Cannot reference 'System.Runtime.CompilerServices.TupleElementNamesAttribute' explicitly. Use the tuple syntax to define tuple names.
```

Workaround: update the declaration using tuple syntax to define tuple names. Or comment/remove this attribute.

## Synthesized dummy generic field of filtered type

Tracking [issue](https://github.com/dotnet/sdk/issues/30627)

```text
public readonly partial struct IncrementalValueProvider<TValue>
{
    // error CS0234: The type or namespace name 'IIncrementalGeneratorNode<>' does not exist in the namespace 'Microsoft.CodeAnalysis'
    private readonly CodeAnalysis.IIncrementalGeneratorNode<TValue> Node;
}
```

Workaround: remove the synthesized field.

## Class without defined implicit constructor

Tracking [issue](https://github.com/dotnet/arcade/issues/12578)

```text
public class Bar
{
    public Bar(int a) { }
}

// error CS7036: There is no argument given that corresponds to the required parameter 'a' of 'Bar.Bar(int)'
public class Foo : Bar
{
}
```

Workaround: introduce an internal default constructor for the `Foo` class with `internal Foo() : base(default) { }`

## Explicit interface declaration is not found among members of the interface that can be implemented

Tracking [issue](https://github.com/dotnet/source-build/issues/3237)

```text
error CS0539: 'ImmutableArray<T>.Item' in explicit interface declaration is not found among members of the interface that can be implemented
error CS0683: 'ImmutableDictionary<TKey, TValue>.get_Item(TKey)' explicit method implementation cannot implement 'IDictionary<TKey, TValue>.this[TKey].get' because it is an accessor
```

Workaround: manually add indexer, explicit interface declaration methods and remove the wrong one as suggested by the compiler.
