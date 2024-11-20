function Test-ProjectNameReturnsUniqueName {
     # Arrange
     New-SolutionFolder 'Folder1'
     $p1 = New-ClassLibrary 'ProjectA' 'Folder1'
     $p3 = New-WebApplication 'ProjectB' 'Folder1'

     $p2 = New-ConsoleApplication 'ProjectA'

     # Act
     $projectUniqueNames = @(Get-Project -All | Select-Object -ExpandProperty ProjectName | Sort-Object)

     # Assert
     Assert-True ($projectUniqueNames.Count -eq 3)
     Assert-AreEqual 'Folder1\ProjectA' $projectUniqueNames[0]
     Assert-AreEqual 'Folder1\ProjectB' $projectUniqueNames[1]
     Assert-AreEqual 'ProjectA' $projectUniqueNames[2]
}

function Test-DefaultProjectIsCorrectWhenProjectsAreAdded {
    # Act
    New-SolutionFolder 'Folder1'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1'

    # Assert
    Assert-DefaultProject $p1

    # Act
    $p2 = New-ClassLibrary 'Projecta'
    Assert-DefaultProject $p1
}

function Test-DefaultProjectIsCorrectWhenProjectsAreAddedInReverseOrder {
    # Act
    $p1 = New-ClassLibrary 'Projecta'

    # Assert
    Assert-DefaultProject $p1

    # Act
    New-SolutionFolder 'Folder1'
    $p2 = New-ClassLibrary 'ProjectA' 'Folder1'
    Assert-DefaultProject $p1
}

function Test-GetProjectThrowsIfProjectNameAmbiguous {
    # Act
    New-SolutionFolder 'foo'
    New-SolutionFolder 'bar'
    $p1 = New-ClassLibrary 'A' 'foo'
    $p2 = New-ClassLibrary 'A' 'bar'

    # Assert
    Assert-Throws { Get-Project A } "Project 'A' is not found."
    Assert-AreEqual $p1 (Get-Project foo\A)
    Assert-AreEqual $p2 (Get-Project bar\A)
}

function Test-GetProjectCommandWithWildCardsWorksWithProjectHavingTheSameName {
    #
    #  Folder1
    #     + ProjectA
    #     + ProjectB
    #  Folder2
    #     + ProjectA
    #     + ProjectC
    #  ProjectA
    #

    # Arrange
    New-SolutionFolder 'Folder1'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1'
    $p2 = New-ClassLibrary 'ProjectB' 'Folder1'

    New-SolutionFolder 'Folder2'
    $p3 = New-ClassLibrary 'ProjectA' 'Folder2'
    $p4 = New-ConsoleApplication 'ProjectC' 'Folder2'

    $p5 = New-ConsoleApplication 'ProjectA'

    # Assert
    Assert-AreEqual $p1 (Get-Project 'Folder1\ProjectA')
    Assert-AreEqual $p2 (Get-Project 'Folder1\ProjectB')
    Assert-AreEqual $p2 (Get-Project 'ProjectB')
    Assert-AreEqual $p3 (Get-Project 'Folder2\ProjectA')
    Assert-AreEqual $p4 (Get-Project 'Folder2\ProjectC')
    Assert-AreEqual $p4 (Get-Project 'ProjectC')
    Assert-AreEqual $p5 (Get-Project 'ProjectA')

    $s1 = (Get-Project 'Folder1' -ea SilentlyContinue)
    Assert-Null $s1

    $s2 = (Get-Project 'Folder2' -ea SilentlyContinue)
    Assert-Null $s2

    $fs = @( Get-Project 'Folder1*' )
    Assert-AreEqual 2 $fs.Count
    Assert-AreEqual $p1 $fs[0]
    Assert-AreEqual $p2 $fs[1]

    $gs = @( Get-Project '*ProjectA*' )
    Assert-AreEqual 3 $gs.Count
    Assert-AreEqual $p1 $gs[0]
    Assert-AreEqual $p3 $gs[1]
    Assert-AreEqual $p5 $gs[2]
}

function Test-SimpleNameDoesNotWorkWhenAllProjectsAreNested {
    # Arrange
    New-SolutionFolder 'Folder1'
    $p1 = New-ClassLibrary 'ProjectA' 'Folder1'

    New-SolutionFolder 'Folder2'
    $p2 = New-ClassLibrary 'ProjectA' 'Folder2'

    # Assert
    Assert-Throws { (Get-Project -Name 'ProjectA') } "Project 'ProjectA' is not found."
}

