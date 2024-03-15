// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServerClient.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;

internal static class VsMocks
{
    public static ITextBuffer CreateTextBuffer(bool core)
        => CreateTextBuffer(core ? ContentTypes.RazorCore : ContentTypes.NonRazor);

    public static ITextBuffer CreateTextBuffer(PropertyCollection? properties = null)
    {
        properties ??= new PropertyCollection();

        return StrictMock.Of<ITextBuffer>(b =>
            b.Properties == properties);
    }

    public static ITextBuffer CreateTextBuffer(IContentType contentType, PropertyCollection? properties = null)
    {
        var buffer = CreateTextBuffer(properties);

        Mock.Get(buffer)
            .SetupGet(x => x.ContentType)
            .Returns(contentType);

        return buffer;
    }

    internal static class ContentTypes
    {
        public static readonly IContentType LegacyRazorCore = Create(RazorConstants.LegacyCoreContentType);
        public static readonly IContentType RazorCore = Create(RazorLanguage.CoreContentType);
        public static readonly IContentType RazorLSP = Create(RazorConstants.RazorLSPContentTypeName);
        public static readonly IContentType NonRazor = StrictMock.Of<IContentType>(c => c.IsOfType(It.IsAny<string>()) == false);
        public static readonly IContentType CSharp = CreateCSharp();

        public static IContentType Create(params string[] types)
        {
            var mock = new StrictMock<IContentType>();
            mock.Setup(x => x.IsOfType(It.IsAny<string>()))
                .Returns((string type) => Array.IndexOf(types, type) >= 0);

            return mock.Object;
        }

        private static IContentType CreateCSharp()
        {
            var contentType = Create(RazorLSPConstants.CSharpContentTypeName);
            var mock = Mock.Get(contentType);

            mock.SetupGet(x => x.TypeName)
                .Returns(RazorLSPConstants.CSharpContentTypeName);
            mock.SetupGet(x => x.DisplayName)
                .Returns(RazorLSPConstants.CSharpContentTypeName);

            return contentType;
        }
    }

    public static IServiceProvider CreateServiceProvider(Action<IServiceProviderBuilder>? configure = null)
    {
        var builder = new ServiceProviderBuilder();
        configure?.Invoke(builder);
        return builder.Mock.Object;
    }

    public interface IServiceProviderBuilder
    {
        void AddService<T>(T? serviceInstance)
            where T : class;
        void AddService<T>(Func<T?> getServiceCallback)
            where T : class;
        void AddService<T>(object? serviceInstance)
            where T : class;
        void AddService<T>(Func<object?> getServiceCallback)
            where T : class;
        void AddService(Type serviceType, object? serviceInstance);
        void AddService(Type serviceType, Func<object?> getServiceCallback);

        void AddComponentModel(Action<IComponentModelBuilder>? configure = null);
    }

    private class ServiceProviderBuilder : IServiceProviderBuilder
    {
        public StrictMock<IServiceProvider> Mock { get; } = new();

        public void AddService<T>(T? serviceInstance)
            where T : class
        {
            AddService(typeof(T), serviceInstance);
        }

        public void AddService<T>(Func<T?> getServiceCallback)
            where T : class
        {
            AddService(typeof(T), getServiceCallback);
        }

        public void AddService<T>(object? serviceInstance)
            where T : class
        {
            AddService(typeof(T), serviceInstance);
        }

        public void AddService<T>(Func<object?> getServiceCallback)
            where T : class
        {
            AddService(typeof(T), getServiceCallback);
        }

        public void AddService(Type serviceType, object? serviceInstance)
        {
            Mock.Setup(x => x.GetService(serviceType))
                .Returns(serviceInstance!);
        }

        public void AddService(Type serviceType, Func<object?> getServiceCallback)
        {
            Mock.Setup<object?>(x => x.GetService(serviceType))
                .Returns(() => getServiceCallback());
        }

        public void AddComponentModel(Action<IComponentModelBuilder>? configure = null)
        {
            AddService<SComponentModel>(CreateComponentModel(configure));
        }
    }

    public static IComponentModel CreateComponentModel(Action<IComponentModelBuilder>? configure = null)
    {
        var builder = new ComponentModelBuilder();
        configure?.Invoke(builder);
        return builder.Mock.Object;
    }

    public interface IComponentModelBuilder
    {
        void AddExport<T>(T instance)
            where T : class;
        void AddExport<T>(Func<T> getInstanceCallback)
            where T : class;
    }

    private class ComponentModelBuilder : IComponentModelBuilder
    {
        private readonly Dictionary<string, Func<object>> _contractNameToExportMap = new();

        public void AddExport<T>(T instance)
            where T : class
        {
            _contractNameToExportMap.Add(typeof(T).FullName, () => instance);
        }

        public void AddExport<T>(Func<T> getInstanceCallback)
            where T : class
        {
            _contractNameToExportMap.Add(typeof(T).FullName, () => getInstanceCallback());
        }

        public StrictMock<IComponentModel> Mock
        {
            get
            {
                var mock = new StrictMock<IComponentModel>();

                mock.SetupGet(x => x.DefaultExportProvider)
                    .Returns(new SimpleExportProvider(_contractNameToExportMap));

                return mock;
            }
        }

        private class SimpleExportProvider(Dictionary<string, Func<object>> contractNameToExportMap) : ExportProvider
        {
            protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
            {
                var contractName = definition.ContractName;

                if (!contractNameToExportMap.TryGetValue(contractName, out var exportValueGetter))
                {
                    throw new InvalidOperationException($"Failed to find export with contract name, '{contractName}'");
                }

                yield return new Export(contractName, exportValueGetter);
            }
        }
    }
}
