# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3531486](https://github.com/dotnet/arcade/tree/3531486ae4b4e78149d815d99db3d3a0e8a3c64d)*
- `src/aspire`  
*[_git/dotnet-aspire@48e42f5](https://dev.azure.com/dnceng/internal/_git/dotnet-aspire/?version=GC48e42f59d64d84b404e904996a9ed61f2a17a569)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@4b78107](https://github.com/dotnet/aspnetcore/tree/4b7810723c27d47cc982055c6615cccf664e2980)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@530d5c8](https://github.com/google/googletest/tree/530d5c8c84abd2a46f38583ee817743c9b3a42b4)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@02026e5](https://github.com/dotnet/cecil/tree/02026e5c1b054958851d2711fefa1b37027cab23)*
- `src/command-line-api`  
*[dotnet/command-line-api@a045dd5](https://github.com/dotnet/command-line-api/tree/a045dd54a4c44723c215d992288160eb1401bb7f)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@fdef093](https://github.com/dotnet/deployment-tools/tree/fdef0932d9953ee12367c8dac9ef638b573d4f42)*
- `src/diagnostics`  
*[dotnet/diagnostics@5ce78f6](https://github.com/dotnet/diagnostics/tree/5ce78f66d89ea529e459abddb129ab36cb5bd936)*
- `src/emsdk`  
*[dotnet/emsdk@9902d2e](https://github.com/dotnet/emsdk/tree/9902d2ec1695a2ea184003b35beb96232a532434)*
- `src/format`  
*[dotnet/format@2c2d58c](https://github.com/dotnet/format/tree/2c2d58cb25064036f853d76e7b6aff7bb7d38401)*
- `src/fsharp`  
*[dotnet/fsharp@0c48954](https://github.com/dotnet/fsharp/tree/0c489541068f311e23b582410c1df3ff86f1d526)*
- `src/installer`  
*[dotnet/installer@f258812](https://github.com/dotnet/installer/tree/f258812cc865ce2e56f0f779d02006254e82ca0c)*
- `src/msbuild`  
*[dotnet/msbuild@b59f07e](https://github.com/dotnet/msbuild/tree/b59f07e4312eb6e3e33e59241453606c81992738)*
- `src/nuget-client`  
*[nuget/nuget.client@6a82332](https://github.com/nuget/nuget.client/tree/6a82332d4936d893fb1e22fd86f2e3cb4d54c471)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@8426ca4](https://github.com/dotnet/razor/tree/8426ca45b1b0737f5d4137913f55e56a184454cd)*
- `src/roslyn`  
*[dotnet/roslyn@45421ad](https://github.com/dotnet/roslyn/tree/45421ad7dfcb8e1563e172eda81d9d892a0fe73c)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8a92037](https://github.com/dotnet/roslyn-analyzers/tree/8a92037d28baf58560622cb4685ecefdc828c2c8)*
- `src/runtime`  
*[dotnet/runtime@bc0b28f](https://github.com/dotnet/runtime/tree/bc0b28f564f0f3fdfa0c0856bd1b3b38d5371c4d)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@0589a90](https://github.com/dotnet/scenario-tests/tree/0589a90cb11bb1daf9c05f20c1dc2d78c49075f2)*
- `src/sdk`  
*[dotnet/sdk@5bdfabf](https://github.com/dotnet/sdk/tree/5bdfabf0df19074dc3a0d4f7460317b634ad15b0)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@63e4253](https://github.com/dotnet/source-build-externals/tree/63e4253ef37427fe51ee50b3225c1504889651df)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[Microsoft/ApplicationInsights-dotnet@5e2e7dd](https://github.com/Microsoft/ApplicationInsights-dotnet/tree/5e2e7ddda961ec0e16a75b1ae0a37f6a13c777f5)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@bb354ce](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/bb354ceabed19189245e075abb864f327b6c14ad)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@5701f66](https://github.com/mthalman/docker-creds-provider/tree/5701f6667c1fbd805684857baaa860383bbdfed7)*
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
*[dotnet/source-build-reference-packages@5f670e4](https://github.com/dotnet/source-build-reference-packages/tree/5f670e45d060b25d5b07646dfcd94eae31893191)*
- `src/sourcelink`  
*[dotnet/sourcelink@ecd8aa3](https://github.com/dotnet/sourcelink/tree/ecd8aa3a3b36eda57831027b55947c7c23c99479)*
- `src/symreader`  
*[dotnet/symreader@aa31e33](https://github.com/dotnet/symreader/tree/aa31e333b952f53910dc6bd08d80596eaaf89360)*
- `src/templating`  
*[dotnet/templating@d5177e9](https://github.com/dotnet/templating/tree/d5177e9d661a29082429a898e82e58e195d11bd4)*
- `src/test-templates`  
*[dotnet/test-templates@ec54b2c](https://github.com/dotnet/test-templates/tree/ec54b2c1553db0a544ef0e8595be2318fc12e08d)*
- `src/vstest`  
*[microsoft/vstest@053d711](https://github.com/microsoft/vstest/tree/053d7114a72aac12d1382ecc2a23b2dfdd5b084b)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@21b3657](https://github.com/dotnet/windowsdesktop/tree/21b365791e27e16ab62d70bb2fc5859834549cca)*
- `src/winforms`  
*[dotnet/winforms@50972fd](https://github.com/dotnet/winforms/tree/50972fd053a8f7b569fac7d214ac25c0251c35ec)*
- `src/wpf`  
*[dotnet/wpf@77ab54a](https://github.com/dotnet/wpf/tree/77ab54ad41d61b58958af54cc06a2a06465b3365)*
- `src/xdt`  
*[dotnet/xdt@0c3c587](https://github.com/dotnet/xdt/tree/0c3c5878cd2f204a4335755f753eda78ecab536b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@194f328](https://github.com/dotnet/xliff-tasks/tree/194f32828726c3f1f63f79f3dc09b9e99c157b11)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
