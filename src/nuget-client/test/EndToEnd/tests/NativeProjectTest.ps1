function Test-NativeProjectInstallPackage {
    param()

    $projectT = New-Project NativeConsoleApplication

    # Act
    $projectT | Install-Package zlib -Version 1.2.8.7 -IgnoreDependencies

    Assert-True ($projectT | Test-InstalledPackage -Id zlib) -Message 'Test package should be installed'
}

function Test-NativeProjectUninstallPackage {
    param()

    $projectT = New-Project NativeConsoleApplication
    $projectT | Install-Package zlib -Version 1.2.8.7 -IgnoreDependencies

    # Act
    $projectT | Uninstall-Package zlib

    Assert-False ($projectT | Test-InstalledPackage -Id zlib) -Message 'Test package should be uninstalled'
}
function Test-NativeProjectInstallNonExistentPackage {
    param()

    $projectT = New-Project NativeConsoleApplication

    # Act and Assert
    Assert-Throws { $projectT | Install-Package NonExisting } "Unable to find package 'NonExisting'"
}

function Test-NativeProjectUpdateNonExistentPackage {
    param()

    $projectT = New-Project NativeConsoleApplication

    # Act and Assert
    Assert-Throws {
        $projectT | Update-Package zlib -version 1.2.8.8
    } "'zlib' was not installed in any project. Update failed."
}

function Test-NativeProjectUninstallNonExistentPackage {
    param()

    $projectT = New-Project NativeConsoleApplication

    # Act and Assert
    Assert-Throws {
        $projectT | Uninstall-Package zlib
    } "Package 'zlib' to be uninstalled could not be found in project '$($projectT.Name)'"
}

function Test-NativeProjectInstallNonNativePackage {
    param()

    $projectT = New-Project NativeConsoleApplication

    # Act and Assert
    Assert-Throws {
        $projectT | Install-Package NuGet.Versioning -Version 3.5.0
    } "Could not install package 'NuGet.Versioning 3.5.0'. You are trying to install this package into a project that targets 'native,Version=v0.0', but the package does not contain any assembly references or content files that are compatible with that framework. For more information, contact the package author."
}