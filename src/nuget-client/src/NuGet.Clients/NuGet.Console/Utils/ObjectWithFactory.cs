// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetConsole
{
    /// <summary>
    /// An object produced by a factory.
    /// </summary>
    /// <typeparam name="T">The factory type.</typeparam>
    internal class ObjectWithFactory<T>
    {
        public T Factory { get; private set; }

        public ObjectWithFactory(T factory)
        {
            UtilityMethods.ThrowIfArgumentNull(factory);
            this.Factory = factory;
        }
    }
}
