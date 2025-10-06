// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;

namespace Microsoft.DotNet.SignTool
{
    internal readonly struct SignInfo
    {
        /// <summary>
        /// Used to flag that the signing for the file is not necessary.
        /// </summary>
        public static readonly SignInfo Ignore = new SignInfo(ignoreThisFile: true, alreadySigned: false, isAlreadyStrongNamed: false);

        /// <summary>
        /// Used to flag that the file is already signed.
        /// </summary>
        public static readonly SignInfo AlreadySigned = new SignInfo(ignoreThisFile: false, alreadySigned: true, isAlreadyStrongNamed: false);

        /// <summary>
        /// The authenticode certificate which should be used to sign the binary. This can be null
        /// in cases where we have a zip container where the contents are signed but not the actual
        /// container itself. This is the case when dealing with nupkg files.
        /// </summary>
        internal string Certificate { get; }

        /// <summary>
        /// This will be null in the case a strong name signing is not required.
        /// </summary>
        internal string StrongName { get; }

        /// <summary>
        /// The app name com.microsoft.[APP NAME] that should be used to notarize the binary.
        /// If empty, the binary is not notarized.
        /// </summary>
        internal string NotarizationAppName { get; }

        internal bool ShouldIgnore { get; }

        internal bool IsAlreadyStrongNamed { get; }

        internal bool IsAlreadySigned { get; }

        /// <summary>
        /// This is used to decide what SignInfos to use in the case of a collision. In case of a collision
        /// we'll use the lower value since it would map a lower node in the graph and has precedence
        /// over the rest
        /// </summary>
        internal string CollisionPriorityId { get; }

        public bool ShouldLocallyStrongNameSign => ShouldStrongName && StrongName.EndsWith(".snk", StringComparison.OrdinalIgnoreCase);

        public bool ShouldSign => !IsAlreadySigned && !ShouldIgnore;

        public bool ShouldStrongName => !IsAlreadyStrongNamed && !string.IsNullOrEmpty(StrongName);

        public bool ShouldNotarize => !string.IsNullOrEmpty(NotarizationAppName) && !ShouldIgnore;

        public SignInfo(string certificate, string strongName, string notarizationAppName, string collisionPriorityId, bool shouldIgnore, bool isAlreadySigned, bool isAlreadyStrongNamed)
        {
            ShouldIgnore = shouldIgnore;
            IsAlreadySigned = isAlreadySigned;
            Certificate = certificate;
            StrongName = strongName;
            CollisionPriorityId = collisionPriorityId;
            IsAlreadyStrongNamed = isAlreadyStrongNamed;
            NotarizationAppName = notarizationAppName;
        }

        private SignInfo(bool ignoreThisFile, bool alreadySigned, bool isAlreadyStrongNamed)
            : this(certificate: null, strongName: null, notarizationAppName: null, collisionPriorityId: null, ignoreThisFile, alreadySigned, isAlreadyStrongNamed)
        {
        }

        internal SignInfo(string certificate, string strongName = null, string notarization = null, string collisionPriorityId = null)
            : this(certificate, strongName, notarization, collisionPriorityId, shouldIgnore: false, isAlreadySigned: false, isAlreadyStrongNamed: false)
        {
        }

        internal SignInfo WithCertificateName(string value, string collisionPriorityId)
            => new SignInfo(value, StrongName, NotarizationAppName, collisionPriorityId, false, false, IsAlreadyStrongNamed);

        internal SignInfo WithNotarization(string appName, string collisionPriorityId)
            => new SignInfo(Certificate, StrongName, appName, collisionPriorityId, false, false, IsAlreadyStrongNamed);

        internal SignInfo WithCollisionPriorityId(string collisionPriorityId)
            => new SignInfo(Certificate, StrongName, NotarizationAppName, collisionPriorityId, ShouldIgnore, IsAlreadySigned, IsAlreadyStrongNamed);

        internal SignInfo WithIsAlreadySigned(bool value = false)
            => Certificate != null ? 
              new SignInfo(Certificate, StrongName, NotarizationAppName, CollisionPriorityId, value, value, IsAlreadyStrongNamed) :
              new SignInfo(Certificate, StrongName, NotarizationAppName, CollisionPriorityId, true, value, IsAlreadyStrongNamed);

        internal SignInfo WithIsAlreadyStrongNamed(bool value = false) =>
              new SignInfo(Certificate, StrongName, NotarizationAppName, CollisionPriorityId, ShouldIgnore, IsAlreadySigned, value);
    }
}
