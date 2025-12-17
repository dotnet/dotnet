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