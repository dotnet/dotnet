# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@a3dae8d](https://github.com/dotnet/arcade/tree/a3dae8d4fd5a17c147cbecfd31e61463731ac0cc)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@0fd7ef1](https://github.com/dotnet/aspnetcore/tree/0fd7ef105555330171e642d26d43cd09e0174238)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@5bcb2d7](https://github.com/google/googletest/tree/5bcb2d78a16edd7110e72ef694d229815aa29542)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b9d928a](https://github.com/dotnet/cecil/tree/b9d928a9d65ed39b9257846e1b8e853cea609c00)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@22e5891](https://github.com/dotnet/deployment-tools/tree/22e58912805b6e31f11a6f6753635c007829cc07)*
- `src/diagnostics`  
*[dotnet/diagnostics@36f19c4](https://github.com/dotnet/diagnostics/tree/36f19c4904821895ca4d08969adb65b46964b3ce)*
- `src/efcore`  
*[dotnet/efcore@8b6956d](https://github.com/dotnet/efcore/tree/8b6956d731cff085ccb91767a74987db5a9e3b93)*
- `src/emsdk`  
*[dotnet/emsdk@bdc1e33](https://github.com/dotnet/emsdk/tree/bdc1e33d5d5ec616dce13320f9ee7189aced17ee)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@85d805a](https://github.com/dotnet/msbuild/tree/85d805a5a26beb66aa7296bd79d5c5a4ec4ba2f2)*
- `src/nuget-client`  
*[nuget/nuget.client@5485ea6](https://github.com/nuget/nuget.client/tree/5485ea697de98eee58746e0b0054cd478e33a1a5)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@36542b2](https://github.com/dotnet/razor/tree/36542b23cfe07ae324274a3bd794c70557b806e4)*
- `src/roslyn`  
*[dotnet/roslyn@b2a6fc8](https://github.com/dotnet/roslyn/tree/b2a6fc8eca8d3332845b142717166303d18362c7)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b70b320](https://github.com/dotnet/roslyn-analyzers/tree/b70b32099c2365092974f6786636e25ae507c8fa)*
- `src/runtime`  
*[dotnet/runtime@1cc0186](https://github.com/dotnet/runtime/tree/1cc0186c3e120ee4ed0494cf74fef0a3ef0118d6)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@b149a15](https://github.com/dotnet/sdk/tree/b149a156f2a3d699bee08c577bcd586b8468da2f)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@abec570](https://github.com/dotnet/source-build-externals/tree/abec570b33d209dde31bd81c412ec8f4f0ecc587)*
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
*[dotnet/source-build-reference-packages@7acff95](https://github.com/dotnet/source-build-reference-packages/tree/7acff95b845d4e44ac902068932076a77bfed595)*
- `src/sourcelink`  
*[dotnet/sourcelink@9c936a6](https://github.com/dotnet/sourcelink/tree/9c936a67bc298a2574f818ae850d089b5c6ba144)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@b9f1c45](https://github.com/dotnet/templating/tree/b9f1c45446631eb116dee4d6ebbf534906fefcd8)*
- `src/test-templates`  
*[dotnet/test-templates@c7852b8](https://github.com/dotnet/test-templates/tree/c7852b88d3f9c5249aef10661cdbca0a93c00576)*
- `src/vstest`  
*[microsoft/vstest@b1e15e5](https://github.com/microsoft/vstest/tree/b1e15e51243982a3396d0136f4fd889a707e1d0e)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@8a1320f](https://github.com/dotnet/windowsdesktop/tree/8a1320ffe042f990c4151e59a00920ccc265b0a6)*
- `src/winforms`  
*[dotnet/winforms@ff2acdf](https://github.com/dotnet/winforms/tree/ff2acdf41abfdc9d6557c545cb61906158fe9976)*
- `src/wpf`  
*[dotnet/wpf@570b992](https://github.com/dotnet/wpf/tree/570b9927c49b4649c9c3de675df74af527f307bd)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
