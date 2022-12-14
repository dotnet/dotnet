// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace System.ComponentModel.Composition.ReflectionModel
{
    internal sealed partial class ExportFactoryCreator
    {
        private sealed class LifetimeContext
        {
            public static Tuple<T, Action> GetExportLifetimeContextFromExport<T>(Export export)
            {
                T exportedValue;
                Action disposeAction;
                IDisposable? disposable = null;

                if (export is CatalogExportProvider.ScopeFactoryExport scopeFactoryExport)
                {
                    // Scoped PartCreatorExport
                    Export exportProduct = scopeFactoryExport.CreateExportProduct();
                    exportedValue = ExportServices.GetCastedExportedValue<T>(exportProduct);
                    disposable = exportProduct as IDisposable;
                }
                else
                {
                    if (export is CatalogExportProvider.FactoryExport factoryExport)
                    {
                        // PartCreatorExport is the more optimized route
                        Export exportProduct = factoryExport.CreateExportProduct();
                        exportedValue = ExportServices.GetCastedExportedValue<T>(exportProduct);
                        disposable = exportProduct as IDisposable;
                    }
                    else
                    {
                        // If it comes from somewhere else we walk through the ComposablePartDefinition
                        var factoryPartDefinition = ExportServices.GetCastedExportedValue<ComposablePartDefinition>(export);
                        var part = factoryPartDefinition.CreatePart();
                        var exportDef = factoryPartDefinition.ExportDefinitions.Single();

                        exportedValue = ExportServices.CastExportedValue<T>(part.ToElement(), part.GetExportedValue(exportDef));
                        disposable = part as IDisposable;
                    }
                }

                if (disposable != null)
                {
                    disposeAction = disposable.Dispose;
                }
                else
                {
                    disposeAction = () => { };
                }

                return new Tuple<T, Action>(exportedValue, disposeAction);
            }
        }
    }
}
