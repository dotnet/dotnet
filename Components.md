# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@610e251](https://github.com/dotnet/arcade/tree/610e251fc34686333b98188320ca1eecd7e6af6c)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@257d690](https://github.com/dotnet/aspnetcore/tree/257d69079e0f7fc84e3f6cd5047272d7f79b4d66)*
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
*[dotnet/diagnostics@1f8c221](https://github.com/dotnet/diagnostics/tree/1f8c2213b61e90ea960b33e5d7fb7a9a88c82296)*
- `src/efcore`  
*[dotnet/efcore@8b6956d](https://github.com/dotnet/efcore/tree/8b6956d731cff085ccb91767a74987db5a9e3b93)*
- `src/emsdk`  
*[dotnet/emsdk@edf3e90](https://github.com/dotnet/emsdk/tree/edf3e90fa25b1fc4f7f63ceb45ef70f49c6b121a)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@6bc91d5](https://github.com/dotnet/msbuild/tree/6bc91d5e2d3d8a199fdbe367ed015b55daf57046)*
- `src/nuget-client`  
*[nuget/nuget.client@5485ea6](https://github.com/nuget/nuget.client/tree/5485ea697de98eee58746e0b0054cd478e33a1a5)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@d5cfe11](https://github.com/dotnet/razor/tree/d5cfe11b00e279b96344440e4d9b403994b376de)*
- `src/roslyn`  
*[dotnet/roslyn@d1fe9e9](https://github.com/dotnet/roslyn/tree/d1fe9e95451c4c24d874981341f653e9decb1d8c)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@3211f48](https://github.com/dotnet/roslyn-analyzers/tree/3211f48253bc18560156d90dc5e710d35f7d03fa)*
- `src/runtime`  
*[dotnet/runtime@68511fd](https://github.com/dotnet/runtime/tree/68511fd27fe4055ce5203742998ba12019dfcbd4)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@4336029](https://github.com/dotnet/sdk/tree/43360291a50c9c7c471551f8f8363919d38014ea)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@b950526](https://github.com/dotnet/source-build-externals/tree/b950526ab527c85f4f8f515a8587b977f0be6348)*
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
*[dotnet/source-build-reference-packages@0d066e6](https://github.com/dotnet/source-build-reference-packages/tree/0d066e61a30c2599d0ced871ea45acf0e10571af)*
- `src/sourcelink`  
*[dotnet/sourcelink@85fb5b9](https://github.com/dotnet/sourcelink/tree/85fb5b9ab7a4d308ccac2c376fb4661df5e752ce)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@5d43d89](https://github.com/dotnet/templating/tree/5d43d89ab9da3a63359215c5abea86f63516e4f9)*
- `src/test-templates`  
*[dotnet/test-templates@512c5c7](https://github.com/dotnet/test-templates/tree/512c5c7434d56c74d4a21ffbd805a47908c03f90)*
- `src/vstest`  
*[microsoft/vstest@b1e15e5](https://github.com/microsoft/vstest/tree/b1e15e51243982a3396d0136f4fd889a707e1d0e)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@b63554f](https://github.com/dotnet/windowsdesktop/tree/b63554fc467bcca47f7d05fa854e1144b189bec9)*
- `src/winforms`  
*[dotnet/winforms@7006c1c](https://github.com/dotnet/winforms/tree/7006c1c2c5515bc4b648e5b3c2ea6a604867e0b9)*
- `src/wpf`  
*[dotnet/wpf@0ff1f28](https://github.com/dotnet/wpf/tree/0ff1f2850fb0a0966da50f727764debd05267947)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
