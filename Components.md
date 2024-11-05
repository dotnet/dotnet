# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1818ed2](https://github.com/dotnet/arcade/tree/1818ed2babf890a1cd62fa96a1f03abdada2d003)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@a3b3c6e](https://github.com/dotnet/aspnetcore/tree/a3b3c6e6e9fc473f8c0871acff54b86c00d77947)*
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
*[dotnet/fsharp@6dcaa8d](https://github.com/dotnet/fsharp/tree/6dcaa8d887951003c56332dc40be6af56dd40c69)*
- `src/msbuild`  
*[dotnet/msbuild@5facb26](https://github.com/dotnet/msbuild/tree/5facb26eb231af66154531735cbc1a0026e897f0)*
- `src/nuget-client`  
*[nuget/nuget.client@aa7eb99](https://github.com/nuget/nuget.client/tree/aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8bf9ad1](https://github.com/dotnet/razor/tree/8bf9ad1ce4cfc0d77916f8db993e2d7f29b22665)*
- `src/roslyn`  
*[dotnet/roslyn@cbb2d12](https://github.com/dotnet/roslyn/tree/cbb2d124058fc80051c9d5e9d798d22e8d5aaab3)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@d6e7d82](https://github.com/dotnet/roslyn-analyzers/tree/d6e7d82e631f0b4c2519284a1c12ed9eb945a388)*
- `src/runtime`  
*[dotnet/runtime@5b56f15](https://github.com/dotnet/runtime/tree/5b56f15ef5ac5ded75b342d31759d22d2202764d)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@39284fb](https://github.com/dotnet/scenario-tests/tree/39284fbc776975659af4fd377b683b11be053cbb)*
- `src/sdk`  
*[dotnet/sdk@cf2c97a](https://github.com/dotnet/sdk/tree/cf2c97a3d005a3c300f636244cafa1f290e9c448)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@a1ca5c7](https://github.com/dotnet/source-build-externals/tree/a1ca5c7e17a24e3a55c911cc42f51881c1990dac)*
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
*[dotnet/source-build-reference-packages@136e43e](https://github.com/dotnet/source-build-reference-packages/tree/136e43e45e20bd58bf86eeabba0a0fa7e1a4b3ae)*
- `src/sourcelink`  
*[dotnet/sourcelink@2dd43fb](https://github.com/dotnet/sourcelink/tree/2dd43fb4a61fa12a918203f91a85bb22e33ce4c9)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@8aae0b2](https://github.com/dotnet/templating/tree/8aae0b239e10a9cc26fd6ead99420ea2b9e2fda6)*
- `src/test-templates`  
*[dotnet/test-templates@cfb55bf](https://github.com/dotnet/test-templates/tree/cfb55bfa45eea729ab2ac64f44d47b7547dc979c)*
- `src/vstest`  
*[microsoft/vstest@eb00b26](https://github.com/microsoft/vstest/tree/eb00b269d6b8734597b8ea888219e105144e7794)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@6be1aec](https://github.com/dotnet/windowsdesktop/tree/6be1aecc178386c490f71dc4b417717882c09f57)*
- `src/winforms`  
*[dotnet/winforms@c9aa1c9](https://github.com/dotnet/winforms/tree/c9aa1c92b9927cfa7ef959d8b735b192cfc50ccf)*
- `src/wpf`  
*[dotnet/wpf@68baa81](https://github.com/dotnet/wpf/tree/68baa81fd05a248be5286d238bc85e67fc15b0c9)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
