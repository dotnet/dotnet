# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@3162419](https://github.com/dotnet/arcade/tree/31624193093a13f765ab5382509e693911264509)*
- `src/aspire`  
*[dotnet/aspire@a1f7880](https://github.com/dotnet/aspire/tree/a1f7880ae14703e747bf79d1e2e947bffea6a604)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@080c909](https://github.com/dotnet/aspnetcore/tree/080c909ed688f619befdceeb936a7eeb97c665a4)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@71815bb](https://github.com/google/googletest/tree/71815bbf7de6e10c11821d654a2fae2cf42de0f7)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@9511905](https://github.com/aspnet/MessagePack-CSharp/tree/95119056ee8f4da1714b055a4f16893afaa73af7)*
- `src/cecil`  
*[dotnet/cecil@9c94433](https://github.com/dotnet/cecil/tree/9c9443396f8deacceb8edb169890e52aac25f311)*
- `src/command-line-api`  
*[dotnet/command-line-api@803d859](https://github.com/dotnet/command-line-api/tree/803d8598f98fb4efd94604b32627ee9407f246db)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@57d7bae](https://github.com/dotnet/deployment-tools/tree/57d7baec5f331a145174d0e8f00d7bbfdf2b77d4)*
- `src/diagnostics`  
*[dotnet/diagnostics@513150c](https://github.com/dotnet/diagnostics/tree/513150c2f25077b1fcb194407e53c433c975f39b)*
- `src/efcore`  
*[dotnet/efcore@1567ab3](https://github.com/dotnet/efcore/tree/1567ab3bc52674b4b79512be83de2b6e8bc0cfda)*
- `src/emsdk`  
*[dotnet/emsdk@4ea46ba](https://github.com/dotnet/emsdk/tree/4ea46baeaf74d5a99cb93593362b6d8263b10550)*
- `src/fsharp`  
*[dotnet/fsharp@c3eb162](https://github.com/dotnet/fsharp/tree/c3eb162ec7bcf7449ca54b2218ab0d0c4d67c1d0)*
- `src/msbuild`  
*[dotnet/msbuild@b8f46eb](https://github.com/dotnet/msbuild/tree/b8f46eb171f3b1c7e3bddb8afaff9fbb0ba59e9d)*
- `src/nuget-client`  
*[nuget/nuget.client@1975634](https://github.com/nuget/nuget.client/tree/19756345139c45de23bd196e9b4be01d48e84fdd)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@3094e2d](https://github.com/dotnet/razor/tree/3094e2df9783ab0c966fafe5f51f148030686444)*
- `src/roslyn`  
*[dotnet/roslyn@fd48dff](https://github.com/dotnet/roslyn/tree/fd48dff6a793eb26ed90d1cb40a70416e3fcd559)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@5dd0dd5](https://github.com/dotnet/roslyn-analyzers/tree/5dd0dd5db8fa79932517f153854c0f2c24ac98a3)*
- `src/runtime`  
*[dotnet/runtime@43295bb](https://github.com/dotnet/runtime/tree/43295bb5378453d2ec4d9272cb44c6f50b4faa1f)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@39284fb](https://github.com/dotnet/scenario-tests/tree/39284fbc776975659af4fd377b683b11be053cbb)*
- `src/sdk`  
*[dotnet/sdk@f0e0ab6](https://github.com/dotnet/sdk/tree/f0e0ab6d01bb926f2a1cbe1c8ce9928cb361052b)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@d22c9e8](https://github.com/dotnet/source-build-externals/tree/d22c9e818d40d0b36874f8db31550b1ea7f66c72)*
    - `src/source-build-externals/src/abstractions-xunit`  
    *[xunit/abstractions.xunit@b75d54d](https://github.com/xunit/abstractions.xunit/tree/b75d54d73b141709f805c2001b16f3dd4d71539d)*
    - `src/source-build-externals/src/application-insights`  
    *[microsoft/ApplicationInsights-dotnet@43825e0](https://github.com/microsoft/ApplicationInsights-dotnet/tree/43825e06a22cdfb702fc199a7ba99a7d541d48c6)*
    - `src/source-build-externals/src/azure-activedirectory-identitymodel-extensions-for-dotnet`  
    *[AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet@e67b25b](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tree/e67b25be77532af9ba405670b34b4d263d505fde)*
    - `src/source-build-externals/src/cssparser`  
    *[dotnet/cssparser@0d59611](https://github.com/dotnet/cssparser/tree/0d59611784841735a7778a67aa6e9d8d000c861f)*
    - `src/source-build-externals/src/docker-creds-provider`  
    *[mthalman/docker-creds-provider@6c73fa4](https://github.com/mthalman/docker-creds-provider/tree/6c73fa4784795ae07f49305a057abf5c473d2adb)*
    - `src/source-build-externals/src/humanizer`  
    *[Humanizr/Humanizer@3ebc38d](https://github.com/Humanizr/Humanizer/tree/3ebc38de585fc641a04b0e78ed69468453b0f8a1)*
    - `src/source-build-externals/src/MSBuildLocator`  
    *[microsoft/MSBuildLocator@e0281df](https://github.com/microsoft/MSBuildLocator/tree/e0281df33274ac3c3e22acc9b07dcb4b31d57dc0)*
    - `src/source-build-externals/src/newtonsoft-json`  
    *[JamesNK/Newtonsoft.Json@0a2e291](https://github.com/JamesNK/Newtonsoft.Json/tree/0a2e291c0d9c0c7675d445703e51750363a549ef)*
    - `src/source-build-externals/src/spectre-console`  
    *[spectreconsole/spectre.console@7397169](https://github.com/spectreconsole/spectre.console/tree/7397169a2757dc3657598bdea4ac222c0f283425)*
    - `src/source-build-externals/src/vs-solutionpersistence`  
    *[microsoft/vs-solutionpersistence@3489af8](https://github.com/microsoft/vs-solutionpersistence/tree/3489af847b089e729a641a6051a02990245e8716)*
    - `src/source-build-externals/src/xunit`  
    *[xunit/xunit@82543a6](https://github.com/xunit/xunit/tree/82543a6df6f5f13b5b70f8a9f9ccb41cd676084f)*
    - `src/source-build-externals/src/xunit/src/xunit.assert/Asserts`  
    *[xunit/assert.xunit@cac8b68](https://github.com/xunit/assert.xunit/tree/cac8b688c193c0f244a0bedf3bb60feeb32d377a)*
    - `src/source-build-externals/src/xunit/tools/builder/common`  
    *[xunit/build-tools-v3@90dba1f](https://github.com/xunit/build-tools-v3/tree/90dba1f5638a4f00d4978a73e23edde5b85061d9)*
    - `src/source-build-externals/src/xunit/tools/media`  
    *[xunit/media@5738b6e](https://github.com/xunit/media/tree/5738b6e86f08e0389c4392b939c20e3eca2d9822)*
- `src/source-build-reference-packages`  
*[dotnet/source-build-reference-packages@4660d88](https://github.com/dotnet/source-build-reference-packages/tree/4660d88cf953fbbd14192c787053a20246ce1aeb)*
- `src/sourcelink`  
*[dotnet/sourcelink@24760f9](https://github.com/dotnet/sourcelink/tree/24760f9b1ed7478863d699b7f6634e82d1b25bd1)*
- `src/symreader`  
*[dotnet/symreader@2edd33f](https://github.com/dotnet/symreader/tree/2edd33f08a7dd1f4ce28df21e4f8f68ae6d020eb)*
- `src/templating`  
*[dotnet/templating@bf40ec0](https://github.com/dotnet/templating/tree/bf40ec00f3761436f9e503691191ed722575f1bb)*
- `src/test-templates`  
*[dotnet/test-templates@deb758b](https://github.com/dotnet/test-templates/tree/deb758b7e7750826a75e937c7df5b97f19c510f8)*
- `src/vstest`  
*[microsoft/vstest@bc91613](https://github.com/microsoft/vstest/tree/bc9161306b23641b0364b8f93d546da4d48da1eb)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@3af3968](https://github.com/dotnet/windowsdesktop/tree/3af3968a4cd80528d55dd6878fe2cb55c551f320)*
- `src/winforms`  
*[dotnet/winforms@b9b6eaf](https://github.com/dotnet/winforms/tree/b9b6eaf241867236ecbc1d714611cf410e88a49d)*
- `src/wpf`  
*[dotnet/wpf@e733ef2](https://github.com/dotnet/wpf/tree/e733ef2640dfc48df0d2a42904fb82d044a83e6a)*
- `src/xdt`  
*[dotnet/xdt@4ddd811](https://github.com/dotnet/xdt/tree/4ddd8113a29852380b7b929117bfe67f401ac320)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
