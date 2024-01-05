# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f4e11a1](https://github.com/dotnet/arcade/tree/f4e11a15c7b8a949d4a366e792a9843ff6e88cd5)*
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
*[dotnet/installer@11788cb](https://github.com/dotnet/installer/tree/11788cb3152423e5886a5f7e29a860c1b4e4c2c7)*
- `src/msbuild`  
*[dotnet/msbuild@bbe185f](https://github.com/dotnet/msbuild/tree/bbe185f58bf8bfc60ad3bd60e7fa3876decdea0f)*
- `src/nuget-client`  
*[nuget/nuget.client@e92be39](https://github.com/nuget/nuget.client/tree/e92be3915309e687044768de38933ac5fc4cb40c)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@df38336](https://github.com/dotnet/razor/tree/df383360c34ada8889fdf18dc36d245f2938db66)*
- `src/roslyn`  
*[dotnet/roslyn@237fb52](https://github.com/dotnet/roslyn/tree/237fb52c683601ed639f1fdeaf38ceef0c768fbc)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@8a92037](https://github.com/dotnet/roslyn-analyzers/tree/8a92037d28baf58560622cb4685ecefdc828c2c8)*
- `src/runtime`  
*[dotnet/runtime@f1a2b92](https://github.com/dotnet/runtime/tree/f1a2b928f4c48bd65fa1bfaf9fa1c84e8607dfb3)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@0589a90](https://github.com/dotnet/scenario-tests/tree/0589a90cb11bb1daf9c05f20c1dc2d78c49075f2)*
- `src/sdk`  
*[dotnet/sdk@997bb1f](https://github.com/dotnet/sdk/tree/997bb1f507871fccdab00a4fa8c9b694d364da7a)*
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
*[dotnet/symreader@236d6c5](https://github.com/dotnet/symreader/tree/236d6c586677394a698d79fb2ff8e60bcc41ab0c)*
- `src/templating`  
*[dotnet/templating@4cb38ef](https://github.com/dotnet/templating/tree/4cb38ef0f516c26607344c1414d67a54971780c8)*
- `src/test-templates`  
*[dotnet/test-templates@ec54b2c](https://github.com/dotnet/test-templates/tree/ec54b2c1553db0a544ef0e8595be2318fc12e08d)*
- `src/vstest`  
*[microsoft/vstest@053d711](https://github.com/microsoft/vstest/tree/053d7114a72aac12d1382ecc2a23b2dfdd5b084b)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@aa402a7](https://github.com/dotnet/windowsdesktop/tree/aa402a708bd9d6c351e4807dd4124d42a881d7df)*
- `src/winforms`  
*[dotnet/winforms@16cba3e](https://github.com/dotnet/winforms/tree/16cba3e6042ea5268835a9d11e462bcd52a20392)*
- `src/wpf`  
*[dotnet/wpf@ca806b2](https://github.com/dotnet/wpf/tree/ca806b21d361f43cf063bc93640c6740d8bbf73f)*
- `src/xdt`  
*[dotnet/xdt@0c3c587](https://github.com/dotnet/xdt/tree/0c3c5878cd2f204a4335755f753eda78ecab536b)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@194f328](https://github.com/dotnet/xliff-tasks/tree/194f32828726c3f1f63f79f3dc09b9e99c157b11)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
