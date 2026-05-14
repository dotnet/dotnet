// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using Xunit;

namespace Roslyn.Test.Utilities
{
    public class ConditionalFactAttribute : FactAttribute
    {
        public ConditionalFactAttribute(params Type[] skipConditions)
        {
            foreach (var skipCondition in skipConditions)
            {
                ExecutionCondition condition = (ExecutionCondition)Activator.CreateInstance(skipCondition);
                if (condition.ShouldSkip)
                {
                    Skip = condition.SkipReason;
                    break;
                }
            }
        }
    }

    public abstract class ExecutionCondition
    {
        public abstract bool ShouldSkip { get; }
        public abstract string SkipReason { get; }
    }

    public class x86 : ExecutionCondition
    {
        public override bool ShouldSkip => IntPtr.Size != 4;

        public override string SkipReason => "Target platform is not x86";
    }

    public class WindowsOnly : ExecutionCondition
    {
        public override bool ShouldSkip => Path.DirectorySeparatorChar != '\\';
        public override string SkipReason => "Test not supported on Mono";
    }

    public class DesktopOnly : ExecutionCondition
    {
        internal static bool IsRunningOnCoreClr => AssemblyLoadContext.Type != null;

        internal static class AssemblyLoadContext
        {
            internal static readonly Type Type = TryGetType(
               "System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        }

        public static Type TryGetType(string assemblyQualifiedName)
        {
            try
            {
                // Note that throwOnError=false only suppresses some exceptions, not all.
                return Type.GetType(assemblyQualifiedName, throwOnError: false);
            }
            catch
            {
                return null;
            }
        }

        public override bool ShouldSkip => IsRunningOnCoreClr;
        public override string SkipReason => "Test not supported on CoreCLR";
    }
}
