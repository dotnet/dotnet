# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@6eb545a](https://github.com/dotnet/arcade/tree/6eb545ab682b04dc72cf574252b7843ec657dd99)*
- `src/aspire`  
*[dotnet/aspire@7f919f9](https://github.com/dotnet/aspire/tree/7f919f99dee96dd326cedeb1c6240f6352754385)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@7d53092](https://github.com/dotnet/aspnetcore/tree/7d5309210d8f7bae8fa074da495e9d009d67f1b4)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b1a777f](https://github.com/google/googletest/tree/b1a777f31913f8a047f43b2a5f823e736e7f5082)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@861f49c](https://github.com/dotnet/cecil/tree/861f49c137941b9722a43e5993ccac7716c8528c)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@6b80783](https://github.com/dotnet/deployment-tools/tree/6b80783f6743ee9f18940eb6acb7135e5c111d4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/efcore`  
*[dotnet/efcore@ef5f7a3](https://github.com/dotnet/efcore/tree/ef5f7a3d7208a5d6f3d82ceeb44da26b19446c83)*
- `src/emsdk`  
*[dotnet/emsdk@19c9523](https://github.com/dotnet/emsdk/tree/19c9523f5c2dd091b49959700723af795d6ad2b4)*
- `src/fsharp`  
*[dotnet/fsharp@27efc28](https://github.com/dotnet/fsharp/tree/27efc282d3c1e3abdf8322191e09d0fe466c489e)*
- `src/installer`  
*[dotnet/installer@d8c790d](https://github.com/dotnet/installer/tree/d8c790d5e8852e78855f56cda60425283508919d)*
- `src/msbuild`  
*[dotnet/msbuild@195e928](https://github.com/dotnet/msbuild/tree/195e9289b39c4e5cd5d25e9422fcc4a93f95cefa)*
- `src/nuget-client`  
*[nuget/nuget.client@e935504](https://github.com/nuget/nuget.client/tree/e9355040881280a661c9d8f9428d07aba1d05d97)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@53f8bea](https://github.com/dotnet/razor/tree/53f8beaad5a205928f9bad24793e3dd44fe40c32)*
- `src/roslyn`  
*[dotnet/roslyn@e465daf](https://github.com/dotnet/roslyn/tree/e465daf014a2b82e87d36eaef0942a07c2e44812)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b07c100](https://github.com/dotnet/roslyn-analyzers/tree/b07c100bfc66013a8444172d00cfa04c9ceb5a97)*
- `src/runtime`  
*[dotnet/runtime@b0118be](https://github.com/dotnet/runtime/tree/b0118bebe385cb68f272a32ca6a5c5bb53c9b14f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4e95e33](https://github.com/dotnet/scenario-tests/tree/4e95e339e9ea1dfb6203122f3fd6b42296e95f39)*
- `src/sdk`  
*[dotnet/sdk@a4242bf](https://github.com/dotnet/sdk/tree/a4242bf39e7f39c999e5a699931a5bf9f7821bc5)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@5a27364](https://github.com/dotnet/source-build-externals/tree/5a273649709de76f61957e3d69e1f031e5ac82e2)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@c1c24e2](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/c1c24e29d5eeac2a2cd53fe0b5656924bdb69e3d)*
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
*[dotnet/source-build-reference-packages@c0b5d69](https://github.com/dotnet/source-build-reference-packages/tree/c0b5d69a1a1513528c77fffff708c7502d57c35c)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@878e6de](https://github.com/dotnet/templating/tree/878e6de43abbb6e988ce2134bce1100c8c911674)*
- `src/test-templates`  
*[dotnet/test-templates@0a1074f](https://github.com/dotnet/test-templates/tree/0a1074fa121ae028c9f4ed06effcf0480cf08d7d)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@c8b6709](https://github.com/dotnet/windowsdesktop/tree/c8b67095ab5f6342f86e1fb32f8f8038dd381352)*
- `src/winforms`  
*[dotnet/winforms@c7aef4e](https://github.com/dotnet/winforms/tree/c7aef4e2932a2ee77d3f923dd18ab9fb65566348)*
- `src/wpf`  
*[dotnet/wpf@b769d01](https://github.com/dotnet/wpf/tree/b769d01cd9e41e755513d0c2cee577e838d5ab80)*
- `src/xdt`  
*[dotnet/xdt@75b4429](https://github.com/dotnet/xdt/tree/75b4429c85a3bfe0af11c83048259d0ad6ca6611)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
