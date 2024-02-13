# List of components

To enable full offline source-building of the VMR, we have no other choice than to synchronize all the necessary code into the VMR. This also includes any code referenced via git submodules. More details on why and how this is done can be found here:
- [Strategy for managing external source dependencies](src/arcade/Documentation/UnifiedBuild/VMR-Strategy-For-External-Source.md)
- [Source Synchronization Process](src/arcade/Documentation/UnifiedBuild/VMR-Design-And-Operation.md#source-synchronization-process)

## Detailed list

<!-- component list beginning -->
- `src/arcade`  
*[dotnet/arcade@f7eb779](https://github.com/dotnet/arcade/tree/f7eb7794c703dc29a83b414b786e9a154f0ca042)*
- `src/aspire`  
*[dotnet/aspire@87d5246](https://github.com/dotnet/aspire/tree/87d5246ddfc1fb9b07fcdf7b4b42830f67427ab9)*
- `src/aspnetcore`  
*[dotnet/aspnetcore@58aff92](https://github.com/dotnet/aspnetcore/tree/58aff923593ed5c8e43993e21c6be482ebf8758d)*
    - `src/aspnetcore/src/submodules/googletest`  
    *[google/googletest@4872968](https://github.com/google/googletest/tree/48729681ad88a89061344ee541b4548833077e00)*
    - `src/aspnetcore/src/submodules/MessagePack-CSharp`  
    *[aspnet/MessagePack-CSharp@ecc4e18](https://github.com/aspnet/MessagePack-CSharp/tree/ecc4e18ad7a0c7db51cd7e3d2997a291ed01444d)*
- `src/cecil`  
*[dotnet/cecil@93dcb57](https://github.com/dotnet/cecil/tree/93dcb576e191a965008eae9b622527436653873f)*
- `src/command-line-api`  
*[dotnet/command-line-api@e9ac4ff](https://github.com/dotnet/command-line-api/tree/e9ac4ff4293cf853f3d07eb9e747aef27f5be965)*
- `src/deployment-tools`  
*[dotnet/deployment-tools@58b859f](https://github.com/dotnet/deployment-tools/tree/58b859ff2939ebee0c4e81fb30e8c69099e195c6)*
- `src/diagnostics`  
*[dotnet/diagnostics@79b59c5](https://github.com/dotnet/diagnostics/tree/79b59c505405b9bee1d62dfa73dfb9750b2d4376)*
- `src/emsdk`  
*[dotnet/emsdk@3ab438b](https://github.com/dotnet/emsdk/tree/3ab438b1e45a1cde263a48a522d7082db14106c8)*
- `src/format`  
*[dotnet/format@124d3ab](https://github.com/dotnet/format/tree/124d3abae92389bb7f8217d8303424566d73f6d9)*
- `src/fsharp`  
*[dotnet/fsharp@9de468a](https://github.com/dotnet/fsharp/tree/9de468a9bcd1dd5f3791639e0be2227aee0696a7)*
- `src/installer`  
*[dotnet/installer@6229962](https://github.com/dotnet/installer/tree/62299625d22b11dbabc49430774a896e8355c631)*
- `src/msbuild`  
*[dotnet/msbuild@15f7ddc](https://github.com/dotnet/msbuild/tree/15f7ddcaafa6622447fa69c1785ab7b3d1183719)*
- `src/nuget-client`  
*[nuget/nuget.client@8b658e2](https://github.com/nuget/nuget.client/tree/8b658e2eee6391936887b9fd1b39f7918d16a9cb)*
    - `src/nuget-client/submodules/NuGet.Build.Localization`  
    *[NuGet/NuGet.Build.Localization@f15db7b](https://github.com/NuGet/NuGet.Build.Localization/tree/f15db7b7c6f5affbea268632ef8333d2687c8031)*
- `src/razor`  
*[dotnet/razor@293afc1](https://github.com/dotnet/razor/tree/293afc1dae1bc89a8d5d92257a60b8ccaef054d2)*
- `src/roslyn`  
*[dotnet/roslyn@76ca39e](https://github.com/dotnet/roslyn/tree/76ca39e7b05779d9616fdc5ef07bacfdb57a4877)*
- `src/roslyn-analyzers`  
*[dotnet/roslyn-analyzers@68c643b](https://github.com/dotnet/roslyn-analyzers/tree/68c643b4667c6808bd21910ef32f7e2f7bd776c5)*
- `src/runtime`  
*[dotnet/runtime@4f09909](https://github.com/dotnet/runtime/tree/4f099091aa1dbcc3db0043241f3a4e81ef5abfdd)*
- `src/scenario-tests`  
*[dotnet/scenario-tests@bfde902](https://github.com/dotnet/scenario-tests/tree/bfde902a10d7b672f4fc7e844198ede405dbb9c6)*
- `src/sdk`  
*[dotnet/sdk@d67400b](https://github.com/dotnet/sdk/tree/d67400bb562e3b251503942e73e34a2cdede52fa)*
- `src/source-build-externals`  
*[dotnet/source-build-externals@f1ef074](https://github.com/dotnet/source-build-externals/tree/f1ef074dfcf79d2f2da6e6ff9df8696a32aa063c)*
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
*[dotnet/source-build-reference-packages@c67054b](https://github.com/dotnet/source-build-reference-packages/tree/c67054b681e1e399483f36f1395243dab5236ac9)*
- `src/sourcelink`  
*[dotnet/sourcelink@aa4b2e8](https://github.com/dotnet/sourcelink/tree/aa4b2e86f36454bdc0e0b012f9c8c520bdc1c036)*
- `src/symreader`  
*[dotnet/symreader@2902dcf](https://github.com/dotnet/symreader/tree/2902dcf06494391dc65552fd0743b7d426c550fb)*
- `src/templating`  
*[dotnet/templating@511e132](https://github.com/dotnet/templating/tree/511e13222864773fd28e7a67a90a6558ea9848b5)*
- `src/test-templates`  
*[dotnet/test-templates@068b070](https://github.com/dotnet/test-templates/tree/068b070bc5ce0add1328253d63f0f960f66a7e44)*
- `src/vstest`  
*[microsoft/vstest@f21d0da](https://github.com/microsoft/vstest/tree/f21d0dae0b91fe59e4afa92166bd721ddd2f0036)*
- `src/windowsdesktop`  
*[dotnet/windowsdesktop@9604194](https://github.com/dotnet/windowsdesktop/tree/9604194ccf0a3d7cab50dbf0f4aee7ff4f32f1a0)*
- `src/winforms`  
*[dotnet/winforms@dd56ded](https://github.com/dotnet/winforms/tree/dd56dedb821c890390992c40ad50bcde35a6dd0f)*
- `src/wpf`  
*[dotnet/wpf@db12339](https://github.com/dotnet/wpf/tree/db12339ccd43037d5e1229fb5c84b87f13c86de4)*
- `src/xdt`  
*[dotnet/xdt@c54253c](https://github.com/dotnet/xdt/tree/c54253c7c4413357772589c6c243b12ba4e7c595)*
- `src/xliff-tasks`  
*[dotnet/xliff-tasks@73f0850](https://github.com/dotnet/xliff-tasks/tree/73f0850939d96131c28cf6ea6ee5aacb4da0083a)*
<!-- component list end -->

The repository also contains a [JSON manifest](https://github.com/dotnet/dotnet/blob/main/src/source-manifest.json) listing all components in a machine-readable format.
