# As of now Find-Package does not suport wildcard yet. 
# TODO: Uncomment the test when the feature is implemented.
function FindPackageByIdWildcard {
    # Act
    $packages = Find-Package *aspnet*
    
    # Assert
	Assert-NotNull $packages
    Assert-True $packages.Count -gt 0 "Find-Package cmdlet does not returns any package"
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
