// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
#if !IS_CORECLR
using System.Reflection;
#endif
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Task = Microsoft.Build.Utilities.Task;


namespace NuGet.Build.Tasks
{
    public class GetGlobalPropertyValueTask : Task
    {
        [Required]
        public string PropertyName { get; set; }

        [Output]
        public string GlobalPropertyValue { get; set; }

        [Output]
        public bool CheckCompleted { get; set; }

        public override bool Execute()
        {
            var logger = new MSBuildLogger(Log);
            var globalProperties = GetGlobalProperties(logger);

            if (globalProperties != null)
            {
                var dictionaryWithOrdinal = globalProperties.ToDictionary(i => i.Key, i => i.Value, StringComparer.OrdinalIgnoreCase);
                CheckCompleted = true;
                if (dictionaryWithOrdinal.TryGetValue(PropertyName, out string globalProperty))
                {
                    GlobalPropertyValue = globalProperty;
                }
            }
            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Get the global property from the IBuildEngine API. 
        /// </summary>
        /// <returns>Returns the dictionary with the global properties if they can be accessed. <see langword="null"/> otherwise, which means that the msbuild version doesn't implement this API. </returns>
        internal IReadOnlyDictionary<string, string> GetGlobalProperties(Common.ILogger logger)
        {
#if IS_CORECLR
            // MSBuild 16.5 and above has a method to get the global properties, older versions do not
            IReadOnlyDictionary<string, string> msBuildGlobalProperties = BuildEngine is IBuildEngine6 buildEngine6
                ? buildEngine6.GetGlobalProperties()
                : null;
#else
            IReadOnlyDictionary<string, string> msBuildGlobalProperties = null;

            // MSBuild 16.5 added a new interface, IBuildEngine6, which has a GetGlobalProperties() method.  However, we compile against
            // Microsoft.Build.Framework version 4.0 when targeting .NET Framework, so reflection is required since type checking
            // can't be done at compile time

            var getGlobalPropertiesMethod = BuildEngine.GetType().GetMethod("GetGlobalProperties", BindingFlags.Instance | BindingFlags.Public);

            if (getGlobalPropertiesMethod != null)
            {
                try
                {
                    if (getGlobalPropertiesMethod.Invoke(BuildEngine, null) is IReadOnlyDictionary<string, string> globalProperties)
                    {
                        msBuildGlobalProperties = globalProperties;
                    }
                }
                catch (Exception e)
                {
                    // This is an unexpected error, so we don't localize.
                    logger.LogError($"Internal Error. Failed calling the Microsoft.Build.Framework.IBuildEngine6.GetGlobalProperties method via reflection. Unable to determine the global properties.{e}");
                }
            }
            else
            {
                // This is an unexpected error, so we don't localize.
                logger.LogError($"Internal Error. Failed calling the Microsoft.Build.Framework.IBuildEngine6.GetGlobalProperties method via reflection. Unable to determine the global properties.");
            }
#endif
            return msBuildGlobalProperties;
        }
    }
}
