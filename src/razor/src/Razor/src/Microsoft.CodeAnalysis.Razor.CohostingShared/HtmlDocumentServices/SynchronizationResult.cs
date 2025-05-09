// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

/// <summary>
/// A result of a synchronization operation for a Html document
/// </summary>
/// <remarks>
/// If <see cref="Synchronized" /> is <see langword="false" />, <see cref="Checksum" /> will be <see langword="default" />.
/// </remarks>
internal readonly record struct SynchronizationResult(bool Synchronized, ChecksumWrapper Checksum);
