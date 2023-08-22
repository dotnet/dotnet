
# Tests for Install-Package intellisense

function Test-TabExpansionForInstallPackageShowSuggestionsForPackageId {
    # Act
    $suggestions = TabExpansion 'Install-Package ' ''

    # Assert
    Assert-True ($suggestions.Count -eq 20)
}

function Test-TabExpansionForInstallPackageShowSuggestionsForPackageIdWithFilter {

    # Act
    $suggestions = @(TabExpansion 'Install-Package sql' 'sql')

    # Assert
    Assert-True ($suggestions.Count -gt 0)
    $suggestions | ForEach-Object { Assert-True $_.StartsWith('sql', 'OrdinalIgnoreCase') }
}

function Test-TabExpansionForInstallPackageSupportsVersion {
    # Act
    $suggestions = TabExpansion 'Install-Package Antlr -Version ' ''

    # Assert
    Assert-AreEqual 4 $suggestions.Count
    Assert-AreEqual '3.5.0.2' $suggestions[0]
    Assert-AreEqual '3.4.1.9004' $suggestions[1]
    Assert-AreEqual '3.1.3.42154' $suggestions[2]
    Assert-AreEqual '3.1.1' $suggestions[3]
}

function Test-TabExpansionForInstallPackageShowSuggestionsForProjectName {

    # Arrange
    $projectNames = @()

    for ($i = 0; $i -lt 3; $i++) {
        $project = New-ConsoleApplication
        $projectNames = $projectNames + $project.Name
    }

    $sortedProjectNames = $projectNames | Sort-Object

    # Act
    $suggestions = TabExpansion 'Install-Package sqlce ' ''

    # Assert
    Assert-AreEqual 3 $suggestions.count
    for ($i = 0; $i -lt 3; $i++) {
        Assert-AreEqual $sortedProjectNames[$i] $suggestions[$i]
    }
}

function Test-TabExpansionForInstallPackageSortByDownloadCountDescending {
    # Act
    $suggestions = TabExpansion 'Install-Package ef' 'ef'

    # Assert

    $packages = $suggestions | % { Get-Package -Remote -Filter $_ } | Select-Object -First 1

    $count = $packages.Count
    for ($i = 0; $i -lt $count-1; $i++) {
         Assert-True ($packages[$i].DownloadCount -ge $packages[$i+1].DownloadCount)
    }
}

# Tests for Uninstall-Package intellisense

function Test-TabExpansionForUninstallPackageShowSuggestionsForPackageId {
    # Arrange
    $project = New-ConsoleApplication
    Install-Package AntiXSS -Project $project.Name
    Install-Package elmah -Project $project.Name -Version 1.1

    # Act
    $suggestions = TabExpansion 'Uninstall-Package ' ''

    # Assert
    Assert-AreEqual 2 $suggestions.count
    Assert-AreEqual 'AntiXSS' $suggestions[0]
    Assert-AreEqual 'elmah' $suggestions[1]
}

function Test-TabExpansionForUninstallPackageShowSuggestionsForProjectNames {
    # Arrange
    $projectNames = @()

    for ($i = 0; $i -lt 3; $i++) {
        $project = New-ConsoleApplication
        $projectNames = $projectNames + $project.Name
    }

    $sortedProjectNames = $projectNames | Sort-Object

    # Act
    $suggestions = TabExpansion 'Uninstall-Package sqlce ' ''

    # Assert
    Assert-AreEqual 3 $suggestions.count
    for ($i = 0; $i -lt 3; $i++) {
        Assert-AreEqual $sortedProjectNames[$i] $suggestions[$i]
    }
}

# Tests for Update-Package intellisense

function Test-TabExpansionForUpdatePackageShowSuggestionsForPackageId {
    # Arrange
    $project = New-ConsoleApplication
    Install-Package 'NHibernate' -Project $project.Name -Version '2.1.2.4000'

    # Act
    $suggestions = TabExpansion 'Update-Package ' ''
    $nhibernateUpdates = @($suggestions | Where-Object { 'NHibernate' -eq $_ })

    # Assert
    Assert-True ($nhibernateUpdates.count -ge 1)
}

