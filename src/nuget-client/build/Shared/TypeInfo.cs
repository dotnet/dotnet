#if IS_NET40_CLIENT
using System;
using System.Reflection;

namespace NuGet.Shared
{
    /// <summary>
    /// A minimal implementation of TypeInfo for net40-client.
    /// </summary>
    internal class TypeInfo
    {
        private readonly Type _type;

        public TypeInfo(Type type)
        {
            _type = type;
        }

        public Assembly Assembly
        {
            get { return _type.Assembly; }
        }
    }
}
#endif
