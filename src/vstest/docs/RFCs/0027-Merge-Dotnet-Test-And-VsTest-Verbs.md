# 0027 Merge dotnet test and dotnet vstest verb

## Motivation
Customer ask to merge the two dotnet verbs "test" and "vstest".
Associated github issue: https://github.com/Microsoft/vstest/issues/1453

## Goal
User should be able to run unit and integration tests with ability to pass in the filename of either a .csproj or a .dll without having to change any other syntax of the dotnet test command.

## Success criteria (as per user ask)
1. The "dotnet test" command can be used in place of the "dotnet vstest" command.
2. The "dotnet test" command can accept a list of dlls
3. Using "dotnet test" and targeting a list of dlls, there is an argument for "--parallel" which will run the tests from the dlls in parallel.
4. Using "dotnet test" and targeting a dll, the inclusion or exclusion of the "--no-build" argument will have no impact on the behavior of execution.

## Issues with merging the two verbs

### Irrelevant Arguments
The arguments that are acceptable for the dotnet test for project/solution will not be applicable to the dotnet test when invoked for dlls.
Here are possible ways to handle this:

1. Ignore the arguments that are not valid.
2. Throw a warning to the user, and continue with the execution.
3. Throw an error, giving user an actionable message and stop the execution.

Note:
1. For option (1), ignoring the arguments seems incorrect. If the user gives certain arguments, the expectation is that the arguments get honored. Ignoring the arguments without notify the user would be incorrect. If an argument like "--no-build" is not relevant 

### Same arguments but different acceptable values
There are arguments that are accepted by both dotnet test and dotnet vstest today. We will have make sure there is parity with respect to these arguments before merge.
One such argument is "--framework". The values accepted by the dotnet test today are different from the ones accepted by dotnet vstest. Test platform today does not understand aliases. For example, ".NETCoreApp,Version=v1.0" is valid framework while netcoreapp1.0 is not valid framework for test platform.

We will have to make sure all such arguments accept the super set of these values now. We might have to add another layer to convert the values into appropriate values that the platform understands.

### Different arguments but similar relevance
There are arguments like "--runtime-identifier" which to some extent map to the "/platform" in the test platform world. 

Should there be added intelligence to convert this argument ? or should this fall in invalid arguments ?

## Possible Solution
Parse the arguments given to "dotnet test" to detect dll/exe present in the arguments. If detected, call dotnet vstest.console.dll with the rest of the arguments.

The above mentioned issues will need to be handled.

Open questions:
- What is the expected behavior if user gives both dlls and csproj ?