function Test-TabExpansionForUpdatePackageShowSuggestionsForPackageIdEvenIfNoUpdatesAvailable {
    # Arrange
    $project = New-ConsoleApplication
    Install-Package jQuery -Project $project.Name

    # Act
    $suggestions = @(TabExpansion 'Update-Package ' '')

    # Assert
    Assert-NotNull $suggestions
    Assert-True ($suggestions.count -ge 1)
}

function Test-TabExpansionForUpdatePackageShowSuggestionsForCustomSource {
    param(
        $context
    )
    # Arrange
    $project = New-ConsoleApplication
    Install-Package jQuery -Project $project.Name -Source $context.RepositoryPath

    # Act
    $suggestions = @(TabExpansion "Update-Package -Source $($context.RepositoryPath) " '')

    # Assert
    Assert-NotNull $suggestions
    Assert-True ($suggestions.count -ge 1)
}

function Test-TabExpansionForUpdatePackageWithVersionAndNoId {
    # Arrange
    $project = New-ConsoleApplication
    Install-Package 'NHibernate' -Project $project.Name -Version '2.1.2.4000'

    # Act
    $suggestions = TabExpansion 'Update-Package -Version ' ''

    # Assert
    Assert-Null $suggestions
}

function Test-TabExpansionForUpdatePackageWithVersionOnlyShowsVersionsHigherThanInstalled {
    # Arrange
    $p1 = New-ConsoleApplication
    $p2 = New-ConsoleApplication
    $p1 | Install-Package NHibernate -Version 3.0.0.4000
    $p2 | Install-Package NHibernate -Version 3.1.0.4000
    $installedVersion = New-Object NuGet.Versioning.NuGetVersion("3.1.0.4000")

    # Act
    $suggestions = TabExpansion 'Update-Package NHibernate -Version ' ''
    $versions = $suggestions | %{ [NuGet.Versioning.NuGetVersion]::Parse($_) }

    # Assert
    $versions[0] | %{ Assert-True ($_ -gt $installedVersion) }
}

function Test-TabExpansionForUpdatePackageShowSuggestionsForProjectNames {
    # Arrange
    $projectNames = @()

    for ($i = 0; $i -lt 3; $i++) {
        $project = New-ConsoleApplication
        $projectNames = $projectNames + $project.Name
    }

    $sortedProjectNames = $projectNames | Sort-Object

    # Act
    $suggestions = TabExpansion 'Update-Package sqlce ' ''

    # Assert
    Assert-AreEqual 3 $suggestions.count
    for ($i = 0; $i -lt 3; $i++) {
        Assert-AreEqual $sortedProjectNames[$i] $suggestions[$i]
    }
}

# Tests to make sure private functions & cmdlets do not show up in the intellisense

function Test-TabExpansionDoNotSuggestFindPackage() {

    # Act
    $suggestions = TabExpansion 'Find-Pac' 'Find-Pac'

    # Assert
    Assert-Null $suggestions
}

function Test-TabExpansionDoNotSuggestGetProjectName() {

    # Act
    $suggestions = TabExpansion 'GetProjectN' 'GetProjectN'

    # Assert
    Assert-Null $suggestions
}

function Test-CustomTabExpansion {
    # Arrange
    function global:Foo($Name) {
        "Hello $Name"
    }

    Register-TabExpansion Foo @{ 'Name' = { 'David Fowler', 'John Doe', "John's Hide Out", "Woah's", "A`tB", "G" | Sort-Object } }

    # Act
    $suggestions = TabExpansion 'Foo ' ''

    # Assert
    Assert-NotNull $suggestions
    Assert-AreEqual 6 $suggestions.Count
    Assert-AreEqual "'A`tB'" $suggestions[0]
    Assert-AreEqual "'David Fowler'" $suggestions[1]
    Assert-AreEqual "G" $suggestions[2]
    Assert-AreEqual "'John Doe'" $suggestions[3]
    Assert-AreEqual "'John''s Hide Out'" $suggestions[4]
    Assert-AreEqual "'Woah''s'" $suggestions[5]

    # Remove the function from global scope
    rm function:\Foo
}


