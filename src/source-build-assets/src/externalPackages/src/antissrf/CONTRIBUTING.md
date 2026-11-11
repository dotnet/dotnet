# Contributing to AntiSSRF

Thank you for your interest in contributing to Microsoft AntiSSRF! This guide will help you get started with contributing to our security library.

## Development Setup

### AntiSSRF .NET Library

**Prerequisites:**
- .NET 8.0 SDK
- Make sure you are in the `/dotnet` folder

**Useful Commands:**
- Restore dependencies: `dotnet restore`
- Build solution: `dotnet build`
- Run all tests: `dotnet test`

### AntiSSRF Node.js Library

**Prerequisites:**
- Node.js 20.0 or later
- Make sure you are in the `/nodejs` folder

**Useful Commands:**
- Install dependencies: `npm install`
- Build TypeScript: `npm run build`
- Run all tests: `npm test`
- Lint code: `npm run lint`
- Format code: `npm run format`

### Documentation

**Prerequisites:**
- Jekyll (for GitHub Pages)
- Ruby 3.4 or later
- Bundler (`gem install bundler`)
- Make sure you are in the `/docs` folder

**Useful Commands:**
- Install dependencies: `bundle install`
- Test locally: `bundle exec jekyll serve`

## Submitting Issues

We welcome bug reports and feature requests! Please use our issue templates to provide the information we need:

### Bug Reports
- Report in our [GitHub Issues](https://github.com/microsoft/AntiSSRF/issues)
- Please search existing issues first to avoid duplicates
- Provide clear reproduction steps and expected behavior
- Include relevant version information and environment details

### Feature Requests
- Report in our [GitHub Issues](https://github.com/microsoft/AntiSSRF/issues)
- Clearly describe the problem your feature would solve
- Explain your proposed solution and any alternatives considered
- Specify which library (or both) the feature applies to

## Contributing Code

We welcome pull requests for bug fixes, features, and documentation improvements:

### Before Submitting
- Verify that all existing tests pass without failures
- Add appropriate test coverage for your changes
- Ensure there are no breaking changes (in rare cases where unavoidable, discuss with maintainers in an issue first)
- Documentation updates are optional but very appreciated

### When Discussion is Required

Before submitting a PR, you must create an issue and discuss with maintainers if your changes involve:

- Breaking changes - Any modification that could break existing functionality. All other possibilities must be considered before introducing a breaking change.
- New API additions - Adding new public methods, classes, or interfaces.
- Configuration file changes - Modifications to IP address ranges or domains configuration files. These changes will almost always be rejected unless there is an exceptional reason.

### Pull Request Process
- Be detailed in your description and reproduction steps
- Reference any related issues, bugs, or feature requests
