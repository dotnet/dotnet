#enable verbose mode
$VerbosePreference = "Continue";

[String]$dnxRoot = [System.Environment]::ExpandEnvironmentVariables('%USERPROFILE%\.dnx');

rm -r $dnxRoot\packages -Force 