function Test-ComplexCustomTabExpansion {
    # Arrange
    function global:Foo($Name, $Age) {
    }

    $ages = @{
        'David''s Sister''s Brother''s Age' = 10
        'Phil' = 11
        'David''s Dog' = 12
        'John Doe' = 14
    }

    Register-TabExpansion Foo @{
        'Name' = { $ages.Keys }
        'Age' = { param($context) $ages[$context.Name] }
    }

    # Act
    $philAge = TabExpansion "Foo -Name Phil -Age " ""
    $johnDoeAge = TabExpansion "Foo -Name 'John Doe' -Age " ""
    $dogAge = TabExpansion "Foo -Name 'David''s Dog' -Age " ""
    $davidAge = TabExpansion "Foo 'David''s Sister''s Brother''s Age' -Age " ""
    $davidAgeQuotes = TabExpansion "Foo `"David's Sister's Brother's Age`" -Age " ""

    Assert-AreEqual 11 $philAge
    Assert-AreEqual 14 $johnDoeAge
    Assert-AreEqual 12 $dogAge
    Assert-AreEqual 10 $davidAge
    Assert-AreEqual 10 $davidAgeQuotes

    # Remove the function from global scope
    rm function:\Foo
}

function Test-TabExpansionForVersionForUninstallPackage {
    # Arrange
    $p = New-WebApplication
    $p | Install-Package elmah -Version 1.1

    # Act
    $suggestion = TabExpansion "Uninstall-Package elmah -Version "

    # Assert
    $expectedVersion = New-Object NuGet.Versioning.NuGetVersion("1.1.0")
    $actualVersion = New-Object NuGet.Versioning.NuGetVersion($suggestion)

    Assert-AreEqual $expectedVersion $actualVersion
}

function Test-TabExpansionForProjectsReturnsBothUniqueNamesAndSafeNames {
    # Arrange
    New-SolutionFolder 'Folder1'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1'
    $p3 = New-WebApplication 'ProjectB' 'Folder1'

    $p2 = New-ConsoleApplication 'ProjectA'

    # Act
    $suggestions = TabExpansion 'Get-Project -name '

    # Assert
    Assert-AreEqual 4 $suggestions.Count

    Assert-AreEqual 'Folder1\ProjectA' $suggestions[0]
    Assert-AreEqual 'Folder1\ProjectB'$suggestions[1]
    Assert-AreEqual 'ProjectA' $suggestions[2]
    Assert-AreEqual 'ProjectB' $suggestions[3]

}

function Test-TabExpansionWorksWithOneProject {
    # Arrange
    $f = New-ClassLibrary 'ProjectA'

    # Act
    $suggestion = @(TabExpansion 'Get-Project -Name ')

    # Assert
    Assert-AreEqual 1 $suggestion.Count
    Assert-AreEqual 'ProjectA' $suggestion[0]
}

function Test-GetPackageCommandShowTabExpansionForSourceParameter {
    # Act
    $suggestions = @(TabExpansion 'Get-Package -Source ')

    # Assert
    Assert-True ($suggestions.Count -gt 0)
}

function Test-InstallPackageCommandShowTabExpansionForSourceParameter {
    # Act
    $suggestions = @(TabExpansion 'Install-Package -Source ')

    # Assert
    Assert-True ($suggestions.Count -gt 0)
}

function Test-UpdatePackageCommandShowTabExpansionForSourceParameter {
    # Act
    $suggestions = @(TabExpansion 'Update-Package -Source ')

    # Assert
    Assert-True ($suggestions.Count -gt 0)
}

