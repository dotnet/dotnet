# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3aba80f](https://github.com/dotnet/arcade/tree/3aba80fecac252e1cdaffcebc0a37a24a960228b)*
- `src/aspire`  
*[dotnet/aspire@9faf59f](https://github.com/dotnet/aspire/tree/9faf59f870abdeb427c51c1380fce84d8163f2f0)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@28481ab](https://github.com/dotnet/aspnetcore/tree/28481ab0d6a31883a6c058d045ca8f72591a7eca)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@a7f443b](https://github.com/google/googletest/tree/a7f443b80b105f940225332ed3c31f2790092f47)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@a1b1b1b](https://github.com/dotnet/Node-Externals/tree/a1b1b1bb01630a6109adf5767d9a2770c6dc5639)*
- `src/cecil`  
*[dotnet/cecil@d145726](https://github.com/dotnet/cecil/tree/d145726036eb9c09a0e3cf03c4f70effd3b31cd7)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@d882ae4](https://github.com/dotnet/deployment-tools/tree/d882ae4af9fb09a89e36487a9c8cb7dfde713927)*
- `src/diagnostics`  
*[dotnet/diagnostics@33d8bf2](https://github.com/dotnet/diagnostics/tree/33d8bf23a6566cd3fb9055acfc9f1141391d5421)*
- `src/efcore`  
*[dotnet/efcore@9a80b6b](https://github.com/dotnet/efcore/tree/9a80b6b085fbe20cdb1c60b549947ecabefe9172)*
- `src/emsdk`  
*[dotnet/emsdk@ffe9afd](https://github.com/dotnet/emsdk/tree/ffe9afdc046cf7a6f82cc7c5796aade54047af64)*
- `src/fsharp`  
*[dotnet/fsharp@2f906b9](https://github.com/dotnet/fsharp/tree/2f906b91f0ee24eb57082b96c731428084089835)*
- `src/msbuild`  
*[dotnet/msbuild@c260ac1](https://github.com/dotnet/msbuild/tree/c260ac1537f50cda642ea27632d82786c670db0b)*
- `src/nuget-client`  
*[nuget/nuget.client@d1d2e26](https://github.com/nuget/nuget.client/tree/d1d2e260de9b8f20175e7766aa88e1ce1ece6b4e)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@0b7c3a0](https://github.com/dotnet/razor/tree/0b7c3a0bc36e8e82845e7e29a88934b774ba0f36)*
- `src/roslyn`  
*[dotnet/roslyn@b6addf7](https://github.com/dotnet/roslyn/tree/b6addf73b05b2c7cf54db037f6f37054dfa59cba)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@43709af](https://github.com/dotnet/roslyn-analyzers/tree/43709af7570da7140fb3e9a5237f55ffb24677e7)*
- `src/runtime`  
*[dotnet/runtime@d38e559](https://github.com/dotnet/runtime/tree/d38e5593427f63e513ca3be11c3fdccb07b88ce1)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@54700bb](https://github.com/dotnet/scenario-tests/tree/54700bbee86f660d37bd519a905b62bb50adc8c8)*
- `src/sdk`  
*[dotnet/sdk@c70ce24](https://github.com/dotnet/sdk/tree/c70ce24205f6d86a023382b0f58bb490ba861b64)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@9a2785b](https://github.com/dotnet/source-build-externals/tree/9a2785b8409e4ee8db848cc2fbfa19b3316a3baa)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@0183521](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/0183521b0f127a214aa28cfb8385acfef8c4aa22)*
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
*[dotnet/symreader@200bba3](https://github.com/dotnet/symreader/tree/200bba3d3695c902b740931b8d9c2ba09d037f24)*
- `src/templating`  
*[dotnet/templating@35d4d26](https://github.com/dotnet/templating/tree/35d4d2654e2ddf43cd0161b049248b818675a1c0)*
- `src/test-templates`  
*[dotnet/test-templates@574efeb](https://github.com/dotnet/test-templates/tree/574efebf9e4d3a768832b4c918968fbf85ead055)*
- `src/vstest`  
*[microsoft/vstest@5b69fd3](https://github.com/microsoft/vstest/tree/5b69fd31169dd07ced917329bbb483f3b73ea98f)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@7e6d714](https://github.com/dotnet/windowsdesktop/tree/7e6d7149d28c29a907d433d9b4d845d4a1ceb693)*
- `src/winforms`  
*[dotnet/winforms@10bd60e](https://github.com/dotnet/winforms/tree/10bd60e8b0bbc4639035a36fb7aed414a3581431)*
- `src/wpf`  
*[dotnet/wpf@2e2b7e7](https://github.com/dotnet/wpf/tree/2e2b7e7bb787c3857f04d65c2faf87a99cc0f83f)*
- `src/xdt`  
*[dotnet/xdt@0d51607](https://github.com/dotnet/xdt/tree/0d51607fb791c51a14b552ed24fe3430c252148b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
