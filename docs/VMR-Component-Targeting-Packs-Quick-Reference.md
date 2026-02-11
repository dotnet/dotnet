# Quick Reference: VMR Components and Microsoft.NETCore.App.Ref

## Summary

✅ **All VMR components are properly configured**

All components that consume runtime packages for compilation properly reference Microsoft.NETCore.App.Ref targeting pack.

## Components Using Microsoft.NETCore.App.Ref (10)

| Component | Runtime Usage | Ref Pack Version | Notes |
|-----------|--------------|------------------|-------|
| arcade | KnownRuntimePack configs | In Version.Details.xml | Build infrastructure |
| aspnetcore | Extensive (all RIDs) | 11.0.0-preview.2.26108.103 | Critical - builds on runtime |
| diagnostics | KnownRuntimePack for net8.0/9.0/10.0 | 10.0.2 | Diagnostic tools |
| efcore | FrameworkReference | 11.0.0-preview.2.26108.103 | EF Core |
| nuget-client | Test assets | In Version.Details.xml | NuGet client |
| runtime | Producer | 11.0.0-preview.1.26069.103 | Runtime itself |
| sdk | Extensive layout management | 11.0.0-preview.1.26069.105 | .NET SDK |
| windowsdesktop | KnownFrameworkReference | 11.0.0-preview.2.26079.111 | Desktop framework |
| winforms | FrameworkReference | 11.0.0-preview.2.26109.104 | Windows Forms |
| wpf | Runtime pack downloads | 11.0.0-preview.2.26080.101 | WPF |

## Special Case: Roslyn

- Uses runtime packs (version 8.0.10) for ReadyToRun/crossgen2 compilation only
- Does NOT need live Microsoft.NETCore.App.Ref
- Targets stable .NET versions for VS compatibility
- Status: ⚠️ This is intentional and correct

## Components Not Using Runtime Packages (15)

cecil, command-line-api, deployment-tools, emsdk, fsharp, msbuild, razor, scenario-tests, source-build-reference-packages, sourcelink, symreader, templating, vstest, xdt

These components don't need Microsoft.NETCore.App.Ref as they don't compile against runtime APIs.

## Quick Check Commands

```bash
# Check if a component has Microsoft.NETCore.App.Ref
grep -r "Microsoft.NETCore.App.Ref" src/<component>/eng/Version.Details.xml

# Check runtime package usage
grep -r "Microsoft.NETCore.App.Runtime" src/<component>/ --include="*.props" --include="*.targets"

# Check KnownFrameworkReference configuration
grep -r "KnownFrameworkReference.*Microsoft.NETCore.App" src/<component>/
```

## When to Add Microsoft.NETCore.App.Ref

Add Microsoft.NETCore.App.Ref dependency when:
1. Your component uses `<FrameworkReference Include="Microsoft.NETCore.App" />`
2. You configure KnownRuntimePack for Microsoft.NETCore.App
3. You need to compile against latest .NET Core APIs
4. You download/reference Microsoft.NETCore.App.Runtime.* packages for runtime deployment

## Standard Pattern

```xml
<!-- eng/Version.Details.xml -->
<Dependency Name="Microsoft.NETCore.App.Ref" Version="11.0.0-preview.2.26108.103">
  <Uri>https://github.com/dotnet/dotnet</Uri>
  <Sha>8c56ff58fc59fd33fc1dab5c6a7155ca16511bb2</Sha>
</Dependency>
```

```xml
<!-- eng/Tools.props or similar -->
<PackageDownload Include="Microsoft.NETCore.App.Ref" 
                 Version="[$(MicrosoftNETCoreAppRefVersion)]" 
                 Condition="'$(MicrosoftNETCoreAppRefVersion)' != ''" />
```

## Related Files

- Full analysis: `docs/VMR-Component-Analysis-Runtime-Targeting-Packs.md`
- Scan scripts: Available in `/tmp/` during analysis session

---
*Last updated: February 11, 2026*
