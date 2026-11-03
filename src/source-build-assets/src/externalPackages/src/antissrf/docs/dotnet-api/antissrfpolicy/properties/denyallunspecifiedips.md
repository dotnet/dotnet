---
layout: default
title: DenyAllUnspecifiedIPs
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "DenyAllUnspecifiedIPs property documentation"
---

# AntiSSRFPolicy.DenyAllUnspecifiedIPs Property

## Definition

Determines whether all IP addresses should be blocked by default or only `DeniedAddresses` should be blocked.

{: .note }
> To allow specific addresses, see [addAllowedAddresses](../methods/addallowedaddresses). 

```csharp
public bool DenyAllUnspecifiedIPs { get; set; }
```

### Property Value

`bool`

* `true` if all IP addresses NOT specified by `AddAllowedAddresses` should be blocked.
* `false` if only addresses in `DeniedAddresses` should be blocked.

Default: `false` (unless using `InternalOnly`, which sets it to `true`)

### Exceptions

`AntiSSRFException`
Thrown when attempting to change the property after the policy has been used to create a handler via `GetHandler()`.
