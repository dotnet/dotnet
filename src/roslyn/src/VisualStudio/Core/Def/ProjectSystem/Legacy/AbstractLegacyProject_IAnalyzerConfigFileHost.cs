﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.LanguageServices.Implementation.ProjectSystem.Interop;

namespace Microsoft.VisualStudio.LanguageServices.Implementation.ProjectSystem.Legacy;

internal abstract partial class AbstractLegacyProject : IAnalyzerConfigFileHost
{
    void IAnalyzerConfigFileHost.AddAnalyzerConfigFile(string filePath)
        => ProjectSystemProject.AddAnalyzerConfigFile(filePath);

    void IAnalyzerConfigFileHost.RemoveAnalyzerConfigFile(string filePath)
        => ProjectSystemProject.RemoveAnalyzerConfigFile(filePath);
}
