// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETFRAMEWORK

using NuGet;

namespace System.Buffers
{
    /// <summary>
    /// Provides a resource pool that enables reusing instances of arrays.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Renting and returning buffers with an <see cref="ArrayPool{T}"/> can increase performance
    /// in situations where arrays are created and destroyed frequently, resulting in significant
    /// memory pressure on the garbage collector.
    /// </para>
    /// <para>
    /// This class is thread-safe.  All members may be used by multiple threads concurrently.
    /// </para>
    /// </remarks>
    internal class ArrayPool<T>
    {
        // .NET Framework/.NET Standard version of .NET Core's ArrayPool<T>, intentionally mimic'ing
        // its API shape so that consumption of the code doesn't need to change between targets.

        // Largest multiple of 4096 under default LOH threadshold
        private const int MaxPooledArraySize = 81920;
        private readonly SimplePool<T[]> _pool = new(() => new T[MaxPooledArraySize]);

        private ArrayPool()
        {
        }

        /// <summary>
        /// Retrieves a shared <see cref="ArrayPool{T}"/> instance.
        /// </summary>
        public static readonly ArrayPool<T> Shared = new();

        /// <summary>
        /// Retrieves a buffer that is at least the requested length.
        /// </summary>
        /// <param name="minimumLength">The minimum length of the array needed.</param>
        /// <returns>
        /// An array that is at least <paramref name="minimumLength"/> in length.
        /// </returns>
        /// <remarks>
        /// This buffer is loaned to the caller and should be returned to the same pool via
        /// <see cref="Return"/> so that it may be reused in subsequent usage of <see cref="Rent"/>.
        /// It is not a fatal error to not return a rented buffer, but failure to do so may lead to
        /// decreased application performance, as the pool may need to create a new buffer to replace
        /// the one lost.
        /// </remarks>
        public T[] Rent(int minimumLength)
        {
            if (minimumLength <= MaxPooledArraySize)
            {
                return _pool.Allocate();
            }

            return new T[minimumLength];
        }

        /// <summary>
        /// Returns to the pool an array that was previously obtained via <see cref="Rent"/> on the same
        /// <see cref="ArrayPool{T}"/> instance.
        /// </summary>
        /// <param name="array">
        /// The buffer previously obtained from <see cref="Rent"/> to return to the pool.
        /// </param>
        /// <remarks>
        /// Once a buffer has been returned to the pool, the caller gives up all ownership of the buffer
        /// and must not use it. The reference returned from a given call to <see cref="Rent"/> must only be
        /// returned via <see cref="Return"/> once.  The default <see cref="ArrayPool{T}"/>
        /// may hold onto the returned buffer in order to rent it again, or it may release the returned buffer
        /// if it's determined that the pool already has enough buffers stored.
        /// </remarks>
        public void Return(T[] array)
        {
            if (array.Length <= MaxPooledArraySize)
            {
                _pool.Free(array);
            }
        }
    }
}

#endif
