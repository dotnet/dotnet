// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal static class ProjectEngineFactories
{
    public static readonly (Lazy<IProjectEngineFactory>, ICustomProjectEngineFactoryMetadata)[] Factories =
       new (Lazy<IProjectEngineFactory>, ICustomProjectEngineFactoryMetadata)[]
       {
            // Razor based configurations
            (new (() => new DefaultProjectEngineFactory()),      new CustomProjectEngineFactoryMetadata("Default") { SupportsSerialization = true }),
            (new (() => new ProjectEngineFactory_1_0()),         new CustomProjectEngineFactoryMetadata("MVC-1.0") { SupportsSerialization = true }),
            (new (() => new ProjectEngineFactory_1_1()),         new CustomProjectEngineFactoryMetadata("MVC-1.1") { SupportsSerialization = true }),
            (new (() => new ProjectEngineFactory_2_0()),         new CustomProjectEngineFactoryMetadata("MVC-2.0") { SupportsSerialization = true }),
            (new (() => new ProjectEngineFactory_2_1()),         new CustomProjectEngineFactoryMetadata("MVC-2.1") { SupportsSerialization = true }),
            (new (() => new ProjectEngineFactory_3_0()),         new CustomProjectEngineFactoryMetadata("MVC-3.0") { SupportsSerialization = true }),
            // Unsupported (Legacy/System.Web.Razor)
            (new (() => new ProjectEngineFactory_Unsupported()), new CustomProjectEngineFactoryMetadata(UnsupportedRazorConfiguration.Instance.ConfigurationName) { SupportsSerialization = true }),
   };
}
