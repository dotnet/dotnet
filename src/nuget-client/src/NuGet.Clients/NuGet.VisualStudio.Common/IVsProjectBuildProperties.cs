// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio
{
    public interface IVsProjectBuildProperties
    {
        /// <summary>Get the value of a property.</summary>
        /// <param name="name">The property name to check.</param>
        /// <returns>The value of the property, if defined by the project. Otherwise, null</returns>
        string GetPropertyValue(string propertyName);

        /// <summary>Get the value of a property, and use DTE if IBuildPropertyStorage doesn't return a value</summary>
        /// <param name="name">The property name to check.</param>
        /// <returns>The value of the property, if defined by the project. Otherwise, null</returns>
        /// <remarks>This method should not be used for anything new, as an exception is thrown and caught internally
        /// when no value is defined, which harms performance.</remarks>
        [Obsolete("New properties should use GetPropertyValue instead. Ideally we should migrate existing properties to stop using DTE as well.")]
        string GetPropertyValueWithDteFallback(string propertyName);
    }
}
