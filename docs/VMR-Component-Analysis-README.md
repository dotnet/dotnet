# VMR Component Analysis Documentation

This directory contains the comprehensive analysis of dotnet/dotnet VMR components and their usage of runtime packages and targeting packs.

## 📊 Analysis Report (February 2026)

### Quick Start

**TL;DR:** ✅ All VMR components are properly configured. No issues found.

### Documentation Files

1. **[Visual Summary](VMR-Component-Analysis-Visual-Summary.md)** 📊
   - Start here for a quick overview
   - Diagrams and status matrix
   - At-a-glance component status

2. **[Quick Reference](VMR-Component-Targeting-Packs-Quick-Reference.md)** 📋
   - Component-by-component status table
   - Quick check commands
   - When to add Microsoft.NETCore.App.Ref

3. **[Full Analysis Report](VMR-Component-Analysis-Runtime-Targeting-Packs.md)** 📖
   - Complete detailed analysis
   - Component-by-component breakdown
   - Patterns and recommendations
   - Methodology and tooling

4. **[Configuration Checklist](VMR-Component-Targeting-Packs-Checklist.md)** ✅
   - Step-by-step checklist for component maintainers
   - Verification steps
   - Common issues and solutions
   - Examples to follow

## 🎯 Key Findings

- **25 components analyzed** across the VMR
- **10 components** consume runtime packages AND properly reference Microsoft.NETCore.App.Ref
- **1 special case** (roslyn) uses runtime packs for AOT only - intentionally does not need live targeting pack
- **14 components** do not use runtime packages - no targeting pack needed
- **0 issues found** - all components properly configured

## 📈 Component Status

| Status | Count |
|--------|-------|
| ✅ Properly Configured | 10 |
| ⚠️ Special Case | 1 |
| ❌ Issues Found | 0 |

## 🔍 What Was Analyzed

The analysis scanned for:
- Components consuming `Microsoft.NETCore.App.Runtime.*` packages
- Usage of `Microsoft.NETCore.App.Ref` targeting pack
- `KnownRuntimePack` and `KnownFrameworkReference` configurations
- `FrameworkReference` declarations
- Version.Details.xml dependencies

## 👥 Audience

- **Component Maintainers:** Use the checklist when adding/updating components
- **Build Engineers:** Reference the full analysis for build system integration
- **Architects:** Review patterns and recommendations for future planning
- **New Contributors:** Start with the quick reference to understand requirements

## 🛠️ For Component Maintainers

### Adding a New Component That Uses .NET APIs

1. Read: [Configuration Checklist](VMR-Component-Targeting-Packs-Checklist.md)
2. Add Microsoft.NETCore.App.Ref to your `eng/Version.Details.xml`
3. Configure targeting pack download in build files
4. Test build in VMR context
5. Document any special requirements

### Example Components to Follow

- **aspnetcore** - Extensive runtime and targeting pack usage
- **wpf** - PackageDownload pattern
- **sdk** - Layout and bundling scenarios

## 📚 Background

### Why This Matters

Components that consume runtime packages (e.g., `Microsoft.NETCore.App.Runtime.*`) for deployment should also reference the targeting pack (`Microsoft.NETCore.App.Ref`) for compilation. This ensures:

1. ✅ Compilation uses reference assemblies (faster builds)
2. ✅ API surface is correctly validated at build time
3. ✅ Runtime and compile-time dependencies are separated
4. ✅ Source-build can resolve dependencies correctly

### What's the Difference?

- **Microsoft.NETCore.App.Ref** (Targeting Pack)
  - Reference assemblies for compilation
  - No implementation details
  - Defines the API contract

- **Microsoft.NETCore.App.Runtime.*** (Runtime Packs)
  - Implementation assemblies
  - Platform-specific (RID-specific)
  - For deployment and execution

## 🔄 Regular Maintenance

This analysis should be re-run:
- When new components are added to the VMR
- Quarterly as part of health monitoring
- After major runtime updates
- Before major VMR restructuring

## 📧 Questions or Issues?

- Review the [Full Analysis Report](VMR-Component-Analysis-Runtime-Targeting-Packs.md) first
- Check the [Checklist](VMR-Component-Targeting-Packs-Checklist.md) for common scenarios
- Open an issue in the dotnet/dotnet repository for clarification

## 🤝 Contributing

If you identify a component that should be analyzed or find an issue:
1. Verify against the full analysis report
2. Check if it's a documented special case
3. Open an issue with details of the component and concern
4. Reference this documentation

## 📜 Analysis Metadata

- **Analysis Date:** February 11, 2026
- **VMR Commit:** c7b9b2ea2
- **Components Analyzed:** 25
- **Files Scanned:** 10,617 project files
- **Methodology:** Automated scanning with manual verification
- **Tools Used:** Bash, ripgrep, Python

---

## Quick Navigation

- 📊 [Visual Summary](VMR-Component-Analysis-Visual-Summary.md) - Start here
- 📋 [Quick Reference](VMR-Component-Targeting-Packs-Quick-Reference.md) - Tables and commands
- 📖 [Full Report](VMR-Component-Analysis-Runtime-Targeting-Packs.md) - Complete analysis
- ✅ [Checklist](VMR-Component-Targeting-Packs-Checklist.md) - For maintainers

---

*This analysis ensures all VMR components properly integrate with the .NET runtime and targeting packs.*
