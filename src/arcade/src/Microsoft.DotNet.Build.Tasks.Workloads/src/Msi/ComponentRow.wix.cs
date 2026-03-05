// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using WixToolset.Dtf.WindowsInstaller;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Defines a single row inside the <see href="https://learn.microsoft.com/en-us/windows/win32/msi/component-table">Component</see> table of an MSI.
    /// </summary>
    public class ComponentRow
    {
        /// <summary>
        /// Identifies the component record.
        /// </summary>
        public string Component
        {
            get;
            set;
        }

        /// <summary>
        /// A string GUID unique to this component, version, and language.
        /// </summary>
        public Guid ComponentId
        {
            get;
            set;
        }

        /// <summary>
        /// External key of an entry in the Directory table.
        /// </summary>
        public string Directory_
        {
            get;
            set;
        }

        /// <summary>
        /// Bit flags that specifies options for remote execution.
        /// </summary>
        public int Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Conditional statement that determines whether the component will be installed. 
        /// </summary>
        public string Condition
        {
            get;
            set;
        }

        /// <summary>
        /// This value points to a file or folder belonging to the component that the installer uses to 
        /// detect the component.
        /// </summary>
        public string KeyPath
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="ComponentRow"/> instance from the given <see cref="Record"/>.
        /// </summary>
        /// <param name="componentRecord">The record to use.</param>
        /// <returns>A new component row.</returns>
        public static ComponentRow Create(Record componentRecord) =>
            new ComponentRow
            {
                Component = componentRecord.GetString("Component"),
                ComponentId = Guid.Parse(componentRecord.GetString("ComponentId")),
                Directory_ = componentRecord.GetString("Directory_"),
                Attributes = componentRecord.GetInteger("Attributes"),
                Condition = componentRecord.GetString("Condition"),
                KeyPath = componentRecord.GetString("KeyPath"),
            };
    }
}
