
function Test-GetPackageRetunsMoreThanServerPagingLimit {
    # Act
    $packages = Get-Package -ListAvailable

    # Assert
    Assert-True $packages.Count -gt 100 "Get-Package cmdlet returns less than (or equal to) than server side paging limit"
}


function Test-GetPackageWithoutOpenSolutionThrows {
    Assert-Throws { Get-Package } "The current environment doesn't have a solution open."
}

function Test-GetPackageWithUpdatesListsUpdates {
    # Arrange
    $p = New-ConsoleApplication

    # Act
    Install-Package NuGet.Core -Version 1.6.0 -Project $p.Name
    Install-Package NuGet.CommandLine -Version 1.6.0 -Project $p.Name
    $packages = Get-Package -Updates

    # Assert
    Assert-AreEqual 2 $packages.Count
}

function Test-GetPackageCollapsesPackageVersionsForListAvailable {
    [SkipTest('https://github.com/NuGet/Home/issues/8849')]
    param()

    # Act
    $packages = Get-Package -ListAvailable jQuery
    $packagesWithMoreThanOne = $packages | group "Id" | Where { $_.count -gt 1 }

    # Assert
    # Ensure we have at least some packages
    Assert-True (1 -le $packages.Count)
    Assert-Null $packagesWithMoreThanOne
}

function Test-GetPackageAcceptsSourceName {
    # Act
    $p = @(Get-Package -Filter elmah -ListAvailable -Source $SourceNuGet )

    # Assert
    Assert-True (1 -le $p.Count)
}

function Test-GetPackageWithUpdatesAcceptsSourceName {
    # Arrange
    $p = New-ConsoleApplication

    # Act
    Install-Package Antlr -Version 3.1.1 -Project $p.Name -Source $SourceNuGet
    Install-Package jQuery -Version 1.4.1 -Project $p.Name -Source $SourceNuGet
    $packages = Get-Package -Updates -Source $SourceNuGet

    # Assert
    Assert-AreEqual 2 $packages.Count
}

function GetPackageAcceptsAllAsSourceName {
     # Act
    $p = @(Get-Package -Filter elmah -ListAvailable -Source 'All')

    # Assert
    Assert-True (1 -le $p.Count)
}

function Test-GetPackagesWithNoUpdatesReturnPackagesWithIsUpdateNotSet {
    # Arrange & Act
    $package = Get-Package -ListAvailable -First 1

    # Assert
    Assert-NotNull $package
    Assert-False $package.IsUpdate
}

function Test-GetPackageDoesNotThrowIfSolutionIsTemporary {
    param($context)

    # Arrange
    New-TextFile

    # Act and Assert
    Assert-Throws { Get-Package } "Solution is not saved. Please save your solution before managing NuGet packages."
}

function Test-GetPackageUpdatesAfterSwitchToSourceThatDoesNotContainInstalledPackageId
{
    [SkipTest('https://github.com/NuGet/Home/issues/10254')]
    param
    (
        $context
    )

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package antlr -Version '3.1.1' -Source $SourceNuGet

    # Act
    $packages = @(Get-Package -updates -Source 'https://pkgs.dev.azure.com/dnceng/public/_packaging/nuget-build/nuget/v3/index.json')

    # Assert
    Assert-AreEqual 0 $packages.Count
}

