# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@97e2f41](https://github.com/dotnet/arcade/tree/97e2f41e909dcabb1103fe98ba4540a246662187)*
- `src/aspire`  
*[dotnet/aspire@0514ea9](https://github.com/dotnet/aspire/tree/0514ea9e12ece4dd764824ce925ae0eae6fcbd86)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@c157c5c](https://github.com/dotnet/aspnetcore/tree/c157c5c1962a66c4fe0212ed767f167bb9cdfb79)*
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
*[dotnet/efcore@34b34b9](https://github.com/dotnet/efcore/tree/34b34b98e9ebf367bf2e212289c6b7fec9742521)*
- `src/emsdk`  
*[dotnet/emsdk@9d28301](https://github.com/dotnet/emsdk/tree/9d28301f6a5f512db14f242f7403a4bfbcbcc8a4)*
- `src/fsharp`  
*[dotnet/fsharp@3ef1cb2](https://github.com/dotnet/fsharp/tree/3ef1cb25ffb292b5c87f9604d1a09b032277bf76)*
- `src/msbuild`  
*[dotnet/msbuild@d542f3a](https://github.com/dotnet/msbuild/tree/d542f3a80101883083b95fff0b67666b63002751)*
- `src/nuget-client`  
*[nuget/nuget.client@f6615e7](https://github.com/nuget/nuget.client/tree/f6615e7a7177dd820b74b007c5f6b691ceedada3)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@7ae5e9a](https://github.com/dotnet/razor/tree/7ae5e9a0e75c13180bf5aff62c1a616e5f03937c)*
- `src/roslyn`  
*[dotnet/roslyn@b9c4495](https://github.com/dotnet/roslyn/tree/b9c44957977af21c20c4addf8b7b788112285003)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@74d9279](https://github.com/dotnet/runtime/tree/74d927967dcf9dc1801c310603d6dc843dc28ad6)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4ab0700](https://github.com/dotnet/scenario-tests/tree/4ab07002cb46cf169c85a09a546709a20642c65b)*
- `src/sdk`  
*[dotnet/sdk@2673f0c](https://github.com/dotnet/sdk/tree/2673f0c77f1a5ee43030f8feccb270ef28ea98c9)*
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
*[dotnet/test-templates@f23bef1](https://github.com/dotnet/test-templates/tree/f23bef195234bbbe7b96f38d056965269fbb3bc0)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@6be92da](https://github.com/dotnet/windowsdesktop/tree/6be92da62a4abca596025bd9f9e7440f753e97fe)*
- `src/winforms`  
*[dotnet/winforms@c5967ff](https://github.com/dotnet/winforms/tree/c5967ff09814601cc8828668589ab66b946a4b16)*
- `src/wpf`  
*[dotnet/wpf@6f044de](https://github.com/dotnet/wpf/tree/6f044de655b9d41eaade4203c99a14f04edf72c4)*
- `src/xdt`  
*[dotnet/xdt@043a1f6](https://github.com/dotnet/xdt/tree/043a1f6e332228705b0b64cd3fdc3cdba1ee682d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
