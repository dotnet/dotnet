Set-Variable DefaultRestoreTimeoutInSeconds 20 -Option Constant

function Test-NetCoreProjectExecuteInitScriptOnInstall {
    param($context)

    Remove-Variable PackageInitPS1Var -Scope Global -Force -ErrorAction Ignore

    $componentModel = Get-VSComponentModel
    $restoreEvents = $componentModel.GetService([NuGet.VisualStudio.IRestoreEvents])

    $restoreEvent = $null

    Get-Event | Remove-Event
    Register-ObjectEvent -InputObject $restoreEvents -EventName SolutionRestoreCompleted -SourceIdentifier RestoreEventSource

    Try {
        $p = New-NetCoreConsoleApp

        # Wait for initial restore
        $restoreEvent = Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Assert-NotNull $restoreEvent
        Assert-AreEqual 'Succeeded' $restoreEvent.SourceEventArgs.RestoreStatus

        # Act
        $p | Install-Package PackageInitPS1 -Source $context.RepositoryRoot

        $restoreEvent = Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Assert-NotNull $restoreEvent
        Assert-AreEqual 'Succeeded' $restoreEvent.SourceEventArgs.RestoreStatus
        Assert-AreEqual 1 $global:PackageInitPS1Var
    }
    Finally {
        Unregister-Event RestoreEventSource
    }
}

function Test-NetCoreProjectExecuteInitScriptOnlyOnce {
    [SkipTest('https://github.com/NuGet/Home/issues/7891')]
    param($context)

    Remove-Variable PackageInitPS1Var -Scope Global -Force -ErrorAction Ignore

    $componentModel = Get-VSComponentModel
    $restoreEvents = $componentModel.GetService([NuGet.VisualStudio.IRestoreEvents])

    $restoreEvent = $null

    Get-Event | Remove-Event
    Register-ObjectEvent -InputObject $restoreEvents -EventName SolutionRestoreCompleted -SourceIdentifier RestoreEventSource

    Try {
        $p = New-NetCoreConsoleApp

        # Wait for initial restore
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        $p | Install-Package PackageInitPS1 -Source $context.RepositoryRoot
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        # Act
        $p | Install-Package NuGet.Versioning -Version 3.5.0

        $restoreEvent = Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Assert-NotNull $restoreEvent
        Assert-AreEqual 'Succeeded' $restoreEvent.SourceEventArgs.RestoreStatus
        Assert-AreEqual 1 $global:PackageInitPS1Var
    }
    Finally {
        Unregister-Event RestoreEventSource
    }
}

function Test-NetCoreProjectExecuteInitScriptAfterReopen {
    [SkipTest('Needs diagnostics event. NuGet/Home#5625')]
    param($context)

    $componentModel = Get-VSComponentModel
    $restoreEvents = $componentModel.GetService([NuGet.VisualStudio.IRestoreEvents])

    $restoreEvent = $null

    Get-Event | Remove-Event
    Register-ObjectEvent -InputObject $restoreEvents -EventName SolutionRestoreCompleted -SourceIdentifier RestoreEventSource

    Try {
        $p = New-NetCoreConsoleApp

        # Wait for initial restore
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        $p | Install-Package PackageInitPS1 -Source $context.RepositoryRoot
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        $p.Save($p.FullName)

        $solutionFile = Get-SolutionFullName
        SaveAs-Solution $solutionFile
        Close-Solution

        Remove-Variable PackageInitPS1Var -Scope Global -Force -ErrorAction Ignore

        # Reset script execution cache
        New-NetCoreConsoleApp
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Close-Solution

        # Act
        Open-Solution $solutionFile
        Wait-ForSolutionLoad

        $restoreEvent = Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Assert-NotNull $restoreEvent
        Assert-AreEqual 'Succeeded' $restoreEvent.SourceEventArgs.RestoreStatus

        Write-Verbose "Sleeping to let the host execute init scripts..."
        Start-Sleep -s 10
        Assert-AreEqual 1 $global:PackageInitPS1Var
    }
    Finally {
        Unregister-Event RestoreEventSource
    }
}

function Test-NetCoreProjectExecuteInitScriptOnSolutionRestore {
    [SkipTest('Needs diagnostics event. NuGet/Home#5625')]
    param($context)

    $componentModel = Get-VSComponentModel
    $restoreEvents = $componentModel.GetService([NuGet.VisualStudio.IRestoreEvents])

    $restoreEvent = $null

    Get-Event | Remove-Event
    Register-ObjectEvent -InputObject $restoreEvents -EventName SolutionRestoreCompleted -SourceIdentifier RestoreEventSource

    Try {
        $p = New-NetCoreConsoleApp

        # Wait for initial restore
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        $p | Install-Package PackageInitPS1 -Source $context.RepositoryRoot
        Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds

        Remove-Variable PackageInitPS1Var -Scope Global -Force -ErrorAction Ignore

        # Act
        Build-Solution

        $restoreEvent = Wait-Event -SourceIdentifier RestoreEventSource -TimeoutSec $DefaultRestoreTimeoutInSeconds
        Assert-NotNull $restoreEvent
        Assert-AreEqual 'Succeeded' $restoreEvent.SourceEventArgs.RestoreStatus
        Assert-Null $global:PackageInitPS1Var
    }
    Finally {
        Unregister-Event RestoreEventSource
    }
}
