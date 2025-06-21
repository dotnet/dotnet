<#
.SYNOPSIS
Adds flow for a .NET preview or RC

.PARAMETER RuntimeChannel
Public runtime channel 

.PARAMETER SdkChannel
Public sdk channel

.PARAMETER RuntimeBranch
Public runtime repo branch (e.g. release/8.0-preview2)

.PARAMETER SdkBranch
Public sdk repo branch (e.g. release/8.0.1xx-preview2)

.PARAMETER VSChannel
Channel for tooling flow (e.g. VS 17.8)

.PARAMETER AddInternalFlow
If true, adds internal flow for the preview/rc.

.PARAMETER InternalRuntimeChannel
Internal runtime channel

.PARAMETER InternalSdkChannel
Internal sdk channel

#>

param
(
    [Parameter(Mandatory=$true)][string]$RuntimeChannel,
    [Parameter(Mandatory=$true)][string]$SdkChannel,
    [Parameter(Mandatory=$true)][string]$RuntimeBranch,
    [Parameter(Mandatory=$true)][string]$SdkBranch,
    [Parameter(Mandatory=$true)][string]$VSChannel,
    [switch]$AddInternalFlow,
    [string]$InternalRuntimeChannel,
    [string]$InternalSdkChannel,
    [switch]$DryRun
)

$publicVMR = "https://github.com/dotnet/dotnet"
$internalVMR = "https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet"

# Make a default channel for a repo+branch
function MakeDefaultChannel($repo, $branch, $channel)
{
    Write-Host "Making default channel - $repo @ $branch -> $channel"
    if (!$DryRun) {
        & darc add-default-channel --repo "$repo" --branch "$branch" --channel "$channel" --quiet
    }
}

# Disable flow on a channel
function DisableFlow($channel)
{
    Write-Host "Disabling flow on $channel"
    if (!$DryRun) {
        & darc subscription-status --disable --channel "$channel" --quiet --exact
    }
}

# Add non-source flow. This is for traditional style (package only) dependency updates.
function AddPackageOnlyFlow($sourceRepo, $sourceChannel, $targetRepo, $targetBranch, $frequency)
{
    Write-Host "Adding flow - $sourceRepo @ $sourceChannel -> $targetRepo @ $targetBranch ($frequency)"
    if (!$DryRun) {
        & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetRepo" --target-branch "$targetBranch" --update-frequency "$frequency" --quiet --no-trigger --standard-automerge
    }
}

# Add batched non-source-only flow. This is for traditional style (package only) dependency updates.
function AddBatchedPackageOnlyFlow($sourceRepo, $sourceChannel, $targetRepo, $targetBranch, $frequency)
{
    Write-Host "Adding flow - $sourceRepo @ $sourceChannel -> $targetRepo @ $targetBranch ($frequency)"
    if (!$DryRun) {
        & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetRepo" --target-branch "$targetBranch" --update-frequency "$frequency" --quiet --no-trigger --batchable
    }
}

# Add forward flow for sources.
function AddForwardFlow($sourceRepo, $sourceChannel, $targetVmr, $mapping, $targetBranch, $frequency)
{
    Write-Host "Adding forward flow - $sourceRepo @ $sourceChannel -> $targetVmr ($mapping) @ $targetBranch ($frequency)"
    if (!$DryRun) {
        & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetVmr" --target-branch "$targetBranch" --source-enabled true --target-directory $mapping --update-frequency "$frequency" --quiet --no-trigger --standard-automerge
    }
}

# Adds batched forward flow for sources. Used for SDK to have NuGet and SDK flow in parallel.
function AddBatchedForwardFlow($sourceRepo, $sourceChannel, $targetVmr, $mapping, $targetBranch, $frequency)
{
    Write-Host "Adding batched forward flow - $sourceRepo @ $sourceChannel -> $targetVmr @ $targetBranch ($frequency)"
    if (!$DryRun) {
        & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetVmr" --target-branch "$targetBranch" --source-enabled true --target-directory $mapping --update-frequency "$frequency" --quiet --no-trigger --batchable
    }
}

# Adds merge policy for batched subscriptions
function AddBatchedMergePolicy($targetRepo, $targetBranch)
{
    Write-Host "Setting batched merge policy for $targetRepo @ $targetBranch"
    if (!$DryRun) {
        & darc set-repository-policies --repo "$targetRepo" --branch "$targetBranch" --standard-automerge --quiet
    }
}

