function Test-ProjectRetargeting-ShowErrorUponRetargeting {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that an error has been added to the error list window)

    $errorlist = Get-Errors

    Assert-AreEqual 1 $errorlist.Count

    $error = $errorlist[$errorlist.Count-1]

    Assert-AreEqual 'Some NuGet packages were installed using a target framework different from the current target framework and may need to be reinstalled. Visit https://docs.nuget.org/docs/workflows/reinstalling-packages for more information.  Packages affected: PackageTargetingNet40AndNet40Client' $error
}

function Test-ProjectRetargeting-ClearErrorUponCleanProject {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )
    $projectName = $p.Name

    Write-Host '2'
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'



    # Assert (Assert that an error has been added to the error list window)
    $errorlist = Get-Errors

    Assert-AreEqual 1 $errorlist.Count

    Clean-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count
}

function Test-ProjectRetargeting-ClearErrorUponCloseSolution {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that an error has been added to the error list window)

    $errorlist = Get-Errors

    Assert-AreEqual 1 $errorlist.Count

    Close-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count
}

function Test-ProjectRetargeting-ClearErrorAndWarningRetargetBackToOriginalFramework {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that an error has been added to the error list window)

    $errorlist = Get-Errors

    Assert-AreEqual 1 $errorlist.Count

    # Change the framework of the project back to .NET 4.0 and verify that the error shown is cleared

    $p = Get-Project $projectName
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=4.0'

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    Build-Solution

    $warnings = Get-Warnings

    Assert-AreEqual 0 $warnings.Count
}

function Test-ProjectRetargeting-ConvertBuildErrorToBuildWarningUponBuild {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that an error has been added to the error list window)

    $errorlist = Get-Errors

    Assert-AreEqual 1 $errorlist.Count

    # Build Solution now and verify that the error has been converted to a warning

    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 1 $warnings.Count

    $warning = $warnings[$warnings.Count - 1]

    Assert-AreEqual 'Some NuGet packages were installed using a target framework different from the current target framework and may need to be reinstalled. Visit https://docs.nuget.org/docs/workflows/reinstalling-packages for more information.  Packages affected: PackageTargetingNet40AndNet40Client' $warning
}

function Test-ProjectRetargeting-ShowWarningOnCleanBuild {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Clean and Build the Solution again. And, verify that the warning is shown as expected)

    Clean-Solution
    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 1 $warnings.Count

    $warning = $warnings[$warnings.Count - 1]

    Assert-AreEqual 'Some NuGet packages were installed using a target framework different from the current target framework and may need to be reinstalled. Visit https://docs.nuget.org/docs/workflows/reinstalling-packages for more information.  Packages affected: PackageTargetingNet40AndNet40Client' $warning
}

function Test-ProjectRetargeting-ClearWarningUponCleanProject {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Verify that the warning is cleared upon cleaning solution)

    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 1 $warnings.Count

    $warning = $warnings[$warnings.Count - 1]

    Clean-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 0 $warnings.Count
}

function Test-ProjectRetargeting-ClearWarningUponCloseSolution {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Verify that the warning is cleared upon closing solution)

    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 1 $warnings.Count

    $warning = $warnings[$warnings.Count - 1]

    Close-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 0 $warnings.Count
}


function Test-ProjectRetargeting-ClearWarningUponPackageReinstallationAndBuild {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Verify that the warning is cleared after reinstalling the package and building the solution)

    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 1 $warnings.Count

    $warning = $warnings[$warnings.Count - 1]

    Update-Package -Reinstall -Project $projectName -Source $context.RepositoryPath
    Build-Solution

    $errorlist = Get-Errors

    Assert-AreEqual 0 $errorlist.Count

    $warnings = Get-Warnings

    Assert-AreEqual 0 $warnings.Count
}

function Test-ProjectRetargeting-ClearReinstallationFlagRetargetBackToOriginalFramework {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that the package reference has requireReinstallation flag marked to true)

    $p = Get-Project $projectName
    $packageReferences = Get-ProjectPackageReferences $p

    Assert-AreEqual 1 $packageReferences.Count
    Assert-True $packageReferences[0].RequireReinstallation

    # Change the framework of the project back to .NET 4.0 and verify that the reinstallation is cleared

    $p = Get-Project $projectName
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=4.0'

    $p = Get-Project $projectName
    $packageReferences = Get-ProjectPackageReferences $p

    Assert-AreEqual 1 $packageReferences.Count
    Assert-False $packageReferences[0].RequireReinstallation
}

function Test-ProjectRetargeting-ClearReinstallationFlagUponPackageReinstallation {
    [SkipTest('https://github.com/NuGet/Home/issues/11221')]
    param($context)

    # Arrange
    $p = New-ClassLibrary

    $p | Install-Package PackageTargetingNet40AndNet40Client -Source $context.RepositoryPath

    Assert-Package $p 'PackageTargetingNet40AndNet40Client'

    # Act (change the target framework of the project to 4.0-Client and verify that an error is thrown )

    $projectName = $p.Name
    $p.Properties.Item("TargetFrameworkMoniker").Value = '.NETFramework,Version=v4.0,Profile=Client'

    # Assert (Assert that the package reference has requireReinstallation flag marked to true)

    $p = Get-Project $projectName
    $packageReferences = Get-ProjectPackageReferences $p

    Assert-AreEqual 1 $packageReferences.Count
    Assert-True $packageReferences[0].RequireReinstallation

    # Assert (Assert that the package reinstallation flag is removed when the package is updated)
    Update-Package -Reinstall -Project $projectName -Source $context.RepositoryPath

    $p = Get-Project $projectName
    $packageReferences = Get-ProjectPackageReferences $p

    Assert-AreEqual 1 $packageReferences.Count
    Assert-False $packageReferences[0].RequireReinstallation
}
