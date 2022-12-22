# Currently, using a fixed list of most downloaded packages as of 02/25/2015. Consider changing this logic to always pull the latest packages and operate on that

$topDownloadedPackageIds = (
    'Newtonsoft.Json', # Lib files
    'jQuery', # content files, PS scripts (install.ps1 and uninstall.ps1)
    'EntityFramework', # Web.config transforms, PS scripts (init.ps1 and install.ps1)
    'Microsoft.AspNet.Mvc', # Deep dependency graph
    'Microsoft.AspNet.WebPages',
    'Microsoft.AspNet.Razor',
    'Microsoft.AspNet.WebApi.Client',
    'Microsoft.AspNet.WebApi.Core',
    'Microsoft.AspNet.WebApi.WebHost',
    'Microsoft.AspNet.WebApi',
    'Microsoft.AspNet.Web.Optimization',
    'Microsoft.Net.Http', # Framework references, PCL Target framework folders
    'WebGrease',
    'jQuery.Validation',
    'jQuery.UI.Combined',
    'Microsoft.jQuery.Unobtrusive.Validation',
    'Microsoft.Data.Edm',
    'Microsoft.Data.OData',
    'System.Spatial',
    'Microsoft.Web.Infrastructure',
    'Modernizr',
    'Microsoft.Bcl.Build', # .Targets files
    'Microsoft.Bcl',
    'Moq',
    'Antlr', # No target framework specific folders
    'Microsoft.Owin',
    'bootstrap',
    'Microsoft.jQuery.Unobtrusive.Ajax',
    'Microsoft.Owin.Host.SystemWeb',
    'Microsoft.Owin.Security',
    'NuGet.CommandLine',
    'WindowsAzure.Storage', # Reference elements in nuspec. And, deep dependency graph
    'NUnit',
    'Microsoft.AspNet.WebApi.OData',
    'log4net',
    'knockoutjs',
    'Microsoft.WindowsAzure.ConfigurationManager',
    'Owin',
    'AutoMapper',
    'NuGet.Build',
    'Microsoft.AspNet.Identity.Core',
    'Microsoft.Owin.Security.OAuth',
    'Microsoft.Owin.Security.Cookies',
    'Respond',
    'Unity',
    'Microsoft.AspNet.WebPages.WebData',
    'Microsoft.Data.Services.Client',
    'Microsoft.AspNet.Identity.Owin',
    'Microsoft.AspNet.Mvc.FixedDisplayModes',
    'Microsoft.AspNet.WebPages.Data',
    'angularjs',
    'nlog',
    'microsoft.aspnet.signalr'
    )

$projectSystemNames = (
    'New-MvcApplication',
    'New-ClassLibrary'
    #'New-JavaScriptWindowsPhoneApp81', # At least 35 out of the 53 packages in the list cannot be installed on this project type
    #'New-JavaScriptApplication' # At least 35 out of the 53 packages in the list cannot be installed on this project type
    )

function DisabledTest-InstallPackageOnProjectSystem
{
    param(
        $context,
        $testCaseObject
    )

    Write-Host 'Starting test...'
    Write-Host $testCaseObject
    if(!$testCaseObject)
    {
        return
    }

    Write-Host 'Started test...'

    $projectSystemName = $testCaseObject.ProjectSystemName
    $packageId = $testCaseObject.PackageId

    $project = &$projectSystemName
    Write-Host 'Installing ' $packageId ' on ' $projectSystemName '...'

	# Latest prerelease versions of Microsoft.AspNet.Mvc and EntityFramework package does not support .NET45 projects.
	# Install latest stable versions instead.
    if (($packageId -eq 'Microsoft.AspNet.Mvc') -or ($packageId -eq 'EntityFramework'))
	{
		Install-Package $packageId -ProjectName $project.Name
	}
	else
	{
	    Install-Package $packageId -Prerelease -ProjectName $project.Name
	}

    Assert-Package $project $packageId
}

function DisabledTestCases-InstallPackageOnProjectSystem {
    $testCases = @()
    # Act
    foreach($projectSystemName in $projectSystemNames)
    {
        foreach($packageId in $topDownloadedPackageIds)
        {
            $testCaseValues = @{
                ProjectSystemName = $projectSystemName
                PackageId = $packageId
            }

            $testCase = New-Object PSObject -Property $testCaseValues
            $testCases += $testCase
        }
    }

    return $testCases
}