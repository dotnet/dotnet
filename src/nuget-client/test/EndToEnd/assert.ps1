# Assert functions
function Build-ErrorMessage {
    param(
        [parameter(Mandatory = $true)]
        [string]$BaseMessage,
        [string]$Message
    )

    if($Message) {
        $BaseMessage += ". $Message"
    }

    $BaseMessage
}

function Get-AssertError {
    param(
        [parameter(Mandatory = $true)]
        [string]$BaseMessage,
        [string]$Message
    )

    $Message = Build-ErrorMessage $BaseMessage $Message

    # Get the last non assert call
    $lastCall = Get-PSCallStack | Select -Skip 1 | ?{ !$_.Command.StartsWith('Assert') } | Select -First 1

    "$Message. At $($lastCall.Location)"
}

function Assert-Fail {
    param(
        [parameter(Mandatory = $true)]
        [string]$Message
    )

    Write-Error (Get-AssertError "Failed" $Message)
}

function Assert-True {
    param(
        $Value,
        [string]$Message
    )

    if($Value -eq $false) {
        Write-Error (Get-AssertError "Value is not true" $Message)
    }
}

function Assert-False {
    param(
        $Value,
        [string]$Message
    )

    if($Value -eq $true) {
        Write-Error (Get-AssertError "Value is not false" $Message)
    }
}

function Assert-NotNull {
    param(
        $Value,
        [string]$Message
    )

    if(!$Value) {
        Write-Error (Get-AssertError "Value is null" $Message)
    }
}

function Assert-Null {
    param(
        $Value,
        [string]$Message
    )

    if($Value) {
        Write-Error (Get-AssertError "Value is not null" $Message)
    }
}

function Assert-AreEqual {
    param(
         [parameter(Mandatory = $true)]
         $Expected,
         [parameter(Mandatory = $true)]
         $Actual,
         [string]$Message
    )

    if($Expected -ne $Actual) {
        Write-Error (Get-AssertError "Expected <$Expected> but got <$Actual>" $Message)
    }
}

function Assert-NotEqual {
    param(
         [parameter(Mandatory = $true)]
         $Expected,
         [parameter(Mandatory = $true)]
         $Actual,
         [string]$Message
    )

    if($Expected -eq $Actual) {
        Write-Error (Get-AssertError "Expected anything but <$Expected> but got <$Actual>" $Message)
    }
}

function Assert-Contains {
    param(
        [parameter(Mandatory = $true)]
        [string]$Expected,
        [parameter(Mandatory = $true)]
        [string]$Actual,
        [string]$Message
    )

    if($Actual -notmatch $Expected) {
        Write-Error (Get-AssertError "Expected <$Expected> but got <$Actual>" $Message)
    }
}


function Assert-PathExists {
    param(
          [parameter(Mandatory = $true)]
          [string]$Path,
          [string]$Message
    )

    if(!(Test-Path $Path)) {
        Write-Error (Get-AssertError "The path `"$Path`" does not exist" $Message)
    }
}

function Assert-PathNotExists {
    param(
          [parameter(Mandatory = $true)]
          [string]$Path,
          [string]$Message
    )

    if((Test-Path $Path)) {
        Write-Error (Get-AssertError "The path `"$Path`" DOES exist" $Message)
    }
}

function Assert-Reference {
    param(
         [parameter(Mandatory = $true)]
         $Project,
         [parameter(Mandatory = $true)]
         [string]$Reference,
         [string]$Version
    )

    $assemblyReference = Get-AssemblyReference $Project $Reference
    Assert-NotNull $assemblyReference "Reference `"$Reference`" does not exist"

    $path = $assemblyReference.Path

    # Support for websites
    if(!$path) {
        $path = $assemblyReference.FullPath
    }

    Assert-NotNull $path "Reference `"$Reference`" exists but is broken"
    Assert-PathExists $path "Reference `"$Reference`" exists but is broken"

    if($Version) {
        $assemblyVersion = $assemblyReference.Version
        if(!$assemblyVersion) {
            $assemblyVersion = [System.Reflection.AssemblyName]::GetAssemblyName($path).Version
        }

        $actualVersion = [Version]::Parse($Version)
        Assert-AreEqual $actualVersion $assemblyVersion
    }
}

function Assert-Throws {
    param(
        [parameter(Mandatory = $true)]
        [scriptblock]$Action,
        [parameter(Mandatory = $true)]
        [string]$ExceptionMessage
    )

    $exceptionThrown = $false

    try {
        & $Action
    }
    catch {
       Assert-AreEqual $ExceptionMessage $_.Exception.Message
       $exceptionThrown = $true
    }

    if(!$exceptionThrown) {
        Write-Error (Get-AssertError "Expected exception was not thrown")
    }
}

function Assert-BindingRedirect {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        $ConfigPath,
        [parameter(Mandatory = $true)]
        $Name,
        [parameter(Mandatory = $true)]
        $OldVersion,
        [parameter(Mandatory = $true)]
        $NewVersion
    )

    $config = [xml](Get-Content (Get-ProjectItemPath $Project $ConfigPath))
    Assert-NotNull $config.configuration.runtime
    Assert-NotNull $config.configuration.runtime.assemblyBinding
    Assert-NotNull $config.configuration.runtime.assemblyBinding.dependentAssembly
    $bindings = @($config.configuration.runtime.assemblyBinding.dependentAssembly | ?{ $_.assemblyIdentity.name -eq $Name -and
                                                                                    $_.bindingRedirect.oldVersion -eq $OldVersion -and
                                                                                    $_.bindingRedirect.newVersion -eq $NewVersion })

    Assert-True ($bindings.Count -eq 1) "Unable to find binding redirect matching $Name, $OldVersion, $NewVersion"
}

function Assert-NoBindingRedirect {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        $ConfigPath,
        [parameter(Mandatory = $true)]
        $Name,
        [parameter(Mandatory = $true)]
        $OldVersion,
        [parameter(Mandatory = $true)]
        $NewVersion
    )
    $itemPath = (Get-ProjectItemPath $Project $ConfigPath)
	if (!$itemPath) {
		return
	}

    $config = [xml](Get-Content $itemPath)

    if (!$config.configuration.runtime -or
        !$config.configuration.runtime.assemblyBinding -or
        !$config.configuration.runtime.assemblyBinding.dependentAssembly) {
        return
    }

    $bindings = @($config.configuration.runtime.assemblyBinding.dependentAssembly | ?{ $_.assemblyIdentity.name -eq $Name -and
                                                                                    $_.bindingRedirect.oldVersion -eq $OldVersion -and
                                                                                    $_.bindingRedirect.newVersion -eq $NewVersion })

    Assert-True ($bindings.Count -eq 0) "Binding redirect matching $Name, $OldVersion, $NewVersion found in project $($Project.Name)"
}

function Assert-StringEqual {
    param(
        [parameter(Mandatory = $true)]
        $StringA,
        [parameter(Mandatory = $true)]
        $StringB,
        [parameter(Mandatory = $true)]
        $CaseSensitive
    )

	$comparison = if ($CaseSensitive) { [StringComparison]::Ordinal } else { [StringComparison]::OrdinalIgnoreCase }
	$areEqual = [string]::Equals($stringA, $stringB, $comparison)

	Assert-True ($areEqual) "Strings `"$stringA`" and `"$stringB`" are not equal."
}