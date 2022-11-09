# Net Core Integration Tests
Integration Tests are fundamentally different than Unit Tests. 
Unlike Unit Tests, these cannot be run as a standalone DLL and must be run in the context of their project (*.csproj).

For more information please visit [Integration tests in ASP.NET Core](https://docs.microsoft.com/aspnet/core/test/integration-tests)

For the purpose of our build servers, please use this solution for all Integration Tests.