function Test-GetPackageCommandShowTabExpansioinForProjectNameParameter {
    # Arrange
    $p1 = New-ClassLibrary "Project1"
    $p2 = New-ClassLibrary "Project2"

    # Act
    $suggestions = @(TabExpansion 'Get-Package -ProjectName ')

    # Assert
    Assert-AreEqual 2 $suggestions.Count

    Assert-AreEqual "Project1" $suggestions[0]
    Assert-AreEqual "Project2" $suggestions[1]
}

function Test-InstallPackageCommandShowTabExpansionForPreReleasePackagesVersions {
    param(
        $context
    )

    # Act
    $suggestions = @(TabExpansion "Install-Package PreReleaseTestPackage -Source '$($context.RepositoryRoot)' -IncludePreRelease -Version ")

    # Assert
    Assert-AreEqual 4 $suggestions.Count

    Assert-AreEqual '1.0.1-a' $suggestions[0]
    Assert-AreEqual '1.0.0' $suggestions[1]
    Assert-AreEqual '1.0.0-b' $suggestions[2]
    Assert-AreEqual '1.0.0-a' $suggestions[3]
}

function Test-UpdatePackageCommandShowTabExpansionForPreReleasePackagesVersions {
    param(
        $context
    )

    # Arrange
    $p = New-ClassLibrary
    Install-Package PreReleaseTestPackage -PreRelease -Version 1.0.0-b -Source $context.RepositoryRoot -ProjectName $p.Name

    # Act
    $suggestions = @(TabExpansion "Update-Package PreReleaseTestPackage -Source '$($context.RepositoryRoot)' -ProjectName '$($p.Name)' -IncludePreRelease -Version ")

    # Assert
    Assert-AreEqual 2 $suggestions.Count

    Assert-AreEqual '1.0.1-a' $suggestions[0]
    Assert-AreEqual '1.0.0' $suggestions[1]
}

function Test-InstallPackageCommandShowTabExpansionForPreReleasePackagesIfPreReleaseFlagIsSet {
    param(
        $context
    )

    # Act
    $suggestions = @(TabExpansion "Install-Package -Source '$($context.RepositoryRoot)\TabExpansionForPrereleasePackages' -PreRelease -Id ")

    # Assert
    Assert-AreEqual 1 $suggestions.Count

    Assert-AreEqual 'MyPackage' $suggestions[0]
}

function Test-InstallPackageCommandShowTabExpansionForDependencyVersion {
    # Act
    $suggestions = @(TabExpansion 'Install-Package -DependencyVersion ')

    # Assert
    Assert-AreEqual 5 $suggestions.Count
    Assert-AreEqual 'Highest' $suggestions[0]
	Assert-AreEqual 'HighestMinor' $suggestions[1]
	Assert-AreEqual 'HighestPatch' $suggestions[2]
	Assert-AreEqual 'Ignore' $suggestions[3]
	Assert-AreEqual 'Lowest' $suggestions[4]
}

function Test-InstallPackageCommandShowTabExpansionForFileConflictAction {
    # Act
    $suggestions = @(TabExpansion 'Install-Package -FileConflictAction ')

    # Assert
    Assert-AreEqual 5 $suggestions.Count
    Assert-AreEqual 'Ignore' $suggestions[0]
    Assert-AreEqual 'IgnoreAll' $suggestions[1]
    Assert-AreEqual 'Overwrite' $suggestions[2]
    Assert-AreEqual 'OverwriteAll' $suggestions[3]
    Assert-AreEqual 'PromptUser' $suggestions[4]
}

function Test-UpdatePackageCommandShowTabExpansionForFileConflictAction {
    # Act
    $suggestions = @(TabExpansion 'Update-Package -FileConflictAction ')

    # Assert
    Assert-AreEqual 5 $suggestions.Count
    Assert-AreEqual 'Ignore' $suggestions[0]
    Assert-AreEqual 'IgnoreAll' $suggestions[1]
    Assert-AreEqual 'Overwrite' $suggestions[2]
    Assert-AreEqual 'OverwriteAll' $suggestions[3]
    Assert-AreEqual 'PromptUser' $suggestions[4]
}

