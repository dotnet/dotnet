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
        /// <summary>
        /// Argument wrapped with a <see cref="SecurityArtifact"/> structure is considered as SecurityArtifact in the message logging process.
        /// </summary>
        private object Argument { get; set; }

        /// <summary>
        /// The ToString callback delegate that return a disarmed SecurityArtifact.
        /// </summary>
        private Func<object, string> _callback;

        /// <summary>
        /// Creates an instance of <see cref="SecurityArtifact"/> that wraps the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument that is considered as SecurityArtifact.</param>
        /// <param name="toStringCallback">A ToString callback.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="argument"/> is null.</exception>
        public SecurityArtifact(object argument, Func<object, string> toStringCallback)
        {
            Argument = argument;
            _callback = toStringCallback;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="Argument"/>.
        /// </summary>
        /// <returns><c>Null</c> if the <see cref="Argument"/> is <see langword="null"/>, otherwise calls <see cref="System.ValueType.ToString()"/> method of the <see cref="Argument"/>.</returns>
        public override string ToString()
        {
            if (_callback != null && Argument != null)
                return _callback(Argument);

            return Argument?.ToString() ?? "null";
        }

        // <inheritdoc/>
        public string UnsafeToString()
        {
            return Argument?.ToString() ?? "null";
        }
    }
}
