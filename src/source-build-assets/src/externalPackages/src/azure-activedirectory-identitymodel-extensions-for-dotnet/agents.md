# Agent Automation and Development Guidelines

This document provides comprehensive guidelines for AI agents and automation workflows in the Azure AD IdentityModel Extensions for .NET repository. These guidelines ensure consistent, high-quality contributions and effective collaboration between human developers and AI assistants.

## Table of Contents

- [Agent Overview](#agent-overview)
- [AI Assistant Workflow](#ai-assistant-workflow)
- [C# Development Standards](#c-development-standards)
- [IdentityModel Guidelines](#identitymodel-guidelines)
- [Configuration Examples](#configuration-examples)

## Agent Overview

### Supported Agents

This repository supports various AI agents for development automation:

- **Cline AI Assistant** - Primary development assistant for code modifications
- **GitHub Copilot** - Code completion and suggestions
- **Other AI Agents** - Following these guidelines ensures compatibility

### Core Principles

```yaml
agent_principles:
  incremental_changes: true
  pattern_analysis: "Always analyze existing code patterns before making changes"
  tool_preference: "Prioritize built-in tools over shell commands"
  conventions: "Follow existing project patterns and conventions"
  test_coverage: "Maintain comprehensive test coverage"
```

## AI Assistant Workflow

### Development Phases

#### Planning Phase (PLAN MODE)

```yaml
planning_requirements:
  complex_tasks:
    - begin_in_plan_mode: true
    - analyze_codebase_patterns: true
    - review_test_files: true
    - present_clear_steps: true
    - ask_clarifying_questions: true
```

**Planning Checklist:**
- [ ] Analyze existing codebase patterns using search tools
- [ ] Review related test files to understand testing patterns
- [ ] Present clear implementation steps for approval
- [ ] Ask clarifying questions early to avoid rework

#### Implementation Phase (ACT MODE)

```yaml
implementation_workflow:
  change_strategy: "incremental"
  verification: "each_step"
  pattern_following: "discovered_during_planning"
  coverage_focus: "maintain_test_coverage"
  feedback_handling: "use_linter_errors_for_guidance"
```

**Implementation Checklist:**
- [ ] Make changes incrementally, one file at a time
- [ ] Verify each change before proceeding
- [ ] Follow patterns discovered during planning phase
- [ ] Focus on maintaining test coverage
- [ ] Use error messages and linter feedback to guide fixes

### Tool Usage Guidelines

#### File Operations

```yaml
file_operations:
  read_file: "Use for examining file contents instead of shell commands like cat"
  replace_in_file: "Use for targeted, specific changes to existing files"
  write_to_file: "Use only for new files or complete file rewrites"
  list_files: "Use to explore directory structures"
  search_files: "Use with precise regex patterns to find code patterns"
  list_code_definition_names: "Use to understand code structure before modifications"
```

#### Command Execution

```yaml
command_execution:
  usage: "sparingly"
  preference: "built_in_file_operation_tools"
  explanations: "always_provide_clear_explanations"
  approval: "set_requires_approval_for_impactful_operations"
```

### Quality Standards

#### Code Modifications

```yaml
code_quality:
  editorconfig: "follow_strictly"
  file_headers: "preserve_license_information"
  documentation: "maintain_consistent_xml_documentation"
  error_handling: "respect_existing_patterns"
  line_endings: "keep_consistent_with_existing_files"
```

#### Quality Checks

```yaml
quality_verification:
  - verify_changes_match_existing_code_style
  - ensure_test_coverage_for_new_code
  - validate_changes_against_project_conventions
  - check_for_proper_error_handling
  - maintain_nullable_reference_type_annotations
```

### Error Handling

```yaml
error_handling:
  messages: "provide_clear_error_messages_and_suggestions"
  failures: "handle_tool_operation_failures_gracefully"
  alternatives: "suggest_alternative_approaches_when_primary_fails"
  rollback: "rollback_changes_if_necessary_to_maintain_stability"
```

## C# Development Standards

### General Guidelines

```yaml
csharp_standards:
  version: "C# 13"
  file_restrictions:
    - never_change_global_json: "unless_explicitly_asked"
    - never_change_package_json: "unless_explicitly_asked"
    - never_change_nuget_config: "unless_explicitly_asked"
```

### Code Formatting

```yaml
formatting_rules:
  editorconfig: "apply_code_formatting_style_defined_in_editorconfig"
  namespace_declarations: "prefer_file_scoped_namespace_declarations"
  using_directives: "single_line_using_directives"
  curly_braces: "insert_newline_before_opening_curly_brace"
  return_statements: "ensure_final_return_statement_on_own_line"
  pattern_matching: "use_pattern_matching_and_switch_expressions"
  member_names: "use_nameof_instead_of_string_literals"
  xml_documentation: "ensure_xml_doc_comments_for_public_apis"
```

### Nullable Reference Types

```yaml
nullable_reference_types:
  declaration: "declare_variables_non_nullable"
  null_checks: "check_for_null_at_entry_points"
  comparison: "use_is_null_or_is_not_null_instead_of_equality"
  trust_annotations: "trust_csharp_null_annotations"
```

### Testing Standards

```yaml
testing_framework:
  sdk: "xUnit SDK v2"
  comments: "emit_Act_Arrange_Assert_comments"
  mocking: "Moq 4.14.x"
  naming: "copy_existing_style_for_test_method_names"
  execution: "dotnet test"
```

## IdentityModel Guidelines

### Repository Overview

```yaml
repository_purpose:
  description: "Foundational authentication and authorization library"
  specializations:
    - jwt_creation_validation_management
    - oidc_oauth2_protocol_implementations
    - token_validation_security_key_management
    - high_performance_token_handling
    - saml_token_support
    - ws_federation_protocol_support
```

### Repository Structure

```yaml
directory_structure:
  src: "All source code for Microsoft.IdentityModel packages"
  src_subdirs:
    JsonWebTokens: "Core JWT functionality"
    Protocols: "Protocol implementations (OIDC, OAuth, WS-Fed)"
    Tokens: "Token handling and validation"
    Xml: "XML security functionality"
    Validators: "Token validation components"
  tests: "Unit tests, integration tests, and test utilities"
  benchmark: "Performance benchmarking infrastructure"
  build: "Build configuration and scripts"
```

### Shipped Packages

```yaml
core_packages:
  token_handling:
    - "Microsoft.IdentityModel.JsonWebTokens"
    - "Microsoft.IdentityModel.Tokens"
    - "System.IdentityModel.Tokens.Jwt"
  protocol_support:
    - "Microsoft.IdentityModel.Protocols"
    - "Microsoft.IdentityModel.Protocols.OpenIdConnect"
    - "Microsoft.IdentityModel.Protocols.WsFederation"
    - "Microsoft.IdentityModel.Protocols.SignedHttpRequest"
  security_integration:
    - "Microsoft.IdentityModel.Tokens.Saml"
    - "Microsoft.IdentityModel.Xml"
    - "Microsoft.IdentityModel.Validators"
```

### Development Principles

```yaml
development_principles:
  compatibility: "maintain_backward_compatibility_due_to_widespread_usage"
  performance: "prioritize_performance_in_token_handling_operations"
  security: "implement_thorough_security_validation"
  dependencies: "keep_dependencies_minimal_and_well_justified"
  testing: "maintain_comprehensive_test_coverage"
```

### Performance Requirements

```yaml
performance_standards:
  - design_for_high_throughput_token_validation
  - optimize_memory_allocation_patterns
  - consider_token_caching_strategies
  - profile_performance_critical_paths
  - benchmark_changes_affecting_token_processing
```

### Security Guidelines

```yaml
security_requirements:
  - follow_security_best_practices_for_cryptographic_operations
  - validate_all_token_parameters_thoroughly
  - handle_security_keys_with_appropriate_precautions
  - implement_proper_error_handling_for_security_operations
  - document_security_considerations_for_public_apis
```

### API Management

#### Public and Internal API Changes

```yaml
api_management:
  analyzer: "Microsoft.CodeAnalysis.PublicApiAnalyzers"
  public_api_changes:
    file: "PublicAPI.Unshipped.txt"
    location: "relevant_package_directory"
    requirement: "update_for_any_public_api_change"
  internal_api_changes:
    file: "InternalAPI.Unshipped.txt"
    location: "relevant_package_directory"
    requirement: "update_for_any_internal_api_change"
  considerations:
    - include_complete_api_signatures
    - consider_backward_compatibility_impacts
    - document_breaking_changes_clearly
```

#### API Change Examples

```yaml
api_change_format: |
  # Adding new API
  +Microsoft.IdentityModel.Tokens.TokenValidationResult.Clone() -> Microsoft.IdentityModel.Tokens.TokenValidationResult
  +Microsoft.IdentityModel.Tokens.SecurityKey.KeySize.get -> int
  
  # Removing API (requires careful consideration)
  -Microsoft.IdentityModel.Tokens.ObsoleteTokenValidationMethod() -> void
```

## Configuration Examples

### Agent Configuration Template

```yaml
agent_config:
  name: "IdentityModel Development Agent"
  capabilities:
    - code_analysis
    - incremental_changes
    - test_maintenance
    - api_documentation
  restrictions:
    - no_global_json_changes
    - no_package_json_changes
    - no_nuget_config_changes
  quality_gates:
    - editorconfig_compliance
    - test_coverage_maintenance
    - api_analyzer_compliance
    - security_validation
```

### Workflow Automation

```yaml
automation_workflows:
  code_review:
    - pattern_analysis
    - style_verification
    - test_coverage_check
    - api_compatibility_verification
  testing:
    - unit_test_execution
    - integration_test_execution
    - performance_benchmark_validation
  documentation:
    - api_documentation_generation
    - changelog_updates
    - security_documentation_updates
```

## Contributing with Agents

### For Contributors

This document serves as the authoritative guide for AI agent behavior in this repository. When working with AI assistants:

1. **Reference this document** - Point agents to specific sections relevant to your task
2. **Verify compliance** - Ensure agent output follows these guidelines
3. **Provide feedback** - Report issues or suggest improvements to these guidelines

### For Agents

When working in this repository:

1. **Follow these guidelines strictly** - These rules ensure code quality and consistency
2. **Prioritize incremental changes** - Make small, verifiable modifications
3. **Maintain test coverage** - Ensure all changes include appropriate tests
4. **Respect existing patterns** - Analyze and follow established code patterns
5. **Document API changes** - Update appropriate .txt files for API modifications

---

*Last updated: During migration from .clinerules directory*
*For questions or updates to these guidelines, please open an issue or pull request.*