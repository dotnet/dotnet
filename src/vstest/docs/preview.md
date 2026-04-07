# Early access to VSTest packages

Stable versions (and selected previews) of VSTest, and related packages, are distributed through <https://nuget.org>.

We also publish every successful merge to main and release branches to our preview NuGet channel called `test-tools`.

To use this channel, you will need to add or edit your [NuGet.Config](https://learn.microsoft.com/nuget/reference/nuget-config-file) file with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <!-- VSTest early access packages. See: https://aka.ms/vstest/preview -->
    <add key="test-tools" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/test-tools/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

You can also browse interactively the available versions using `https://dev.azure.com/dnceng/public/_artifacts/feed/test-tools/NuGet/<PackageName>/versions`, where `<PackageName>` is the name of the package you are looking for. For example, for Microsoft.TestPlatform, the link is <https://dev.azure.com/dnceng/public/_artifacts/feed/test-tools/NuGet/Microsoft.TestPlatform/versions>.

## Warranty

Packages from `test-tools` feed are considered experimental. They might not have the usual quality, may contain experimental and breaking changes, and come without warranty.

## Feed information

### NuGet.config placement

NuGet.Config file can be placed next to solution file, or next to project file when you don't have solution file. But in cases where you have solution file, you should always place it next to solution file, to ensure consistent behavior in Visual Studio and in command line.

### Dependency confusion attack

Adding additional NuGet feeds might lead to warnings or errors from build systems that check compliance. This is because using multiple public and private sources might lead to possible dependency confusion attacks. All the packages we publish to nuget.org are using a reserved prefix. But this might not mitigate the risk in your setup. If this is a concern to you, please discuss with your internal security department.

### Usage with central package management

Solutions that use central package management through `Directory.Packages.props` will see `NU1507` warnings about multiple package sources. To solve this add this section to your `NuGet.Config` file:

```xml
<packageSourceMapping>
  <!-- key value for <packageSource> should match key values from <packageSources> element -->
  <packageSource key="nuget.org">
    <package pattern="*" />
  </packageSource>
  <packageSource key="test-tools">
    <package pattern="Microsoft.TestPlatform.*" />
    <package pattern="Microsoft.NET.Test.SDK" />
  </packageSource>
</packageSourceMapping>
```

Full documentation of package source mapping can be [found here](https://learn.microsoft.com/nuget/consume-packages/package-source-mapping#enable-by-manually-editing-nugetconfig).
