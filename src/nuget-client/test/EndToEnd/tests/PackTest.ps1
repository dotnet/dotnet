function NoTest-PackFromProject {
    param(
        $context
    )

    $p = New-ClassLibrary
    $p.Properties.Item("Company").Value = "Some Company"
    $item = Get-ProjectItem $p Properties\AssemblyInfo.cs
    $item.Save()
    $p.Save()

    $output = (Get-PropertyValue $p FullPath)
    & $context.NuGetExe pack $p.FullName -build -o $output

    $packageFile = Get-ChildItem $output -Filter *.nupkg
    Assert-NotNull $packageFile
    $zipPackage = New-Object NuGet.ZipPackage($packageFile.FullName)
    Assert-AreEqual $p.Name $zipPackage.Id
    Assert-AreEqual '1.0.0.0' $zipPackage.Version.ToString()
    Assert-AreEqual 'Some Company' $zipPackage.Authors
    Assert-AreEqual 'Description' $zipPackage.Description
    $files = @($zipPackage.GetFiles())
    Assert-AreEqual 1 $files.Count
    Assert-AreEqual "lib\net40\$($p.Name).dll" $files[0].Path
    $assemblies = @($zipPackage.AssemblyReferences)
    Assert-AreEqual 1 $assemblies.Count
    Assert-AreEqual "$($p.Name).dll" $assemblies[0].Name
}

function Test-PackFromProjectWithDevelopmentDependencySet {
    param(
        $context
    )

    # Arrange 

    $p = New-WebApplication

    # install packages from the Basic web app manually

    install-package EntityFramework -version 5.0.0 -ignoreDependencies
    install-package jquery -version 1.8.2 -ignoreDependencies
    install-package jquery.validation -version 1.10.0 -ignoreDependencies
    install-package jquery.ui.combined -version 1.8.24 -ignoreDependencies
    install-package Microsoft.jQuery.Unobtrusive.Validation -version 2.0.30116.0 -ignoreDependencies
    install-package Microsoft.jQuery.Unobtrusive.Ajax -version 2.0.30116.0 -ignoreDependencies
    install-package Modernizr -version 2.6.2 -ignoreDependencies
    install-package Microsoft.Web.Infrastructure -version 1.0.0 -ignoreDependencies
    install-package Newtonsoft.Json -version 13.0.1 -ignoreDependencies
    install-package Microsoft.AspNet.Razor -version 2.0.20715.0 -ignoreDependencies
    install-package Microsoft.AspNet.WebPages -version 2.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.Mvc -version 4.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.Mvc.FixedDisplayModes -version 1.0.0 -ignoreDependencies
    install-package Microsoft.AspNet.WebApi.Client -version 4.0.20710.0 -ignoreDependencies
    install-package Microsoft.Net.Http -version 2.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.WebApi.Core -version 4.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.WebApi.WebHost -version 4.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.WebApi -version 4.0.20710.0 -ignoreDependencies
    install-package Microsoft.AspNet.Providers.Core -version 1.2 -ignoreDependencies
    install-package Microsoft.AspNet.Providers.LocalDB -version 1.1 -ignoreDependencies
    install-package Microsoft.AspNet.Web.Optimization -version 1.0.0 -ignoreDependencies
    install-package WebGrease -version 1.3.0 -ignoreDependencies
    install-package knockoutjs -version 2.2.0 -ignoreDependencies

    # trying to insert the developmentDependency attribute to jQuery package entry

    $output = (Get-PropertyValue $p FullPath)
    $packageConfigPath = Join-Path $output 'packages.config'

    $config = [xml](Get-Content $packageConfigPath)       
    $config.packages.package | ? { $_.id -eq 'jquery' } | % { $_.setAttribute("developmentDependency", "true") }
    $config.Save($packageConfigPath)
    
    $p.Save()

    # Act
    
    & $context.NuGetExe pack $p.FullName -build -OutputDirectory $output

    # Assert
    $packageFile = Get-ChildItem $output -Filter *.nupkg
    Assert-NotNull $packageFile
    
}

function NoTest-PackFromProjectUsesInstalledPackagesAsDependencies {
    param(
        $context
    )

    $p = New-ClassLibrary
    
    $p | Install-Package PackageWithContentFileAndDependency -Source $context.RepositoryRoot
    $p.Save()

    $output = (Get-PropertyValue $p FullPath)
    & $context.NuGetExe pack $p.FullName -build -o $output

    $packageFile = Get-ChildItem $output -Filter *.nupkg
    Assert-NotNull $packageFile
    $zipPackage = New-Object NuGet.ZipPackage($packageFile.FullName)
    $dependencySets = @($zipPackage.DependencySets)

    Assert-NotNull $dependencySets
    Assert-AreEqual 1 $dependencySets.Count
    Assert-Null $dependencySets[0].TargetFramework
    $dependencies = $dependencySets[0].Dependencies
    Assert-AreEqual 'PackageWithContentFileAndDependency' $dependencies[0].Id
    Assert-AreEqual "1.0" $dependencies[0].VersionSpec.ToString()
}

function NoTest-PackFromProjectUsesVersionSpecForDependencyIfApplicable {
    $p = New-ClassLibrary
    
    $p | Install-Package PackageWithContentFileAndDependency -Source $context.RepositoryRoot
    Add-PackageConstraint $p PackageWithContentFileAndDependency "[1.0, 2.5)"
    $p.Save()

    $output = (Get-PropertyValue $p FullPath)
    & $context.NuGetExe pack $p.FullName -build -o $output

    $packageFile = Get-ChildItem $output -Filter *.nupkg
    Assert-NotNull $packageFile
    $zipPackage = New-Object NuGet.ZipPackage($packageFile.FullName)
    $dependencySets = @($zipPackage.DependencySets)

    Assert-NotNull $dependencySets
    Assert-AreEqual 1 $dependencySets.Count
    Assert-Null $dependencySets[0].TargetFramework
    $dependencies = $dependencySets[0].Dependencies
    Assert-AreEqual 'PackageWithContentFileAndDependency' $dependencies[0].Id
    Assert-AreEqual "[1.0, 2.5)" $dependencies[0].VersionSpec.ToString()
}
