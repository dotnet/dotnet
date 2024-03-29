# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f311667](https://github.com/dotnet/arcade/tree/f311667e0587f19c3fa9553a909975662107a351)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[_git/dotnet-aspnetcore@8486d31](https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore/?version=GC8486d31e24f30e3fa1809a95699a0adc16f448d7)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@31993df](https://github.com/google/googletest/tree/31993dfa6b47e11c7a6ef67cfa8af90892b9bd1c)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@45dd3a7](https://github.com/dotnet/cecil/tree/45dd3a73dd5b64b010c4251303b3664bb30df029)*
- `src/command-line-api`  
*[dotnet/command-line-api@02fe27c](https://github.com/dotnet/command-line-api/tree/02fe27cd6a9b001c8feb7938e6ef4b3799745759)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@5957c5c](https://github.com/dotnet/deployment-tools/tree/5957c5c5f85f17c145e7fab4ece37ad6aafcded9)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/tree/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@08a90ca](https://github.com/dotnet/emsdk/tree/08a90ca2c88b17f1b5d081318354a41db0882cff)*
- `src/format`  
*[dotnet/format@d237e17](https://github.com/dotnet/format/tree/d237e172b324021b97effa244af44d63d1a8bb7e)*
- `src/fsharp`  
*[dotnet/fsharp@fc5e9ed](https://github.com/dotnet/fsharp/tree/fc5e9eda234e2b69aa479f4f83faddc31fdd4da7)*
- `src/installer`  
*[dotnet/installer@8dfcac5](https://github.com/dotnet/installer/tree/8dfcac5e401437cfe5ab5ae3a3a64e011c4f9c57)*
- `src/msbuild`  
*[dotnet/msbuild@b5265ef](https://github.com/dotnet/msbuild/tree/b5265ef370a651f8c3458110b804e5cbf869eeb5)*
- `src/nuget-client`  
*[_git/NuGet-NuGet.Client-Trusted@550277e](https://dev.azure.com/devdiv/DevDiv/_git/NuGet-NuGet.Client-Trusted/?version=GC550277e0616e549446f03fda35d3e23dff75dc01)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@d135dd8](https://github.com/dotnet/razor/tree/d135dd8d2ec1c2fbdee220e8656b308694e17a4b)*
- `src/roslyn`  
*[dotnet/roslyn@2635711](https://github.com/dotnet/roslyn/tree/263571123fc3dc4a638e071234ac9fbf91913962)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@b4d9a13](https://github.com/dotnet/roslyn-analyzers/tree/b4d9a1334d5189172977ba8fddd00bda70161e4a)*
- `src/runtime`  
*[_git/dotnet-runtime@2d7eea2](https://dev.azure.com/dnceng/internal/_git/dotnet-runtime/?version=GC2d7eea252964e69be94cb9c847b371b23e4dd470)*
- `src/sdk`  
*[_git/dotnet-sdk@88e8f61](https://dev.azure.com/dnceng/internal/_git/dotnet-sdk/?version=GC88e8f6129d6e8bb551a121f7969a5e0f33eb6881)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@0fac378](https://github.com/dotnet/source-build-externals/tree/0fac378047750fa8bd850a98b159560f9f7627c3)*
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
    *[mthalman/docker-creds-provider@b381eaf](https://github.com/mthalman/docker-creds-provider/tree/b381eafbeecb1039f5839fc98ef45e7b3e52dee9)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@f110e5b](https://github.com/xunit/xunit/tree/f110e5bee5dfd4c08339587c9c3df9292fcb597c)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@5c8c10e](https://github.com/xunit/assert.xunit/tree/5c8c10e085eb42f39f2fe0b40c94bf56649eb0a4)*
    - `src/source-build-externals/src/xunit/tools/build`  
    *[xunit/build-tools@8e186b0](https://github.com/xunit/build-tools/tree/8e186b0f8e398796e75453f3f18952b06d29fdfd)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@79827ee](https://github.com/dotnet/source-build-reference-packages/tree/79827eed138fd2575a8b24820b4f385ee4ffb6e6)*
- `src/sourcelink`  
*[dotnet/sourcelink@94eaac3](https://github.com/dotnet/sourcelink/tree/94eaac3385cafff41094454966e1af1d1cf60f00)*
- `src/symreader`  
*[dotnet/symreader@2c8079e](https://github.com/dotnet/symreader/tree/2c8079e2e8e78c0cd11ac75a32014756136ecdb9)*
- `src/templating`  
*[dotnet/templating@2d33450](https://github.com/dotnet/templating/tree/2d33450179cbdbf09e9dac5817ce1c5a75587c4a)*
- `src/test-templates`  
*[dotnet/test-templates@1e5f360](https://github.com/dotnet/test-templates/tree/1e5f3603af2277910aad946736ee23283e7f3e16)*
- `src/vstest`  
*[microsoft/vstest@aa59400](https://github.com/microsoft/vstest/tree/aa59400b11e1aeee2e8af48928dbd48748a8bef9)*
- `src/xdt`  
*[dotnet/xdt@9a1c3e1](https://github.com/dotnet/xdt/tree/9a1c3e1b7f0c8763d4c96e593961a61a72679a7b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
