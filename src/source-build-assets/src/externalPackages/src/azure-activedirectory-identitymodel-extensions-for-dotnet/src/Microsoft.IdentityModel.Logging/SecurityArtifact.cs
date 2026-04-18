// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// An internal structure that is used to mark an argument as SecurityArtifact.
    /// Arguments wrapped with a SecurityArtifact structure will be considered as a SecurityArtifact in the message logging process.
    /// </summary>
    /// <remarks>
    /// SecurityToken and encoded token are considered as SecurityArtifacts.
    /// </remarks>
    internal struct SecurityArtifact : ISafeLogSecurityArtifact
    {
        private const string _scrubbedArtifact = "#ScrubbedArtifact#";

        /// <summary>
        /// Argument wrapped with a <see cref="SecurityArtifact"/> structure is considered as SecurityArtifact in the message logging process.
        /// </summary>
        private object Argument { get; set; }

        /// <summary>
        /// The ToString callback delegate that returns a disarmed SecurityArtifact.
        /// </summary>
        private readonly Func<object, string> _disarmCallback;

        /// <summary>
        /// The ToString callback delegate that returns an unscrubbed SecurityArtifact.
        /// </summary>
        private readonly Func<object, string> _callbackUnsafe;

        /// <summary>
        /// Creates an instance of <see cref="SecurityArtifact"/> that wraps the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument that is considered as SecurityArtifact.</param>
        /// <param name="toStringCallback">A callback used to disarm the token.</param>
        /// <remarks>
        /// Since even the payload may sometimes contain security artifacts, naïve disarm algorithms (such as removing signatures
        /// in the case of JWTs) will not work. For now the <paramref name="toStringCallback"/> will only be leveraged if
        /// <see cref="IdentityModelEventSource.LogCompleteSecurityArtifact"/> is set and no unsafe callback is provided. Future changes
        /// may introduce a support for best effort disarm logging.
        /// </remarks>
        public SecurityArtifact(object argument, Func<object, string> toStringCallback)
        {
            Argument = argument;
            _disarmCallback = toStringCallback;
        }

        /// <summary>
        /// Creates an instance of <see cref="SecurityArtifact"/> that wraps the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument that is considered as SecurityArtifact.</param>
        /// <param name="toStringCallback">A ToString callback.</param>
        /// <param name="toStringCallbackUnsafe">A ToString callback which will return the unscrubbed artifact.</param>
        /// <remarks>
        /// Since even the payload may sometimes contain security artifacts, naïve disarm algorithms (such as removing signatures
        /// in the case of JWTs) will not work. For now the <paramref name="toStringCallback"/> is currently unused. Future changes
        /// may introduce a support for best effort disarm logging which will leverage <paramref name="toStringCallback"/>.
        /// </remarks>
        public SecurityArtifact(object argument, Func<object, string> toStringCallback, Func<object, string> toStringCallbackUnsafe)
        {
            Argument = argument;
            _disarmCallback = toStringCallback;
            _callbackUnsafe = toStringCallbackUnsafe;
        }

        /// <summary>
        /// A dummy callback which can be leveraged to return a standard scrubbed token in the case where expected token is unknown.
        /// </summary>
        /// <param name="_">Ignored token.</param>
        /// <returns>The standard scrubbed token string.</returns>
        public static string UnknownSafeTokenCallback(object _)
        {
            return _scrubbedArtifact;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="Argument"/>.
        /// </summary>
        /// <returns><c>Null</c> if the <see cref="Argument"/> is <see langword="null"/>, otherwise calls the provided safe callback on <see cref="Argument"/>.</returns>
        public override string ToString()
        {
            // Defense in depth, ideally callers will set a callback which actually provides information but, since not initially required in a publicly facing API we
            // don't explicitly check and so it's possible we can instrument without a callback in which case we'll return a generic _scrubbedArtifact string.
            if (_disarmCallback == null)
                return _scrubbedArtifact;
            if (Argument == null)
                return "null";
            else
                return _disarmCallback(Argument);
        }

        // <inheritdoc/>
        public string UnsafeToString()
        {
            if (_callbackUnsafe == null || Argument == null)
                return ToString();
            else
                return _callbackUnsafe(Argument);
        }
    }
}