# Adds flow from a VMR back to a source repo. Source+packages
function AddBackwardsFlow($sourceVmr, $sourceChannel, $targetRepo, $mapping, $targetBranch, $frequency)
{
    # Update splatting: use an array for extra arguments
    $mainSubscriptionObject = & darc get-subscriptions --source-repo "$sourceVmr" --channel ".NET 10.0.1xx SDK" --target-repo "$targetRepo" --output-format json | ConvertFrom-Json
    $excludedAssets = $mainSubscriptionObject.excludedAssets -join ";"
    $excludedAssetsArg = @()
    if ($excludedAssets) {
        $excludedAssetsArg = @("--excluded-assets", $excludedAssets)
    }
    Write-Host "Adding backflow - $sourceVmr @ $sourceChannel -> $targetRepo @ $targetBranch ($frequency) with excluded assets ($excludedAssets)"
    if (!$DryRun) {
        & darc add-subscription --source-repo "$sourceVmr" --channel "$sourceChannel" --target-repo "$targetRepo" --target-branch "$targetBranch" --source-enabled true --source-directory $mapping $excludedAssetsArg --update-frequency "$frequency" --quiet --no-trigger --standard-automerge
    }
}

# Adds a package only flow for arcade.
function AddArcadeFlow($targetRepo, $targetBranch)
{
    AddPackageOnlyFlow https://github.com/dotnet/arcade ".NET Eng - Latest" $targetRepo $targetBranch None
}

$InternalRuntimeBranch = "internal/$RuntimeBranch"
$InternalSdkBranch = "internal/$SdkBranch"

Write-Host "Making default channels for runtime repos"
MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-wpf-int $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/aspnetcore $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/efcore $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/emsdk $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/icu "dotnet/$RuntimeBranch" $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/runtime $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/windowsdesktop $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/wpf $RuntimeBranch $RuntimeChannel
MakeDefaultChannel https://github.com/dotnet/winforms $RuntimeBranch $RuntimeChannel

if ($AddInternalFlow) {
    # Because of where internal fixes tend to be, we eliminate some leaves in the graph
    # and flow them through the normal public channels: emsdk, icu.
    # wpf-int is flowed from the public channel (there is no internal branch or merge) due to
    # an issue in the staging pipeline where it will give the test feed for the public preview build
    # internal permissions in some cases because it sees the internal repo publishing locations before
    # the public locations. The logic is such that if any asset has non-public permissions, the test feed will have
    # internal permissions.
    Write-Host "Making default channels for internal branches of runtime repos"
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeBranch $InternalRuntimeChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-efcore $InternalRuntimeBranch $InternalRuntimeChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeBranch $InternalRuntimeChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop $InternalRuntimeBranch $InternalRuntimeChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-wpf $InternalRuntimeBranch $InternalRuntimeChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-winforms $InternalRuntimeBranch $InternalRuntimeChannel
}

Write-Host "Making default channels for SDK repos"
MakeDefaultChannel https://github.com/dotnet/sdk $SdkBranch $SdkChannel
MakeDefaultChannel https://github.com/dotnet/roslyn-analyzers $SdkBranch $SdkChannel
MakeDefaultChannel https://github.com/dotnet/templating $SdkBranch $SdkChannel

if ($AddInternalFlow) {
    # Because of where internal fixes tend to be, we eliminate some leaves in the sdk graph
    # and flow them through the normal public channels: templating, roslyn-analyzers
    Write-Host "Making default channels for SDK repos"
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkBranch $InternalSdkChannel
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-templating $InternalSdkBranch $InternalSdkChannel
}

Write-Host "Make default channel for the VMR"
MakeDefaultChannel https://github.com/dotnet/dotnet $SdkBranch $SdkChannel

if ($AddInternalFlow) {
    Write-Host "Make internal default channel for the VMR"
    MakeDefaultChannel https://dev.azure.com/dnceng/internal/_git/dotnet-dotnet $InternalSdkBranch $InternalSdkChannel
}

Write-Host "Setting up batched merge policies"
AddBatchedMergePolicy https://github.com/dotnet/aspnetcore $RuntimeBranch

if ($AddInternalFlow) {
    AddBatchedMergePolicy https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeBranch
}

