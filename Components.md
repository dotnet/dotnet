# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@be93330](https://github.com/dotnet/arcade/tree/be933308b9024d798a9a22c0b8f3c8e3616ffbd8)*
- `src/aspire`  
*[dotnet/aspire@0514ea9](https://github.com/dotnet/aspire/tree/0514ea9e12ece4dd764824ce925ae0eae6fcbd86)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@923b62f](https://github.com/dotnet/aspnetcore/tree/923b62f1d56cadfcef43b70df3940a9324c8391d)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@d83fee1](https://github.com/google/googletest/tree/d83fee138a9ae6cb7c03688a2d08d4043a39815d)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@4abe3e6](https://github.com/dotnet/cecil/tree/4abe3e63a5d4653ca098c633644432c1395411c1)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@6b80783](https://github.com/dotnet/deployment-tools/tree/6b80783f6743ee9f18940eb6acb7135e5c111d4b)*
- `src/diagnostics`  
*[dotnet/diagnostics@831eee3](https://github.com/dotnet/diagnostics/tree/831eee3a9e69dd886fa190a9914a7f66260c653a)*
- `src/efcore`  
*[dotnet/efcore@34b34b9](https://github.com/dotnet/efcore/tree/34b34b98e9ebf367bf2e212289c6b7fec9742521)*
- `src/emsdk`  
*[dotnet/emsdk@53288f8](https://github.com/dotnet/emsdk/tree/53288f87c588907e8ff01f129786820fe998573c)*
- `src/fsharp`  
*[dotnet/fsharp@3ef1cb2](https://github.com/dotnet/fsharp/tree/3ef1cb25ffb292b5c87f9604d1a09b032277bf76)*
- `src/msbuild`  
*[dotnet/msbuild@d542f3a](https://github.com/dotnet/msbuild/tree/d542f3a80101883083b95fff0b67666b63002751)*
- `src/nuget-client`  
*[nuget/nuget.client@4020570](https://github.com/nuget/nuget.client/tree/4020570fd78096035b551a5ac0869ca995f4dbed)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@05aed40](https://github.com/dotnet/razor/tree/05aed40d356c6a9f2e4e662a7031e62293abc75a)*
- `src/roslyn`  
*[dotnet/roslyn@5e95dce](https://github.com/dotnet/roslyn/tree/5e95dce38575ad63d86a923162e21740f2b2ab33)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@d7a6ffa](https://github.com/dotnet/runtime/tree/d7a6ffa857ee8bee4732604c0cb7e9008f0df90c)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@4ab0700](https://github.com/dotnet/scenario-tests/tree/4ab07002cb46cf169c85a09a546709a20642c65b)*
- `src/sdk`  
*[dotnet/sdk@f5d2a18](https://github.com/dotnet/sdk/tree/f5d2a18d76ac17829785c172433325db6b9e77ab)*
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
*[dotnet/templating@041b402](https://github.com/dotnet/templating/tree/041b402d33bce09977b15e9dda45ebbf46a45afc)*
- `src/test-templates`  
*[dotnet/test-templates@a74fa37](https://github.com/dotnet/test-templates/tree/a74fa370b91e752af249db46fdf782c742031db8)*
- `src/vstest`  
*[microsoft/vstest@56d2884](https://github.com/microsoft/vstest/tree/56d28849af08dc3143d019694aa92f186b89d2ac)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@7e74c87](https://github.com/dotnet/windowsdesktop/tree/7e74c87196cd78e83b8bcf9e319d4a44823a3852)*
- `src/winforms`  
*[dotnet/winforms@a94848e](https://github.com/dotnet/winforms/tree/a94848e40a75046430d3718f512bdabde3687096)*
- `src/wpf`  
*[dotnet/wpf@33343cb](https://github.com/dotnet/wpf/tree/33343cb58a1b3967c2ffe3e43f3b1fd4b03f41dd)*
- `src/xdt`  
*[dotnet/xdt@043a1f6](https://github.com/dotnet/xdt/tree/043a1f6e332228705b0b64cd3fdc3cdba1ee682d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
