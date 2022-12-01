// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Reflection;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;

public static class RoslynTestCompositions
{
    public static readonly TestComposition Roslyn = TestComposition.Empty
        .AddAssemblies(MefHostServices.DefaultAssemblies)
        .AddAssemblies(Assembly.LoadFrom("Microsoft.CodeAnalysis.dll"))
        .AddAssemblies(Assembly.LoadFrom("Microsoft.CodeAnalysis.CSharp.EditorFeatures.dll"))
        .AddAssemblies(Assembly.LoadFrom("Microsoft.CodeAnalysis.EditorFeatures.dll"))
        .AddAssemblies(Assembly.LoadFrom("Microsoft.CodeAnalysis.ExternalAccess.Razor.dll"))
        .AddAssemblies(Assembly.LoadFrom("Microsoft.CodeAnalysis.LanguageServer.Protocol.dll"))
        .AddParts(typeof(RazorTestWorkspaceRegistrationService));
}
