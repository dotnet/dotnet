// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Class representing an InProcDataCollector loaded by InProcDataCollectionExtensionManager
/// </summary>
internal class InProcDataCollector : IInProcDataCollector
{
    /// <summary>
    /// DataCollector Class Type
    /// </summary>
    private readonly Type? _dataCollectorType;

    /// <summary>
    /// Instance of the
    /// </summary>
    private object? _dataCollectorObject;

    /// <summary>
    /// Config XML from the runsettings for current datacollector
    /// </summary>
    private readonly string? _configXml;

    /// <summary>
    /// AssemblyLoadContext for current platform
    /// </summary>
    private readonly IAssemblyLoadContext _assemblyLoadContext;

    public InProcDataCollector(
        string codeBase,
        string assemblyQualifiedName,
        Type interfaceType,
        string? configXml)
        : this(codeBase, assemblyQualifiedName, interfaceType, configXml, new PlatformAssemblyLoadContext(), TestPluginCache.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InProcDataCollector"/> class.
    /// </summary>
    /// <param name="codeBase">
    /// </param>
    /// <param name="assemblyQualifiedName">
    /// </param>
    /// <param name="interfaceTypeInfo">
    /// </param>
    /// <param name="configXml">
    /// </param>
    /// <param name="assemblyLoadContext">
    /// </param>
    internal InProcDataCollector(string codeBase, string assemblyQualifiedName, Type interfaceType, string? configXml, IAssemblyLoadContext assemblyLoadContext, TestPluginCache testPluginCache)
    {
        _configXml = configXml;
        _assemblyLoadContext = assemblyLoadContext;

        var assembly = LoadInProcDataCollectorExtension(codeBase);

        Func<Type, bool> filterPredicate;
        if (Path.GetFileName(codeBase) == Constants.CoverletDataCollectorCodebase)
        {
            // If we're loading coverlet collector we skip to check the version of assembly
            // to allow upgrade through nuget package
            filterPredicate = x => Constants.CoverletDataCollectorTypeName.Equals(x.FullName) && interfaceType.IsAssignableFrom(x);

            // Coverlet collector is consumed as nuget package we need to add assemblies directory to resolver to correctly load references.
            TPDebug.Assert(Path.IsPathRooted(codeBase), "Absolute path expected");
            testPluginCache.AddResolverSearchDirectories(new string[] { Path.GetDirectoryName(codeBase)! });
        }
        else
        {
            filterPredicate = x => string.Equals(x.AssemblyQualifiedName, assemblyQualifiedName) && interfaceType.IsAssignableFrom(x);
        }

        _dataCollectorType = assembly?.GetTypes().FirstOrDefault(filterPredicate);
        AssemblyQualifiedName = _dataCollectorType?.AssemblyQualifiedName;
    }

    /// <summary>
    /// AssemblyQualifiedName of the datacollector type
    /// </summary>
    public string? AssemblyQualifiedName { get; private set; }

    /// <summary>
    /// Loads the DataCollector type
    /// </summary>
    /// <param name="inProcDataCollectionSink">Sink object to send data</param>
    public void LoadDataCollector(IDataCollectionSink inProcDataCollectionSink)
    {
        _dataCollectorObject = CreateObjectFromType(_dataCollectorType);
        InitializeDataCollector(_dataCollectorObject, inProcDataCollectionSink);
    }

    /// <summary>
    /// Triggers InProcDataCollection Methods
    /// </summary>
    /// <param name="methodName">Name of the method to trigger</param>
    /// <param name="methodArg">Arguments for the method</param>
    public void TriggerInProcDataCollectionMethod(string methodName, InProcDataCollectionArgs methodArg)
    {
        var methodInfo = GetMethodInfoFromType(_dataCollectorObject?.GetType(), methodName, new[] { methodArg.GetType() });

        if (methodName.Equals(Constants.TestSessionStartMethodName))
        {
            var testSessionStartArgs = (TestSessionStartArgs)methodArg;
            testSessionStartArgs.Configuration = _configXml!;
            methodInfo?.Invoke(_dataCollectorObject, new object[] { testSessionStartArgs });
        }
        else
        {
            methodInfo?.Invoke(_dataCollectorObject, new object[] { methodArg });
        }
    }

    private static void InitializeDataCollector(object? obj, IDataCollectionSink inProcDataCollectionSink)
    {
        var initializeMethodInfo = GetMethodInfoFromType(obj?.GetType(), "Initialize", new Type[] { typeof(IDataCollectionSink) });
        initializeMethodInfo?.Invoke(obj, new object[] { inProcDataCollectionSink });
    }

    private static MethodInfo? GetMethodInfoFromType(Type? type, string funcName, Type[] argumentTypes)
    {
        return type?.GetMethod(funcName, argumentTypes);
    }

    private static object? CreateObjectFromType(Type? type)
    {
        var constructorInfo = type?.GetConstructor(Type.EmptyTypes);
        object? obj = constructorInfo?.Invoke(new object[] { });
        return obj;
    }

    /// <summary>
    /// Loads the assembly into the default context based on the code base path
    /// </summary>
    /// <param name="codeBase"></param>
    /// <returns></returns>
    private Assembly? LoadInProcDataCollectorExtension(string codeBase)
    {
        Assembly? assembly = null;
        try
        {
            assembly = _assemblyLoadContext.LoadAssemblyFromPath(Environment.ExpandEnvironmentVariables(codeBase));
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "InProcDataCollectionExtensionManager: Error occurred while loading the InProcDataCollector : {0} , Exception Details : {1}", codeBase, ex);
        }

        return assembly;
    }

}