Write-Host "Adding arcade flow to repos not building in the VMR"
AddArcadeFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf-int $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/icu "dotnet/$RuntimeBranch"

Write-Host "Adding non-VMR runtime flow"
AddPackageOnlyFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf-int $RuntimeChannel https://github.com/dotnet/wpf $RuntimeBranch EveryBuild
AddPackageOnlyFlow https://github.com/dotnet/icu $RuntimeChannel https://github.com/dotnet/runtime $RuntimeBranch EveryBuild

Write-Host "Adding arcade VMR forward flow (non-automatic)"
AddForwardFlow https://github.com/dotnet/arcade $RuntimeChannel $publicVMR arcade $SdkBranch None

Write-Host "Adding VMR runtime repo forward flow"
AddForwardFlow https://github.com/dotnet/aspnetcore $RuntimeChannel $publicVMR aspnetcore $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/efcore $RuntimeChannel $publicVMR efcore $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/emsdk $RuntimeChannel $publicVMR emsdk $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/runtime $RuntimeChannel $publicVMR runtime $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/winforms $RuntimeChannel $publicVMR winforms $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/windowsdesktop $RuntimeChannel $publicVMR windowsdesktop $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/wpf $RuntimeChannel $publicVMR wpf $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal VMR runtime repo forward flow"
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeChannel $internalVMR aspnetcore $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-efcore $InternalRuntimeChannel $internalVMR efcore $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-emsdk $InternalRuntimeChannel $internalVMR emsdk $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel $internalVMR runtime $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-winforms $InternalRuntimeChannel $internalVMR winforms $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop $InternalRuntimeChannel $internalVMR windowsdesktop $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf $InternalRuntimeChannel $internalVMR wpf $InternalSdkBranch EveryBuild
    
    Write-Host "Disabling internal VMR runtime repo forward flow"
    DisableFlow $InternalRuntimeChannel
}

Write-Host "Add VMR sdk repo forward flow"
AddForwardFlow https://github.com/dotnet/roslyn-analyzers $SdkChannel $publicVMR roslyn-analyzers $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/templating $SdkChannel $publicVMR templating $SdkBranch EveryBuild
# SDK is batched so that it batches alongside NuGet for a cohesive set of changes.
AddBatchedForwardFlow https://github.com/dotnet/sdk $SdkChannel $publicVMR sdk $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal VMR sdk repo forward flow"
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-roslyn-analyzers $InternalSdkChannel $internalVMR roslyn-analyzers $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkChannel $internalVMR sdk $InternalSdkBranch EveryBuild
    AddForwardFlow https://dev.azure.com/dnceng/internal/_git/dotnet-templating $InternalSdkChannel $internalVMR templating $InternalSdkBranch EveryBuild
    
    Write-Host "Disabling internal sdk repo forward flow"
    DisableFlow $InternalSdkChannel
}

Write-Host "Adding tooling repo VMR forward flow"
# NuGet is special in that it flows into the SDK and then batched source into the VMR
# Change to traditional flow when https://github.com/dotnet/arcade-services/issues/4665 is resolved
AddPackageOnlyFlow https://github.com/nuget/nuget.client $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddBatchedForwardFlow https://github.com/nuget/nuget.client $VSChannel $publicVMR nuget-client $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/roslyn $VSChannel $publicVMR roslyn $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/fsharp $VSChannel $publicVMR fsharp $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/msbuild $VSChannel $publicVMR msbuild $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/razor $VSChannel $publicVMR razor $SdkBranch EveryBuild
AddForwardFlow https://github.com/dotnet/vstest $VSChannel $publicVMR vstest $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal VMR sdk repo forward flow"
    # It's not quite yet clear what the flow will look like for internal VMR flow.
    throw "NYI"
}

Write-Host "Adding VMR->repo backflow."
# Only repos that branch for a release get backflow
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/runtime runtime $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/aspnetcore aspnetcore $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/efcore efcore $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/emsdk emsdk $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/windowsdesktop windowsdesktop $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/winforms winforms $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/wpf wpf $RuntimeBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/roslyn-analyzers templating $SdkBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/sdk sdk $SdkBranch EveryBuild
AddBackwardsFlow $publicVMR $SdkChannel https://github.com/dotnet/templating templating $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal VMR sdk repo backflow"

    # It's not quite yet clear what the flow will look like for internal VMR backflow.
    throw "NYI"
}
