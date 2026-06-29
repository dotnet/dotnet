# AOT and Trimming Compatibility

NuGet has `IsAotCompatible` enabled for all NuGet.Core libraries and the code itself is AOT compatible.
However, NuGet still utilizes Newtonsoft.Json for deserialization, which uses reflection and is not AOT/trim compatible.

We are in the process of migrating to System.Text.Json source-generated deserialization.
Until the migration is complete, both deserialization paths coexist, gated under a feature switch.
Enabling the feature switch ensures NuGet.Protocol uses System.Text.Json instead of Newtonsoft.Json, allowing the linker to trim the Newtonsoft.Json code path entirely.

## Using NuGet in a Native AOT Application

If you consume NuGet libraries in a native AOT app, add the following feature switch to your project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NuGet.UseSystemTextJsonDeserialization"
                                  Value="true"
                                  Trim="true" />
</ItemGroup>
```

This tells NuGet to use the AOT-safe System.Text.Json path and tells the linker the value is constant so it can eliminate the Newtonsoft.Json code path from the binary.
