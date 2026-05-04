# This repository should never write to the global nuget package cache as it creates
# and then later restores packages with a partial layout.
$script:useGlobalNuGetCache = $false