// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// </summary>
    public class TransformFactory
    {
        /// <summary>
        /// Static constructor that initializes the default <see cref="TransformFactory"/>.
        /// </summary>
        static TransformFactory()
        {
            Default = new TransformFactory();
        }

        /// <summary>
        /// Gets the default instance of <see cref="TransformFactory"/>
        /// </summary>
        public static TransformFactory Default
        {
            get;
        }

        /// <summary>
        /// Determine if the transform is supported.
        /// </summary>
        /// <param name="transform">the name of the transform.</param>
        /// <returns>if the transform is supported</returns>
        public virtual bool IsSupportedTransform(string transform)
        {
            if (string.IsNullOrEmpty(transform))
                return false;

            return transform == SecurityAlgorithms.EnvelopedSignature;
        }

        /// <summary>
        /// Determine if the canonicalizing transform is supported.
        /// </summary>
        /// <param name="transform">the name of the canonicalizing transform.</param>
        /// <returns>if the canonicalizing transform is supported</returns>
        public virtual bool IsSupportedCanonicalizingTransfrom(string transform)
        {
            if (string.IsNullOrEmpty(transform))
                return false;

            return transform == SecurityAlgorithms.ExclusiveC14nWithComments || transform == SecurityAlgorithms.ExclusiveC14n;
        }

        /// <summary>
        /// Gets a XML transform that modifies a XmlTokenStream.
        /// </summary>
        /// <param name="transform">the name of the transform.</param>
        /// <returns><see cref="Transform"/></returns>
        public virtual Transform GetTransform(string transform)
        {
            if (transform == SecurityAlgorithms.EnvelopedSignature)
                return new EnvelopedSignatureTransform();

            throw LogExceptionMessage(new NotSupportedException(FormatInvariant(LogMessages.IDX30210, transform)));
        }

        /// <summary>
        /// Gets a XML transform that is capable of Canonicalizing XML and returning bytes.
        /// </summary>
        /// <param name="transform">the name of the transform.</param>
        /// <returns><see cref="CanonicalizingTransfrom"/></returns>
        public virtual CanonicalizingTransfrom GetCanonicalizingTransform(string transform)
        {
            if (transform == SecurityAlgorithms.ExclusiveC14nWithComments)
                return new ExclusiveCanonicalizationTransform(true);

            else if (transform == SecurityAlgorithms.ExclusiveC14n)
                return new ExclusiveCanonicalizationTransform(false);

            throw LogExceptionMessage(new NotSupportedException(FormatInvariant(LogMessages.IDX30211, transform)));
        }
    }
}
