---
layout: default
title: AddDeniedAddresses
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "AddDeniedAddresses method documentation"
---

# AntiSSRFPolicy.AddDeniedAddresses Method

## Definition

Adds IP networks to be explicitly blocked by the policy.

```csharp
public void AddDeniedAddresses(string[] networks)
```

{: .note }
> `AllowedAddresses` takes precedence over `DeniedAddresses`. If an IP address matches both, it will be considered allowed by the policy.

{: .note }
> `DenyAllUnspecifiedIPs` takes precedence over `DeniedAddresses`. If `DenyAllUnspecifiedIPs` is `true`, `DeniedAddresses` will not be considered when determining if an IP address is allowed or blocked by the policy.

### Parameters

`networks`: `string[]`

The list of IP networks to be explicitly blocked by the policy.

### Exceptions

`ArgumentNullException`
* The `networks` parameter is `null` or contains `null` values.

`FormatException`
* A network string is not in valid CIDR format.

`AntiSSRFException`
* Attempted to edit the policy after it has been used to create a handler via `GetHandler()`.
* `DenyAllUnspecifiedIPs` is already set to `true`.

## Examples

```csharp
using Microsoft.Security.AntiSSRF;
using System;
using System.Net.Http;
using System.Threading.Tasks;

// Customize the policy
var policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
policy.AddDeniedAddresses(new[] { "1.2.3.4" });

// Create HttpClient with the policy handler
using var httpClient = new HttpClient(policy.GetHandler());

try
{
    // If the untrusted hostname directs to anything besides 1.2.3.4,
    // the request will succeed here
    var response = await httpClient.GetAsync("https://<some_untrusted_hostname>/public/data");
}
catch (AntiSSRFException ex)
{
    // If untrusted hostname directs to 1.2.3.4,
    // the request will fail here with an AntiSSRFException
}
```