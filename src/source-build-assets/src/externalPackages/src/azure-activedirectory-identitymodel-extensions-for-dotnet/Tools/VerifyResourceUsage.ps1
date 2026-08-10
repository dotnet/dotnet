$folderPath = $PSScriptRoot + "/../src"

# Get LogMessages C# files
$files = Get-ChildItem -Path $folderPath -Filter LogMessages.cs -Recurse

# Dictionary to hold constants and their usage status
$constants = @{}

# Extract constants
foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName
    foreach ($line in $content) {
        if (($line -match 'const\s+\w+\s+(\w+)\s*=') -and !($line.Contains("//"))) {
            $constantName = $matches[1]
            $constants[$constantName] = [PSCustomObject]@{
                File = $file.FullName
                ConstantName = $constantName
                Found = $false
            }
        }
    }
}

$files = Get-ChildItem -Path $folderPath -Filter *.cs -Exclude LogMessages.cs -Recurse
$keys = @($constants.Keys)

# Check for usage
foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName
    foreach ($constantName in $keys) {
        if (Select-String -InputObject $content -Pattern $constantName) {
            $constants[$constantName].Found = $true
        }
    }
}

# Output unused constants
$unusedConstants = $constants.GetEnumerator() | Where-Object { $_.Value.Found -eq $false }
if ($unusedConstants.Count -eq 0) {
    Write-Output "No unused constants found."
} else {
    Write-Output "Unused constants:"
    foreach ($unused in $unusedConstants) {
        $constName = $unused.Value.ConstantName
        $filePath = $unused.Value.File
        $message = "'$constName' in file '$filePath'"
        Write-Output $message
    }

    throw "found unused constants"
}
