// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Security.AntiSSRF
{
    public sealed class AntiSSRFException : Exception
    {
        internal AntiSSRFException() : base() { }
        internal AntiSSRFException(string message) : base(message) { }
    }
}