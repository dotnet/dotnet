// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// Supports async iteration over a generic collection.
    /// Prior to calling Current, MoveNextAsync needs to be called otherwise the behavior is undefined 
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam><filterpriority>1</filterpriority>
    public interface IEnumeratorAsync<T>
    {
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// If <ref>MoveNextAsync</ref> has not been called prior to calling current the behavior is undefined
        /// If the last <ref>MoveNextAsync</ref> call returned false then the next call to Current should throw an InvalidOperationException
        /// </summary>
        /// 
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        T Current { get; }

        /// <summary>
        /// Moves the pointer to the next element in the collection. 
        /// </summary>
        /// <returns>
        /// Success status of the pointer move
        /// </returns>
        Task<bool> MoveNextAsync();
    }
}
