# .NET VMR (Virtual Monolithic Repository)

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

The .NET VMR is a Virtual Monolithic Repository containing all source code and infrastructure needed to build the complete .NET SDK from source.

## Critical Build Rules

**NEVER CANCEL builds or long-running commands.** Builds may take 60+ minutes, tests may take 30+ minutes. Always set timeouts to 90+ minutes for builds and 45+ minutes for tests.

**ALWAYS validate that EVERY command works** before including it in any code changes or suggestions.

## Working Effectively

### Prerequisites for Linux/Ubuntu 24.04

**ALWAYS install these dependencies before attempting any builds:**

```bash
sudo apt update && sudo apt install -y build-essential clang cmake cpio curl git \
  libicu-dev libkrb5-dev liblttng-ust-dev libssl-dev lld lldb llvm \
  ninja-build python-is-python3
```

**Note:** The VMR builds are complex and may fail due to package version conflicts or missing dependencies even with prerequisites installed.

### Repository Structure

**Key directories:**
- `src/` - All .NET product repositories (runtime, sdk, aspnetcore, etc.)
- `.github/` - VMR-level workflows and policies  
- `eng/` - VMR build infrastructure and tooling
- `docs/` - VMR documentation and design docs
- `repo-projects/` - MSBuild project files for each sub-repository

**Important sub-repositories in src/:**
- `src/runtime/` - .NET runtime (CoreCLR, Mono, libraries)
- `src/sdk/` - .NET SDK and CLI tools
- `src/aspnetcore/` - ASP.NET Core framework
- `src/roslyn/` - C# and VB.NET compilers
- `src/msbuild/` - MSBuild build engine
- `src/efcore/` - Entity Framework Core
- `src/fsharp/` - F# compiler and tools

### Build Approaches

**There are two main build modes:**

1. **Microsoft-based build (Recommended for testing):**
   ```bash
   ./build.sh --clean-while-building
   ```
   - Uses pre-built Microsoft packages
   - Faster but may have version conflicts
   - **Expected time: 45-60 minutes. NEVER CANCEL. Set timeout to 90+ minutes.**

2. **Source-build (Complete from source):**
   ```bash
   # ALWAYS run prep first
   ./prep-source-build.sh
   # Then build - CRITICAL: May take 90+ minutes
   ./build.sh -sb --clean-while-building
   ```
   - Builds everything from source
   - **prep-source-build.sh takes ~5-6 minutes**
   - **Source build takes 90+ minutes. NEVER CANCEL. Set timeout to 120+ minutes.**

### Build Commands Reference

**View all build options:**
```bash
./build.sh --help
```

**Common build flags:**
- `--clean-while-building` / `-cwb` - Cleans each repo after building (saves disk space)
- `--verbosity minimal` / `-v minimal` - Reduces build output
- `--configuration Release` / `-c Release` - Release build (default)
- `--source-build` / `-sb` - Enable source-build mode
- `--test` / `-t` - Run tests after build

### Testing

**NEVER CANCEL test runs.** Test suites can take 30+ minutes to complete.

**Run tests after build:**
```bash
./build.sh --test
```

**Test individual repositories:**
Navigate to the specific src directory and use their build scripts:
```bash
cd src/runtime
./build.sh --test
```

### Working with Sub-repositories

**Each sub-repository has its own copilot instructions** (10 found):
- `src/runtime/.github/copilot-instructions.md`
- `src/sdk/.github/copilot-instructions.md` 
- `src/aspnetcore/.github/copilot-instructions.md`
- `src/efcore/.github/copilot-instructions.md`
- `src/fsharp/.github/copilot-instructions.md`
- `src/msbuild/.github/copilot-instructions.md`
- `src/arcade/.github/copilot-instructions.md`
- `src/nuget-client/.github/copilot-instructions.md`
- `src/vstest/.github/copilot-instructions.md`
- `src/winforms/.github/copilot-instructions.md`

**ALWAYS refer to sub-repository instructions** when working on code within specific src/ directories.

**Testing individual sub-repositories:**
Most sub-repositories can be built independently:
```bash
cd src/command-line-api
./build.sh  # Takes ~45 seconds - verified working
```

