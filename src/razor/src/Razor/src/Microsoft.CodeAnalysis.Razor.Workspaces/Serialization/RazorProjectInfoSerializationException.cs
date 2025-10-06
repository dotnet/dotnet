// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal class RazorProjectInfoSerializationException(string? message) : Exception(message)
{
}
