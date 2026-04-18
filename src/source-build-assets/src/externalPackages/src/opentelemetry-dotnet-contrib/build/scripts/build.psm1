function ResolveProjectForTag {
  param(
    [Parameter()][string]$tag,
    [Parameter()][ref]$title,
    [Parameter()][ref]$project,
    [Parameter()][ref]$component
  )

  # Scheduled builds. No tag, build solution.
  if ([string]::IsNullOrEmpty($tag))
  {
      $title.value = "opentelemetry-dotnet-contrib.proj"
      $project.value = "opentelemetry-dotnet-contrib.proj"
      Return
  }

  $match = [regex]::Match($tag, '^(.*?-)(.*)$')
  if ($match.Success -eq $false)
  {
      throw 'Could not parse prefix or version from tag'
  }

  $tagPrefix = $match.Groups[1].Value
  $version = $match.Groups[2].Value

  # Step 1: Look for a .proj file in build/Projects with a matching MinVerTagPrefix
  $buildProjects = @(Get-ChildItem -Path build/Projects/*.proj | Select-String "<MinVerTagPrefix>$tagPrefix</MinVerTagPrefix>" -List | Select Path)

  if ($buildProjects.Length -gt 1)
  {
      throw 'Multiple build project files found matching tag prefix'
  }
  elseif ($buildProjects.Length -eq 1)
  {
    $buildProject = [System.IO.Path]::GetFileNameWithoutExtension($buildProjects[0].Path)

    $title.value = "$buildProject.proj"
    $project.value = "./build/Projects/$buildProject.proj"
    Return
  }

  # Step 2: If no .proj file found use component build for the csproj found matching MinVerTagPrefix
  $projects = @(Get-ChildItem -Path src/**/*.csproj | Select-String "<MinVerTagPrefix>$tagPrefix</MinVerTagPrefix>" -List | Select Path)

  if ($projects.Length -gt 1)
  {
      throw 'Multiple project files found matching tag prefix'
  }
  elseif ($projects.Length -ne 1)
  {
      throw 'No project file found matching tag prefix'
  }

  $component.value = [System.IO.Path]::GetFileNameWithoutExtension($projects[0].Path)
  $title.value = "Component.proj for $($component.value)"
  $project.value = "./build/Projects/Component.proj"
}

Export-ModuleMember -Function ResolveProjectForTag

function ResolveProject {
  param(
    [Parameter(Mandatory=$true)][string]$projectNameOrComponentData,
    [Parameter()][ref]$title,
    [Parameter()][ref]$project,
    [Parameter()][ref]$component
  )

  $match = [regex]::Match($projectNameOrComponentData, '^Component\[(.*)\]$')
  if ($match.Success -eq $false)
  {
    $title.value = "$projectNameOrComponentData.proj"
    $project.value = "./build/Projects/$projectNameOrComponentData.proj"
    Return
  }

  $component.value = $match.Groups[1].Value
  $title.value = "Component.proj for $($component.value)"
  $project.value = "./build/Projects/Component.proj"
}

Export-ModuleMember -Function ResolveProject

function FindComponentOwners {
  param(
    [Parameter(Mandatory=$true)][string]$component,
    [Parameter()][string]$issueNumber,
    [Parameter()][ref]$componentOwners
  )

  $projectPath = "src/$component/$component.csproj"

  if ((Test-Path -Path $projectPath) -eq $false)
  {
      if ([string]::IsNullOrEmpty($issueNumber) -eq $false)
      {
        gh issue comment $issueNumber `
          --body "I couldn't find the project file for the requested component. Please create a new release request and select a valid component or edit the description and set a valid component."
      }
      Return
  }

  $projectContent = Get-Content -Path $projectPath

  $match = [regex]::Match($projectContent, '<MinVerTagPrefix>(.*)<\/MinVerTagPrefix>')
  if ($match.Success -eq $false)
  {
      if ([string]::IsNullOrEmpty($issueNumber) -eq $false)
      {
        gh issue comment $issueNumber `
          --body "I couldn't find ``MinVerTagPrefix`` in the project file for the requested component. Please create a new release request and select a valid component or edit the description and set a valid component."
      }
      Return
  }

  $minVerTagPrefix = $match.Groups[1].Value

  $projectDirs = Get-ChildItem -Path src/**/*.csproj | Select-String "<MinVerTagPrefix>$minVerTagPrefix</MinVerTagPrefix>" -List | Select Path | Split-Path -Parent

  $componentOwnersContent = Get-Content '.github/component_owners.yml' -Raw

  $componentOwners.Value = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

  foreach ($projectDir in $projectDirs)
  {
    $projectName = [System.IO.Path]::GetFileName($projectDir)

    $match = [regex]::Match($componentOwnersContent, "src\/$projectName\/:([\w\W\s]*?)(src|test)")
    if ($match.Success -eq $true)
    {
      $matches = [regex]::Matches($match.Groups[1].Value, "-\s*(.*)")
      foreach ($match in $matches)
      {
        $owner = $match.Groups[1].Value
        $_ = $componentOwners.Value.Add($owner.Trim())
      }
    }
  }
}

Export-ModuleMember -Function FindComponentOwners
