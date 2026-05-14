# 0032 Extensions versioning attribute

# Summary
This document explain how to use the `TestExtensionTypesAttribute` and `TestExtensionTypesV2Attribute` to handle the versioning for every platform extension.

# Motivation
The test platform offers many extensions points like custom test adapters, test execution providers, data collectors, etc...   
The main problem with the extensions is that the platform is in continuous evolution. Therefore, we need to keep the back-compatibility; we want to load new extensions in the old test platform implementation and vice versa.

The logic used to load the extensions if inside the [TestPluginDiscoverer.cs](./src/Microsoft.TestPlatform.Common/ExtensionFramework/TestPluginDiscoverer.cs) file.  

# How to use `TestExtensionTypesAttribute` and `TestExtensionTypesV2Attribute`

By design, the `TestPluginDiscoverer` receives a list of all files to search for some specific extensions(for instance, search for a class that implements a particular interface). However, the list of files is out of the scope of this document and depends on the running context(VS, SDK, stand-alone, etc...) and some user-defined settings. 

The discovery algorithm is:  
1) `TestPluginDiscoverer` loads the assemblies using the `Assembly.Load(new AssemblyName(assemblyName));` where the `assemblyName` is the file name without extension `Path.GetFileNameWithoutExtension(file);`.

2) As the next step, the `TestPluginDiscoverer` loads every `Type` inside the assembly and tries to find out if it's compatible with the extension requested using the `Type.IsAssignableFrom(Type)` api.

3) The final step of `TestPluginDiscoverer` is to check if the extension is already loaded. It checks if the plugin `IdentifierData` is already present in the already loaded extension list.

Due to the design explained above, the `TestPluginDiscoverer` suffers from some defects:

1) The order of files **matters**; use the file name and load with `Assembly.Load(new AssemblyName(assemblyName));` means that if we have more than one file with the exact implementation but different versions, only the first one will be loaded; all other loads will be silently skipped because, for the runtime, the "same" `Assembly` is already loaded.

2) The order of types inside the assembly **matters** because we're using the `assembly.GetTypes().Where(type => type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract)` api. So if we have 2 types that are implementing the same extension only the first one will be loaded.  

3) If we have a lot of types inside the loaded assembly, the perf is not so great because we're checking all types.  

4) If we ship new public types used by the extensions, we'll fail to load these in the old shipped test platform because that type doesn't exist, and the runtime will raise a `TypeLoadException.`

An attempt was made to fix the 2nd and 3rd defects above, introducing an attribute that allows the user to "register" every extension shipped inside the assembly.   
***To avoid compatibility issues, we don't ship the attribute type as a public type, but we search for a "specific" attribute type implementation signature/shape.***

The attribute signature/shape is:
```cs
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
internal sealed class TestExtensionTypesAttribute : Attribute
{
    public TestExtensionTypesAttribute(params Type[] types)
    {
        this.Types = types;
    }

    public Type[] Types { get; }
}
```
Suppose to have an extension called `MyExtension` and you want to avoid the two issues above you can register your type in this way:
```cs
[assembly: TestExtensionTypes(typeof(MyExtension))]
```

Anyway, this first attempt doesn't solve the 1st and 4th issues.  
To fix the ***versioning*** issue, we decided to ship another attribute that can be used with the first one compatible with old test platform versions. 
The new attribute address some more issue:
1) **versioning**: we can specify the extension version. At runtime, the `TestPluginDiscoverer` will load the extensions using the version number(a simple int), **from the higher to the lower in desc order.**  
2) **avoid the `TypeLoadException`**: `TestPluginDiscoverer` will try to load every registered extension using the `MetadataReader` type, once it find an extension it tries to load the type using `Assembly.GetType(string)` api. In case of an issue with the loading, it will skip the extension.   
The attribute signature/shape is:
```cs
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
internal sealed class TestExtensionTypesV2Attribute : Attribute
{
    public string ExtensionType { get; }
    public string ExtensionIdentifier { get; }
    public Type ExtensionImplementation { get; }
    public int Version { get; }

    public TestExtensionTypesV2Attribute(string extensionType, string extensionIdentifier, Type extensionImplementation, int version)
    {
        ExtensionType = extensionType;
        ExtensionIdentifier = extensionIdentifier;
        ExtensionImplementation = extensionImplementation;
        Version = version;
    }
}
```
Suppose to have a data collector extension called `MyDataCollectorV1` using some shipped extension. 
After some time, you want to ship a newer version that will use a new type shipped only in the newer test platform object model. So you cannot use the current version of your extension because it won't work in the old test platform(it will silently fail with a `TypeLoadException`).  
In this case, you can register your time specifying the new version of your extension:
```cs
[assembly: TestExtensionTypes(typeof(MyDataCollectorV1))]
[assembly: TestExtensionTypesV2("MyDataCollector", "datacollector://Microsoft/MyLocalTest/1.0", typeof(MyDataCollectorV1), 1)]
[assembly: TestExtensionTypesV2("MyDataCollector", "datacollector://Microsoft/MyLocalTest/1.0", typeof(MyDataCollectorV2), 2)]
```
With the configuration above, the `TestPluginDiscoverer` will try to load using this order:   
1) Search for every attribute with the expected shape of `TestExtensionTypesV2`. `TestExtensionTypesV2` will use **the binary of the extension file** and won't use the `Assembly.Load(new AssemblyName(assemblyName));` api. It means that we can search for same extension in different files.
2) Put them in order by `Version` (first `MyDataCollectorV2`, second `MyDataCollectorV1`)
3) Try to create the type(first `MyDataCollectorV2`, second `MyDataCollectorV1`) and put these in the candidate extension types list.
4) Search for every attribute with the expected shape of `TestExtensionTypes` using the default order `assembly.GetCustomAttributes` and put these in the candidate extension types list.

If we suppose to be able to load all types at the end of the discovery, we'll have a candidate extension types list like:

`MyDataCollectorV2`   
`MyDataCollectorV1`  
`MyDataCollectorV1`  

Now the `TestPluginDiscoverer` will go on with the usual `Type.IsAssignableFrom(Type)` check. It will load `MyDataCollectorV2`, skipping all the other extensions because we already loaded the plugin with the same `IdentifierData.`

If we try to load the same extension in an old test platform, we will be able to load only using the `TestExtensionTypes` the `MyDataCollectorV1` version. 

