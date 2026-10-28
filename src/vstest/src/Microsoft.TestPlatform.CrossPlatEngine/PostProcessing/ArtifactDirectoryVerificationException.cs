// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;

/// <summary>
/// Thrown when the directory the test session artifacts would be written to, or read back from, cannot be
/// established as private to the current user.
/// </summary>
/// <remarks>
/// This is deliberately a type of its own. Artifact processing traces and swallows its failures, because
/// losing the artifacts of a run degrades post processing rather than breaking it, but a directory that
/// could not be verified must never be written to or read from, so this one has to reach the caller.
/// </remarks>
internal sealed class ArtifactDirectoryVerificationException : Exception
{
    public ArtifactDirectoryVerificationException(string message)
        : base(message)
    {
    }

    public ArtifactDirectoryVerificationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
