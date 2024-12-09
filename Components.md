# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@45d845e](https://github.com/dotnet/arcade/tree/45d845e04c05fbe5da9838c454bbc3af1df6be81)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@ec77307](https://github.com/dotnet/aspnetcore/tree/ec77307b34ff97c6270638077ab062dc78544a81)*
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
*[dotnet/efcore@20849ed](https://github.com/dotnet/efcore/tree/20849edb8bf9574066a054a92db816a168729b80)*
- `src/emsdk`  
*[dotnet/emsdk@6755ffe](https://github.com/dotnet/emsdk/tree/6755ffefdb9899c08738941d7498d880bc25e52d)*
- `src/fsharp`  
*[dotnet/fsharp@96e77ae](https://github.com/dotnet/fsharp/tree/96e77ae98391e41b5d940081b3c15a4b77a610aa)*
- `src/msbuild`  
*[dotnet/msbuild@be5b082](https://github.com/dotnet/msbuild/tree/be5b0821fe91247c26b8e09fc5d0dcd79adc6a26)*
- `src/nuget-client`  
*[nuget/nuget.client@a96fbf6](https://github.com/nuget/nuget.client/tree/a96fbf6593fa744d07b5dfa7614cfb4e6749e763)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@acf6972](https://github.com/dotnet/razor/tree/acf6972501eb279605fc8345a7e412c15e06b94a)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@7f449a5](https://github.com/dotnet/roslyn-analyzers/tree/7f449a5d6f05c6aed77e4abf85aac2ce19f6a2d6)*
- `src/runtime`  
*[dotnet/runtime@b66200e](https://github.com/dotnet/runtime/tree/b66200e5448be50673b0387ca4632d3bfa25887b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@61173bb](https://github.com/dotnet/scenario-tests/tree/61173bbe1b4ab5f60e760cca9c5fd7eae6e48546)*
- `src/sdk`  
*[dotnet/sdk@bd5f2dd](https://github.com/dotnet/sdk/tree/bd5f2ddcb31de6e34c4dabb14dc0f45652c08e1a)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@e05b759](https://github.com/dotnet/source-build-externals/tree/e05b759f3b6b09e30bf52535375181bfa25e2c49)*
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
    *[microsoft/vs-solutionpersistence@87ee8ea](https://github.com/microsoft/vs-solutionpersistence/tree/87ee8ea069662d55c336a9bd68fe4851d0384fa5)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@82543a6](https://github.com/xunit/xunit/tree/82543a6df6f5f13b5b70f8a9f9ccb41cd676084f)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@cac8b68](https://github.com/xunit/assert.xunit/tree/cac8b688c193c0f244a0bedf3bb60feeb32d377a)*
    - `src/source-build-externals/src/xunit/tools/builder/common`  
    *[xunit/build-tools-v3@90dba1f](https://github.com/xunit/build-tools-v3/tree/90dba1f5638a4f00d4978a73e23edde5b85061d9)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@dd1d66e](https://github.com/dotnet/source-build-reference-packages/tree/dd1d66ed9123ad8f993b1c00711f2c50f7f48a96)*
- `src/sourcelink`  
*[dotnet/sourcelink@599ca6d](https://github.com/dotnet/sourcelink/tree/599ca6dd9547cef0c10c1363fade40b044fed3b7)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@1dd4fde](https://github.com/dotnet/templating/tree/1dd4fdebb07926ab2d807bceda7d80cc9c857a61)*
- `src/test-templates`  
*[dotnet/test-templates@0171225](https://github.com/dotnet/test-templates/tree/01712257e7ac9363b002637d399206fd93fc724b)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@9bae053](https://github.com/dotnet/windowsdesktop/tree/9bae053da57e9108b4bc4dd8f713eaebf61331a0)*
- `src/winforms`  
*[dotnet/winforms@6ecb29a](https://github.com/dotnet/winforms/tree/6ecb29a0b34c73048062257c1cab74b219759260)*
- `src/wpf`  
*[dotnet/wpf@c6bff27](https://github.com/dotnet/wpf/tree/c6bff274b42ea186c4a53aab6e6447cf92aeb1e1)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
