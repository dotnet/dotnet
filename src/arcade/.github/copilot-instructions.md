# Arcade SDK - .NET Infrastructure Tooling

Arcade SDK is the core infrastructure tooling used across the .NET ecosystem for consistent build, test, packaging, signing, and deployment processes. This repository contains over 50 projects providing MSBuild tasks, SDK components, Helix testing infrastructure, SignTool, and shared build tooling.

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Critical Prerequisites

### .NET SDK Installation
- **NEVER** install an SDK. It is installed by building the repository.

### Network Dependencies
- **CRITICAL**: Requires access to Azure DevOps package feeds (dev.azure.com/dnceng)
- If build fails with "Unable to load service index" errors, network access to Azure feeds is blocked
- Key feeds: dotnet-public, dotnet-tools, dotnet-eng, dotnet9, dotnet10 (see NuGet.config)

## Working Effectively

### Build Process
**NEVER CANCEL BUILDS OR TESTS** - they may take 90+ minutes. Always use appropriate timeouts.

```bash

# Full restore, build, and test - TAKES 90+ MINUTES - NEVER CANCEL
timeout 6000 ./build.sh --restore --build
# Set timeout to 100+ minutes (6000 seconds) for build commands

# Test execution - TAKES 30+ MINUTES - NEVER CANCEL  
timeout 2400 ./build.sh --restore --build --test
# Set timeout to 40+ minutes (2400 seconds) for test commands

# Restore only (faster for dependency checks)
timeout 1800 ./build.sh --restore
# Set timeout to 30+ minutes (1800 seconds) for restore

# Clean build artifacts
./build.sh --clean
```

### Platform-Specific Commands
- **Linux/macOS**: `./build.sh`, `./test.sh`, `./restore.sh`
- **Windows**: `Build.cmd`, `Test.cmd`, `Restore.cmd`
- All commands use `eng/common/build.sh` or `eng/common/Build.ps1` internally

## Key Projects and Components

### Core Infrastructure
- **Microsoft.DotNet.Arcade.Sdk**: Core MSBuild SDK with props/targets for consistent builds
- **Microsoft.DotNet.Helix**: Distributed testing infrastructure (Client, JobSender, Sdk)
- **Microsoft.DotNet.SignTool**: Code signing automation and certificate management
- **Microsoft.DotNet.Build.Tasks.Feed**: NuGet package publishing and feed management

### Build Tasks and Tooling
- **Microsoft.DotNet.Build.Tasks.Packaging**: NuGet package creation and validation
- **Microsoft.DotNet.Build.Tasks.Installers**: MSI/PKG installer generation
- **Microsoft.DotNet.Build.Tasks.Workloads**: .NET workload packaging
- **Microsoft.DotNet.Build.Tasks.VisualStudio**: VSIX and VS insertion support

### Testing Infrastructure  
- **Microsoft.DotNet.XUnitExtensions**: Enhanced XUnit capabilities for .NET testing
- **Microsoft.DotNet.RemoteExecutor**: Cross-platform process execution for tests
- **Microsoft.DotNet.PackageTesting**: Automated package validation testing

## Build Artifacts Structure
```
artifacts/
├── bin/                    # Compiled binaries by project/configuration
├── packages/               # Generated NuGet packages (Shipping/NonShipping)
├── TestResults/            # Unit and integration test results
├── log/                    # Build logs and binary logs (.binlog)
├── tmp/                    # Temporary build artifacts
└── toolset/               # Downloaded build tools and dependencies
```

## Validation and Testing

### Manual Validation Requirements
After making changes, **ALWAYS** run complete validation scenarios:

```bash
# Build validation - NEVER CANCEL - 90+ minute timeout
timeout 6000 ./build.sh --restore --build --configuration Release --test
```

### Test Categories
- **Unit Tests**: Individual project test suites (tests/*.Tests.csproj)
- **Integration Tests**: Cross-component validation via Helix
- **SDK Tests**: Validate Arcade SDK works in sample projects
- **Packaging Tests**: Ensure generated packages are valid

## Common Development Tasks

### Adding New Build Tasks
1. Create new project under `src/Microsoft.DotNet.Build.Tasks.*`
2. Reference `Microsoft.Build.Utilities.Core` and `Microsoft.Build.Framework`
3. Add props/targets files for MSBuild integration
4. Include in `Arcade.slnx` solution file
5. **ALWAYS** add unit tests for new tasks

### Package Development
1. Follow naming convention: `Microsoft.DotNet.*`
2. Update `eng/Versions.props` with version dependencies
3. Add to appropriate `Directory.Packages.props` if needed
4. Validate packaging via: `./build.sh --pack --configuration Release`

### Helix Test Development
1. Create test projects targeting `$(BundledNETCoreAppTargetFramework)` framework
2. Configure in `tests/UnitTests.proj` for Helix execution
3. Use XUnit with `Microsoft.DotNet.XUnitExtensions` for enhanced capabilities
4. Test timeout default: 300 seconds (override with XUnitWorkitemTimeout)

## Troubleshooting

### Build Failures
- **"Could not resolve SDK Microsoft.DotNet.Arcade.Sdk"**: Network connectivity to Azure feeds issue
- **Wrong .NET version**: Run `./eng/common/dotnet.sh` to install correct SDK
- **MSBuild errors**: Check `artifacts/log/` for detailed binary logs (.binlog)

### Test Failures
- Check `artifacts/TestResults/` for detailed test output
- Use `--verbosity diagnostic` for detailed MSBuild logging
- Helix test failures: Check Azure DevOps Helix console for distributed test results

### Development Environment
- IDE: Use Visual Studio 2019+ or VS Code with C# extension
- Required: MSBuild 16.8+ (included with correct .NET SDK)
- Recommended: Install .NET Upgrade Assistant for migration scenarios

## Network and Security Requirements

### Internal Dependencies
- **Azure DevOps Feeds**: Required for package restore (dnceng organization)
- **Maestro/BAR**: Dependency flow automation (internal Microsoft systems)
- **MicroBuild**: Code signing and compliance (Microsoft internal)

### External Dependencies
- **NuGet.org**: For third-party packages (when not source-mapped)
- **GitHub**: Source code and issue tracking
- **Build artifacts**: Published to Azure blob storage

## CI/CD Integration

### Azure DevOps Pipelines
- **PR Validation**: `azure-pipelines-pr.yml` (90-minute timeout)
- **Official Builds**: `azure-pipelines.yml` (90-minute timeout + validation)
- **Daily Builds**: `azure-pipelines-daily.yaml` (extended validation)

### Pipeline Stages
1. **Build**: Restore, build, pack (90 minutes)
2. **Test**: Unit and integration tests (30 minutes) 
3. **Validate SDK**: Test against sample projects (90 minutes)
4. **Publish**: Package publishing and signing (varies)

## Important Notes

- **Build Times**: Expect 45-90 minutes for full builds, 15-30 minutes for tests
- **Timeouts**: Always set 90+ minute timeouts for builds, 30+ minutes for tests
- **Cancellation**: **NEVER CANCEL** long-running operations - they will complete
- **Dependencies**: This repo builds infrastructure used by 100+ .NET repositories
- **Servicing**: Changes affect the entire .NET ecosystem - test thoroughly
- **Validation**: Manual testing required - automated tests don't cover all scenarios

## Getting Help

- **Issues**: File in dotnet/arcade repository for all infrastructure problems
- **Discussions**: Use dotnet/arcade discussions for questions
- **Documentation**: See `/Documentation/` folder for detailed guides
- **Contact**: @dotnet/dnceng team for infrastructure issues
