# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@2a3bf4e](https://github.com/dotnet/arcade/tree/2a3bf4e3a4c473135d058adcd7193a5a4bcd38a7)*
- `src/aspire`  
*[dotnet/aspire@5fa9337](https://github.com/dotnet/aspire/tree/5fa9337a84a52e9bd185d04d156eccbdcf592f74)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@35c31ca](https://github.com/dotnet/aspnetcore/tree/35c31ca295d83c938e159abae0a7aa65ed46d16f)*
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
*[dotnet/fsharp@e9dab83](https://github.com/dotnet/fsharp/tree/e9dab83bc86ec414b7288d3f8be48a2b14eabb5d)*
- `src/msbuild`  
*[dotnet/msbuild@37fc828](https://github.com/dotnet/msbuild/tree/37fc8280dd8516257e7d04b9fc5e426de33091f2)*
- `src/nuget-client`  
*[nuget/nuget.client@cee6d94](https://github.com/nuget/nuget.client/tree/cee6d943f9a6fa38daf5adba4d4dae9d230e83c6)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@6db4477](https://github.com/dotnet/razor/tree/6db4477bba491530b660bea8c98bc6709c6446f7)*
- `src/roslyn`  
*[dotnet/roslyn@543cb45](https://github.com/dotnet/roslyn/tree/543cb4568f28b0d2f2cfecdf2d56365b9252e848)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5435ba7](https://github.com/dotnet/roslyn-analyzers/tree/5435ba7b1037f21237adc1b3845f97e9fdbc075d)*
- `src/runtime`  
*[dotnet/runtime@b66200e](https://github.com/dotnet/runtime/tree/b66200e5448be50673b0387ca4632d3bfa25887b)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@c7a3689](https://github.com/dotnet/scenario-tests/tree/c7a36892e292d8c644662e823913ed82540be66f)*
- `src/sdk`  
*[dotnet/sdk@b05ca6b](https://github.com/dotnet/sdk/tree/b05ca6b0d3044a053b883d147157d11b245ebdee)*
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
*[dotnet/source-build-reference-packages@239b264](https://github.com/dotnet/source-build-reference-packages/tree/239b264c86cb505180968ab98adee25145902fc3)*
- `src/sourcelink`  
*[dotnet/sourcelink@cb197e7](https://github.com/dotnet/sourcelink/tree/cb197e7177e288db2eba6357982f9e7c397951bb)*
- `src/symreader`  
*[dotnet/symreader@8783523](https://github.com/dotnet/symreader/tree/878352351804a2339d595c1f74f9e6b32c6c6e6b)*
- `src/templating`  
*[dotnet/templating@a0a5bac](https://github.com/dotnet/templating/tree/a0a5baca73c2a056cb4ab5cd8f6c9f33c3718452)*
- `src/test-templates`  
*[dotnet/test-templates@0171225](https://github.com/dotnet/test-templates/tree/01712257e7ac9363b002637d399206fd93fc724b)*
- `src/vstest`  
*[microsoft/vstest@7d34b30](https://github.com/microsoft/vstest/tree/7d34b30433259fb914aaaf276fde663a47b6ef2f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@1bb8762](https://github.com/dotnet/windowsdesktop/tree/1bb8762826ee0ab83551712c468d4acc970dd2a8)*
- `src/winforms`  
*[dotnet/winforms@ac8b760](https://github.com/dotnet/winforms/tree/ac8b7609d7b4afb768db1bccf40cd0b3eb30f562)*
- `src/wpf`  
*[dotnet/wpf@d32d194](https://github.com/dotnet/wpf/tree/d32d194d44ccca2dd580ba87de46e160343aaa88)*
- `src/xdt`  
*[dotnet/xdt@1a54480](https://github.com/dotnet/xdt/tree/1a54480f52703fb45fac2a6b955247d33758383e)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
