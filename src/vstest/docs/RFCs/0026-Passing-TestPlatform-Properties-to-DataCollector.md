# 0026 DataCollector Extensibility - Passing TestPlatform properties to DataCollector extensions 

## Summary
Passing TestPlatform properties as part of property bag to datacollectors. These properties can be used by the datacollectors for processing before test run start.

## Motivation
Data collector might need test platform properties. Example: Static code coverage data collector needs to instrument the test sources before test run start thus need list of test sources during initialization.

## Design
`SessionStartEventArgs` will contain `Properties` which are passed to the data collector in `SessionStart` event.
Currently test platform passes following properties to the datacollector extensions :

**"TestSources"** : `IEnumerable<string>`<br/>
TestSources is an enumerable of string of all test sources that is used by the test run.

The public APIs exposed in `SessionStartEventArgs` would be as given below.
```csharp
/// <summary>
/// Gets session start properties enumerator
/// </summary>
public IEnumerator<KeyValuePair<string, object>> GetProperties()

/// <summary>
/// Gets property value
/// </summary>
/// <param name="property"> Property name </param>
public T GetPropertyValue<T>(string property)

/// <summary>
/// Gets property value
/// </summary>
/// <param name="property"> Property name </param>
public object GetPropertyValue(string property)

```

## Usage
In the datacollector, the user can get test sources as given below.
```csharp
IEnumerable sources = args.GetPropertyValue<IEnumerable>("TestSources");
```

Also, all properties can be accessed via the `GetProperties` API as given.
```csharp
var properties = args.GetProperties();
while (Properties.MoveNext())
{
    Console.WriteLine(properties.Current.Key);
    Console.WriteLine(properties.Current.Value);
}
```
