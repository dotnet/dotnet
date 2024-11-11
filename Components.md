# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@78eb939](https://github.com/dotnet/arcade/tree/78eb939933628c20c88ddd88536a70f02ecc2945)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@5278d28](https://github.com/dotnet/aspnetcore/tree/5278d28ab78b13a69c468939fff1968ca935ee5a)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@d144031](https://github.com/google/googletest/tree/d144031940543e15423a25ae5a8a74141044862f)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@b897087](https://github.com/dotnet/cecil/tree/b897087e8b76481a9213ae422f5dc16f64a124b5)*
- `src/command-line-api`  
*[dotnet/command-line-api@feb61c7](https://github.com/dotnet/command-line-api/tree/feb61c7f328a2401d74f4317b39d02126cfdfe24)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@4d818b1](https://github.com/dotnet/deployment-tools/tree/4d818b1bfd1cf450492eb8ab3877eb3875488642)*
- `src/diagnostics`  
*[dotnet/diagnostics@335c0c0](https://github.com/dotnet/diagnostics/tree/335c0c013c3a761792bfa83c0dbaadd1f0545f07)*
- `src/efcore`  
*[dotnet/efcore@507e7f1](https://github.com/dotnet/efcore/tree/507e7f196a11aebaf26d1cf34e5d6a3e7044bb52)*
- `src/emsdk`  
*[dotnet/emsdk@e965cbb](https://github.com/dotnet/emsdk/tree/e965cbbf7eae80e2a62e46f0fcf68ab8f47792c4)*
- `src/fsharp`  
*[dotnet/fsharp@28c4395](https://github.com/dotnet/fsharp/tree/28c439568b9cfd9978c8c0a91e05eaf0e0d39396)*
- `src/msbuild`  
*[dotnet/msbuild@77312bc](https://github.com/dotnet/msbuild/tree/77312bc7f9ae339b9d7a0c5ed750fa9effd82111)*
- `src/nuget-client`  
*[nuget/nuget.client@aa7eb99](https://github.com/nuget/nuget.client/tree/aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@f6384a9](https://github.com/dotnet/razor/tree/f6384a995a7032f50f4e3d07fad1c87e2cd8a560)*
- `src/roslyn`  
*[dotnet/roslyn@c0b2f36](https://github.com/dotnet/roslyn/tree/c0b2f366d0fb54cf49040a82f0d044bb959a9e73)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5435ba7](https://github.com/dotnet/roslyn-analyzers/tree/5435ba7b1037f21237adc1b3845f97e9fdbc075d)*
- `src/runtime`  
*[dotnet/runtime@9c1f53e](https://github.com/dotnet/runtime/tree/9c1f53e39f48b09be71097f1b7a47e45331e4906)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@39284fb](https://github.com/dotnet/scenario-tests/tree/39284fbc776975659af4fd377b683b11be053cbb)*
- `src/sdk`  
*[dotnet/sdk@408ca9f](https://github.com/dotnet/sdk/tree/408ca9ff41f0e742a225fb68b62166ba96bfefe4)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f85bef3](https://github.com/dotnet/source-build-externals/tree/f85bef35b34955a287e21a32f3107b24b9514723)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/vs-solutionpersistence`  
    *[microsoft/vs-solutionpersistence@3489af8](https://github.com/microsoft/vs-solutionpersistence/tree/3489af847b089e729a641a6051a02990245e8716)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@82543a6](https://github.com/xunit/xunit/tree/82543a6df6f5f13b5b70f8a9f9ccb41cd676084f)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@cac8b68](https://github.com/xunit/assert.xunit/tree/cac8b688c193c0f244a0bedf3bb60feeb32d377a)*
    - `src/source-build-externals/src/xunit/tools/builder/common`  
    *[xunit/build-tools-v3@90dba1f](https://github.com/xunit/build-tools-v3/tree/90dba1f5638a4f00d4978a73e23edde5b85061d9)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@346f2ca](https://github.com/dotnet/source-build-reference-packages/tree/346f2ca639adb1b363592e27bf62aba52916ea3f)*
- `src/sourcelink`  
*[dotnet/sourcelink@02c795c](https://github.com/dotnet/sourcelink/tree/02c795c78776db6051730c33d80324228d602255)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@86e492a](https://github.com/dotnet/templating/tree/86e492aaa79228d60271d59fc1d063a9ecf094bb)*
- `src/test-templates`  
*[dotnet/test-templates@37e5097](https://github.com/dotnet/test-templates/tree/37e5097eeb9196aadfbbde131f58a5df63476f93)*
- `src/vstest`  
*[microsoft/vstest@eb00b26](https://github.com/microsoft/vstest/tree/eb00b269d6b8734597b8ea888219e105144e7794)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@1b5303e](https://github.com/dotnet/windowsdesktop/tree/1b5303ecc1e869051f707a98a36656a165d87266)*
- `src/winforms`  
*[dotnet/winforms@ef16383](https://github.com/dotnet/winforms/tree/ef1638398be38c549106049a7c99943bab2f83f1)*
- `src/wpf`  
*[dotnet/wpf@4841c5d](https://github.com/dotnet/wpf/tree/4841c5d7a6215ce5aa827c6b02775bb2221ffbec)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
