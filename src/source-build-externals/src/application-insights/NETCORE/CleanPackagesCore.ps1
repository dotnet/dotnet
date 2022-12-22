#enable verbose mode
$VerbosePreference = "Continue";

[String]$dnxRoot = [System.Environment]::ExpandEnvironmentVariables('%USERPROFILE%\.nuget');

rm -r $dnxRoot\packages -Force 

