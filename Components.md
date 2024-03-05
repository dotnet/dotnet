# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@87d8902](https://github.com/dotnet/arcade/tree/87d89025bdd8827c016e4083660d31f497670e5c)*
- `src/aspire`  
*[dotnet/aspire@fa3e45e](https://github.com/dotnet/aspire/tree/fa3e45ee2a76c81e7a4876c6bef05282e35243f1)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@2c0ee4c](https://github.com/dotnet/aspnetcore/tree/2c0ee4c870556d823ea5a0b35a8ba1b8ae81142c)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@e4fdb87](https://github.com/google/googletest/tree/e4fdb87e76b9fc4b01c54ad81aea19d6e994b994)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
    - `src/aspnetcore/src/submodules/Node-Externals`  
    *[dotnet/Node-Externals@1bcb78c](https://github.com/dotnet/Node-Externals/tree/1bcb78ca694568f7993d9d385eee0687ad0f5dfe)*
- `src/cecil`  
*[dotnet/cecil@0d0bc8e](https://github.com/dotnet/cecil/tree/0d0bc8e0f47fdae9834e1eac678f364c50946133)*
- `src/command-line-api`  
*[dotnet/command-line-api@5ea97af](https://github.com/dotnet/command-line-api/tree/5ea97af07263ea3ef68a18557c8aa3f7e3200bda)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@822ff26](https://github.com/dotnet/deployment-tools/tree/822ff266c5f999ab9ceb6928df59d79285ea4a4f)*
- `src/diagnostics`  
*[dotnet/diagnostics@5eb514a](https://github.com/dotnet/diagnostics/tree/5eb514a41f900ac1aa1e9a3e12b2931dcb064069)*
- `src/emsdk`  
*[dotnet/emsdk@e926eb7](https://github.com/dotnet/emsdk/tree/e926eb7d9614243be6b936cbbe9988fd8cc6d8a6)*
- `src/format`  
*[dotnet/format@a8002e3](https://github.com/dotnet/format/tree/a8002e38ae10922926adf567d4dad7321063fad4)*
- `src/fsharp`  
*[dotnet/fsharp@ca66024](https://github.com/dotnet/fsharp/tree/ca66024e6da7cced9921216763386bd43f400fbf)*
- `src/installer`  
*[dotnet/installer@75494d3](https://github.com/dotnet/installer/tree/75494d3ada6624c411116d4c6e52daaf49154ee7)*
- `src/msbuild`  
*[dotnet/msbuild@6f44380](https://github.com/dotnet/msbuild/tree/6f44380e4fdea6ddf5c11f48efeb25c2bf181e62)*
- `src/nuget-client`  
*[nuget/nuget.client@6009531](https://github.com/nuget/nuget.client/tree/6009531090c927a8e61da9a0f97bdd5eb6f01a47)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@c5c96a2](https://github.com/dotnet/razor/tree/c5c96a26b1e0bd202dfd0941815dc44711e2abfd)*
- `src/roslyn`  
*[dotnet/roslyn@8537440](https://github.com/dotnet/roslyn/tree/8537440d8c831b8533a18f6530945ee91c243e1b)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@0e9cb2a](https://github.com/dotnet/roslyn-analyzers/tree/0e9cb2a3c38706574f2d02fd70ce1e66c7dd4c5f)*
- `src/runtime`  
*[dotnet/runtime@8510651](https://github.com/dotnet/runtime/tree/8510651bc6b5181cf258664c2d0b4b4ff4de1c61)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@d8e9245](https://github.com/dotnet/sdk/tree/d8e924540fa415a8b8fa7cb666174c5a97138147)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@7fbf7ef](https://github.com/dotnet/source-build-externals/tree/7fbf7ef59ca7ddcfa92269ef84e3fcb7544f8699)*
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
*[dotnet/source-build-reference-packages@936b4a4](https://github.com/dotnet/source-build-reference-packages/tree/936b4a4b4b8a74b65098983660c5814fb4afee15)*
- `src/sourcelink`  
*[dotnet/sourcelink@aef6b23](https://github.com/dotnet/sourcelink/tree/aef6b239ba3c11823cf150d9f042d9965e04bf9f)*
- `src/symreader`  
*[dotnet/symreader@71a20ad](https://github.com/dotnet/symreader/tree/71a20ad4aaedc284ef2d9a7302f5d2ec4df7dca3)*
- `src/templating`  
*[dotnet/templating@42e2d12](https://github.com/dotnet/templating/tree/42e2d12758afd312fbbb8585284cf59320676ac1)*
- `src/test-templates`  
*[dotnet/test-templates@36fdfeb](https://github.com/dotnet/test-templates/tree/36fdfebe8af73ed1c2ff796750984f1ebf6230b5)*
- `src/vstest`  
*[microsoft/vstest@39c7dd1](https://github.com/microsoft/vstest/tree/39c7dd12c7ec24d0552513e84d95476f2077ca33)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@9e4cede](https://github.com/dotnet/windowsdesktop/tree/9e4cede9d34b51c91ad10fa2462042aceeb3937b)*
- `src/winforms`  
*[dotnet/winforms@db743c1](https://github.com/dotnet/winforms/tree/db743c12c82bfe6f1265bc20a5a97c6ecfa68ba2)*
- `src/wpf`  
*[dotnet/wpf@d4d3ab8](https://github.com/dotnet/wpf/tree/d4d3ab8a86a71baed25b9eb076affd9c93bb9298)*
- `src/xdt`  
*[dotnet/xdt@9f7ab07](https://github.com/dotnet/xdt/tree/9f7ab07fc4914f76048d54d79afaee3bf89d7ede)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
