# Copyright (c) Microsoft. All rights reserved.
# Portable to Full PDB conversion script for Test Platform.

[CmdletBinding()]
Param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("Debug", "Release")]
    [System.String] $Configuration = "Release"
)

#
# Variables
#
Write-Verbose "Setup environment variables."
$TP_ROOT_DIR = (Get-Item (Split-Path $MyInvocation.MyCommand.Path)).Parent.FullName
$TP_PACKAGES_DIR = Join-Path $TP_ROOT_DIR "packages"
$TP_OUT_DIR = Join-Path $TP_ROOT_DIR "artifacts"

$PdbConverterToolVersion = "1.1.0-beta2-21075-01"

function Locate-PdbConverterTool {
    $pdbConverter = Join-Path -path $TP_PACKAGES_DIR -ChildPath "Microsoft.DiaSymReader.Pdb2Pdb\$PdbConverterToolVersion\tools\Pdb2Pdb.exe"

    if (!(Test-Path -path $pdbConverter)) {
        throw "Unable to locate Pdb2Pdb converter exe in path '$pdbConverter'."
    }

    Write-Verbose "Pdb2Pdb converter path is : $pdbConverter"
    return $pdbConverter

}

function ConvertPortablePdbToWindowsPdb {
    $allPdbs = Get-ChildItem -path $TP_OUT_DIR\$Configuration *.pdb -Recurse | % { $_.FullName }
    $portablePdbs = New-Object System.Collections.Generic.List[System.Object]

    foreach ($pdb in $allPdbs) {
        # First four bytes should be 'BSJB' for portable pdb
        $bytes = [char[]](Get-Content $pdb -Encoding byte -TotalCount 4) -join ''

        if ( $bytes -eq "BSJB") {
            $portablePdbs.Add($pdb)
        }
    }

    $pdbConverter = Locate-PdbConverterTool

    foreach ($portablePdb in $portablePdbs) {
        # First check if corresponding dll exists
        $dllOrExePath = $portablePdb -replace ".pdb", ".dll"

        if (!(Test-Path -path $dllOrExePath)) {
            # If no corresponding dll found, check if exe exists
            $dllOrExePath = $portablePdb -replace ".pdb", ".exe"

            if (!(Test-Path -path $dllOrExePath)) {
                throw "Unable to locate dll/exe corresponding to $portablePdb"
            }
        }

        $fullpdb = $portablePdb -replace ".pdb", ".pdbfull"

        Write-Verbose "$pdbConverter $dll /pdb $portablePdb /out $fullpdb"
        & $pdbConverter $dllOrExePath /pdb $portablePdb /out $fullpdb

        Remove-Item -Path $portablePdb
    }
}

Write-Verbose "Converting Portable pdbs to Windows(Full) Pdbs..."
ConvertPortablePdbToWindowsPdb

