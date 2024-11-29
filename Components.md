# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@d0f89c6](https://github.com/dotnet/arcade/tree/d0f89c635d780e183a97ad86af4f3c8d7e95977f)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@81a2bab](https://github.com/dotnet/aspnetcore/tree/81a2bab8704d87d324039b42eb1bab0d977f25b8)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@35d0c36](https://github.com/google/googletest/tree/35d0c365609296fa4730d62057c487e3cfa030ff)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9aeb12b](https://github.com/aspnet/MessagePack-CSharp/tree/9aeb12b9bdb024512ffe2e4bddfa2785dca6e39e)*
- `src/cecil`  
*[dotnet/cecil@b897087](https://github.com/dotnet/cecil/tree/b897087e8b76481a9213ae422f5dc16f64a124b5)*
- `src/command-line-api`  
*[dotnet/command-line-api@feb61c7](https://github.com/dotnet/command-line-api/tree/feb61c7f328a2401d74f4317b39d02126cfdfe24)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@0cae9f6](https://github.com/dotnet/deployment-tools/tree/0cae9f631980a7f09a852f80549274d7b819da70)*
- `src/diagnostics`  
*[dotnet/diagnostics@8c505ca](https://github.com/dotnet/diagnostics/tree/8c505ca6921b5f7e9b8acc234cc8f15035537ee4)*
- `src/efcore`  
*[dotnet/efcore@d1e1dfa](https://github.com/dotnet/efcore/tree/d1e1dfa531dbf2d90df4a42ba71890ed5025dffd)*
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
*[dotnet/razor@5f32f26](https://github.com/dotnet/razor/tree/5f32f2648d0b10317487b25a9dcd17ea1b45dc13)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5435ba7](https://github.com/dotnet/roslyn-analyzers/tree/5435ba7b1037f21237adc1b3845f97e9fdbc075d)*
- `src/runtime`  
*[dotnet/runtime@b66200e](https://github.com/dotnet/runtime/tree/b66200e5448be50673b0387ca4632d3bfa25887b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@5a67083](https://github.com/dotnet/scenario-tests/tree/5a67083512f32fb0187d42caeae3969c958ea1d5)*
- `src/sdk`  
*[dotnet/sdk@52c37fd](https://github.com/dotnet/sdk/tree/52c37fd561fad990a997d8fe6e0289295b4d555f)*
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
*[dotnet/sourcelink@8918549](https://github.com/dotnet/sourcelink/tree/891854929f588c720666cb9fd0a23f498f76f7d3)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@054bc4e](https://github.com/dotnet/templating/tree/054bc4eb47b4c0c6863c3d6935c2f6943dc7f6e1)*
- `src/test-templates`  
*[dotnet/test-templates@0171225](https://github.com/dotnet/test-templates/tree/01712257e7ac9363b002637d399206fd93fc724b)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@831eab0](https://github.com/dotnet/windowsdesktop/tree/831eab0d84a77f326d8738411cf8af01919fea3c)*
- `src/winforms`  
*[dotnet/winforms@ac8b760](https://github.com/dotnet/winforms/tree/ac8b7609d7b4afb768db1bccf40cd0b3eb30f562)*
- `src/wpf`  
*[dotnet/wpf@3195d22](https://github.com/dotnet/wpf/tree/3195d22d78a4fe8a4f4a347a08d4d11999a94e48)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
