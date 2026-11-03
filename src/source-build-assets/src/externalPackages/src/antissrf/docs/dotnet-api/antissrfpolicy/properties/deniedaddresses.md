---
layout: default
title: DeniedAddresses
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "DeniedAddresses property documentation"
---

# AntiSSRFPolicy.DeniedAddresses Property

## Definition

Gets a read-only view of IP networks explicitly blocked by the policy.

```csharp
public IReadOnlyList<string> DeniedAddresses { get; }
```

{: .note }
> `AllowedAddresses` takes precedence over `DeniedAddresses`. If an IP address matches both, it will be considered allowed by the policy.

{: .note }
> `DenyAllUnspecifiedIPs` takes precedence over `DeniedAddresses`. If `DenyAllUnspecifiedIPs` is `true`, `DeniedAddresses` will not be considered when determining if an IP address is allowed or blocked by the policy.

### Property Value

`IReadOnlyList<string>`

A read-only list of the denied IP networks.
