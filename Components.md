# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e4df288](https://github.com/dotnet/arcade/tree/e4df2888e714255352a6afa2ec1246e70ac21a49)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@4a3a99d](https://github.com/dotnet/aspnetcore/tree/4a3a99d044d345d7bbf7c279a1bd1776da1144c1)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@df1544b](https://github.com/google/googletest/tree/df1544bcee0c7ce35cd5ea0b3eb8cc81855a4140)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@b897087](https://github.com/dotnet/cecil/tree/b897087e8b76481a9213ae422f5dc16f64a124b5)*
- `src/command-line-api`  
*[dotnet/command-line-api@feb61c7](https://github.com/dotnet/command-line-api/tree/feb61c7f328a2401d74f4317b39d02126cfdfe24)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@ceda306](https://github.com/dotnet/deployment-tools/tree/ceda306bddb87b73dcb27525f04a8df623bab8fe)*
- `src/diagnostics`  
*[dotnet/diagnostics@335c0c0](https://github.com/dotnet/diagnostics/tree/335c0c013c3a761792bfa83c0dbaadd1f0545f07)*
- `src/efcore`  
*[dotnet/efcore@17ed217](https://github.com/dotnet/efcore/tree/17ed217ebb494392e08d8f5634db7eb3bb4ab5ed)*
- `src/emsdk`  
*[dotnet/emsdk@e965cbb](https://github.com/dotnet/emsdk/tree/e965cbbf7eae80e2a62e46f0fcf68ab8f47792c4)*
- `src/fsharp`  
*[dotnet/fsharp@02f6bb0](https://github.com/dotnet/fsharp/tree/02f6bb08e742b0bb1cfd8cd7c104c5ddbcc4cbf8)*
- `src/msbuild`  
*[dotnet/msbuild@0e0f413](https://github.com/dotnet/msbuild/tree/0e0f4130a2a8f20acc957bbab7402de04294c923)*
- `src/nuget-client`  
*[nuget/nuget.client@1975634](https://github.com/nuget/nuget.client/tree/19756345139c45de23bd196e9b4be01d48e84fdd)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8bf9ad1](https://github.com/dotnet/razor/tree/8bf9ad1ce4cfc0d77916f8db993e2d7f29b22665)*
- `src/roslyn`  
*[dotnet/roslyn@57f05a3](https://github.com/dotnet/roslyn/tree/57f05a3c12e332f8408a878852b46237632aaf67)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@d6e7d82](https://github.com/dotnet/roslyn-analyzers/tree/d6e7d82e631f0b4c2519284a1c12ed9eb945a388)*
- `src/runtime`  
*[dotnet/runtime@1b27ee9](https://github.com/dotnet/runtime/tree/1b27ee9197251edcc9167a48b87786d3a6b5c163)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@39284fb](https://github.com/dotnet/scenario-tests/tree/39284fbc776975659af4fd377b683b11be053cbb)*
- `src/sdk`  
*[dotnet/sdk@ae79890](https://github.com/dotnet/sdk/tree/ae79890320430849dad4f2fb17eafeca52011a91)*
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
*[dotnet/source-build-reference-packages@e797b98](https://github.com/dotnet/source-build-reference-packages/tree/e797b98817c7750dc8c21160de78c33d73d277a2)*
- `src/sourcelink`  
*[dotnet/sourcelink@196983c](https://github.com/dotnet/sourcelink/tree/196983c6e97621b051c67cd898c4db630168cd81)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@3f2837c](https://github.com/dotnet/templating/tree/3f2837cb8b9390ff62c1e413a4fc0f65d196cb10)*
- `src/test-templates`  
*[dotnet/test-templates@764e988](https://github.com/dotnet/test-templates/tree/764e98851850e9ebdf7c3ff556f893068bc5871d)*
- `src/vstest`  
*[microsoft/vstest@eb00b26](https://github.com/microsoft/vstest/tree/eb00b269d6b8734597b8ea888219e105144e7794)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@7efc7d9](https://github.com/dotnet/windowsdesktop/tree/7efc7d96e02316bf3195dba2b64cceff232a6ff7)*
- `src/winforms`  
*[dotnet/winforms@132e6e7](https://github.com/dotnet/winforms/tree/132e6e7e5d98bccce3b77dbb3737c3069cb00ec5)*
- `src/wpf`  
*[dotnet/wpf@6c96913](https://github.com/dotnet/wpf/tree/6c96913d693c56922e697b23b3053aa604d3bd2c)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
