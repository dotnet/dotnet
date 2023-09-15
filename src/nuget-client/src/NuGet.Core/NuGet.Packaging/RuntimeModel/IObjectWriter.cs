// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.RuntimeModel
{
    /// <summary>
    /// Provides functionality for writing an object graph.
    /// The output format is defined by implementors.
    /// </summary>
    public interface IObjectWriter
    {
        /// <summary>
        /// Writes the start of the root object or an object in an array.
        ///
        /// This new object becomes the scope for all other method calls until either WriteObjectStart
        /// is called again to start a new nested object or WriteObjectEnd is called.
        ///
        /// Every call to WriteObjectStart must be balanced by a corresponding call to WriteObjectEnd.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteObjectStart();

        /// <summary>
        /// Writes the start of a nested object.
        ///
        /// This new object becomes the scope for all other method calls until either WriteObjectStart
        /// is called again to start a new nested object or WriteObjectEnd is called.
        ///
        /// Every call to WriteObjectStart must be balanced by a corresponding call to WriteObjectEnd.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteObjectStart(string name);

        /// <summary>
        /// Writes the end of a nested object.
        ///
        /// The parent object for this object becomes the scope for subsequent method calls.
        /// If this object is the root object, no further writing is allowed.
        ///
        /// Every call to WriteObjectStart must be balanced by a corresponding call to WriteObjectEnd.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteObjectEnd();

        /// <summary>
        /// Writes a name-value pair.
        /// </summary>
        /// <param name="name">The name of the datum.</param>
        /// <param name="value">The datum.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteNameValue(string name, int value);

        /// <summary>
        /// Writes a name-value pair.
        /// </summary>
        /// <param name="name">The name of the datum.</param>
        /// <param name="value">The datum.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteNameValue(string name, bool value);

        /// <summary>
        /// Writes a name-value pair.
        /// </summary>
        /// <param name="name">The name of the datum.</param>
        /// <param name="value">The datum.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteNameValue(string name, string value);

        /// <summary>
        /// Writes a name-collection pair.
        /// </summary>
        /// <param name="name">The name of the data.</param>
        /// <param name="values">The data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteNameArray(string name, IEnumerable<string> values);

        /// <summary>
        /// Writes the start of an array.
        /// The new object becomes the scope of all other methods until WriteArrayStart is called to start a new object in the array, or WriteArrayEnd is called.
        /// Every call to WriteArrayStart needs to be balanced with a corresponding call to WriteArrayEnd and not WriteObjectEnd.
        /// </summary>
        /// <param name="name">The array name</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name" /> is <see langword="null" />.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteArrayStart(string name);

        /// <summary>
        /// Writes the end of an array.
        ///
        /// The parent object for this array becomes the scope for subsequent method calls.
        /// If this object is the root object, no further writing is allowed.
        ///
        /// Every call to WriteArrayStart needs to be balanced with a corresponding call to WriteArrayEnd and not WriteObjectEnd.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        void WriteArrayEnd();
    }
}
