# Known issues

## The explicit declaration of `TupleElementNamesAttribute` attribute

```text
error CS8138: Cannot reference 'System.Runtime.CompilerServices.TupleElementNamesAttribute' explicitly. Use the tuple syntax to define tuple names.
```

Workaround: update the declaration using tuple syntax to define tuple names. Or comment/remove this attribute.
