---
layout: default
title: InAzureKeyVaultDomain
parent: URIValidator
grand_parent: .NET API Reference
description: "Check if a URI belongs to Azure Key Vault domains"
---

# URIValidator.InAzureKeyVaultDomain Method

## Use Case

The code is making requests to a URL constructed using untrusted inputs, where an input is considered untrusted if it comes from *user input* or *other services*.

**AND**

The URL is expected to belong to an [**Azure Key Vault Domain**](#azure-key-vault-domain-names).

{: .note }
> * If you instead expect the domain to be in **any domain** or **an untrusted domain**, see [AntiSSRFPolicy](../antissrfpolicy).
> * If you instead expect the URL to be an **Azure Storage endpoint**, see [InAzureStorageDomain](inazurestoragedomain).
> * If you instead expect the domain to be another **specific, trusted domain**, see [InDomain](indomain).

## Definition

Validates if a URL is an Azure Key Vault endpoint.

## Overloads

| Method | Description |
| --- | --- |
| [InAzureKeyVaultDomain(Uri)](#inazurekeyvaultdomainuri) | Validates if a URL is an Azure Key Vault endpoint. |
| [InAzureKeyVaultDomain(string)](#inazurekeyvaultdomainstring) | Validates if a URL is an Azure Key Vault endpoint. |

## InAzureKeyVaultDomain(Uri)

```csharp
public static bool InAzureKeyVaultDomain(Uri uri)
```

### Parameters

`uri`: `Uri`

The URI to be evaluated.

### Returns

`bool`

* `true` if `uri` belongs to any of the listed Azure Key Vault domains.
* `false` if `uri` does not belong to any of the listed Azure Key Vault domains, the URI is not valid, or the protocol is not HTTP/S.

## InAzureKeyVaultDomain(string)

```csharp
public static bool InAzureKeyVaultDomain(string address)
```

### Parameters

`address`: `string`

The URI string to be evaluated.

### Returns

`bool`

* `true` if `address` belongs to any of the listed Azure Key Vault domains.
* `false` if `address` does not belong to any of the listed Azure Key Vault domains, the string is not a valid URI, or the protocol is not HTTP/S.

## Examples

```csharp
using Microsoft.Security.AntiSSRF;
using System;

URIValidator.InAzureKeyVaultDomain("https://myvault.vault.azure.net/secrets/api-key");
// → true

URIValidator.InAzureKeyVaultDomain("https://evil.com/secrets");
// → false

var uri = new Uri("https://myvault.vault.azure.net/secrets/api-key");
URIValidator.InAzureKeyVaultDomain(uri);
// → true
```

## Azure Key Vault Domain Names

`InAzureKeyVaultDomain` will evaluate whether the given parameter belongs to any of the following domains:

* `vault.azure.net`
* `managedhsm.azure.net`
* `vault.azure.cn`
* `managedhsm.azure.cn`
* `vault.usgovcloudapi.net`
* `managedhsm.usgovcloudapi.net`
