# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@97e2f41](https://github.com/dotnet/arcade/tree/97e2f41e909dcabb1103fe98ba4540a246662187)*
- `src/aspire`  
*[dotnet/aspire@9d6b4ff](https://github.com/dotnet/aspire/tree/9d6b4ff780da64c972637368d6e1f58afef9535f)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@f18510c](https://github.com/dotnet/aspnetcore/tree/f18510c2fbdfc11b8144ced4857b3d878fa70965)*
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
*[dotnet/efcore@32b6fe4](https://github.com/dotnet/efcore/tree/32b6fe47020ef50d3ab442dc59b09caf1efb06d3)*
- `src/emsdk`  
*[dotnet/emsdk@19c9523](https://github.com/dotnet/emsdk/tree/19c9523f5c2dd091b49959700723af795d6ad2b4)*
- `src/fsharp`  
*[dotnet/fsharp@9881e3b](https://github.com/dotnet/fsharp/tree/9881e3b215a6b0ae64b83ffacac518314ca6a19d)*
- `src/installer`  
*[dotnet/installer@a8a8fd3](https://github.com/dotnet/installer/tree/a8a8fd30e27cace96fc336444988c0599466c388)*
- `src/msbuild`  
*[dotnet/msbuild@9cdb361](https://github.com/dotnet/msbuild/tree/9cdb3615adb4115f92b390de2f258fac5f320909)*
- `src/nuget-client`  
*[nuget/nuget.client@252bbee](https://github.com/nuget/nuget.client/tree/252bbee32a839623dc9af267f950d21cde5e9497)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@7ee5016](https://github.com/dotnet/razor/tree/7ee5016ba99c249660431ea1867cfb12b1b106c4)*
- `src/roslyn`  
*[dotnet/roslyn@c5b98ae](https://github.com/dotnet/roslyn/tree/c5b98ae61ab894230786db2cde711ce4525a2597)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b07c100](https://github.com/dotnet/roslyn-analyzers/tree/b07c100bfc66013a8444172d00cfa04c9ceb5a97)*
- `src/runtime`  
*[dotnet/runtime@d92ac1f](https://github.com/dotnet/runtime/tree/d92ac1f892a7f9c00561db6541671f25f3972eed)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4ab0700](https://github.com/dotnet/scenario-tests/tree/4ab07002cb46cf169c85a09a546709a20642c65b)*
- `src/sdk`  
*[dotnet/sdk@e209541](https://github.com/dotnet/sdk/tree/e2095416638437d2b084521306fa229abd819284)*
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
*[dotnet/templating@c0add26](https://github.com/dotnet/templating/tree/c0add2639e0f6321a13b1e6f6e310d9fc2524872)*
- `src/test-templates`  
*[dotnet/test-templates@31ab6a2](https://github.com/dotnet/test-templates/tree/31ab6a2d0e3b19f17c4126c859564ccc3a0cae94)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@c730f1c](https://github.com/dotnet/windowsdesktop/tree/c730f1cc15c964ff65f1f5a6751cdd6c1fed1a46)*
- `src/winforms`  
*[dotnet/winforms@c0dad19](https://github.com/dotnet/winforms/tree/c0dad198e0c75e50ebefaea76a5bf42f4f0b25ba)*
- `src/wpf`  
*[dotnet/wpf@04d8cc3](https://github.com/dotnet/wpf/tree/04d8cc359f518cdb16ac7ffecf23f876fded0155)*
- `src/xdt`  
*[dotnet/xdt@75b4429](https://github.com/dotnet/xdt/tree/75b4429c85a3bfe0af11c83048259d0ad6ca6611)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
