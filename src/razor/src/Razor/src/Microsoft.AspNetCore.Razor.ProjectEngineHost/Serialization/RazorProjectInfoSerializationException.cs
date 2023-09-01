// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal class RazorProjectInfoSerializationException(string? message) : Exception(message)
{
}
