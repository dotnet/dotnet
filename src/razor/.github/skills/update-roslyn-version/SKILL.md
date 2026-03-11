---
name: update-roslyn-version
description: Update all Roslyn dependency versions in the razor repository. Use this when asked to update, bump, or change the Roslyn version.
---

# Update Roslyn Version

Update all Roslyn dependency versions in the razor repository. The user will provide the new version number (e.g. `5.5.0-2.26118.1`).

## Steps

1. **Identify the current Roslyn version** by reading `eng/Version.Details.xml` — look at the `Version` attribute on any `<Dependency>` element with `<Uri>https://github.com/dotnet/roslyn</Uri>`.

2. **Update `eng/Version.Details.props`**: Replace all Roslyn package version values (in the `<!-- dotnet/roslyn dependencies -->` section) with the new version. These are the `*PackageVersion` elements. The old value may be a `-dev` placeholder or a full version string.

3. **Update `eng/Version.Details.xml`**: Replace all Roslyn `<Dependency>` elements' `Version` attributes with the new version.

4. **Run `restore.cmd`** (or `restore.sh` on non-Windows) from the repo root to download the new packages. Wait for it to complete successfully.

5. **Get the commit hash**: After restore, find a `.nuspec` file for one of the restored Roslyn packages in the NuGet cache (`D:\NugetCache` on this machine). For example:
   ```
   D:\NugetCache\microsoft.codeanalysis.common\<NEW_VERSION>\microsoft.codeanalysis.common.nuspec
   ```
   Parse the `<repository commit="...">` element from the nuspec to extract the commit SHA. All Roslyn packages share the same commit.

6. **Update the commit SHA in `eng/Version.Details.xml`**: Replace the old Roslyn `<Sha>` values with the new commit hash from step 5. Only update Roslyn entries (those with `<Uri>https://github.com/dotnet/roslyn</Uri>`), not arcade/aspnetcore/runtime entries.

7. **Verify** by running `git diff` to confirm only the expected version and SHA changes were made in the two files.

## Notes

- Do NOT modify non-Roslyn dependencies (arcade, aspnetcore, runtime).
- The NuGet cache path may vary by machine. Common locations:
  - Windows: `D:\NugetCache` or `%USERPROFILE%\.nuget\packages`
  - macOS/Linux: `~/.nuget/packages`
- If restore fails, the version may not exist on the configured NuGet feeds — verify the version string.
