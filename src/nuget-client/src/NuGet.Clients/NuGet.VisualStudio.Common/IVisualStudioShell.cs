// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.VisualStudio.Common
{
    /// <summary> Abstraction over various DTE functionality. </summary>
    internal interface IVisualStudioShell
    {
        /// <summary> Subscribe to project's BuildBegin events. </summary>
        /// <param name="onBuildBegin"> Action to do on BuildBegin event. </param>
        Task SubscribeToBuildBeginAsync(Action onBuildBegin);

        /// <summary> Subscribe to solution's AfterClosing events. </summary>
        /// <param name="afterClosing"> Action to do on AfterClosing event. </param>
        Task SubscribeToAfterClosingAsync(Action afterClosing);

        /// <summary> Get a property value. </summary>
        /// <param name="category"> Settings category. </param>
        /// <param name="page"> Settings page. </param>
        /// <param name="propertyName"> Property name. </param>
        /// <returns></returns>
        Task<object> GetPropertyValueAsync(string category, string page, string propertyName);
    }
}
