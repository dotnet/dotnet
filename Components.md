# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e8de341](https://github.com/dotnet/arcade/tree/e8de3415124309210e4cbd0105e4a9da8dc01696)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@d69eb7e](https://github.com/dotnet/aspnetcore/tree/d69eb7e46ead086293f2b3a5a2a2ae6eff2a926c)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@35d0c36](https://github.com/google/googletest/tree/35d0c365609296fa4730d62057c487e3cfa030ff)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@b897087](https://github.com/dotnet/cecil/tree/b897087e8b76481a9213ae422f5dc16f64a124b5)*
- `src/command-line-api`  
*[dotnet/command-line-api@feb61c7](https://github.com/dotnet/command-line-api/tree/feb61c7f328a2401d74f4317b39d02126cfdfe24)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@973af82](https://github.com/dotnet/deployment-tools/tree/973af82c66a8907a8a565a305d88ae8151cc75b1)*
- `src/diagnostics`  
*[dotnet/diagnostics@8c505ca](https://github.com/dotnet/diagnostics/tree/8c505ca6921b5f7e9b8acc234cc8f15035537ee4)*
- `src/efcore`  
*[dotnet/efcore@c70d281](https://github.com/dotnet/efcore/tree/c70d281fdb63167214b9e405cb33acade25ed532)*
- `src/emsdk`  
*[dotnet/emsdk@6755ffe](https://github.com/dotnet/emsdk/tree/6755ffefdb9899c08738941d7498d880bc25e52d)*
- `src/fsharp`  
*[dotnet/fsharp@3dc980e](https://github.com/dotnet/fsharp/tree/3dc980eeb06dd912f1e6fe5d06a23a67a4b659e7)*
- `src/msbuild`  
*[dotnet/msbuild@37fc828](https://github.com/dotnet/msbuild/tree/37fc8280dd8516257e7d04b9fc5e426de33091f2)*
- `src/nuget-client`  
*[nuget/nuget.client@cee6d94](https://github.com/nuget/nuget.client/tree/cee6d943f9a6fa38daf5adba4d4dae9d230e83c6)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@a235a98](https://github.com/dotnet/razor/tree/a235a980ba3a90d32841bdbcb61602ea2fcaf11d)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@53b9935](https://github.com/dotnet/roslyn-analyzers/tree/53b99356010534ce12654c29a1af4d303e7ff058)*
- `src/runtime`  
*[dotnet/runtime@b66200e](https://github.com/dotnet/runtime/tree/b66200e5448be50673b0387ca4632d3bfa25887b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@f4af055](https://github.com/dotnet/scenario-tests/tree/f4af05597702ef56e9c68c2e9fd2c1bb6ab3124f)*
- `src/sdk`  
*[dotnet/sdk@6dd9f93](https://github.com/dotnet/sdk/tree/6dd9f933788a1587c4fb514fdeb2beecaa40299d)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@476ab5c](https://github.com/dotnet/source-build-externals/tree/476ab5c5aab87185e7232268e564832ea8ca5b56)*
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
*[dotnet/source-build-reference-packages@450c665](https://github.com/dotnet/source-build-reference-packages/tree/450c665d6f838ff0ede862b20e30f80b9d7846a0)*
- `src/sourcelink`  
*[dotnet/sourcelink@928f459](https://github.com/dotnet/sourcelink/tree/928f459936292b3128b949957c7b46ae5ed59e4c)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@157cd53](https://github.com/dotnet/templating/tree/157cd53097ceb5a5e05de83fa94bb37dd203bdcb)*
- `src/test-templates`  
*[dotnet/test-templates@0171225](https://github.com/dotnet/test-templates/tree/01712257e7ac9363b002637d399206fd93fc724b)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@f15ae07](https://github.com/dotnet/windowsdesktop/tree/f15ae0774e83b0dc67f6f8532c48073862b50369)*
- `src/winforms`  
*[dotnet/winforms@d02c89d](https://github.com/dotnet/winforms/tree/d02c89d400d27ac493ed78d1e307c3d4aaaab5d5)*
- `src/wpf`  
*[dotnet/wpf@227c5cf](https://github.com/dotnet/wpf/tree/227c5cfddb3b6098d56706c1c22161d1b6dbe88c)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
