# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@91ce594](https://github.com/dotnet/arcade/tree/91ce5946fec9f81f15f81d20e1aab63527302410)*
- `src/aspire`  
*[dotnet/aspire@9d6b4ff](https://github.com/dotnet/aspire/tree/9d6b4ff780da64c972637368d6e1f58afef9535f)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@839d252](https://github.com/dotnet/aspnetcore/tree/839d2527fb7a5ec63342bfae04d0e739d3e1c76d)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@5a37b51](https://github.com/google/googletest/tree/5a37b517ad4ab6738556f0284c256cae1466c5b4)*
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
*[dotnet/efcore@97e7174](https://github.com/dotnet/efcore/tree/97e717475b14b93aa59e66b19993159e609a0229)*
- `src/emsdk`  
*[dotnet/emsdk@7546e31](https://github.com/dotnet/emsdk/tree/7546e312322a7264183c967b9b59c1a4e825b570)*
- `src/fsharp`  
*[dotnet/fsharp@3ef1cb2](https://github.com/dotnet/fsharp/tree/3ef1cb25ffb292b5c87f9604d1a09b032277bf76)*
- `src/installer`  
*[dotnet/installer@df80b5e](https://github.com/dotnet/installer/tree/df80b5eb607242b1d8ded158ec97a25e5d5e5e05)*
- `src/msbuild`  
*[dotnet/msbuild@bd0b1e4](https://github.com/dotnet/msbuild/tree/bd0b1e4667e708d5cf24e116516ee6f18ad20cd7)*
- `src/nuget-client`  
*[nuget/nuget.client@12d5c66](https://github.com/nuget/nuget.client/tree/12d5c661b77d4933f82a293008c0d56d1d6ce32b)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@26ce354](https://github.com/dotnet/razor/tree/26ce354a9fc38830a0f98ca28a7ae9050a52463e)*
- `src/roslyn`  
*[dotnet/roslyn@fa7ece1](https://github.com/dotnet/roslyn/tree/fa7ece1ca25ef529db616ed6c839af9f027fa216)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b07c100](https://github.com/dotnet/roslyn-analyzers/tree/b07c100bfc66013a8444172d00cfa04c9ceb5a97)*
- `src/runtime`  
*[dotnet/runtime@2270e31](https://github.com/dotnet/runtime/tree/2270e3185f538a1581dcf11dd5317da1bd92909c)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4ab0700](https://github.com/dotnet/scenario-tests/tree/4ab07002cb46cf169c85a09a546709a20642c65b)*
- `src/sdk`  
*[dotnet/sdk@7bda360](https://github.com/dotnet/sdk/tree/7bda36013487a9c13cae18b05750d1506a452fc2)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@1223ec4](https://github.com/dotnet/source-build-externals/tree/1223ec47c74e79d44950d429a36386de6c7bf9c8)*
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
*[dotnet/templating@bd885d2](https://github.com/dotnet/templating/tree/bd885d2e7e7173780d0b08713c451b8ccbf92892)*
- `src/test-templates`  
*[dotnet/test-templates@31ab6a2](https://github.com/dotnet/test-templates/tree/31ab6a2d0e3b19f17c4126c859564ccc3a0cae94)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@3b04376](https://github.com/dotnet/windowsdesktop/tree/3b04376f7d83eb49c68b32eeffa1424010220318)*
- `src/winforms`  
*[dotnet/winforms@68d5dda](https://github.com/dotnet/winforms/tree/68d5ddae55848adac78609e7cc306941c2819843)*
- `src/wpf`  
*[dotnet/wpf@9d2ebe4](https://github.com/dotnet/wpf/tree/9d2ebe43fe4cb27a7649d6d3435104968178a772)*
- `src/xdt`  
*[dotnet/xdt@75b4429](https://github.com/dotnet/xdt/tree/75b4429c85a3bfe0af11c83048259d0ad6ca6611)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
