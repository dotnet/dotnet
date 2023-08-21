<#
.SYNOPSIS
Flips the flow of a preview/rc between public flow and internal flow. Internal subscriptions and default channel must exist (see create-preview-flow.ps1)

.PARAMETER RuntimeChannel
Public runtime channel 

.PARAMETER SdkChannel
Public sdk channel

.PARAMETER InternalRuntimeChannel
Internal runtime channel

.PARAMETER InternalSdkChannel
Internal sdk channel

.PARAMETER FlowType
What type of flow to use.

.PARAMETER TriggerSubscriptions
If true, triggers subscriptions after the switch

#>

param
(
    [Parameter(Mandatory=$true)][string]$RuntimeChannel,
    [Parameter(Mandatory=$true)][string]$SdkChannel,
    [Parameter(Mandatory=$true)][string]$InternalRuntimeChannel,
    [Parameter(Mandatory=$true)][string]$InternalSdkChannel,
    [Parameter(Mandatory=$true)][ValidateSet('Public','Internal')][string]$FlowType,
    [switch]$TriggerSubscriptions
)

Write-Host "Switching flow to $FlowType"

switch ( $FlowType )
{
    'Public' {
        # Switching to public flow is easy. Enable all the subscriptions in the public runtime and SDK channels
        # and disable any on the internal channels
        & darc subscription-status --disable --channel "$InternalRuntimeChannel" --quiet --exact
        & darc subscription-status --disable --channel "$InternalSdkChannel" --quiet --exact
        & darc subscription-status --enable --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --enable --channel "$SdkChannel" --quiet --exact

        # Trigger public subscriptions
        if ($TriggerSubscriptions) {
            & darc trigger-subscriptions --channel "$RuntimeChannel" --quiet --exact
            & darc trigger-subscriptions --channel "$SdkChannel" --quiet --exact
        }
    }
    'Internal' {
        # Switching to internal flow is a little more subtle. Enable the internal subscriptions,
        # then disable the corresponding public subscriptions, which is a subset of the full public channel
        & darc subscription-status --enable --channel "$InternalRuntimeChannel" --quiet --exact
        & darc subscription-status --enable --channel "$InternalSdkChannel" --quiet --exact

        & darc subscription-status --disable --source-repo https://github.com/dotnet/runtime --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/winforms --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/wpf --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/windowsdesktop --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/aspnetcore --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/efcore --channel "$RuntimeChannel" --quiet --exact
        & darc subscription-status --disable --source-repo https://github.com/dotnet/sdk --channel "$SdkChannel" --quiet --exact

        # Trigger internal subscriptions
        if ($TriggerSubscriptions) {
            & darc trigger-subscriptions --channel "$InternalRuntimeChannel" --quiet --exact
            & darc trigger-subscriptions --channel "$InternalSdkChannel" --quiet --exact
        }
    }
    default { Write-Error "Unknown flow type $FlowType" }
}