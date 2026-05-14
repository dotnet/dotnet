// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// <see cref="CommandLineBuilder"/> extension methods.
    /// </summary>
    public static class CommandBuilderExtensions
    {
        /// <summary>
        /// Appends an array of command line switches. The switch name is repeated for each value.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="switchName"></param>
        /// <param name="values"></param>
        public static void AppendArrayIfNotNull(this CommandLineBuilder builder, string switchName, string[] values)
        {
            if (values != null)
            {
                foreach (string value in values)
                {
                    builder.AppendSwitchIfNotNull(switchName, value);
                }
            }
        }

        /// <summary>
        /// Appends a command line switch that has no separate value, wihtout quoting if the specified value is <see langword="true"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="switchName">The switch to append to the command line.</param>
        /// <param name="value">The value to evaluate.</param>
        public static void AppendSwitchIfTrue(this CommandLineBuilder builder, string switchName, bool value)
        {
            if (value)
            {
                builder.AppendSwitch(switchName);
            }
        }
    }
}
