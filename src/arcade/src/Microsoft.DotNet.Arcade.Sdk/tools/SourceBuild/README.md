# Running source-build

These args control parts of source-build:

* `/p:ArcadeBuildFromSource=true` - Enable simple developer machine repro defaults.
* Not implemented but recognized as potentially useful:
  * Disable Git repo isolation (slightly faster, but not recommended for dev machines). We may want to use this for CI.
  * Clone and build upstreams from source rather than using intermediate nupkgs.
    * Not as useful as it sounds: this is only the "production" build so building this way is still not appropriate for a typical Linux distro archive.

*All* source-build functionality is brought in only when `ArcadeBuildFromSource`
is set to true, to ensure the new MSBuild props/targets don't introduce bugs
into ordinary builds.

File issues encountered with this directory's tooling in the
[dotnet/source-build](https://github.com/dotnet/source-build) repository rather
than [dotnet/arcade](https://github.com/dotnet/arcade) to contact the team
maintaining this functionality directly.

## MSBuild execution

The source-build targets work by having the build noop, and instead recursively
call an inner build after some setup. The targets work roughly like this:

* Run `./build.sh /p:ArcadeBuildFromSource=true`
  * Run `dotnet msbuild ... Build.proj /p:ArcadeBuildFromSource=true`
    * [Hook] Before **Outer Execute**:
      * Clone the source into `artifacts/source-build/self/src`
      * Assemble a build command by appending to the `dotnet msbuild` call.
      * Run `dotnet msbuild ... Build.proj /p:ArcadeBuildFromSource=true ... /p:ArcadeInnerBuildFromSource=true`
        * [Hook] Before **Inner Execute**:
          * Compile source-build MSBuild tasks. (Temporary, should migrate to Arcade task DLL.)
        * During **Inner Execute**:
          * `MSBuild Projects=Tools.proj Targets=Restore`
            * [Hook] Inject intermediate nupkg package reference through MSBuild task.
            * [Hook] After Restore, copy the extracted source-built nupkgs to a new dir and inject the dir into the NuGet.config.
          * The build happens!
        * **Inner Execute** complete!
      * Create intermediate nupkg that contains the inner source-build's artifacts.
        * MSBuild `Projects=SourceBuildIntermediate.proj Targets=Restore;Pack`
      * Empty out the list of `ProjectToBuild`, because we already built them from source.
        * Put `Noop.proj` in the list as a sentinel value.
    * During **Outer Execute**:
      * MSBuild `Projects=Tools.proj Targets=Restore`
        * Does nothing interesting.
      * "Builds" `Noop.proj`, doing nothing.
    * **Outer Execute** complete!
