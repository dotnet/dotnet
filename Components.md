# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@020255b](https://github.com/dotnet/arcade/tree/020255bcf7d0b8beed7de05338d97396982ae527)*
- `src/aspire`  
*[dotnet/aspire@0514ea9](https://github.com/dotnet/aspire/tree/0514ea9e12ece4dd764824ce925ae0eae6fcbd86)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@da3aa27](https://github.com/dotnet/aspnetcore/tree/da3aa27233a2cec2f6780884f71934b2f5e686ce)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@d83fee1](https://github.com/google/googletest/tree/d83fee138a9ae6cb7c03688a2d08d4043a39815d)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@7a4a59f](https://github.com/dotnet/cecil/tree/7a4a59f9f66baf6711a6ce2de01d3b2c62ed72d8)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@6b80783](https://github.com/dotnet/deployment-tools/tree/6b80783f6743ee9f18940eb6acb7135e5c111d4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@b641817](https://github.com/dotnet/diagnostics/tree/b6418173e784ec41a65c710c559120f8996fca7d)*
- `src/efcore`  
*[dotnet/efcore@2a43dce](https://github.com/dotnet/efcore/tree/2a43dcea27c5245b0fb63320e71c939373baa348)*
- `src/emsdk`  
*[dotnet/emsdk@53288f8](https://github.com/dotnet/emsdk/tree/53288f87c588907e8ff01f129786820fe998573c)*
- `src/fsharp`  
*[dotnet/fsharp@3ef1cb2](https://github.com/dotnet/fsharp/tree/3ef1cb25ffb292b5c87f9604d1a09b032277bf76)*
- `src/msbuild`  
*[dotnet/msbuild@66dcc32](https://github.com/dotnet/msbuild/tree/66dcc32c3344eb76a422917dcb8c9e8f621e18fd)*
- `src/nuget-client`  
*[nuget/nuget.client@0f32917](https://github.com/nuget/nuget.client/tree/0f32917aaba18c2db765fc7ad5bc95ebf12ec58d)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@7a23d44](https://github.com/dotnet/razor/tree/7a23d444842d4b9d287ab6d5d548a3cd6dd059e5)*
- `src/roslyn`  
*[dotnet/roslyn@062ad3d](https://github.com/dotnet/roslyn/tree/062ad3db597a8096b5da2b188dbbbcc7f6137275)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@84b3339](https://github.com/dotnet/runtime/tree/84b33395057737db3ea342a5151feb6b90c1b6f6)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@259edc6](https://github.com/dotnet/scenario-tests/tree/259edc6efe049ed49f9e37890be702a886ba5ed8)*
- `src/sdk`  
*[dotnet/sdk@c17607f](https://github.com/dotnet/sdk/tree/c17607fc4ee1c381db03bb7a623a9a3eb4e343dd)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@b027696](https://github.com/dotnet/source-build-externals/tree/b02769661c9a51985877819e8bdebfbcbee65710)*
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
*[dotnet/source-build-reference-packages@9c1cc99](https://github.com/dotnet/source-build-reference-packages/tree/9c1cc994f8123ec2a923c5179c238c13da1b4ab7)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@cd9256c](https://github.com/dotnet/templating/tree/cd9256c828c215e121ad99deacde3e8fbbb9b4c5)*
- `src/test-templates`  
*[dotnet/test-templates@36e4339](https://github.com/dotnet/test-templates/tree/36e4339a33f9bdf3680591e2a3fcbc421aabc22c)*
- `src/vstest`  
*[microsoft/vstest@b521aa2](https://github.com/microsoft/vstest/tree/b521aa2c9c981f53b85af7c923175a850986173a)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@436e9a8](https://github.com/dotnet/windowsdesktop/tree/436e9a8e566984a5ffe5f022de266f18d9aa9c4b)*
- `src/winforms`  
*[dotnet/winforms@c926856](https://github.com/dotnet/winforms/tree/c926856b3313d34df58e82c73675b4cf30b6e604)*
- `src/wpf`  
*[dotnet/wpf@5a8619a](https://github.com/dotnet/wpf/tree/5a8619ac9bda541fcf0f6639f7efe6e234cc99eb)*
- `src/xdt`  
*[dotnet/xdt@bfa1e2d](https://github.com/dotnet/xdt/tree/bfa1e2d75f668a47c55a8db4e265ac837bc21229)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
