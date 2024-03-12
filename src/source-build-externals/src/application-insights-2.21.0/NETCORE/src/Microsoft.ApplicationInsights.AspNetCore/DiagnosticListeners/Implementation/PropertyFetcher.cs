namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation
{
    using System;
    using System.Reflection;

    // see https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/DiagnosticSourceEventSource.cs

    /// <summary>
    /// Efficient implementation of fetching properties of anonymous types with reflection.
    /// </summary>
    internal class PropertyFetcher
    {
        private readonly string propertyName;
        private volatile PropertyFetch innerFetcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyFetcher"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property this instance will be responsible for.</param>
        public PropertyFetcher(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Fetch the property from a provided object.
        /// </summary>
        /// <param name="obj">Anonymous object to fetch from.</param>
        /// <returns>Returns the value of the property if it exists in the provided object. Otherwise returns null.</returns>
        public object Fetch(object obj)
        {
            PropertyFetch fetch = this.innerFetcher;
            Type objType = obj?.GetType();

            if (fetch == null || fetch.Type != objType)
            {
                this.innerFetcher = fetch = PropertyFetch.FetcherForProperty(objType, objType?.GetTypeInfo()?.GetDeclaredProperty(this.propertyName));
            }

            return fetch?.Fetch(obj);
        }

        private class PropertyFetch
        {
            public PropertyFetch(Type type)
            {
                this.Type = type;
            }

            /// <summary>
            /// Gets the type of the object that the property is fetched from. For well-known static methods that
            /// aren't actually property getters this will return null.
            /// </summary>
            internal Type Type { get; }

            /// <summary>
            /// Create a property fetcher from a .NET Reflection PropertyInfo class that
            /// represents a property of a particular type.  
            /// </summary>
            public static PropertyFetch FetcherForProperty(Type type, PropertyInfo propertyInfo)
            {
                if (propertyInfo == null)
                {
                    // returns null on any fetch.
                    return new PropertyFetch(type);
                }

                var typedPropertyFetcher = typeof(TypedFetchProperty<,>);
                var instantiatedTypedPropertyFetcher = typedPropertyFetcher.GetTypeInfo().MakeGenericType(
                    propertyInfo.DeclaringType, propertyInfo.PropertyType);
                return (PropertyFetch)Activator.CreateInstance(instantiatedTypedPropertyFetcher, type, propertyInfo);
            }

            /// <summary>
            /// Given an object, fetch the property that this propertyFetch represents.
            /// </summary>
            public virtual object Fetch(object obj)
            {
                return null;
            }

            private class TypedFetchProperty<TObject, TProperty> : PropertyFetch
            {
                private readonly Func<TObject, TProperty> propertyFetch;

                public TypedFetchProperty(Type type, PropertyInfo property) : base(type)
                {
                    this.propertyFetch = (Func<TObject, TProperty>)property.GetMethod.CreateDelegate(typeof(Func<TObject, TProperty>));
                }

                public override object Fetch(object obj)
                {
                    return this.propertyFetch((TObject)obj);
                }
            }
        }
    }
}
