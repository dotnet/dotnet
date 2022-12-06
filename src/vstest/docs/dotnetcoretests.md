For dotnet core test projects, the test platform is acquired as a nuget package (Micorosoft.Net.Test.SDK) and the runtime (testhost.dll similar to testhost.exe) is also part of the nuget package. 

When dotnet build runs, the above mentioned packages are restored to the users' global nuget cache in the absence of any overridden config. When vstest.console.exe runs <UnitTestProject>.runtimeconfig.dev.json is looked into to determine the folders to look for the testhost.dll (something like the below) in addition to the folder where the test dll is present (https://github.com/Microsoft/vstest/blob/c7472a479966a218fb0ac508ed799418eb4bfc00/src/Microsoft.TestPlatform.TestHostProvider/Hosting/DotnetTestHostManager.cs#L374)

{
  "runtimeOptions": {
    "additionalProbingPaths": [
      "C:\\Users\\shivash\\.dotnet\\store\\|arch|\\|tfm|",
      "C:\\Users\\shivash\\.nuget\\packages",
      "C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder"
    ]
  }
}

In the case of the customer, the nuget packages aren't being restored to the paths defined in the <UnitTestProject>.runtimeconfig.dev.json resulting in the testhost.dll not being determined. The locations used for the nuget packages can be determined using the below : https://docs.microsoft.com/en-us/nuget/consume-packages/managing-the-global-packages-and-cache-folders#viewing-folder-locations

To mitigate the issue and as a general recommendation for running dot net core tests with vstest task please ask the customer to publish the test project and point to the publish location for running tests. Publish ensures all needed dependencies are present for the tests to be executed alongside the test dll in case of dot net core tests.
