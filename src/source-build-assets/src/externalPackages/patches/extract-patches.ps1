<#
.SYNOPSIS
    Given a set of changes checked in locally to an external component submodule, prepare
    patches for the external component.
.DESCRIPTION
    The src/ directory contains a set of submodules for each external component. These
    are typically set to a specific desired version based on a tag (e.g. Foo @ v1.0.0).
    When changes are required to make the component build in Linux source build, we must supply
    patches on top of submodule. To create these patches, do the following:
    1. Make changes in the submodule.
    2. Commit changes in the submodule.
    3. From the root directory of the submodule, run this script. The script will prepare a patch
       based on the base sha of the submodule and the latest committed changes. The patch
       will be added to patches/<component>/*.patch
    4. Outside the submodule location, add the patch to the working tree, commit, and test.
#>

$repoRoot = $PSScriptRoot
$submoduleRoot = Join-Path $repoRoot 'src'
$componentName = Split-Path -Leaf $pwd
$patchesDir = Join-Path $repoRoot 'patches' $componentName

# Retrieve the sha for the submodule
$baseSha = git -C $submoduleRoot ls-tree HEAD $componentName --object-only

if  (!(Test-Path -Path $patchesDir)) {
    New-Item -ItemType directory -Path $patchesDir
}

# Remove existing patches
Remove-Item $patchesDir/*.patch

# Now format and reset back to the original state to be ready for testing
git format-patch -N -o $patchesDir $baseSha
git reset --hard $baseSha