function Test-RemovingAmbiguousProjectAllowsSimpleNameToBeUsed {
    # Act
    New-SolutionFolder 'foo'
    $p1 = New-ClassLibrary 'A'
    $p2 = New-ClassLibrary 'A' 'foo'


    Assert-AreEqual $p2 (Get-Project -Name foo\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Remove-Project $p1.Name

    Assert-AreEqual $p2 (Get-Project -Name foo\A)
    Assert-AreEqual $p2 (Get-Project -Name A)
}

function Test-RenameCreatingAmbiguityFollowedByRemovalAllowsSimpleNameToBeUsed {
    # Act
    New-SolutionFolder 'foo'
    $p1 = New-ClassLibrary 'A'
    $p2 = New-ClassLibrary 'B' 'foo'


    Assert-AreEqual $p2 (Get-Project -Name foo\B)
    Assert-AreEqual $p1 (Get-Project -Name A)

    $p2.Name =  'A'

    Assert-AreEqual $p2 (Get-Project -Name foo\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Remove-Project $p1.Name

    Assert-AreEqual $p2 (Get-Project -Name foo\A)
    Assert-AreEqual $p2 (Get-Project -Name A)
}

function Test-RenamingSolutionFolderDoesNotAffectGetProject {
    # Act
    New-SolutionFolder 'foo'
    $p1 = New-ClassLibrary 'A'
    $p2 = New-ClassLibrary 'B' 'foo'


    Assert-AreEqual $p2 (Get-Project -Name foo\B)
    Assert-AreEqual $p1 (Get-Project -Name A)

    $p2.Name =  'A'

    Assert-AreEqual $p2 (Get-Project -Name foo\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Rename-SolutionFolder 'foo' 'bar'

    Assert-AreEqual $p2 (Get-Project -Name bar\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Remove-Project $p1.Name
    Assert-AreEqual $p2 (Get-Project -Name bar\A)
    Assert-AreEqual $p2 (Get-Project -Name A)
}

function Test-RenamingSolutionFolderWithDeeplyNestedProjectsDoesNotAffectGetProject {
    # Act
    New-SolutionFolder 'foo'
    New-SolutionFolder 'foo\bar'
    New-SolutionFolder 'foo\empty'

    $p1 = New-ClassLibrary 'A'
    $p2 = New-ClassLibrary 'B' 'foo\bar'


    Assert-AreEqual $p2 (Get-Project -Name foo\bar\B)
    Assert-AreEqual $p1 (Get-Project -Name A)

    $p2.Name =  'A'

    Assert-AreEqual $p2 (Get-Project -Name foo\bar\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Rename-SolutionFolder 'foo' 'bar2'

    Assert-AreEqual $p2 (Get-Project -Name bar2\bar\A)
    Assert-AreEqual $p1 (Get-Project -Name A)

    Remove-Project $p1.Name
    Assert-AreEqual $p2 (Get-Project -Name bar2\bar\A)
    Assert-AreEqual $p2 (Get-Project -Name A)
}

function Test-AmbiguousStartupProject {
    # Arrange
    New-SolutionFolder foo
    $p1 = New-ClassLibrary A foo
    $p2 = New-ClassLibrary A

    # Make sure the default project is p1
    Assert-DefaultProject $p1

    $solutionFile = Get-SolutionFullName
    $p1.Save()
    $p2.Save()
    SaveAs-Solution($solutionFile)
    Close-Solution

    # Re open the solution
    Open-Solution($solutionFile)
    Wait-ForSolutionLoad
    $p1 = Get-Project foo\A
    $p2 = Get-Project A

    # Make sure the default project is p1
    Assert-DefaultProject $p1
}

function Test-GetProjectAfterDefaultProjectRemoved
{
	param($context)

    # Arrange
    $p1 = New-ClassLibrary
	$p2 = New-ClassLibrary

	#Act
	Remove-Project $p1.Name

    # Assert
	Assert-DefaultProject $p2
}

function Assert-DefaultProject($p) {
    Assert-AreEqual $p (Get-Project) "Default project is actually $($p.UniqueName)"
}