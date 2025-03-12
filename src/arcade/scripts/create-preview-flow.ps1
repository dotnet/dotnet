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
    [string]$InternalSdkChannel
)

function MakeDefaultChannel($repo, $branch, $channel)
{
    Write-Host "Making default channel - $repo @ $branch -> $channel"
    & darc add-default-channel --repo "$repo" --branch "$branch" --channel "$channel" --quiet
}

function DisableFlow($channel)
{
    Write-Host "Disabling flow on $channel"
    & darc subscription-status --disable --channel "$channel" --quiet --exact
}

function AddFlow($sourceRepo, $sourceChannel, $targetRepo, $targetBranch, $frequency)
{
    Write-Host "Adding flow - $sourceRepo @ $sourceChannel -> $targetRepo @ $targetBranch ($frequency)"
    & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetRepo" --target-branch "$targetBranch" --update-frequency "$frequency" --quiet --no-trigger --standard-automerge
}

function AddBatchedFlow($sourceRepo, $sourceChannel, $targetRepo, $targetBranch, $frequency)
{
    Write-Host "Adding flow - $sourceRepo @ $sourceChannel -> $targetRepo @ $targetBranch ($frequency)"
    & darc add-subscription --source-repo "$sourceRepo" --channel "$sourceChannel" --target-repo "$targetRepo" --target-branch "$targetBranch" --update-frequency "$frequency" --quiet --no-trigger --batchable
}

function AddBatchedMergePolicy($targetRepo, $targetBranch)
{
    Write-Host "Setting batched merge policy for $targetRepo @ $targetBranch"
    & darc set-repository-policies --repo "$targetRepo" --branch "$targetBranch" --standard-automerge --quiet
}

function AddArcadeFlow($targetRepo, $targetBranch)
{
    AddFlow https://github.com/dotnet/arcade ".NET Eng - Latest" $targetRepo $targetBranch None
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

Write-Host "Setting up batched merge policies"
AddBatchedMergePolicy https://github.com/dotnet/aspnetcore $RuntimeBranch

if ($AddInternalFlow) {
    AddBatchedMergePolicy https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeBranch
}

Write-Host "Adding arcade flow"
AddArcadeFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf-int $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/aspnetcore $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/efcore $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/emsdk $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/icu "dotnet/$RuntimeBranch"
AddArcadeFlow https://github.com/dotnet/runtime $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/windowsdesktop $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/wpf $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/winforms $RuntimeBranch
AddArcadeFlow https://github.com/dotnet/sdk $SdkBranch
AddArcadeFlow https://github.com/dotnet/roslyn-analyzers $SdkBranch
AddArcadeFlow https://github.com/dotnet/templating $SdkBranch

Write-Host "Adding runtime -> runtime flow"
AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf-int $RuntimeChannel https://github.com/dotnet/wpf $RuntimeBranch EveryBuild
AddBatchedFlow https://github.com/dotnet/efcore $RuntimeChannel https://github.com/dotnet/aspnetcore $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/emsdk $RuntimeChannel https://github.com/dotnet/runtime $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/icu $RuntimeChannel https://github.com/dotnet/runtime $RuntimeBranch EveryBuild
AddBatchedFlow https://github.com/dotnet/runtime $RuntimeChannel https://github.com/dotnet/aspnetcore $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/runtime $RuntimeChannel https://github.com/dotnet/efcore $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/runtime $RuntimeChannel https://github.com/dotnet/winforms $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/winforms $RuntimeChannel https://github.com/dotnet/wpf $RuntimeBranch EveryBuild
AddFlow https://github.com/dotnet/wpf $RuntimeChannel https://github.com/dotnet/windowsdesktop $RuntimeBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal runtime -> internal runtime flow"
    AddBatchedFlow https://dev.azure.com/dnceng/internal/_git/dotnet-efcore $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeBranch EveryBuild
    AddBatchedFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-efcore $InternalRuntimeBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-winforms $InternalRuntimeBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-winforms $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-wpf $InternalRuntimeBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-wpf $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop $InternalRuntimeBranch EveryBuild
    
    Write-Host "Disabling internal runtime -> internal runtime flow"
    DisableFlow $InternalRuntimeChannel
}

Write-Host "Add runtime->sdk flow"
AddFlow https://github.com/dotnet/aspnetcore $RuntimeChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/windowsdesktop $RuntimeChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/runtime $RuntimeChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal runtime->internal sdk flow"
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-aspnetcore $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-windowsdesktop $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkBranch EveryBuild
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-runtime $InternalRuntimeChannel https://dev.azure.com/dnceng/internal/_git/dotnet-templating $InternalSdkBranch EveryBuild
    
    Write-Host "Disabling internal runtime->internal sdk flow"
    DisableFlow $InternalRuntimeChannel
}

Write-Host "Add sdk->sdk flow"
AddFlow https://github.com/dotnet/roslyn-analyzers $SdkChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/templating $SdkChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild

if ($AddInternalFlow) {
    Write-Host "Adding internal sdk->internal sdk flow"
    AddFlow https://dev.azure.com/dnceng/internal/_git/dotnet-templating $InternalSdkChannel https://dev.azure.com/dnceng/internal/_git/dotnet-sdk $InternalSdkBranch EveryBuild
    
    Write-Host "Disabling internal sdk->internal sdk flow"
    DisableFlow $InternalSdkChannel
}

Write-Host "Adding tooling->sdk flow"
AddFlow https://github.com/nuget/nuget.client $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/roslyn $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/fsharp $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/msbuild $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild
AddFlow https://github.com/dotnet/razor $VSChannel https://github.com/dotnet/sdk $SdkBranch EveryBuild



