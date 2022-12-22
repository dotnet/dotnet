// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Signing
{
    public abstract class VerificationAllowListEntry
    {
        /// <summary>
        /// Target type of signature to verify.
        /// </summary>
        public VerificationTarget Target { get; }

        /// <summary>
        /// Signature placement to verify.
        /// </summary>
        public SignaturePlacement Placement { get; }

        public VerificationAllowListEntry(VerificationTarget target, SignaturePlacement placement)
        {
            if (target == VerificationTarget.Author && placement.HasFlag(SignaturePlacement.Countersignature))
            {
                throw new ArgumentException(Strings.ErrorAuthorTargetCannotBeACountersignature);
            }

            Target = target;
            Placement = placement;
        }
    }
}
