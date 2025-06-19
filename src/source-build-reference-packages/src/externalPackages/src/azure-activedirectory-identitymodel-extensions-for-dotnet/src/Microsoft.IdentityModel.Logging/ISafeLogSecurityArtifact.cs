// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Logging
{
    /// <summary>
    /// Interface that provides an unsafe method to log a security artifact.
    /// </summary>
    /// <remarks>
    /// SecurityToken and encoded token are considered as SecurityArtifacts.
    /// </remarks>
    public interface ISafeLogSecurityArtifact
    {
        /// <summary>
        /// Returns a string that represents the complete security artifact.
        /// This may include sensitive information and should only be used for debugging purposes.
        /// </summary>
        string UnsafeToString();
    }
}

