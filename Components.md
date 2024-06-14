# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@d20e5f7](https://github.com/dotnet/arcade/tree/d20e5f76ae843c2f1fe3ba5d05ba3fbf4547e719)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@7f2dfb0](https://github.com/dotnet/aspnetcore/tree/7f2dfb0020ff29ab57df31e910f2b2d5ef48bf82)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@a7f443b](https://github.com/google/googletest/tree/a7f443b80b105f940225332ed3c31f2790092f47)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@7a4a59f](https://github.com/dotnet/cecil/tree/7a4a59f9f66baf6711a6ce2de01d3b2c62ed72d8)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@d882ae4](https://github.com/dotnet/deployment-tools/tree/d882ae4af9fb09a89e36487a9c8cb7dfde713927)*
- `src/diagnostics`  
*[dotnet/diagnostics@3f6ec18](https://github.com/dotnet/diagnostics/tree/3f6ec187afd27d5f242c9275ca1b6e16c43a1b0c)*
- `src/efcore`  
*[dotnet/efcore@e969995](https://github.com/dotnet/efcore/tree/e969995b2701f228815259941467104b6ee8dbee)*
- `src/emsdk`  
*[dotnet/emsdk@fae2c95](https://github.com/dotnet/emsdk/tree/fae2c9534679912d43304de91e622f63e7110919)*
- `src/fsharp`  
*[dotnet/fsharp@887849a](https://github.com/dotnet/fsharp/tree/887849ab11fafb6e5da2e29ecbbcfd292cb6cf5c)*
- `src/msbuild`  
*[dotnet/msbuild@8f22862](https://github.com/dotnet/msbuild/tree/8f2286275c24562f0b4b82cef9ff831bd10420c6)*
- `src/nuget-client`  
*[nuget/nuget.client@9985c40](https://github.com/nuget/nuget.client/tree/9985c402f27c2dd8c04196749fe7b184be870ca1)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@0b7c3a0](https://github.com/dotnet/razor/tree/0b7c3a0bc36e8e82845e7e29a88934b774ba0f36)*
- `src/roslyn`  
*[dotnet/roslyn@0b83719](https://github.com/dotnet/roslyn/tree/0b8371953e61f6179f39f1d62ebbd6a251f335e0)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@52913b2](https://github.com/dotnet/roslyn-analyzers/tree/52913b28e20a224a5f6d56fe0b84b13210e274b0)*
- `src/runtime`  
*[dotnet/runtime@8fac5af](https://github.com/dotnet/runtime/tree/8fac5af2b11dc98fa0504f6fd06df790164ec958)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@54700bb](https://github.com/dotnet/scenario-tests/tree/54700bbee86f660d37bd519a905b62bb50adc8c8)*
- `src/sdk`  
*[dotnet/sdk@2e3f05f](https://github.com/dotnet/sdk/tree/2e3f05f178221464cc4a53f4f04d45a7e10db66c)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@7db0052](https://github.com/dotnet/source-build-externals/tree/7db00527ef8fbbe61f67e9295beebddf187efff8)*
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
*[dotnet/source-build-reference-packages@9ae78a4](https://github.com/dotnet/source-build-reference-packages/tree/9ae78a4e6412926d19ba97cfed159bf9de70b538)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@df69655](https://github.com/dotnet/templating/tree/df6965525ff1b8bf600b553976ef37dd5f093e49)*
- `src/test-templates`  
*[dotnet/test-templates@3f9ca30](https://github.com/dotnet/test-templates/tree/3f9ca30feb8df458b808672e4ac14fb544bdd8ed)*
- `src/vstest`  
*[microsoft/vstest@614363f](https://github.com/microsoft/vstest/tree/614363f91499f2fbebe9f7cc1a622b3d9fc32033)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@20ae128](https://github.com/dotnet/windowsdesktop/tree/20ae1288baaffc436184580b3035c6d3d0b3c828)*
- `src/winforms`  
*[dotnet/winforms@889fca6](https://github.com/dotnet/winforms/tree/889fca6df2aa684e3557ef7737b443ed3af32455)*
- `src/wpf`  
*[dotnet/wpf@e2be737](https://github.com/dotnet/wpf/tree/e2be7375951f8cb691d3436c4eca35ddff6bb01e)*
- `src/xdt`  
*[dotnet/xdt@3081f87](https://github.com/dotnet/xdt/tree/3081f87e9bf53c96a5b692f1de4bd530c4447080)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
