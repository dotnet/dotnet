# Building Internal Branches Locally

This guide details how devs, working locally, can build branches of various repositories that have had dependency flow from non-public branches.

## The Problem

For instance, let's say runtime has an internal fix in `internal/release/9.0`. Runtime flows outwards to winforms, wpf, efcore, aspnetcore, etc. When that runtime build flows to winforms, the version numbers are updated, but the assets are not publicly accessible. They are only available on internal feeds and authenticated blob storage. Internal build automation will pass various credentials or use built in identities to access these asset locations, but this is not automatic when building locally.

## The Solution

1. Request and obtain access to the following entitlement: https://coreidentity.microsoft.com/manage/Entitlement/entitlement/netdailyinte-q2ql 
2. Add/Enable internal NuGet Sources - This step adds `-internal` feeds that correspond to non-internal feeds (e.g. `dotnet9-internal` if `dotnet9` is present). It also *enables* `darc-int-*` sources added for stable packages.
   
   **All platforms (w/Powershell core installed)**
   ```
   ./eng/common/SetupNuGetSources.ps1` -ConfigFile <path to NuGet.config>
   ```

   **Shell**
   ```
   ./eng/common/SetupNuGetSources.sh <path to NuGet.config>
   ```

3. Generate a base64 encoded SAS token used for downloading runtimes. This step uses the Azure CLI and your personal identity to generate a SAS token for ci.dot.net/internal. It relies on being approved for the above CoreIdentity entitlement.

    **All platforms (w/Powershell core and azure cli installed )**
   ```
   $mySasBase64 = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($(az storage container generate-sas --account-name dotnetbuilds --name internal --permissions rl --auth-mode login --as-user --expiry $((Get-Date).AddHours(12).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") ) | ConvertFrom-Json)))
   ```

   **Shell**
   ```
   mySasBase64=$(az storage container generate-sas --account-name dotnetbuilds --name internal --permissions rl --auth-mode login --as-user --expiry $(date -u -d "12 hours" +%Y-%m-%dT%H:%MZ) --output tsv | base64)
   ```
4. Build with required parameters. The SAS token and internal storage location is typically passed with `/p:DotNetRuntimeSourceFeed=<>` and `/p:DotNetRuntimeSourceFeedKey=<base64 SAS>`. Some repositories MAY have additional parameters.

    ```
    build.cmd /p:DotNetRuntimeSourceFeed=https://ci.dot.net/internal and `/p:DotNetRuntimeSourceFeedKey=$mySasBase64
    ```

## This doesn't work for my repo. What can I do?

Please check the official build YAML for the repository in question. The internal build parameters are almost always passed to the build.cmd.


