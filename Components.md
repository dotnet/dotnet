# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@d5b02a4](https://github.com/dotnet/arcade/tree/d5b02a4900c4d521cb48b8f0d7e3f28175268f7c)*
- `src/aspire`  
*[dotnet/aspire@1dd4b32](https://github.com/dotnet/aspire/tree/1dd4b3265f01a50b20522fd3d7f3cd315db5be6b)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@6a05eb6](https://github.com/dotnet/aspnetcore/tree/6a05eb6b345023d518b6dd2dbb93a804bcd4ec31)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@b75ecf1](https://github.com/google/googletest/tree/b75ecf1bed2fcd416b66c86cb6fe79122abf132e)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@61250b0](https://github.com/dotnet/cecil/tree/61250b0ed403b3f9b69a33f7d8f66f311338d6a1)*
- `src/command-line-api`  
*[dotnet/command-line-api@e9ac4ff](https://github.com/dotnet/command-line-api/tree/e9ac4ff4293cf853f3d07eb9e747aef27f5be965)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@5eb514a](https://github.com/dotnet/diagnostics/tree/5eb514a41f900ac1aa1e9a3e12b2931dcb064069)*
- `src/emsdk`  
*[dotnet/emsdk@f87c716](https://github.com/dotnet/emsdk/tree/f87c7165942d0bb19fc6ee33a55b49d574ec142a)*
- `src/format`  
*[dotnet/format@c1c02fd](https://github.com/dotnet/format/tree/c1c02fdd0840d72fa7adfdb0e0713f26ade83bbf)*
- `src/fsharp`  
*[dotnet/fsharp@b57dee7](https://github.com/dotnet/fsharp/tree/b57dee7cec971021547a7b8a36a46d7271fea99e)*
- `src/installer`  
*[dotnet/installer@a7533dc](https://github.com/dotnet/installer/tree/a7533dce5ef0e0b6fdeed49af421e0856ffe91e3)*
- `src/msbuild`  
*[dotnet/msbuild@6f44380](https://github.com/dotnet/msbuild/tree/6f44380e4fdea6ddf5c11f48efeb25c2bf181e62)*
- `src/nuget-client`  
*[nuget/nuget.client@5ecd0be](https://github.com/nuget/nuget.client/tree/5ecd0be8cb73db807189427eb8e0f7bfd334c1e0)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@59180c2](https://github.com/dotnet/razor/tree/59180c27a58b4f95da28041373ddabdc6894814f)*
- `src/roslyn`  
*[dotnet/roslyn@5e6349e](https://github.com/dotnet/roslyn/tree/5e6349e1c07c535ec698b00075d8b4f5babfd2b6)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@61a45f8](https://github.com/dotnet/roslyn-analyzers/tree/61a45f87a68c59429cb0359f385455bfec58f5cb)*
- `src/runtime`  
*[dotnet/runtime@50f7525](https://github.com/dotnet/runtime/tree/50f7525b65ac7e51ff1a135699ae9341bf8102e9)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@5ae34e5](https://github.com/dotnet/sdk/tree/5ae34e5889c750af699428422b3af8551ad951da)*
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
*[dotnet/source-build-reference-packages@539af5d](https://github.com/dotnet/source-build-reference-packages/tree/539af5d8ae183d4fe61e8b2f8f4a8505c8a765a7)*
- `src/sourcelink`  
*[dotnet/sourcelink@01eadf6](https://github.com/dotnet/sourcelink/tree/01eadf6bfd22b6be33c6861a51c86bd552f0ece0)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@f9bda49](https://github.com/dotnet/templating/tree/f9bda49b7454083ac7361a1f4dc7dc7a73481699)*
- `src/test-templates`  
*[dotnet/test-templates@1591b24](https://github.com/dotnet/test-templates/tree/1591b24326caa98288e04e18e5c1b75c36c917c1)*
- `src/vstest`  
*[microsoft/vstest@48d8e77](https://github.com/microsoft/vstest/tree/48d8e778c871315db0bad221b00f4843b06242c3)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@4ec74ab](https://github.com/dotnet/windowsdesktop/tree/4ec74ab000d5fe841e00150b602e17d569c4ac2a)*
- `src/winforms`  
*[dotnet/winforms@26bb842](https://github.com/dotnet/winforms/tree/26bb84231a0bfd8778eb9f88fcef4ffe6c56fc6e)*
- `src/wpf`  
*[dotnet/wpf@21c17be](https://github.com/dotnet/wpf/tree/21c17bee658dba0531fa7b89143de1bbf7efe347)*
- `src/xdt`  
*[dotnet/xdt@3ae12c7](https://github.com/dotnet/xdt/tree/3ae12c7ded587740bb113b453b5a738c5017681d)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
