#
# This private script adds $InterfaceType members to $psObject which invokes on $wrappedObject
#
Param(
    $psObject,
    $wrappedObject,
    [Type]$InterfaceType
)

function GetInvoker
{
    Param(
        $Target,
        $Method
    )

    if ($Method.IsGenericMethodDefinition) {
        return {
            $t = $Target
            $m = $Method.MakeGenericMethod($args)
            
            if (!$m.GetParameters()) {
                return $m.Invoke($t, @())
            }
            
            $o = New-Object PSObject
            Add-Member -InputObject $o -MemberType ScriptMethod -Name Invoke -Value {
                    [NuGetConsole.Host.PowerShell.Implementation.PSTypeWrapper]::InvokeMethod($t, $m, $args)
                }.GetNewClosure()
            return $o
        }.GetNewClosure()
    }
    
    return {
        [NuGetConsole.Host.PowerShell.Implementation.PSTypeWrapper]::InvokeMethod($Target, $Method, $args)
    }.GetNewClosure()
}

$InterfaceType.GetMembers() | %{
    $m = $_
    $getter = $null
    $setter = $null
    
    if ($m.MemberType -eq [System.Reflection.MemberTypes]"Property") {
    
        if ($m.CanRead) {
            $getter = GetInvoker $wrappedObject $m.GetGetMethod()
        }
        if ($m.CanWrite) {
            $setter = GetInvoker $wrappedObject $m.GetSetMethod()
        }
        
        $prop = New-Object Management.Automation.PSScriptProperty $m.Name, $getter, $setter
        $psObject.PSObject.Properties.Add($prop)
        
    } elseif (!$m.IsSpecialName -and
            ($m.MemberType -eq [System.Reflection.MemberTypes]"Method")) {
    
        $invoker = GetInvoker $wrappedObject $m
        $method = New-Object Management.Automation.PSScriptMethod $m.Name, $invoker
        $psObject.PSObject.Methods.Add($method)
    }
}
