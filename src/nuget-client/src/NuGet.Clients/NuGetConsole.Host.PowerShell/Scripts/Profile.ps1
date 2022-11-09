#
# PowerConsole profile
#

<#
.SYNOPSIS
    Clear the host content.
    
.DESCRIPTION
    This function replaces the standard Clear-Host and is aliased by "cls".
#>
function Clear-Host
{
    $host.PrivateData.ClearHost()
}

<#
.SYNOPSIS
    Simple path completion function for PowerConsole.
#>
function _TabExpansionPath($line)
{
    function UnquoteString($s) {
        if ($s.StartsWith('"') -or $s.StartsWith("'")) {
            $s = $s.Substring(1)
        }
        if ($s.EndsWith('"') -or $s.EndsWith("'")) {
            $s = $s.Substring(0, $s.Length - 1)
        }
        return $s
    }
    
    $e = $null
    $tokens = @([System.Management.Automation.PSParser]::Tokenize($line, [ref]$e))
    $lastToken = $tokens | Select-Object -Last 1
    
    $replaceStart = -1
    $lastWord = $null
    
    if ($lastToken -and ($lastToken.EndColumn -gt $line.Length)) {
        #Last token is terminated
                        
        if ($tokens.Length -gt 1) {
            $prevToken = $tokens[$tokens.Length - 2]
            if ($prevToken.EndColumn -eq $lastToken.StartColumn) {
                $replaceStart = $prevToken.StartColumn - 1
                $lastWord = (UnquoteString $prevToken.Content) + (UnquoteString $lastToken.Content)
            }
        }
                                
        if ($replaceStart -lt 0) {
            $replaceStart = $lastToken.StartColumn - 1
            $lastWord = UnquoteString $lastToken.Content
        }
    } else {
        #There is unrecognized/unterminated words
    
        if(!$lastToken) {
            $lastWord = $line
        } else {
            $lastWord = $line.Substring($lastToken.EndColumn - 1).TrimStart()
        }
        $replaceStart = $line.Length - $lastWord.Length
        $lastWord = UnquoteString $lastWord
    }

    # If previously unquoted, put back quote in results
    $unquoted = ($replaceStart -lt ($line.Length - $lastWord.Length))    
    $relative = !(($lastWord.IndexOf(':') -ge 0) -or $lastWord.StartsWith('/') -or $lastWord.StartsWith('\'))    

    $result = "" | select ReplaceStart, Paths
    $result.ReplaceStart = $replaceStart
    $result.Paths = @(Resolve-Path ${lastWord}* -ErrorAction SilentlyContinue -Relative:$relative | %{
    
        # Resolve-Path may return PathInfo or String (e.g. when passing different -Relative)
        $path = $_.ToString()
        
        if ($unquoted -or ($path.IndexOf(' ') -ge 0)) {
            "'$path'"
        } else {
            $path
        }
    })
    
    $result
}

<#
.SYNOPSIS
    Get an explicit interface on an object so that you can invoke the interface members.
    
.DESCRIPTION
    PowerShell object adapter does not provide explicit interface members. For COM objects
    it only makes IDispatch members available.
    
    This function helps access interface members on an object through reflection. A new
    object is returned with the interface members as ScriptProperties and ScriptMethods.
    
.EXAMPLE
    $dte2 = Get-Interface $dte ([EnvDTE80.DTE2])
#>
function Get-Interface
{
    Param(
        $Object,
        [type]$InterfaceType
    )
    
    [NuGetConsole.Host.PowerShell.Implementation.PSTypeWrapper]::GetInterface($Object, $InterfaceType)
}

<#
.SYNOPSIS
    Get a VS service.

.EXAMPLE
    Get-VSService ([Microsoft.VisualStudio.Shell.Interop.SVsShell]) ([Microsoft.VisualStudio.Shell.Interop.IVsShell])
#>
function Get-VSService
{
    Param(
        [type]$ServiceType,
        [type]$InterfaceType
    )

    $service = [Microsoft.VisualStudio.Shell.Package]::GetGlobalService($ServiceType)
    if ($service -and $InterfaceType) {
        $service = Get-Interface $service $InterfaceType
    }

    $service
}

<#
.SYNOPSIS
    Get VS IComponentModel service to access VS MEF hosting.
#>
function Get-VSComponentModel
{
    Get-VSService ([Microsoft.VisualStudio.ComponentModelHost.SComponentModel]) ([Microsoft.VisualStudio.ComponentModelHost.IComponentModel])
}

# Set initial directory
Set-Location "$env:USERPROFILE"

# For PowerShell v2, we need to create a reference to the default TabExpansion function
# so we can delegate back to it in our custom function. This isn't needed in PowerShell v3, 
# as omitting output in a custom TabExpansion function signals to TabExpansion2 that it 
# should use its own completion list.
if ((Test-Path Function:\DefaultTabExpansion) -eq $false -and (Test-Path Function:\TabExpansion)) {
    Rename-Item Function:\TabExpansion DefaultTabExpansion
}

function TabExpansion([string]$line, [string]$lastWord) {
       $nugetSuggestions = & (Get-Module NuGet) NuGetTabExpansion $line $lastWord
       
       if ($nugetSuggestions.NoResult) {
              # We only want to delegate back to the default tab completion in PowerShell v2.
              # PowerShell v3's TabExpansion2 will use its own command completion list if the
              # custom TabExpansion doesn't return anything.
              if (Test-Path Function:\DefaultTabExpansion) {
                     $line = $line.ToUpperInvariant()
                     $lastWord = $lastWord.ToUpperInvariant()
                     return DefaultTabExpansion $line $lastWord
              }
       }
       else {
              return $nugetSuggestions
       }
}

# default prompt
function prompt {
    "PM>"
}
