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
*[dotnet/aspnetcore@387b17b](https://github.com/dotnet/aspnetcore/tree/387b17b4b22d7c3abd03e5ad623e03aa2d9a7157)*
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
*[dotnet/efcore@76e0a92](https://github.com/dotnet/efcore/tree/76e0a920b841cbf7201394c8ace4da790b29b1f2)*
- `src/emsdk`  
*[dotnet/emsdk@edf3e90](https://github.com/dotnet/emsdk/tree/edf3e90fa25b1fc4f7f63ceb45ef70f49c6b121a)*
- `src/fsharp`  
*[dotnet/fsharp@54464f5](https://github.com/dotnet/fsharp/tree/54464f520298ad6e436619a381e43c79694bb19a)*
- `src/msbuild`  
*[dotnet/msbuild@f422d8d](https://github.com/dotnet/msbuild/tree/f422d8d7dfe0a7115b11b31470215ad6b7723138)*
- `src/nuget-client`  
*[nuget/nuget.client@0539e8e](https://github.com/nuget/nuget.client/tree/0539e8e2516f708c834cb606340c7c9a110f96b3)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@da79940](https://github.com/dotnet/razor/tree/da79940a3af779e3dc973baa1e4e6ebfa100abe1)*
- `src/roslyn`  
*[dotnet/roslyn@75b26a2](https://github.com/dotnet/roslyn/tree/75b26a2088e76855528483674a1fbeea0137241b)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@fdb9965](https://github.com/dotnet/roslyn-analyzers/tree/fdb9965ce68c1f4e1c0ff301488adf9caa958615)*
- `src/runtime`  
*[dotnet/runtime@0fbd814](https://github.com/dotnet/runtime/tree/0fbd81404d1f211572387498474063bc6f407f0f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@d92413b](https://github.com/dotnet/scenario-tests/tree/d92413b87d36250859d8cb51ff69a03b5f5c4cab)*
- `src/sdk`  
*[dotnet/sdk@8476855](https://github.com/dotnet/sdk/tree/8476855caf937e50a3b403e09e22809fbe37793c)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@2cef086](https://github.com/dotnet/source-build-externals/tree/2cef086137a68586fdd69848261e2a8cf8c48b73)*
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
*[dotnet/sourcelink@aeb2830](https://github.com/dotnet/sourcelink/tree/aeb2830c11db6fd2a81b82700afba2525549bef7)*
- `src/symreader`  
*[dotnet/symreader@0710a78](https://github.com/dotnet/symreader/tree/0710a7892d89999956e8808c28e9dd0512bd53f3)*
- `src/templating`  
*[dotnet/templating@0ae441f](https://github.com/dotnet/templating/tree/0ae441fe922548f88da27f54edbe99e3a3e08948)*
- `src/test-templates`  
*[dotnet/test-templates@983f1eb](https://github.com/dotnet/test-templates/tree/983f1ebe67a7d37762132790c4384151c177a655)*
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
