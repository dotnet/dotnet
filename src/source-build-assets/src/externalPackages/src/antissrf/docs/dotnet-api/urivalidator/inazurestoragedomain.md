---
layout: default
title: InAzureStorageDomain
parent: URIValidator
grand_parent: .NET API Reference
description: "Check if a URI belongs to Azure Storage domains"
---

# URIValidator.InAzureStorageDomain Method

## Use Case

The code is making requests to a URL constructed using untrusted inputs, where an input is considered untrusted if it comes from *user input* or *other services*.

**AND**

The URL is expected to belong to an [**Azure Storage Domain**](#azure-storage-domain-names).

{: .note }
> * If you instead expect the domain to be in **any domain** or **an untrusted domain**, see [AntiSSRFPolicy](../antissrfpolicy).
> * If you instead expect the URL to be an **Azure Key Vault endpoint**, see [InAzureKeyVaultDomain](inazurekeyvaultdomain).
> * If you instead expect the domain to be another **specific, trusted domain**, see [InDomain](indomain).

## Definition

Validates if a URL is an Azure Storage endpoint.

## Overloads

| Method | Description |
| --- | --- |
| [InAzureStorageDomain(Uri)](#inazurestoragedomainuri) | Validates if a URL is an Azure Storage endpoint. |
| [InAzureStorageDomain(string)](#inazurestoragedomainstring) | Validates if a URL is an Azure Storage endpoint. |

## InAzureStorageDomain(Uri)

```csharp
public static bool InAzureStorageDomain(Uri uri)
```

### Parameters

`uri`: `Uri`

The URI to be evaluated.

### Returns

`bool`

* `true` if `uri` belongs to any of the listed Azure Storage domains.
* `false` if `uri` does not belong to any of the listed Azure Storage domains, the URI is not valid, or the protocol is not HTTP/S.

## InAzureStorageDomain(string)

```csharp
public static bool InAzureStorageDomain(string address)
```

### Parameters

`address`: `string`

The URI string to be evaluated.

### Returns

`bool`

* `true` if `address` belongs to any of the listed Azure Storage domains.
* `false` if `address` does not belong to any of the listed Azure Storage domains, the string is not a valid URI, or the protocol is not HTTP/S.

## Examples

```csharp
using Microsoft.Security.AntiSSRF;
using System;

URIValidator.InAzureStorageDomain("https://mystorageaccount.blob.core.windows.net/container/file.txt");
// → true

URIValidator.InAzureStorageDomain("https://evil.com/data");
// → false

var uri = new Uri("https://mystorageaccount.blob.core.windows.net/container/file.txt");
URIValidator.InAzureStorageDomain(uri);
// → true
```

## Azure Storage Domain Names

`InAzureStorageDomain` will evaluate whether the given parameter belongs to any combination of the following domains and services:

**Domains:**
- `core.windows.net`
- `storage.azure.net`
- `core.usgovcloudapi.net`
- `core.chinacloudapi.cn`

**Services:**
- `blob`
- `web`
- `dfs`
- `file`
- `queue`
- `table`
