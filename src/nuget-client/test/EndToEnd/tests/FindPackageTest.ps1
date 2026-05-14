function Test-FindPackageByIdjQuery {
    # Act
    $packages = Find-Package jQuery
    
    # Assert
    Assert-True $packages.Count -gt 0 "Find-Package cmdlet does not returns any package"
}

function Test-FindPackageByIdMVC {
    # Act
    $packages = Find-Package microsoft.aspnet.mvc
    
    # Assert
    Assert-True $packages.Count -gt 0 "Find-Package cmdlet does not returns any package"
}

function Test-FindPackageByIdaspnet {
    # Act
    $packages = Find-Package aspnet
    
    # Assert
    Assert-True $packages.Count -gt 0 "Find-Package cmdlet does not returns any package"
}

# As of now Find-Package does not suport wildcard yet. 
# TODO: Uncomment the test when the feature is implemented.
function FindPackageByIdWildcard {
    # Act
    $packages = Find-Package *aspnet*
    
    # Assert
	Assert-NotNull $packages
    Assert-True $packages.Count -gt 0 "Find-Package cmdlet does not returns any package"
}

function Test-FindPackageByIdAndVersion {
    # Act
    $packages = Find-Package entityframework -version 6.1.2
    $version = [NuGet.Versioning.NuGetVersion]::Parse("6.1.2")
    
    # Assert
	Assert-True $packages[0].Versions[0] -eq $version
    Assert-True $packages[0].Id -eq "EntityFramework"
}

function Test-FindPackageByIdAndPrereleaseVersion {
    [SkipTest('https://github.com/NuGet/Home/issues/8496')]
    param()

    # Act 1
    $packages = Find-Package TestPackage.AlwaysPrerelease
    
    # Assert 1
	Assert-Null $packages

	# Act 2
    $packages = Find-Package TestPackage.AlwaysPrerelease -Pre
    
    # Assert 2
	Assert-True $packages[0].Count -ne 0
    Assert-True $packages[0].Id -eq TestPackage.AlwaysPrerelease
	Assert-True $packages[0].Versions[0].ToString() -eq "5.0.0-beta"
}

function Test-FindPackageByIdExactMatch {
    [SkipTest('https://github.com/NuGet/Home/issues/10066')]
    param()

   # Act 
    $packages = Find-Package TestPackage.OverwriteTest -ExactMatch
    
    # Assert 
	Assert-True $packages[0].Count -eq 1
    Assert-True $packages[0].Id -eq TestPackage.OverwriteTest
	Assert-True $packages[0].Versions[0].ToString() -eq "1.0.0"
}

function Test-FindPackageByIdWithAllVersions {
    # Act 
    $packages = Find-Package elmah.io -allversions
    
    # Assert 
	Assert-True $packages[0].Count -gt 1
    Assert-True $packages[0].Id -eq elmah.io
	Assert-True $packages[0].Versions.Count -gt 4
}

function Test-FindPackageByIdWithFirstAndSkip {
    [SkipTest('https://github.com/NuGet/Home/issues/8496')]
    param()

    # Act 1
    $packages = Find-Package elmah -First 5
    
    # Assert 1
	Assert-True $packages[0].Count -eq 5

	# Testpackage.MinclientVersion is owned by us and only 1 version is uploaded.
	# We will just keep one version in the gallery for testing minclientversion.
	# Act 2
    $packages = Find-Package Testpackage.MinclientVersion

    # Assert 2
	Assert-True $packages[0].Count -eq 1

	# Act 3
    $packages = Find-Package Testpackage.MinclientVersion -skip 1

    # Assert 3
	Assert-Null $packages

	# Act 4
    $packages = Find-Package elmah -First 5 -Skip 45
    
    # Assert 4
	Assert-True $packages[0].Count -eq 5
}
