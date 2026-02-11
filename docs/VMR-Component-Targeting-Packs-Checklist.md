# VMR Component Checklist: Runtime Package and Targeting Pack Configuration

## Purpose

This checklist helps ensure new components added to the VMR properly configure their runtime package dependencies and targeting pack references.

## For Component Maintainers

### When Your Component Uses .NET Core APIs

If your component:
- Compiles against .NET Core libraries
- Uses `<FrameworkReference Include="Microsoft.NETCore.App" />`
- Targets `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, or `$(NetCoreAppCurrent)`

Then you MUST:

- [ ] Add Microsoft.NETCore.App.Ref to `eng/Version.Details.xml`
  ```xml
  <Dependency Name="Microsoft.NETCore.App.Ref" Version="[latest-version]">
    <Uri>https://github.com/dotnet/dotnet</Uri>
    <Sha>[commit-sha]</Sha>
  </Dependency>
  ```

- [ ] Configure targeting pack download in build files (e.g., `eng/Tools.props`)
  ```xml
  <PackageDownload Include="Microsoft.NETCore.App.Ref" 
                   Version="[$(MicrosoftNETCoreAppRefVersion)]" 
                   Condition="'$(MicrosoftNETCoreAppRefVersion)' != ''" />
  ```

- [ ] Import Arcade's targeting pack targets (if using Arcade)
  ```xml
  <Import Project="$(MSBuildThisFileDirectory)..\arcade\src\Microsoft.DotNet.Arcade.Sdk\tools\TargetingPacks.BeforeCommonTargets.targets" />
  ```

### When Your Component Uses Runtime Packs

If your component:
- Downloads or packages Microsoft.NETCore.App.Runtime.* packages
- Configures KnownRuntimePack items
- Needs runtime binaries for testing or deployment

Then you SHOULD:

- [ ] Add Microsoft.NETCore.App.Ref to Version.Details.xml (for compilation)
- [ ] Configure KnownRuntimePack metadata
  ```xml
  <KnownRuntimePack Update="Microsoft.NETCore.App">
    <LatestRuntimeFrameworkVersion>$(MicrosoftNETCoreAppRefVersion)</LatestRuntimeFrameworkVersion>
  </KnownRuntimePack>
  ```
- [ ] Document which RIDs (Runtime Identifiers) are needed
- [ ] Ensure runtime pack versions align with targeting pack version

### Special Cases

#### AOT/ReadyToRun/Crossgen Only
If you only use runtime packs for ahead-of-time compilation (like Roslyn's ServiceHub):
- [ ] Document that you're using a specific stable version
- [ ] Explain why you don't need live targeting pack updates
- [ ] Configure PackageReference with `ExcludeAssets="all"` and `GeneratePathProperty="true"`

#### Test-Only Usage
If runtime packages are only used in test projects:
- [ ] Consider if targeting pack is needed for test compilation
- [ ] Document the testing scenario

## Verification Steps

After configuration, verify:

1. **Build succeeds with latest runtime**
   ```bash
   ./build.sh --restore --build
   ```

2. **Version.Details.xml is valid**
   ```bash
   xmllint --noout eng/Version.Details.xml
   ```

3. **Targeting pack is resolved**
   ```bash
   # Check that MicrosoftNETCoreAppRefVersion is set
   dotnet msbuild -getProperty:MicrosoftNETCoreAppRefVersion
   ```

4. **No missing reference errors**
   - Build should not complain about missing Microsoft.NETCore.App assemblies
   - IntelliSense should resolve BCL types

## Common Issues and Solutions

### Issue: Build fails with "Framework reference 'Microsoft.NETCore.App' not found"

**Solution:** Add Microsoft.NETCore.App.Ref to Version.Details.xml and configure PackageDownload

### Issue: Wrong version of BCL assemblies used

**Solution:** Ensure targeting pack version matches or is newer than runtime pack version

### Issue: Compilation uses runtime assemblies instead of ref assemblies

**Solution:** Configure EnableTargetingPackDownload and TargetingPackPath properly

### Issue: Source-build fails to find targeting pack

**Solution:** Verify dependency is in Version.Details.xml with correct Uri pointing to dotnet/dotnet

## Examples to Follow

Good examples of proper configuration:

1. **aspnetcore** - Extensive runtime and targeting pack usage
   - File: `src/aspnetcore/eng/Version.Details.xml`
   - File: `src/aspnetcore/eng/Dependencies.props`

2. **wpf** - PackageDownload pattern
   - File: `src/wpf/eng/Tools.props`
   - File: `src/wpf/eng/Version.Details.xml`

3. **sdk** - Layout and bundling
   - File: `src/sdk/eng/Version.Details.xml`
   - File: `src/sdk/src/Layout/redist/targets/GenerateBundledVersions.targets`

## Monitoring and Maintenance

### Regular Checks

- [ ] Run VMR analysis scan quarterly
- [ ] Verify all targeting pack versions are aligned
- [ ] Check for new components that need configuration

### When Runtime Updates

When dotnet/runtime produces a new Microsoft.NETCore.App.Ref version:

- [ ] Darc will auto-update Version.Details.xml files
- [ ] Verify builds pass with new version
- [ ] Check for API breaking changes

### When Adding New Component

- [ ] Run through this checklist
- [ ] Test build in VMR context
- [ ] Document any special configuration needs

## Automation

To check your component programmatically:

```bash
# Check if ref pack is referenced
if grep -q "Microsoft.NETCore.App.Ref" "src/YOUR-COMPONENT/eng/Version.Details.xml"; then
    echo "✅ Targeting pack referenced"
else
    echo "⚠️  No targeting pack reference found"
fi

# Check if runtime pack is used
if grep -rq "Microsoft.NETCore.App.Runtime" "src/YOUR-COMPONENT/" --include="*.props" --include="*.targets"; then
    echo "⚠️  Runtime pack usage found - verify targeting pack is also configured"
fi
```

## Contact

For questions about targeting pack configuration:
- Review: `docs/VMR-Component-Analysis-Runtime-Targeting-Packs.md`
- Reference: `docs/VMR-Component-Targeting-Packs-Quick-Reference.md`
- Open issue in dotnet/dotnet repository

---
*This checklist is based on the VMR component analysis performed in February 2026*
