// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NuGetConsole.Host
{
    /// <summary>
    /// This class wraps an object so that it can only be accessed through an interface.
    /// This simulates COM QueryInterface (Get-Interface).
    /// TypeWrapper holds the wrapped object and a map of {InterfaceType -> InterfaceWrapper}.
    /// </summary>
    /// <typeparam name="T">
    /// An InterfaceWrapper type. An interface wrapper wraps one interface
    /// and keeps a reference to this type wrapper object to find the wrapped object and other
    /// interface wrappers.
    /// </typeparam>
    public abstract class TypeWrapper<T>
        where T : class
    {
        /// <summary>
        /// The real target object to perform interface member calls.
        /// </summary>
        internal object WrappedObject { get; private set; }

        /// <summary>
        /// A binder for binding interface method calls.
        /// </summary>
        internal abstract MethodBinder Binder { get; }

        private readonly Dictionary<Type, T> _interfaceMap = new Dictionary<Type, T>(TypeEquivalenceComparer.Instance);
        private readonly object _interfaceMapLock = new object();

        /// <summary>
        /// Create a new wrapper on an object.
        /// </summary>
        /// <param name="wrappedValue">The real target object to be wrapped.</param>
        protected TypeWrapper(object wrappedValue)
        {
            UtilityMethods.ThrowIfArgumentNull(wrappedValue);
            WrappedObject = wrappedValue;
        }

        #region object overrides

        public override bool Equals(object obj)
        {
            return obj != null && obj.Equals(WrappedObject);
        }

        public override int GetHashCode()
        {
            return WrappedObject.GetHashCode();
        }

        public override string ToString()
        {
            return WrappedObject.ToString();
        }

        #endregion

        /// <summary>
        /// Get an interface to access the wrapped object.
        /// </summary>
        /// <param name="interfaceType">An interface type implemented by the wrapped object.</param>
        /// <returns>
        /// An interface wrapper for calling the interface members on the wrapped object.
        /// null if fails to get the interface.
        /// </returns>
        protected T GetInterface(Type interfaceType)
        {
            if (!interfaceType.IsInstanceOfType(WrappedObject))
            {
                return default(T); // E_NOINTERFACE
            }

            lock (_interfaceMapLock)
            {
                T interfaceWrapper;
                if (_interfaceMap.TryGetValue(interfaceType, out interfaceWrapper))
                {
                    return interfaceWrapper;
                }

                interfaceWrapper = CreateInterfaceWrapper(this, interfaceType);
                _interfaceMap[interfaceType] = interfaceWrapper;

                return interfaceWrapper;
            }
        }

        /// <summary>
        /// Subclass is responsible to create a new interface wrapper for a given interface type.
        /// </summary>
        /// <param name="wrapper">The type wrapper object.</param>
        /// <param name="interfaceType">The interface type.</param>
        /// <returns>A new interface wrapper.</returns>
        protected abstract T CreateInterfaceWrapper(TypeWrapper<T> wrapper, Type interfaceType);

        /// <summary>
        /// A type equivalence comparer.
        /// </summary>
        private class TypeEquivalenceComparer : IEqualityComparer<Type>
        {
            public static readonly TypeEquivalenceComparer Instance = new TypeEquivalenceComparer();

            private TypeEquivalenceComparer()
            {
            }

            public bool Equals(Type x, Type y)
            {
                return x.IsEquivalentTo(y);
            }

            public int GetHashCode(Type obj)
            {
                return obj.GUID.GetHashCode();
            }
        }

        /// <summary>
        /// Helper method for subclass to implement GetInterface.
        /// </summary>
        /// <param name="scriptValue">The script object that the interface targets.</param>
        /// <param name="interfaceType">The interface type to obtain.</param>
        /// <param name="getTypeWrapper">
        /// A function to get the TypeWrapper from scriptObject,
        /// or create a new one to wrap the object if scriptObject was not wrapped in a TypeWrapper.
        /// </param>
        /// <returns>An object through which to invoke the interfaceType members on the scriptObject.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected static T GetInterface(object scriptValue, Type interfaceType, Func<object, TypeWrapper<T>> getTypeWrapper)
        {
            if (scriptValue == null)
            {
                return null;
            }

            UtilityMethods.ThrowIfArgumentNull(interfaceType);
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Invalid argument", nameof(interfaceType));
            }

            TypeWrapper<T> wrapper = getTypeWrapper(scriptValue);
            return wrapper.GetInterface(interfaceType);
        }
    }
}
