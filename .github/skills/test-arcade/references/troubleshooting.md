# Troubleshooting

Common issues when building and testing Arcade locally, with diagnosis steps and fixes.

## Build Failures

### Arcade build fails: "Unable to load service index"

**Symptom**: NuGet restore fails with feed connectivity errors.

**Cause**: Network access to Azure DevOps package feeds is blocked.

**Fix**:
- Verify VPN/network connectivity to `dev.azure.com/dnceng`
- Check that `NuGet.config` in the arcade repo has the correct feed URLs
- Try restoring with `--verbosity diagnostic` to see which feed is failing:
  ```bash
  cd /path/to/arcade && ./build.sh --restore --verbosity diagnostic 2>&1 | grep -i "unable to load"
  ```

### Arcade build fails: wrong .NET SDK

**Symptom**: Build errors related to SDK version mismatch.

**Cause**: The global .NET SDK doesn't match what Arcade expects.

**Fix**:
- Arcade's `build.sh` should auto-install the correct SDK via `eng/common/dotnet.sh`
- **NEVER** manually install an SDK — the build installs its own
- If the auto-install fails, check `global.json` in the Arcade repo for the expected version

### Arcade build: no packages produced

**Symptom**: Build succeeds but `artifacts/packages/` is empty or missing.

**Cause**: Build ran without the `--pack` flag.

**Fix**: Ensure the build command includes `--pack`:
```powershell
$OfficialBuildId = "{0}.1" -f (Get-Date).AddYears(5).ToString('yyyyMMdd')
./build.sh --configuration Release --pack /p:OfficialBuildId=$OfficialBuildId
```

## Feed Configuration Failures

### "Failed to add local NuGet feed source"

**Symptom**: `dotnet nuget add source` command fails.

**Cause**: Usually a duplicate source name or invalid NuGet.config.

**Fix**:
- Remove existing source first: `dotnet nuget remove source ArcadeLocalFeed --configfile /path/to/NuGet.config`
- Verify NuGet.config is valid XML
- Check file permissions on NuGet.config

### Test repo restore: "Unable to find package Microsoft.DotNet.Arcade.Sdk"

**Symptom**: Test repo can't find the Arcade SDK package during restore.

**Cause**: Feed path doesn't contain the expected package, or NuGet cache is stale.

**Fix**:
1. Verify the package exists in the feed: `Get-ChildItem (Join-Path ([System.IO.Path]::GetTempPath()) 'arcade-local-feed') -Filter 'Microsoft.DotNet.Arcade.Sdk.*.nupkg'`
2. Clear NuGet caches: `dotnet nuget locals all --clear`
3. Verify the source is registered: `dotnet nuget list source --configfile /path/to/NuGet.config`
4. Check that `global.json` version matches the package version exactly

### SignCheck restore: "Unable to find package Microsoft.DotNet.SignCheckTask"

**Symptom**: SignCheck fails with `NU1102: Unable to find package Microsoft.DotNet.SignCheckTask with version (>= ...)`. The error lists remote feeds but not the local feed.

**Cause**: The `SigningValidation.proj` file runs from inside the NuGet packages cache (not the test repo directory), so it doesn't inherit the test repo's `NuGet.config`. The local feed must be passed explicitly via `/p:RestoreAdditionalProjectSources`.

**Fix**:
1. Verify the SignCheckTask package exists in the local feed: `Get-ChildItem (Join-Path ([System.IO.Path]::GetTempPath()) 'arcade-local-feed') -Filter 'Microsoft.DotNet.SignCheckTask.*.nupkg'`
2. Ensure the script passes `/p:RestoreAdditionalProjectSources=$FeedPath` to the SDK task invocation
3. Clear NuGet caches and retry: `dotnet nuget locals all --clear`

### Version mismatch between global.json and packages

**Symptom**: Restore fails even though packages exist in the feed.

**Cause**: The version in `global.json` doesn't exactly match the `.nupkg` filename.

**Fix**:
- Check the exact version: `Get-ChildItem (Join-Path ([System.IO.Path]::GetTempPath()) 'arcade-local-feed') -Filter 'Microsoft.DotNet.Arcade.Sdk.*.nupkg'`
- Compare with `global.json`: `Get-Content /path/to/test-repo/global.json | Select-String 'Arcade'`
- The script handles this automatically, but manual runs may have mismatches

## Test Repo Build Failures

### Test repo build fails with Arcade-specific errors

**Symptom**: Build errors in MSBuild targets or tasks from Arcade packages.

**Diagnosis**:
1. Check if the same error occurs with the official Arcade SDK (revert `global.json` and `NuGet.config` changes)
2. Compare the `.binlog` files between local and official Arcade builds
3. Look for breaking API changes in your Arcade modifications

### Test repo build fails with unrelated errors

**Symptom**: Errors that don't reference Arcade packages (e.g., source code compilation errors).

**Cause**: The test repo itself has issues independent of Arcade.

**Fix**:
- Build the test repo without Arcade changes first to establish a baseline
- Check the test repo's issue tracker for known build issues
- Ensure the test repo branch is compatible with the Arcade version being tested

## Performance Issues

### Disk space issues

**Symptom**: Build fails with "No space left on device".

**Fix**:
- Clean previous artifacts: `Remove-Item -Recurse -Force /path/to/arcade/artifacts, /path/to/test-repo/artifacts`
- Clean NuGet caches: `dotnet nuget locals all --clear`
- Remove old feed directories


## Investigating Build Logs

For detailed build diagnostics, use the MSBuild binary log (`.binlog`). The `mcp-binlog-tool` MCP server can parse these files directly:

```powershell
# Find binlog files
Get-ChildItem /path/to/repo/artifacts/log -Recurse -Filter '*.binlog'

# The mcp-binlog-tool (baronfel.binlog.mcp) can open and query these files
# See the Prerequisites section in SKILL.md for setup
```

Key locations:
- Arcade build log: `arcade/artifacts/log/{Configuration}/Build.binlog`
- Test repo build log: `test-repo/artifacts/log/{Configuration}/Build.binlog`
