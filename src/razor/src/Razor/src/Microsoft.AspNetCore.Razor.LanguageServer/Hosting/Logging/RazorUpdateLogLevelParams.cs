// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;

/// <summary>
/// Request parameters for updating the log level in the server dynamically.
/// </summary>
/// <param name="LogLevel">the int value of the <see cref="LogLevel"/> enum</param>
internal record class UpdateLogLevelParams([property: JsonPropertyName("logLevel")] int LogLevel);
