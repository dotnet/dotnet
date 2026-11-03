---
layout: default
title: AddDeniedHeaders
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "AddDeniedHeaders method documentation"
---

# AntiSSRFPolicy.AddDeniedHeaders Method

## Definition

Adds headers to be explicitly blocked by the policy. Requests that include a denied header will be blocked.

```csharp
public void AddDeniedHeaders(string[] deniedHeaders)
```

{: .note }
> Both `RequiredHeaders` and `DeniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Parameters

`deniedHeaders`: `string[]`

The list of headers for the policy to block.

### Exceptions

`ArgumentNullException`
* The `deniedHeaders` parameter is `null` or contains `null` values.

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
policy.AddDeniedHeaders(new[] { "X-Real-IP", "X-Forwarded-Host" });

// Create HttpClient with the policy handler
using var httpClient = new HttpClient(policy.GetHandler());

try
{
    // This request will succeed (no denied headers)
    httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    
    var response = await httpClient.GetAsync("https://<some_untrusted_hostname>/public/data");
}
catch (AntiSSRFException ex)
{
    // Should not reach here
}

try
{
    // This request will be blocked (contains denied header)
    using var blockedClient = new HttpClient(policy.GetHandler());
    blockedClient.DefaultRequestHeaders.Add("X-Real-IP", "192.168.1.1"); // This header is denied
    blockedClient.DefaultRequestHeaders.Add("Accept", "application/json");
    
    var blockedResponse = await blockedClient.GetAsync("https://<some_untrusted_hostname>/admin/endpoint");
}
catch (AntiSSRFException ex)
{
    Console.WriteLine($"Request blocked due to denied header: {ex.Message}");
}
```
