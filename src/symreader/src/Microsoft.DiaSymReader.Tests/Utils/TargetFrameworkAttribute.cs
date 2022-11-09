// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
#if NET46

namespace System.Runtime.Versioning
{
    [AttributeUsageAttribute(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    internal sealed class TargetFrameworkAttribute : Attribute
    {
        public string FrameworkName { get; }
        public string FrameworkDisplayName { get; set; }

        public TargetFrameworkAttribute(string frameworkName) 
            => FrameworkName = frameworkName;
    }
}
#endif
