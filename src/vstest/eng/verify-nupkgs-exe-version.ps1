Get-ChildItem  S:\p\vstest3\artifacts\packages\Debug\Shipping -Filter vstest.console.exe -Recurse -Force | ForEach-Object {
    if ($_.VersionInfo.ProductVersion.Contains("+")) {
        throw "Some files contain '+' in the ProductVersion, this breaks DTAAgent in AzDO."
    }
    else {
        "$_ version $($_.VersionInfo.ProductVersion) is ok."
    }
} 