**Use VMR-local .NET SDK:**
Each sub-repository may download its own .NET SDK to `.dotnet/` directory. The VMR also has a shared SDK at the root level in `.dotnet/`.

### Validation Scenarios

**Before committing changes to the VMR:**

1. **Always build the VMR successfully** (if changes affect multiple repositories):
   ```bash
   ./build.sh --clean-while-building  # 45-60 minutes
   ```

2. **For single repository changes, build that repository:**
   ```bash
   cd src/[repository-name]
   ./build.sh  # Usually 30 seconds to 5 minutes for individual repos
   ```

3. **Run relevant tests:**
   ```bash
   ./build.sh --test  # 30+ minutes for VMR-level tests
   # OR for individual repositories:
   cd src/[repository-name] && ./build.sh --test
   ```

4. **Follow sub-repository validation steps** as specified in their copilot instructions.

### Known Issues and Limitations

**Build Environment Challenges:**
- Complex dependency requirements may cause build failures
- Package version conflicts are common in the current VMR state (especially with roslyn)
- Some builds may require specific OS versions or additional system packages
- Not all build configurations work in all environments

**Common Build Failures and Solutions:**

1. **Missing libkrb5-dev:** 
   ```bash
   sudo apt install -y libkrb5-dev
   ```

2. **Missing liblttng-ust-dev:**
   ```bash
   sudo apt install -y liblttng-ust-dev
   ```

3. **Package version conflicts in roslyn build:**
   - This is a known issue with the current VMR state
   - Individual sub-repository builds often work when VMR build fails
   - Consider using Docker-based builds for full VMR builds

4. **NuGet restore failures:**
   - Check that all prerequisites are installed
   - Verify network connectivity to NuGet feeds
   - Review `NuGet.config` for proper feed configuration

**When builds fail:**
1. Check that all prerequisites are installed using the commands above
2. Review the specific error messages in build logs under `artifacts/log/`
3. Try building individual sub-repositories instead of full VMR
4. Consider using Docker-based builds as documented in README.md
5. Refer to individual repository build instructions in `src/*/docs/`

### Docker Alternative

**If local builds fail, use Docker:**
```bash
docker run --rm -it -v vmr:/vmr -w /vmr mcr.microsoft.com/dotnet-buildtools/prereqs:centos-stream-10-amd64
git clone https://github.com/dotnet/dotnet .
./prep-source-build.sh && ./build.sh -sb --clean-while-building
```

### Common File Locations

**Build outputs:**
- `artifacts/assets/Release/` - Final SDK packages
- `artifacts/log/` - Build logs
- `artifacts/bin/` - Built binaries

**Configuration files:**
- `global.json` - .NET SDK version
- `Directory.Build.props` - Global MSBuild properties
- `NuGet.config` - NuGet package sources

### Timing Expectations

**CRITICAL - NEVER CANCEL these operations:**
- `./prep-source-build.sh` - **5-6 minutes** ✓ Verified working
- Individual sub-repository builds (e.g., `src/command-line-api/build.sh`) - **30 seconds to 5 minutes** ✓ Verified working  
- `./build.sh --clean-while-building` - **45-60 minutes** ⚠️ May fail due to version conflicts
- `./build.sh -sb --clean-while-building` - **90+ minutes** ⚠️ May fail due to missing dependencies
- `./build.sh --test` - **30+ minutes**

**Always set timeouts with significant buffer:**
- Build commands: **120+ minutes timeout**
- Test commands: **45+ minutes timeout**
- Prep commands: **15+ minutes timeout**

### SDK Usage After Build

**Once built, use the local SDK:**
```bash
export PATH="$(pwd)/artifacts/assets/Release:$PATH"
# Or extract and install the built SDK tarball
```

## Key Principles

- **Always validate commands before suggesting them**
- **Never cancel long-running builds or tests**
- **Use sub-repository instructions for specific component work**
- **Expect build complexity and potential failures**
- **Set generous timeouts for all operations**
- **Clean while building to manage disk space**

For the latest information and troubleshooting, refer to the main README.md and docs/ directory.