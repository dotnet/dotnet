// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace NuGet
{
    /// <summary>
    /// Provides a resource pool that enables reusing instances of <see cref="StringBuilder" /> instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Renting and returning buffers with an <see cref="StringBuilderPool" /> can increase performance
    /// in situations where <see cref="StringBuilder" /> instances are created and destroyed frequently,
    /// resulting in significant memory pressure on the garbage collector.
    /// </para>
    /// <para>
    /// This class is thread-safe.  All members may be used by multiple threads concurrently.  The string
    /// builder must be returned, even if an exception is thrown while it is in use.  Failure to do results
    /// in other callers not being able to rent the string builder.
    /// </para>
    /// </remarks>
    internal class SharedStringBuilder
    {
        public static readonly SharedStringBuilder Instance = new SharedStringBuilder();

        private const int MaxSize = 256;

        [ThreadStatic]
        private static StringBuilder? BuilderInstance = null;

        [ThreadStatic]
        private static bool InUse = false;

        private SharedStringBuilder()
        {
        }

        /// <summary>
        /// Retrieves a <see cref="StringBuilder" /> that is at least the requested length.
        /// </summary>
        /// <param name="minimumCapacity">The minimum capacity of the <see cref="StringBuilder" /> needed.</param>
        /// <returns>
        /// A <see cref="StringBuilder" /> that is at least <paramref name="minimumCapacity" /> in length.
        /// </returns>
        /// <remarks>
        /// This buffer is loaned to the caller and should be returned to the same pool via
        /// <see cref="ToStringAndReturn" /> so that it may be reused in subsequent usage of <see cref="Rent" />.
        /// It is not a fatal error to not return a rented string builder, but failure to do so may lead to
        /// decreased application performance, as the pool may need to create a new instance to replace
        /// the one lost.
        /// </remarks>
#pragma warning disable CA1822 // Mark members as static.  Callers are supposed to access this method via SharedStringBuilder.Instance
        public StringBuilder Rent(int minimumCapacity)
#pragma warning restore CA1822 // Mark members as static
        {
            if (!InUse)
            {
                if (minimumCapacity <= MaxSize)
                {
                    InUse = true;

                    if (BuilderInstance is null)
                    {
                        BuilderInstance = new StringBuilder(MaxSize);
                    }

                    return BuilderInstance;
                }
            }

            return new StringBuilder(minimumCapacity);
        }

        /// <summary>
        /// Returns to the pool an array that was previously obtained via <see cref="Rent" /> on the same
        /// <see cref="StringBuilderPool" /> instance, returning the built string.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder" /> previously obtained from <see cref="Rent" /> to return to the pool.
        /// </param>
        /// <remarks>
        /// Once a <see cref="StringBuilder" /> has been returned to the pool, the caller gives up all ownership
        /// of the instance and must not use it. The reference returned from a given call to <see cref="Rent" />
        /// must only be returned via <see cref="ToStringAndReturn" /> once.  The default <see cref="StringBuilderPool" />
        /// may hold onto the returned instance in order to rent it again, or it may release the returned instance
        /// if it's determined that the pool already has enough instances stored.
        /// </remarks>
        /// <returns>The string, built from <paramref name="builder" />.</returns>
#pragma warning disable CA1822 // Mark members as static.  Callers are supposed to access this method via SharedStringBuilder.Instance
        public string ToStringAndReturn(StringBuilder builder)
#pragma warning restore CA1822 // Mark members as static
        {
            string result = builder.ToString();

            if (ReferenceEquals(BuilderInstance, builder))
            {
                InUse = false;
                builder.Clear();
            }

            return result;
        }
    }
}
