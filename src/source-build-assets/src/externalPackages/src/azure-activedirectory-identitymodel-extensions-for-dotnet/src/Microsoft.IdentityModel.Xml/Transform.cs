// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Defines a XMLTransform
    /// </summary>
    public abstract class Transform
    {
        /// <summary>
        /// Called to transform a <see cref="XmlTokenStream"/>
        /// </summary>
        /// <param name="tokenStream">the <see cref="XmlTokenStream"/> to process.</param>
        /// <returns></returns>
        public abstract XmlTokenStream Process(XmlTokenStream tokenStream);

        /// <summary>
        /// Gets the algorithm
        /// </summary>
        public abstract string Algorithm { get; }
    }
}
