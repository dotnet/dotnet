# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@87b015b](https://github.com/dotnet/arcade/tree/87b015b938e5400d6e57afd7650348c17a764b73)*
- `src/aspire`  
*[dotnet/aspire@7f919f9](https://github.com/dotnet/aspire/tree/7f919f99dee96dd326cedeb1c6240f6352754385)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@7412c71](https://github.com/dotnet/aspnetcore/tree/7412c71cf881392233b5652310ab2b9e7fdf71eb)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b1a777f](https://github.com/google/googletest/tree/b1a777f31913f8a047f43b2a5f823e736e7f5082)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@9c8ea96](https://github.com/dotnet/cecil/tree/9c8ea966df62f764523b51772763e74e71040a92)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@6b80783](https://github.com/dotnet/deployment-tools/tree/6b80783f6743ee9f18940eb6acb7135e5c111d4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/efcore`  
*[dotnet/efcore@45448ef](https://github.com/dotnet/efcore/tree/45448efb1da8914489739bc4116f7a8f6c9374a2)*
- `src/emsdk`  
*[dotnet/emsdk@c42ff64](https://github.com/dotnet/emsdk/tree/c42ff642a91a8aa4345de1728c3fa585ec13e1e6)*
- `src/fsharp`  
*[dotnet/fsharp@d11b46f](https://github.com/dotnet/fsharp/tree/d11b46f7cdaa90ab5663bf16f7f54349204545a9)*
- `src/installer`  
*[dotnet/installer@9da4954](https://github.com/dotnet/installer/tree/9da4954752fcd04b64c5c88fbe42d720ccad6fb1)*
- `src/msbuild`  
*[dotnet/msbuild@3671494](https://github.com/dotnet/msbuild/tree/367149434a51b18724b7a92b419644fabf06604f)*
- `src/nuget-client`  
*[nuget/nuget.client@d01e48a](https://github.com/nuget/nuget.client/tree/d01e48ace560689897c1c642591659126736feb8)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@843b9af](https://github.com/dotnet/razor/tree/843b9af51d1f4913193883b66a1b20f5562ddaa5)*
- `src/roslyn`  
*[dotnet/roslyn@ca66296](https://github.com/dotnet/roslyn/tree/ca66296efa86bd8078508fe7b38b91b415364f78)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b07c100](https://github.com/dotnet/roslyn-analyzers/tree/b07c100bfc66013a8444172d00cfa04c9ceb5a97)*
- `src/runtime`  
*[dotnet/runtime@86a1911](https://github.com/dotnet/runtime/tree/86a1911d7cbff65fc82b373e7fece572c9f0fd52)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4e95e33](https://github.com/dotnet/scenario-tests/tree/4e95e339e9ea1dfb6203122f3fd6b42296e95f39)*
- `src/sdk`  
*[dotnet/sdk@9526350](https://github.com/dotnet/sdk/tree/95263506ef74c8e4cd4a91d1e7bf0e77b7382d0d)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f4e7502](https://github.com/dotnet/source-build-externals/tree/f4e750201aaf6bad67391a52e8138bbd7abe3019)*
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
*[dotnet/test-templates@b471a5d](https://github.com/dotnet/test-templates/tree/b471a5d85c03cbd41eb83fdfb6157362c2c04d9a)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@ab429ef](https://github.com/dotnet/windowsdesktop/tree/ab429efeedfc52f211fbe597bbc8df003706e766)*
- `src/winforms`  
*[dotnet/winforms@9313f05](https://github.com/dotnet/winforms/tree/9313f05e84328c3176eb7d80497c9505cd8aaebf)*
- `src/wpf`  
*[dotnet/wpf@1357005](https://github.com/dotnet/wpf/tree/1357005541a7ca8088e22e79d7b8ecebe4e6afce)*
- `src/xdt`  
*[dotnet/xdt@282e006](https://github.com/dotnet/xdt/tree/282e0064c30d5cef9b1c1cabb12ef6b55775c54d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
