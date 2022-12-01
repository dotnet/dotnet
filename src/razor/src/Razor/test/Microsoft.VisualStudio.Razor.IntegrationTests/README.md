# Razor Tooling Visual Studio Integration tests

## Running

If you run the integration tests from the command line using something like "dotnet test" they may fail because the Extension was now deployed. To ensure the extension was deployed you can either launch tests through Visual Studio or first run a command like `eng\cibuild.cmd -configuration Debug -msbuildEngine vs -prepareMachine /p:BuildVsix=true`
