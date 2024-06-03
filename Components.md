# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@e6b3f32](https://github.com/dotnet/arcade/tree/e6b3f32f9855dccbe2447471c8f729b66f17d242)*
- `src/aspire`  
*[dotnet/aspire@0514ea9](https://github.com/dotnet/aspire/tree/0514ea9e12ece4dd764824ce925ae0eae6fcbd86)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@83573c7](https://github.com/dotnet/aspnetcore/tree/83573c72d12e1c2018805bc5d461d730bcb9dcc4)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@9b4993c](https://github.com/google/googletest/tree/9b4993ca7d1279dec5c5d41ba327cb11a77bdc00)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@fb911de](https://github.com/dotnet/Node-Externals/tree/fb911deddbaf7367146718374a403d393571f18a)*
- `src/cecil`  
*[dotnet/cecil@7a4a59f](https://github.com/dotnet/cecil/tree/7a4a59f9f66baf6711a6ce2de01d3b2c62ed72d8)*
- `src/command-line-api`  
*[dotnet/command-line-api@963d34b](https://github.com/dotnet/command-line-api/tree/963d34b1fb712c673bfb198133d7e988182c9ef4)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@4017482](https://github.com/dotnet/deployment-tools/tree/40174825bd85a333428e68e6049bfe7cc28e9061)*
- `src/diagnostics`  
*[dotnet/diagnostics@3f6ec18](https://github.com/dotnet/diagnostics/tree/3f6ec187afd27d5f242c9275ca1b6e16c43a1b0c)*
- `src/efcore`  
*[dotnet/efcore@e969995](https://github.com/dotnet/efcore/tree/e969995b2701f228815259941467104b6ee8dbee)*
- `src/emsdk`  
*[dotnet/emsdk@fae2c95](https://github.com/dotnet/emsdk/tree/fae2c9534679912d43304de91e622f63e7110919)*
- `src/fsharp`  
*[dotnet/fsharp@61cf1c1](https://github.com/dotnet/fsharp/tree/61cf1c139360aec37ccadd3e1a0701fd91fd81fe)*
- `src/msbuild`  
*[dotnet/msbuild@db79545](https://github.com/dotnet/msbuild/tree/db79545e5940a69560c4885b792ef3a359f0de68)*
- `src/nuget-client`  
*[nuget/nuget.client@f1525fc](https://github.com/nuget/nuget.client/tree/f1525fc4017aab865deb9325b5f53aac2d8c4cf4)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@759c0de](https://github.com/dotnet/razor/tree/759c0de08dcded6408de65fdc05dd58c54af8180)*
- `src/roslyn`  
*[dotnet/roslyn@f840f7f](https://github.com/dotnet/roslyn/tree/f840f7f23cf6592c30606dbe581e3325739575d6)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8dcccce](https://github.com/dotnet/roslyn-analyzers/tree/8dccccec1ce3bd2fb532ec77d7e092ab9d684db7)*
- `src/runtime`  
*[dotnet/runtime@3750ac5](https://github.com/dotnet/runtime/tree/3750ac51619efbbc59bf07d3879758a9c18c4b0e)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@54700bb](https://github.com/dotnet/scenario-tests/tree/54700bbee86f660d37bd519a905b62bb50adc8c8)*
- `src/sdk`  
*[dotnet/sdk@ecc5aee](https://github.com/dotnet/sdk/tree/ecc5aeef08c88ce7eee0857dc17b06028b80ffe3)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f1b44a3](https://github.com/dotnet/source-build-externals/tree/f1b44a365df0b8dbc38403ee8696adc25fbdd76a)*
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
*[dotnet/source-build-reference-packages@87ebd07](https://github.com/dotnet/source-build-reference-packages/tree/87ebd07371adfe1d8140889c33da692eb2134f1a)*
- `src/sourcelink`  
*[dotnet/sourcelink@14a0a42](https://github.com/dotnet/sourcelink/tree/14a0a42ffb29b53fb9939f14da5a4be8c6c07e0b)*
- `src/symreader`  
*[dotnet/symreader@409af43](https://github.com/dotnet/symreader/tree/409af431ee684f9e07d34bbd4e51b9933345c1e1)*
- `src/templating`  
*[dotnet/templating@992a268](https://github.com/dotnet/templating/tree/992a26854d23749a01364b6fec3cbaf4fd8a5b24)*
- `src/test-templates`  
*[dotnet/test-templates@820d544](https://github.com/dotnet/test-templates/tree/820d5449b63655f4b63b87077ab6891456e4fa7f)*
- `src/vstest`  
*[microsoft/vstest@614363f](https://github.com/microsoft/vstest/tree/614363f91499f2fbebe9f7cc1a622b3d9fc32033)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@f778f53](https://github.com/dotnet/windowsdesktop/tree/f778f53d3a473d811614cb817d9aa6b5eed43d05)*
- `src/winforms`  
*[dotnet/winforms@c3dd6f4](https://github.com/dotnet/winforms/tree/c3dd6f470693606b407e3fa9f60abc338ab07669)*
- `src/wpf`  
*[dotnet/wpf@b77fd34](https://github.com/dotnet/wpf/tree/b77fd3453e4f34de34afdb012baaddd291cbf470)*
- `src/xdt`  
*[dotnet/xdt@b1d534f](https://github.com/dotnet/xdt/tree/b1d534fe8831bbf62dc4356a7e611befee6ece33)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
