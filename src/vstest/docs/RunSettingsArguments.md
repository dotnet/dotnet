# Passing runsettings arguments through commandline

You are here because you are looking for syntax and details to pass runsettings configurations to either `vstest.console.exe` or `dotnet test` through commandline.

`RunSettings arguments` are used to add/update specific `runsettings configurations`. The updated `runsettings configurations` will be available to `TestPlatform` and `test extensions` (E.g. test adapter, etc.) through runsettings.

## Syntax

* `RunSettings arguments` are specified as name-value pair of the form `[name]=[value]` after `--`. Note the space after --.
* Use a space to separate multiple `[name]=[value]`.
* All the arguments after `--` will be treated as `RunSettings arguments`, means `RunSettings arguments` should be at the end of the command line.

## Example

Passing argument `-- MSTest.MapInconclusiveToFailed=True` in (1) below is equivalent to passing argument
`--settings additionalargs.runsettings` in (2) below.

```shell
1) dotnet test  -- MSTest.MapInconclusiveToFailed=True MSTest.DeploymentEnabled=False
```

```shell
2) dotnet test --settings additionalargs.runsettings
```

where `additionalargs.runsettings` is:

```xml
<?xml version="1.0" encoding="utf-8"?>  
<RunSettings>  
  <!-- MSTest adapter -->  
  <MSTest>  
    <MapInconclusiveToFailed>True</MapInconclusiveToFailed>
    <DeploymentEnabled>False</DeploymentEnabled>
  </MSTest>   
</RunSettings> 
```

The syntax in (1) is another way of passing runsettings configuration and you need not author a runsetting file while using `Runsettings arguments`. More details about runsettings can be found [here](https://msdn.microsoft.com/library/jj635153.aspx).

`Runsettings arguments` takes precedence over `runsettings`.

For example, in below command the final value for `MapInconclusiveToFailed` will be `False` and value for `DeploymentEnabled` will be unchanged, that is `False`.

```shell
dotnet test --settings additionalargs.runsettings -- MSTest.MapInconclusiveToFailed=False
```

Starting from .NET SDK 5.0 you can also set `TestRunParameters` using command line option:

```cmd
# cmd
dotnet test  -- TestRunParameters.Parameter(name=\"myParam\", value=\"value\")

# powershell (prior 7.3, or with $PSNativeCommandArgumentPassing = "legacy")
dotnet test --%  -- TestRunParameters.Parameter(name=\"myParam\", value=\"value\")

# powershell (7.3+)
dotnet test --%  -- TestRunParameters.Parameter(name="myParam", value="value") 

# bash
dotnet test -- TestRunParameters.Parameter\(name=\"myParam\",\ value=\"value\"\) 
```

In this example, `\"myParam\"` corresponds to the name of you parameter and `\"value\"` - the value of your parameter. Note, that `\` are escaping characters and they should stay as shown above, unless you are in PowerShell 7.3+. For more examples in PowerShell, such as using variables for the data, please [refer here](https://github.com/microsoft/vstest/issues/4637).
