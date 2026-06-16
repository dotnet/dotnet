// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using WixToolset.Dtf.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines a single row from the <see href="https://docs.microsoft.com/en-us/windows/win32/msi/customaction-table">CustomAction</see> table of an MSI.
    /// </summary>
    public class CustomActionRow
    {
        /// <summary>
        /// Name of the action. The action normally appears in a sequence table unless it is called by another custom action.
        /// </summary>
        public string Action
        {
            get;
            set;
        }

        /// <summary>
        /// A field of flag bits specifying the basic type of custom action and options.
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// A property name or external key into another table
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// An execution parameter that depends on the basic type of custom action.
        /// </summary>
        public string Target
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="CustomActionRow"/> instance from the specified <see cref="Record"/>.
        /// </summary>
        /// <param name="customActionRecord">The custom action record obtained from querying the MSI CustomAction table.</param>
        /// <returns>A single custom action row.</returns>
        public static CustomActionRow Create(Record customActionRecord)
        {
            return new CustomActionRow
            {
                Action = (string)customActionRecord["Action"],
                Type = (int)customActionRecord["Type"],
                Source = (string)customActionRecord["Source"],
                Target = (string)customActionRecord["Target"],
            };
        }
    }
}
