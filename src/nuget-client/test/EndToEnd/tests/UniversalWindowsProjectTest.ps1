function Test-UwpNativeAppInstallPackage {
    param($context)

    $projectT = New-Project UwpNativeApp

    # Act
    $projectT | Install-Package PackageWithNativeCustomControl -Version '1.0.14' -Source $context.RepositoryRoot
    Assert-True ($projectT | Test-InstalledPackage -Id PackageWithNativeCustomControl) -Message 'Test package should be installed'
}

function Test-UwpNativeAppUninstallPackage {
    param($context)

    $projectT = New-Project UwpNativeApp
    $projectT | Install-Package PackageWithNativeCustomControl -Version '1.0.14' -Source $context.RepositoryRoot

    # Act
    $projectT | Uninstall-Package PackageWithNativeCustomControl

    Assert-False ($projectT | Test-InstalledPackage -Id PackageWithNativeCustomControl) -Message 'Test package should be uninstalled'
}

function Test-UwpNativeProjectJsonBuild {
    [SkipTest('https://github.com/NuGet/Home/issues/12185')]
    param($context)

    $projectT = New-Project UwpNativeProjectJson
    
    # Act
    Build-Solution

    # Assert
    $errorlist = Get-Errors
    Assert-AreEqual 0 $errorlist.Count
}