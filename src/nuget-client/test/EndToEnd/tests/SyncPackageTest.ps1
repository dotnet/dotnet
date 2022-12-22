
# Test sync-package basic scenario - moving up version
function Test-SyncPackagesInSolutionUp {
    param(
        $context
    )

    # Arrange
    $p1 = New-WebApplication
    $p2 = New-ClassLibrary
    
    # Act
    $p1 | Install-Package A -Version 1.0.0 -Source $context.RepositoryPath
    $p2 | Install-Package A -Version 2.0.0 -Source $context.RepositoryPath
    
    Assert-Package $p1 A 1.0.0
    Assert-Package $p1 B 1.0.0
    Assert-Package $p2 A 2.0.0
    Assert-Package $p2 B 2.0.0

    Sync-Package -ProjectName $p2.Name -Id A -Source $context.RepositoryPath

    # Assert
    Assert-Package $p1 A 2.0.0
    Assert-Package $p1 B 2.0.0
    Assert-Package $p2 A 2.0.0
    Assert-Package $p2 B 2.0.0
    Assert-Null (Get-ProjectPackage $p1 A 1.0.0)
    Assert-Null (Get-ProjectPackage $p1 B 1.0.0)
}

# Test sync-package basic scenario - moving down version
function Test-SyncPackagesInSolutionDown {
    param(
        $context
    )

    # Arrange
    $p1 = New-WebApplication
    $p2 = New-ClassLibrary
    
    # Act
    $p1 | Install-Package A -Version 1.0.0 -Source $context.RepositoryPath
    $p2 | Install-Package A -Version 2.0.0 -Source $context.RepositoryPath
    
    Assert-Package $p1 A 1.0.0
    Assert-Package $p1 B 1.0.0
    Assert-Package $p2 A 2.0.0
    Assert-Package $p2 B 2.0.0

    Sync-Package -ProjectName $p1.Name -Id A -Source $context.RepositoryPath

    # Assert
    Assert-Package $p1 A 1.0.0
    Assert-Package $p1 B 1.0.0
    Assert-Package $p2 A 1.0.0
    Assert-Package $p2 B 2.0.0
    Assert-Null (Get-ProjectPackage $p1 A 2.0.0)
}

# Test sync-package basic scenario - plurality of project
function Test-SyncPackagesInSolutionPlural {
    param(
        $context
    )

    # Arrange
    $p1 = New-WebApplication
    $p2 = New-ClassLibrary
    $p3 = New-ClassLibrary
    $p4 = New-ClassLibrary
    $p5 = New-ClassLibrary
    
    # Act
    $p1 | Install-Package A -Version 1.0.0 -Source $context.RepositoryPath
    $p2 | Install-Package A -Version 2.0.0 -Source $context.RepositoryPath
    $p3 | Install-Package A -Version 3.0.0 -Source $context.RepositoryPath
    $p4 | Install-Package A -Version 4.0.0 -Source $context.RepositoryPath
    $p5 | Install-Package A -Version 5.0.0 -Source $context.RepositoryPath
    
    Assert-Package $p1 A 1.0.0
    Assert-Package $p2 A 2.0.0
    Assert-Package $p3 A 3.0.0
    Assert-Package $p4 A 4.0.0
    Assert-Package $p5 A 5.0.0

    Sync-Package -ProjectName $p3.Name -Id A -Source $context.RepositoryPath

    # Assert
    Assert-Package $p1 A 3.0.0
    Assert-Package $p2 A 3.0.0
    Assert-Package $p3 A 3.0.0
    Assert-Package $p4 A 3.0.0
    Assert-Package $p5 A 3.0.0

    Assert-Null (Get-ProjectPackage $p1 A 1.0.0)
    Assert-Null (Get-ProjectPackage $p2 A 2.0.0)
    Assert-Null (Get-ProjectPackage $p4 A 4.0.0)
    Assert-Null (Get-ProjectPackage $p5 A 5.0.0)
}
