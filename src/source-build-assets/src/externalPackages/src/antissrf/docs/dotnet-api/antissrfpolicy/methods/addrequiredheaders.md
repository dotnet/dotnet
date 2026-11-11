---
layout: default
title: AddRequiredHeaders
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "AddRequiredHeaders method documentation"
---

# AntiSSRFPolicy.AddRequiredHeaders Method

## Definition

Adds headers to be explicitly required by the policy. Requests that are missing a required header will be blocked.

```csharp
public void AddRequiredHeaders(string[] requiredHeaders)
```

{: .note }
> Both `RequiredHeaders` and `DeniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Parameters

`requiredHeaders`: `string[]`

The list of headers for the policy to require.

### Exceptions

`ArgumentNullException`
* The `requiredHeaders` parameter is `null` or contains `null` values.

`ArgumentException`
* A header name is empty or whitespace.

`AntiSSRFException`
* Attempted to edit the policy after it has been used to create a handler via `GetHandler()`.

## Examples

```csharp
using Microsoft.Security.AntiSSRF;
using System;
using System.Net.Http;
using System.Threading.Tasks;

// Customize the policy
var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
policy.AddRequiredHeaders(new[] { "Authorization", "X-API-Key" });

// Create HttpClient with the policy handler
using var httpClient = new HttpClient(policy.GetHandler());

try
{
    // This request will succeed (all required headers present)
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer token123");
    httpClient.DefaultRequestHeaders.Add("X-API-Key", "key456");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    
    var response = await httpClient.GetAsync("https://<some_untrusted_hostname>/secure/data");
}
catch (AntiSSRFException ex)
{
    // Should not reach here
}

try
{
    // This request will be blocked (missing required header)
    using var blockedClient = new HttpClient(policy.GetHandler());
    blockedClient.DefaultRequestHeaders.Add("Authorization", "Bearer token123");
    blockedClient.DefaultRequestHeaders.Add("Accept", "application/json");
    // Missing X-API-Key header
    
    var blockedResponse = await blockedClient.GetAsync("https://<some_untrusted_hostname>/secure/admin");
    // This will not execute - request will be blocked
}
catch (AntiSSRFException ex)
{
    Console.WriteLine($"Request blocked due to missing required header: {ex.Message}");
}
```
