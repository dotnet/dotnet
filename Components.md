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
*[dotnet/aspnetcore@cacdb97](https://github.com/dotnet/aspnetcore/tree/cacdb971a55098c93b9a1807407e9b090edcfce4)*
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
*[dotnet/emsdk@8808f75](https://github.com/dotnet/emsdk/tree/8808f75f212bcec6a2050fe045a7e07ab8a5a8c1)*
- `src/fsharp`  
*[dotnet/fsharp@e9dab83](https://github.com/dotnet/fsharp/tree/e9dab83bc86ec414b7288d3f8be48a2b14eabb5d)*
- `src/msbuild`  
*[dotnet/msbuild@37fc828](https://github.com/dotnet/msbuild/tree/37fc8280dd8516257e7d04b9fc5e426de33091f2)*
- `src/nuget-client`  
*[nuget/nuget.client@ce95a56](https://github.com/nuget/nuget.client/tree/ce95a567627472f8abd9d155047392e22142ff72)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@fa5b57f](https://github.com/dotnet/razor/tree/fa5b57f1c7823e60c8bfab5016bd6d3c746fff12)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5435ba7](https://github.com/dotnet/roslyn-analyzers/tree/5435ba7b1037f21237adc1b3845f97e9fdbc075d)*
- `src/runtime`  
*[dotnet/runtime@b5750e6](https://github.com/dotnet/runtime/tree/b5750e65aea5d9e6246cdeb16a52608c454a4e40)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@44c807f](https://github.com/dotnet/scenario-tests/tree/44c807fb89cb9eea8bdec164af363ce6b2ce5abe)*
- `src/sdk`  
*[dotnet/sdk@e6c8e57](https://github.com/dotnet/sdk/tree/e6c8e57159724c0d22640f5eadcd32954276b22f)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@09aaed0](https://github.com/dotnet/source-build-externals/tree/09aaed01f4bd113a1aca58d620096fe77c5a2da7)*
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
*[dotnet/source-build-reference-packages@e8a18f5](https://github.com/dotnet/source-build-reference-packages/tree/e8a18f527c035b4bb65f470d5f85e1fed9f86120)*
- `src/sourcelink`  
*[dotnet/sourcelink@a190fdd](https://github.com/dotnet/sourcelink/tree/a190fddd4aeb983b59b682c984692639c8c7d100)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@2dee524](https://github.com/dotnet/templating/tree/2dee524c182ce6e6f3c98ea15aee54071ad49132)*
- `src/test-templates`  
*[dotnet/test-templates@2adf582](https://github.com/dotnet/test-templates/tree/2adf5822dc94d97d0f6e1584129fcf65ba837097)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@826aeb4](https://github.com/dotnet/windowsdesktop/tree/826aeb454995a0137b05d00ec0a2da4c3d2a9f42)*
- `src/winforms`  
*[dotnet/winforms@dc4314e](https://github.com/dotnet/winforms/tree/dc4314ea49b7cbecd2024e7944d6cf98e01c52fc)*
- `src/wpf`  
*[dotnet/wpf@bbfc24f](https://github.com/dotnet/wpf/tree/bbfc24fd13804a191e20064acd599b0a359092df)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
