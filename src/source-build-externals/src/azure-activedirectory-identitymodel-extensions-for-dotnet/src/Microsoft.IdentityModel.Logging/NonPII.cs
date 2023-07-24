// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// An internal structure that is used to mark an argument as NonPII.
    /// Arguments wrapped with a NonPII structure will be considered as NonPII in the message logging process.
    /// </summary>
    internal struct NonPII
    {
        /// <summary>
        /// Argument wrapped with a <see cref="NonPII"/> structure is considered as NonPII in the message logging process.
        /// </summary>
        public object Argument { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="NonPII"/> that wraps the <paramref name="argument"/>.
        /// </summary>
        /// <param name="argument">An argument that is considered as NonPII.</param>
        public NonPII(object argument)
        {
            Argument = argument;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="Argument"/>.
        /// </summary>
        /// <returns><c>Null</c> if the <see cref="Argument"/> is <see langword="null"/>, otherwise calls <see cref="System.ValueType.ToString()"/> method of the <see cref="Argument"/>.</returns>
        public override string ToString()
        {
            return Argument?.ToString() ?? "Null";
        }
    }
}
