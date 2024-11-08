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
*[dotnet/aspnetcore@dc68e3b](https://github.com/dotnet/aspnetcore/tree/dc68e3b6d51ba96191a58e3b9775b4d43358c30d)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@1204d63](https://github.com/google/googletest/tree/1204d634444b0ba6da53201a8b6caf2a502d883c)*
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
*[dotnet/fsharp@9704831](https://github.com/dotnet/fsharp/tree/970483121932c4cad0c3bf346077393121cddfe9)*
- `src/msbuild`  
*[dotnet/msbuild@8a5384d](https://github.com/dotnet/msbuild/tree/8a5384d8a85ed6e74b55789e42c5383f45591b71)*
- `src/nuget-client`  
*[nuget/nuget.client@aa7eb99](https://github.com/nuget/nuget.client/tree/aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@390aced](https://github.com/dotnet/razor/tree/390aced03bc51b264c0a5ad7d3f1803bcc83f85e)*
- `src/roslyn`  
*[dotnet/roslyn@b0b8e0f](https://github.com/dotnet/roslyn/tree/b0b8e0fe16f29a602422fa93e6366521437a4188)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@d6e7d82](https://github.com/dotnet/roslyn-analyzers/tree/d6e7d82e631f0b4c2519284a1c12ed9eb945a388)*
- `src/runtime`  
*[dotnet/runtime@9c1f53e](https://github.com/dotnet/runtime/tree/9c1f53e39f48b09be71097f1b7a47e45331e4906)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@39284fb](https://github.com/dotnet/scenario-tests/tree/39284fbc776975659af4fd377b683b11be053cbb)*
- `src/sdk`  
*[dotnet/sdk@3247fe5](https://github.com/dotnet/sdk/tree/3247fe5a61e4935109e8a4682c7f7016f6428c59)*
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
*[dotnet/sourcelink@474ceee](https://github.com/dotnet/sourcelink/tree/474ceee2ae8a35d35692586f1a8a0f84fe55f572)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@00aefe7](https://github.com/dotnet/templating/tree/00aefe715463b38b9c96123eacfa8f76d929c599)*
- `src/test-templates`  
*[dotnet/test-templates@1ab4cae](https://github.com/dotnet/test-templates/tree/1ab4caea85ef1ccdb735be3e8a908f4728daf624)*
- `src/vstest`  
*[microsoft/vstest@eb00b26](https://github.com/microsoft/vstest/tree/eb00b269d6b8734597b8ea888219e105144e7794)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@19ba68f](https://github.com/dotnet/windowsdesktop/tree/19ba68fc870eee3aef71a146456ec905c1109ebb)*
- `src/winforms`  
*[dotnet/winforms@65f897b](https://github.com/dotnet/winforms/tree/65f897bc43dd631fac10cecf0571e3d5db782e4f)*
- `src/wpf`  
*[dotnet/wpf@3a1f918](https://github.com/dotnet/wpf/tree/3a1f91800de8d1303032a513b6dbe8bcd6f5a64f)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
