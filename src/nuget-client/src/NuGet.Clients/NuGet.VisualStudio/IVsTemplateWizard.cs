// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Defines the logic for a template wizard extension.
    /// </summary>
    [ComImport]
    [Guid("D6DEA71B-4A42-4B55-8A59-3191B91EF36E")]
    public interface IVsTemplateWizard : IWizard
    {
    }
}
