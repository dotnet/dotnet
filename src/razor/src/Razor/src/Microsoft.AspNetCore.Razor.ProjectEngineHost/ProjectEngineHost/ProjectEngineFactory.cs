// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal sealed partial class ProjectEngineFactory(string configurationName, string assemblyName) : IProjectEngineFactory
{
    private static readonly Dictionary<InitializerKey, RazorExtensionInitializer> s_initializerMap = [];

    private readonly string _assemblyName = assemblyName;

    public string ConfigurationName => configurationName;

    public RazorProjectEngine Create(
        RazorConfiguration configuration,
        RazorProjectFileSystem fileSystem,
        Action<RazorProjectEngineBuilder>? configure)
    {
        RazorExtensionInitializer? initializer;

        lock (s_initializerMap)
        {
            var key = new InitializerKey(configuration.ConfigurationName, _assemblyName);
            if (!s_initializerMap.TryGetValue(key, out initializer))
            {
                initializer = CreateInitializer(key);

                s_initializerMap.Add(key, initializer);
            }
        }

        return RazorProjectEngine.Create(configuration, fileSystem, builder =>
        {
            CompilerFeatures.Register(builder);
            initializer.Initialize(builder);
            configure?.Invoke(builder);
        });

        static RazorExtensionInitializer CreateInitializer(InitializerKey key)
        {
            // Rewrite the assembly name into a full name just like this one, but with the name of the MVC design time assembly.
            var assemblyFullName = typeof(RazorProjectEngine).Assembly.FullName.AssumeNotNull();

            var extensionAssemblyName = new AssemblyName(assemblyFullName)
            {
                Name = key.AssemblyName
            };

            var extension = new AssemblyExtension(key.ConfigurationName, Assembly.Load(extensionAssemblyName));

            return extension.CreateInitializer();
        }
    }
}
