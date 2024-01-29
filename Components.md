# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@1d8f27f](https://github.com/dotnet/arcade/tree/1d8f27f89c3b167f63e28e73a3d9ab345e81d310)*
- `src/aspire`  
*[dotnet/aspire@66a1dd7](https://github.com/dotnet/aspire/tree/66a1dd77e4077592a587c1429c8814d1057dc474)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@bd01694](https://github.com/dotnet/aspnetcore/tree/bd016947b6928159814950c06a6e2278be7a256f)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@6a59382](https://github.com/google/googletest/tree/6a5938233b6519ba99ddb7c7314d45d3fa877969)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@b8c2293](https://github.com/dotnet/cecil/tree/b8c2293cd1cbd9d0fe6f32d7b5befbd526b5a175)*
- `src/command-line-api`  
*[dotnet/command-line-api@ecd2ce5](https://github.com/dotnet/command-line-api/tree/ecd2ce5eafbba3008a7d4f5d04b025d30928c812)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@e56c69b](https://github.com/dotnet/deployment-tools/tree/e56c69b0610b50407d29fdc2dda2574712a7b94d)*
- `src/diagnostics`  
*[dotnet/diagnostics@82c9a13](https://github.com/dotnet/diagnostics/tree/82c9a134f8c13dcfe8a9c243a19ee1861bbcb8ea)*
- `src/emsdk`  
*[dotnet/emsdk@ae7c93e](https://github.com/dotnet/emsdk/tree/ae7c93e25ae596594b3b22d64115f374a3595912)*
- `src/format`  
*[dotnet/format@ac63bf9](https://github.com/dotnet/format/tree/ac63bf9b1c8ff196908d75e2442e96025db26c03)*
- `src/fsharp`  
*[dotnet/fsharp@32898dc](https://github.com/dotnet/fsharp/tree/32898dc51efc669de98e7e47f57d521bc07ac4cc)*
- `src/installer`  
*[dotnet/installer@1c49697](https://github.com/dotnet/installer/tree/1c496970b7479284364bc7eaf5c30e59f1cdb2ec)*
- `src/msbuild`  
*[dotnet/msbuild@e7a44d7](https://github.com/dotnet/msbuild/tree/e7a44d757e097eb17dcac5a8436645dc612fec4b)*
- `src/nuget-client`  
*[nuget/nuget.client@d55931a](https://github.com/nuget/nuget.client/tree/d55931a69dcda3dcb87ba46a09fe268e0febc223)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8e5fbd4](https://github.com/dotnet/razor/tree/8e5fbd463e970140b303fcb9a6268148cb4f51ad)*
- `src/roslyn`  
*[dotnet/roslyn@3cd939f](https://github.com/dotnet/roslyn/tree/3cd939f76803da435c20b082a5cfcc844386fcfb)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@4d00bdb](https://github.com/dotnet/roslyn-analyzers/tree/4d00bdb0dc0c863887994feeaf2e0cb07a6f5475)*
- `src/runtime`  
*[dotnet/runtime@bcc1d3d](https://github.com/dotnet/runtime/tree/bcc1d3d6f00fbcea3f454b2e35bceeaa51e604b1)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@3512b92](https://github.com/dotnet/sdk/tree/3512b9275a5af9f78a379acac6f876453918ed83)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@414a85b](https://github.com/dotnet/source-build-externals/tree/414a85bf970355c0e91d6a2de1ee183fafbfcecd)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.0`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
    - `src/source-build-externals/src/docker-creds-provider-2.2.1`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@e659f32](https://github.com/dotnet/source-build-reference-packages/tree/e659f328bf255d3e17e81296117c3aed1d461f2f)*
- `src/sourcelink`  
*[dotnet/sourcelink@b414cec](https://github.com/dotnet/sourcelink/tree/b414cec7c199f9fda8915803d5f70a2e5cc8aa3f)*
- `src/symreader`  
*[dotnet/symreader@2902dcf](https://github.com/dotnet/symreader/tree/2902dcf06494391dc65552fd0743b7d426c550fb)*
- `src/templating`  
*[dotnet/templating@a3512f1](https://github.com/dotnet/templating/tree/a3512f1f92747c73d6dd3a9df99cd324b95bf03a)*
- `src/test-templates`  
*[dotnet/test-templates@5fc17f5](https://github.com/dotnet/test-templates/tree/5fc17f5abd212c64fe9adcc3641283954a7555aa)*
- `src/vstest`  
*[microsoft/vstest@c0c0e75](https://github.com/microsoft/vstest/tree/c0c0e75fb8ea396a8b0717d17c2e738975d412e7)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@fba94d7](https://github.com/dotnet/windowsdesktop/tree/fba94d7a096cb102ee2ea8a2214010d5c02460c4)*
- `src/winforms`  
*[dotnet/winforms@63f2d85](https://github.com/dotnet/winforms/tree/63f2d85bdc754063f935729029b1287f5b22d285)*
- `src/wpf`  
*[dotnet/wpf@bec0472](https://github.com/dotnet/wpf/tree/bec0472431f3c127deb2b9823f17bfd9bfd55c1d)*
- `src/xdt`  
*[dotnet/xdt@d71290d](https://github.com/dotnet/xdt/tree/d71290db981c297b17054b64b2bc7c707a547545)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
