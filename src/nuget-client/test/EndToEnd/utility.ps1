function New-Guid {
    [System.Guid]::NewGuid().ToString("d").Substring(0, 4).Replace("-", "")
}

function Get-HostSemanticVersion {
    return [NuGet.Packaging.MinClientVersionUtility]::GetNuGetClientVersion()
}

class SkipTest : Attribute {
    SkipTest([string]$Reason) { }
    [bool] ShouldRun() { return $False }
}

function ShouldRunTest {
    param(
        [System.Management.Automation.CommandInfo]
        $Command
    )
    $SkipTest = $Command.ScriptBlock.Attributes | ?{ $_.TypeID.Name -like 'SkipTest*' }

    return -not $SkipTest -or $SkipTest.ShouldRun()
}