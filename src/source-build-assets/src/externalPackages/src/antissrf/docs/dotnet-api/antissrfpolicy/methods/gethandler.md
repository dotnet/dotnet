---
layout: default
title: GetHandler
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "GetHandler method documentation"
---

# AntiSSRFPolicy.GetHandler Method

## Definition

Builds an `AntiSSRFHandler` that will enforce the policy on all outgoing requests.

```csharp
public AntiSSRFHandler GetHandler()
```

{: .note }
> After calling `GetHandler()`, the policy becomes immutable. Any attempt to change properties or call customization methods will throw an `AntiSSRFException`.

### Returns

`AntiSSRFHandler`

A new `AntiSSRFHandler` instance that enforces this policy.

### Example

```csharp
using Microsoft.Security.AntiSSRF;
using System.Net.Http;

// Customize the policy
var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
policy.AddAllowedAddresses(new[] { "10.0.1.0/24" });

// Get the handler — policy is now locked
using var handler = policy.GetHandler();
using var client = new HttpClient(handler);
```
