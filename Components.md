# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@0c8f498](https://github.com/dotnet/arcade/tree/0c8f4989db0251f49a4f37d1b35bf4da1e9d6415)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@79d06db](https://github.com/dotnet/aspnetcore/tree/79d06db8a4be29165e24eb841054a337161bd09a)*
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
*[dotnet/diagnostics@8c505ca](https://github.com/dotnet/diagnostics/tree/8c505ca6921b5f7e9b8acc234cc8f15035537ee4)*
- `src/efcore`  
*[dotnet/efcore@a4a350a](https://github.com/dotnet/efcore/tree/a4a350a6f77f6260a7f2f2bbd3fe91e5502a3540)*
- `src/emsdk`  
*[dotnet/emsdk@6755ffe](https://github.com/dotnet/emsdk/tree/6755ffefdb9899c08738941d7498d880bc25e52d)*
- `src/fsharp`  
*[dotnet/fsharp@e9dab83](https://github.com/dotnet/fsharp/tree/e9dab83bc86ec414b7288d3f8be48a2b14eabb5d)*
- `src/msbuild`  
*[dotnet/msbuild@37fc828](https://github.com/dotnet/msbuild/tree/37fc8280dd8516257e7d04b9fc5e426de33091f2)*
- `src/nuget-client`  
*[nuget/nuget.client@46b470c](https://github.com/nuget/nuget.client/tree/46b470cbc5033821d470add040adffe3a1ddf76f)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@eef0189](https://github.com/dotnet/razor/tree/eef0189b5fbef8ad4446dc0e409163448dff81e9)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5435ba7](https://github.com/dotnet/roslyn-analyzers/tree/5435ba7b1037f21237adc1b3845f97e9fdbc075d)*
- `src/runtime`  
*[dotnet/runtime@b66200e](https://github.com/dotnet/runtime/tree/b66200e5448be50673b0387ca4632d3bfa25887b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@2acfac2](https://github.com/dotnet/scenario-tests/tree/2acfac2e45092b884281b76fb6fc33fadb1cba58)*
- `src/sdk`  
*[dotnet/sdk@f03cda3](https://github.com/dotnet/sdk/tree/f03cda3dfc01f724fee89760b978c37b749565f7)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@a5a31c7](https://github.com/dotnet/source-build-externals/tree/a5a31c71349868357945ccff44a47a997b4f5717)*
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
    *[microsoft/vs-solutionpersistence@eb339e2](https://github.com/microsoft/vs-solutionpersistence/tree/eb339e21ca8e5beb1a4301c1df73c9a5389738a9)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@82543a6](https://github.com/xunit/xunit/tree/82543a6df6f5f13b5b70f8a9f9ccb41cd676084f)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@cac8b68](https://github.com/xunit/assert.xunit/tree/cac8b688c193c0f244a0bedf3bb60feeb32d377a)*
    - `src/source-build-externals/src/xunit/tools/builder/common`  
    *[xunit/build-tools-v3@90dba1f](https://github.com/xunit/build-tools-v3/tree/90dba1f5638a4f00d4978a73e23edde5b85061d9)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@ef10a9f](https://github.com/dotnet/source-build-reference-packages/tree/ef10a9fae360251bdd4b7552a4c90bf626977453)*
- `src/sourcelink`  
*[dotnet/sourcelink@a190fdd](https://github.com/dotnet/sourcelink/tree/a190fddd4aeb983b59b682c984692639c8c7d100)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@f06a471](https://github.com/dotnet/templating/tree/f06a47155a0d3fe8b9c5837fea38a8b712f939bf)*
- `src/test-templates`  
*[dotnet/test-templates@2adf582](https://github.com/dotnet/test-templates/tree/2adf5822dc94d97d0f6e1584129fcf65ba837097)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@d212e0b](https://github.com/dotnet/windowsdesktop/tree/d212e0b4021f0e5569b9f07e543b3ae681bc2181)*
- `src/winforms`  
*[dotnet/winforms@441fb2a](https://github.com/dotnet/winforms/tree/441fb2a0cace54d5d9d5c9ac397d6b212a457181)*
- `src/wpf`  
*[dotnet/wpf@722025a](https://github.com/dotnet/wpf/tree/722025ad9701120167501f8cc981d2cb2cc9e1ea)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
