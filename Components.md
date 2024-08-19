# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@60ae233](https://github.com/dotnet/arcade/tree/60ae233c3d77f11c5fdb53e570b64d503b13ba59)*
- `src/aspire`  
*[dotnet/aspire@d304c5f](https://github.com/dotnet/aspire/tree/d304c5f6f15bcd4f34f1841b33870cfab88e6937)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@42af9fe](https://github.com/dotnet/aspnetcore/tree/42af9fe6ddd7c3f9cde04ac003bf97509881873b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@ff233bd](https://github.com/google/googletest/tree/ff233bdd4cac0a0bf6e5cd45bda3406814cb2796)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b9d928a](https://github.com/dotnet/cecil/tree/b9d928a9d65ed39b9257846e1b8e853cea609c00)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@7871ee3](https://github.com/dotnet/deployment-tools/tree/7871ee378dce87b64d930d4f33dca9c888f4034d)*
- `src/diagnostics`  
*[dotnet/diagnostics@5838149](https://github.com/dotnet/diagnostics/tree/5838149521e80185e0dbe79720bd9ce6e81d2d35)*
- `src/efcore`  
*[dotnet/efcore@d94cc5a](https://github.com/dotnet/efcore/tree/d94cc5a79cb9d557485a95fba59ee3738c46aa81)*
- `src/emsdk`  
*[dotnet/emsdk@edf3e90](https://github.com/dotnet/emsdk/tree/edf3e90fa25b1fc4f7f63ceb45ef70f49c6b121a)*
- `src/fsharp`  
*[dotnet/fsharp@02adf13](https://github.com/dotnet/fsharp/tree/02adf13f8d69e0105fff4d68dbd5fb1d43bc0e17)*
- `src/msbuild`  
*[dotnet/msbuild@6bc91d5](https://github.com/dotnet/msbuild/tree/6bc91d5e2d3d8a199fdbe367ed015b55daf57046)*
- `src/nuget-client`  
*[nuget/nuget.client@e68fb0f](https://github.com/nuget/nuget.client/tree/e68fb0f2d96800ad4b78461fd491886e9e5084b2)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@3920563](https://github.com/dotnet/razor/tree/3920563359e73a7b8d3c8d7a1bba61bbf1c9efe5)*
- `src/roslyn`  
*[dotnet/roslyn@75b26a2](https://github.com/dotnet/roslyn/tree/75b26a2088e76855528483674a1fbeea0137241b)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@fdb9965](https://github.com/dotnet/roslyn-analyzers/tree/fdb9965ce68c1f4e1c0ff301488adf9caa958615)*
- `src/runtime`  
*[dotnet/runtime@7cb32e1](https://github.com/dotnet/runtime/tree/7cb32e193a55a95c74fc3bd56501b951b48b700f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@5760d05](https://github.com/dotnet/sdk/tree/5760d059d5f15bb5d6ef1afd1420810df7e72093)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@51b029e](https://github.com/dotnet/source-build-externals/tree/51b029e3272f35af0af337823cd122725f316c69)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
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
*[dotnet/templating@2aad619](https://github.com/dotnet/templating/tree/2aad6193928e85f0ecffa0590d55baa4b2f84323)*
- `src/test-templates`  
*[dotnet/test-templates@0b1b52e](https://github.com/dotnet/test-templates/tree/0b1b52e10bf70c53ed9fc63da52281b7d10ddd48)*
- `src/vstest`  
*[microsoft/vstest@c5c8ee5](https://github.com/microsoft/vstest/tree/c5c8ee586c6bd58eee6e845aee32e88f789c127d)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@e2d37ff](https://github.com/dotnet/windowsdesktop/tree/e2d37ff24309778391e4a1d2038ad47e200ef6a2)*
- `src/winforms`  
*[dotnet/winforms@7006c1c](https://github.com/dotnet/winforms/tree/7006c1c2c5515bc4b648e5b3c2ea6a604867e0b9)*
- `src/wpf`  
*[dotnet/wpf@e7ddcf4](https://github.com/dotnet/wpf/tree/e7ddcf41e1342d889aa7273b836909bdf15154e2)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
