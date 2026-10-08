// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

internal sealed class PeerProcessValidationException : TestPlatformException
{
    internal PeerProcessValidationException(string message)
        : base(message)
    {
    }

    internal PeerProcessValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

internal enum PeerProcessValidationFailureKind
{
    None,
    Rejected,
    Fatal,
}

internal readonly struct PeerProcessValidationResult
{
    private PeerProcessValidationResult(bool succeeded, PeerProcessValidationFailureKind failureKind, string? error)
    {
        Succeeded = succeeded;
        FailureKind = failureKind;
        Error = error;
    }

    internal bool Succeeded { get; }

    internal PeerProcessValidationFailureKind FailureKind { get; }

    internal string? Error { get; }

    internal static PeerProcessValidationResult Success()
        => new(true, PeerProcessValidationFailureKind.None, null);

    internal static PeerProcessValidationResult Rejected(string error)
        => new(false, PeerProcessValidationFailureKind.Rejected, error);

    internal static PeerProcessValidationResult Fatal(string error)
        => new(false, PeerProcessValidationFailureKind.Fatal, error);
}