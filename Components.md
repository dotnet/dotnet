# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3e18bd4](https://github.com/dotnet/arcade/tree/3e18bd4f62388b829d0b9dcd1ee131431674aa5f)*
- `src/aspire`  
*[dotnet/aspire@a6e341e](https://github.com/dotnet/aspire/tree/a6e341ebbf956bbcec0dda304109815fcbae70c9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@27ae082](https://github.com/dotnet/aspnetcore/tree/27ae0821dded5d31b8f7a1cd4519a8ef3ccd6cc3)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@9ff2450](https://github.com/google/googletest/tree/9ff2450a56aed4f7f124f5104d9e3088bf791ee9)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@e05101e](https://github.com/dotnet/cecil/tree/e05101e694b7c86f03f767014fc203cec5dc7f18)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@22e5891](https://github.com/dotnet/deployment-tools/tree/22e58912805b6e31f11a6f6753635c007829cc07)*
- `src/diagnostics`  
*[dotnet/diagnostics@33d8bf2](https://github.com/dotnet/diagnostics/tree/33d8bf23a6566cd3fb9055acfc9f1141391d5421)*
- `src/efcore`  
*[dotnet/efcore@8b6956d](https://github.com/dotnet/efcore/tree/8b6956d731cff085ccb91767a74987db5a9e3b93)*
- `src/emsdk`  
*[dotnet/emsdk@40781ca](https://github.com/dotnet/emsdk/tree/40781ca2fc6be1dd81143d8773105a0f9f77d923)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@87ffcb5](https://github.com/dotnet/msbuild/tree/87ffcb5c1611d98f911fc2058a649b99a93bf8e6)*
- `src/nuget-client`  
*[nuget/nuget.client@5485ea6](https://github.com/nuget/nuget.client/tree/5485ea697de98eee58746e0b0054cd478e33a1a5)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8806e92](https://github.com/dotnet/razor/tree/8806e92225d1c47d6122b308c1c163416d55a5e0)*
- `src/roslyn`  
*[dotnet/roslyn@b2a6fc8](https://github.com/dotnet/roslyn/tree/b2a6fc8eca8d3332845b142717166303d18362c7)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@81d9b5d](https://github.com/dotnet/roslyn-analyzers/tree/81d9b5d58e3d384ed4a91b75f4a1ece6352e76d9)*
- `src/runtime`  
*[dotnet/runtime@0912e94](https://github.com/dotnet/runtime/tree/0912e94a6c81f4743946831137ba6c3ec3f1a179)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@328820c](https://github.com/dotnet/sdk/tree/328820c7966e9fa45c519548984a66c25297016c)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@d6e05f9](https://github.com/dotnet/source-build-externals/tree/d6e05f9c80394626ffb7af82d0e84547ddd265c0)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@2e7c701](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/2e7c701881d3d67aff7bf54f22063a49bc4727d2)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@26ae006](https://github.com/dotnet/source-build-reference-packages/tree/26ae006620fb4518276b494efac55dbcd8f80224)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@ca1d036](https://github.com/dotnet/templating/tree/ca1d0362b94500d8f26abf478d67061b89feafa4)*
- `src/test-templates`  
*[dotnet/test-templates@c7852b8](https://github.com/dotnet/test-templates/tree/c7852b88d3f9c5249aef10661cdbca0a93c00576)*
- `src/vstest`  
*[microsoft/vstest@a1f5a65](https://github.com/microsoft/vstest/tree/a1f5a6500b8cfefa81adbb652a84ad0ba884c140)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@b334a16](https://github.com/dotnet/windowsdesktop/tree/b334a1668379eba1eb5968cf9007a80efb127057)*
- `src/winforms`  
*[dotnet/winforms@bacb9f8](https://github.com/dotnet/winforms/tree/bacb9f8821506abd78f18c7f65bcf8d5416064be)*
- `src/wpf`  
*[dotnet/wpf@f136b48](https://github.com/dotnet/wpf/tree/f136b48da71f6ba6e95c349f7fec36b68165a08d)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
