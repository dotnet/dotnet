# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@87b015b](https://github.com/dotnet/arcade/tree/87b015b938e5400d6e57afd7650348c17a764b73)*
- `src/aspire`  
*[dotnet/aspire@413d3ee](https://github.com/dotnet/aspire/tree/413d3ee8cf12372ead951abef360faf5c78fab25)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@8135545](https://github.com/dotnet/aspnetcore/tree/81355450754f0e6471244b7f1f457b866e982a15)*
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
*[dotnet/emsdk@9ad7c26](https://github.com/dotnet/emsdk/tree/9ad7c262f14dc5e40a64030ade7788b36e74adf0)*
- `src/fsharp`  
*[dotnet/fsharp@90a81d7](https://github.com/dotnet/fsharp/tree/90a81d78e3a2780e8fc599fff60c7bfcc5ab4526)*
- `src/installer`  
*[dotnet/installer@573c98a](https://github.com/dotnet/installer/tree/573c98a59edb6e815193326e4c9307fce442c369)*
- `src/msbuild`  
*[dotnet/msbuild@72d49c3](https://github.com/dotnet/msbuild/tree/72d49c37b5a6ece96c57521db5188825f76a9304)*
- `src/nuget-client`  
*[nuget/nuget.client@95b7931](https://github.com/nuget/nuget.client/tree/95b79314dfece146ddcbac902111d9ff946cf514)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@2211229](https://github.com/dotnet/razor/tree/2211229abf8d35a92dafebf7f77bda3c7ffd9168)*
- `src/roslyn`  
*[dotnet/roslyn@84c5476](https://github.com/dotnet/roslyn/tree/84c5476ef3111c9abd78d43e65063280bb7202d9)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@4d72fc1](https://github.com/dotnet/roslyn-analyzers/tree/4d72fc19879fbc78a12d3a84ed60e7d17777d8b7)*
- `src/runtime`  
*[dotnet/runtime@93010da](https://github.com/dotnet/runtime/tree/93010da2628bcd16aaea0288d52a7af8d4542037)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4e95e33](https://github.com/dotnet/scenario-tests/tree/4e95e339e9ea1dfb6203122f3fd6b42296e95f39)*
- `src/sdk`  
*[dotnet/sdk@cf8c245](https://github.com/dotnet/sdk/tree/cf8c24575410adf397c0823fd7061f9451049ea1)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@8356611](https://github.com/dotnet/source-build-externals/tree/83566118e44922c30d146654d42c7c3745cc119d)*
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
*[dotnet/sourcelink@53ec303](https://github.com/dotnet/sourcelink/tree/53ec303043f0338d9f2d57fb6e8076994a79b547)*
- `src/symreader`  
*[dotnet/symreader@7ae564f](https://github.com/dotnet/symreader/tree/7ae564f9397e5d1b6035f6cf9ebe4f6b0b94882a)*
- `src/templating`  
*[dotnet/templating@1961e35](https://github.com/dotnet/templating/tree/1961e3566f686c040d1f9076e47ebf10166ddddf)*
- `src/test-templates`  
*[dotnet/test-templates@4ff595d](https://github.com/dotnet/test-templates/tree/4ff595d7e7a51c0e16088658d6ab7de94f550328)*
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
