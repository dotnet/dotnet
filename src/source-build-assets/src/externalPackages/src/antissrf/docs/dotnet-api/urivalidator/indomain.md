---
layout: default
title: InDomain
parent: URIValidator
grand_parent: .NET API Reference
nav_order: 1
description: "Check if a URI belongs to specified domain(s)"
---

# URIValidator.InDomain Method

## Use Case

The code is making requests to a URL constructed using untrusted inputs, where an input is considered untrusted if it comes from *user input* or *other services*.

**AND**

The URL is expected to belong to a **specific set of trusted domains**.

{: .note }
> * If you instead expect the domain to be in **any domain** or **an untrusted domain**, see [AntiSSRFPolicy](../antissrfpolicy).
> * If you instead expect the URL to be an **Azure Key Vault endpoint**, see [InAzureKeyVaultDomain](inazurekeyvaultdomain).
> * If you instead expect the URL to be an **Azure Storage endpoint**, see [InAzureStorageDomain](inazurestoragedomain).

{: .important }
> If your untrusted URL needs to belong to a specific domain, but you do not fully control all subdomains of the domain, you can use BOTH `InDomain` AND `AntiSSRFPolicy` to be protected. If the untrusted URL belongs to a domain that cannot be fully trusted, at least `AntiSSRFPolicy` is required for full protection.

## Definition

Validates if a URL belongs to any of a list of trusted domains.

## Overloads

| Method | Description |
| --- | --- |
| [InDomain(Uri, string)](#indomainuri-string) | Validates if a URL belongs to a trusted domain. |
| [InDomain(string, string)](#indomainstring-string) | Validates if a URL belongs to a trusted domain. |
| [InDomain(Uri, string[])](#indomainuri-string-1) | Validates if a URL belongs to any of a list of trusted domains. |
| [InDomain(string, string[])](#indomainstring-string-1) | Validates if a URL belongs to any of a list of trusted domains. |

## InDomain(Uri, string)

```csharp
public static bool InDomain(Uri untrustedUri, string trustedDomain)
```

### Parameters

`untrustedUri`: `Uri`

The URI to be evaluated.

`trustedDomain`: `string`

The domain name that `untrustedUri` will be compared against.

### Returns

`bool`

* `true` if `untrustedUri` belongs to `trustedDomain`.
* `false` if `untrustedUri` does not belong to `trustedDomain`, if `untrustedUri` is not a valid URI, if protocol is not HTTP/S or WS/S, or if either argument is `null`.

## InDomain(string, string)

```csharp
public static bool InDomain(string untrustedAddress, string trustedDomain)
```

### Parameters

`untrustedAddress`: `string`

The URI string to be evaluated.

`trustedDomain`: `string`

The domain name that `untrustedAddress` will be compared against.

### Returns

`bool`

* `true` if `untrustedAddress` belongs to `trustedDomain`.
* `false` if `untrustedAddress` does not belong to `trustedDomain`, if `untrustedAddress` cannot be converted to a valid URI, if protocol is not HTTP/S or WS/S, or if either argument is `null`.

## InDomain(Uri, string[])

```csharp
public static bool InDomain(Uri untrustedUri, string[] trustedDomains)
```

### Parameters

`untrustedUri`: `Uri`

The URI to be evaluated.

`trustedDomains`: `string[]`

The list of domain names that `untrustedUri` will be compared against.

### Returns

`bool`

* `true` if `untrustedUri` belongs to any domain in `trustedDomains`.
* `false` if `untrustedUri` does not belong to any domain in `trustedDomains`, if `untrustedUri` is not a valid URI, if protocol is not HTTP/S or WS/S, or if either argument is `null`.

## InDomain(string, string[])

```csharp
public static bool InDomain(string untrustedAddress, string[] trustedDomains)
```

### Parameters

`untrustedAddress`: `string`

The URI string to be evaluated.

`trustedDomains`: `string[]`

The list of domain names that `untrustedAddress` will be compared against.

### Returns

`bool`

* `true` if `untrustedAddress` belongs to any domain in `trustedDomains`.
* `false` if `untrustedAddress` does not belong to any domain in `trustedDomains`, if `untrustedAddress` cannot be converted to a valid URI, if protocol is not HTTP/S or WS/S, or if either argument is `null`.

## Examples

```csharp
using Microsoft.Security.AntiSSRF;
using System;

// Single domain validation
URIValidator.InDomain("https://api.mycompany.com/data", "mycompany.com");
// → true

// Multiple domain validation
URIValidator.InDomain("https://api.mycompany.com/data", new[] { "mycompany.com", "trusted.com" });
// → true

// Domain not in trusted list
URIValidator.InDomain("https://evil.com/secrets", "mycompany.com");
// → false

// Using Uri overload
var uri = new Uri("https://api.mycompany.com/data");
URIValidator.InDomain(uri, "mycompany.com");
// → true
```
