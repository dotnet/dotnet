# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@c3f5cbf](https://github.com/dotnet/arcade/tree/c3f5cbfb2829795294f5c2d9fa5a0522f47e91fb)*
- `src/aspire`  
*[dotnet/aspire@8ec92cb](https://github.com/dotnet/aspire/tree/8ec92cbc5fbcba7a677fb52aaa4b0118f1ed17f4)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@4ea9f39](https://github.com/dotnet/aspnetcore/tree/4ea9f39a3447596ed9bf42320882cc6423e0c60b)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b75ecf1](https://github.com/google/googletest/tree/b75ecf1bed2fcd416b66c86cb6fe79122abf132e)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@93dcb57](https://github.com/dotnet/cecil/tree/93dcb576e191a965008eae9b622527436653873f)*
- `src/command-line-api`  
*[dotnet/command-line-api@e9ac4ff](https://github.com/dotnet/command-line-api/tree/e9ac4ff4293cf853f3d07eb9e747aef27f5be965)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@d260974](https://github.com/dotnet/deployment-tools/tree/d260974045322b57853c0b1625123cf92b330574)*
- `src/diagnostics`  
*[dotnet/diagnostics@79b59c5](https://github.com/dotnet/diagnostics/tree/79b59c505405b9bee1d62dfa73dfb9750b2d4376)*
- `src/emsdk`  
*[dotnet/emsdk@4106737](https://github.com/dotnet/emsdk/tree/4106737f31432e0408d3afd95bf242233daf48a6)*
- `src/format`  
*[dotnet/format@42462b3](https://github.com/dotnet/format/tree/42462b3946db3f6cad8604b9f8c2d27d1a55257b)*
- `src/fsharp`  
*[dotnet/fsharp@5e40303](https://github.com/dotnet/fsharp/tree/5e40303518d76a13e9ace08190adb9243ba6ed56)*
- `src/installer`  
*[dotnet/installer@48acc18](https://github.com/dotnet/installer/tree/48acc18db18df4b5ffe6543dcb2ba0c2411bc230)*
- `src/msbuild`  
*[dotnet/msbuild@15f7ddc](https://github.com/dotnet/msbuild/tree/15f7ddcaafa6622447fa69c1785ab7b3d1183719)*
- `src/nuget-client`  
*[nuget/nuget.client@8b658e2](https://github.com/nuget/nuget.client/tree/8b658e2eee6391936887b9fd1b39f7918d16a9cb)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@9082178](https://github.com/dotnet/razor/tree/90821780eb485a66734f1d68a40a047d55345a39)*
- `src/roslyn`  
*[dotnet/roslyn@744a0ae](https://github.com/dotnet/roslyn/tree/744a0ae8691c5c463f63e9936b7d56592c0ed57c)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@68c643b](https://github.com/dotnet/roslyn-analyzers/tree/68c643b4667c6808bd21910ef32f7e2f7bd776c5)*
- `src/runtime`  
*[dotnet/runtime@72ee969](https://github.com/dotnet/runtime/tree/72ee969ab5ade8b238c46596a7f118ae4c07619c)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@3c8be6c](https://github.com/dotnet/sdk/tree/3c8be6c66d5903936c6ae16fcc065aed21bbea5c)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@ddfb604](https://github.com/dotnet/source-build-externals/tree/ddfb60463c966af55fd0e222c2266170e83d1324)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@a607fa5](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/a607fa5e0005a6178cf1d2fed4fa0f8179cdb186)*
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
*[dotnet/source-build-reference-packages@2f79f97](https://github.com/dotnet/source-build-reference-packages/tree/2f79f97b7a6a0cf2ca3297a8fa526e6f4ea98ce2)*
- `src/sourcelink`  
*[dotnet/sourcelink@a121109](https://github.com/dotnet/sourcelink/tree/a12110929d43888e7a97349961e360d84c623cea)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@1091651](https://github.com/dotnet/templating/tree/109165196c3cfbd263f0e510e52b9e3d77805089)*
- `src/test-templates`  
*[dotnet/test-templates@068b070](https://github.com/dotnet/test-templates/tree/068b070bc5ce0add1328253d63f0f960f66a7e44)*
- `src/vstest`  
*[microsoft/vstest@5eed35d](https://github.com/microsoft/vstest/tree/5eed35d4a9a2e1b688eb86c5e4171e370a561b2a)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@5bee3d3](https://github.com/dotnet/windowsdesktop/tree/5bee3d319e61624015f18b0a4d282fd474e9c9b4)*
- `src/winforms`  
*[dotnet/winforms@fad8ead](https://github.com/dotnet/winforms/tree/fad8ead656f72043b67ae5aa2f983ad448daa00d)*
- `src/wpf`  
*[dotnet/wpf@06b815c](https://github.com/dotnet/wpf/tree/06b815c6811987d20ba4682afe2bf4e8a8b5184c)*
- `src/xdt`  
*[dotnet/xdt@c54253c](https://github.com/dotnet/xdt/tree/c54253c7c4413357772589c6c243b12ba4e7c595)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
