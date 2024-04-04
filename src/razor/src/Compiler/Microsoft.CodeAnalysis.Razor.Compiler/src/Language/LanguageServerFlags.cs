// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
/// This class represents flags provided by the language server instead of project configuration.
/// They should not be serialized as part of the configuration, as they instead change runtime behavior
/// impacted by LSP configuration rather than any project configuration
/// </summary>
public sealed record class LanguageServerFlags(bool ForceRuntimeCodeGeneration);
