---
layout: default
title: URIValidator
parent: .NET API Reference
description: "URL validation class for SSRF protection"
nav_order: 3
has_children: true
has_toc: false
---

# URIValidator Class

## Use Case

The code is making requests to a URL constructed using untrusted inputs, where an input is considered untrusted if it comes from *user input* or *other services*.

**AND**

The URL is expected to belong to a specific set of trusted domains, the [Azure Storage domains](inazurestoragedomain#azure-storage-domain-names), or the [Azure Key Vault domains](inazurekeyvaultdomain#azure-key-vault-domain-names).

{: .note }
> If you instead expect the domain to be in **any domain** or **an untrusted domain**, see [AntiSSRFPolicy](../antissrfpolicy).

## Definition

Provides static methods for validating the hostname and protocol of URLs.

## Methods

| Method | Description |
| --- | --- |
| [InAzureKeyVaultDomain(Uri)](inazurekeyvaultdomain#inazurekeyvaultdomainuri) | Validates if a URL is an Azure Key Vault endpoint. |
| [InAzureKeyVaultDomain(string)](inazurekeyvaultdomain#inazurekeyvaultdomainstring) | Validates if a URL is an Azure Key Vault endpoint. |
| [InAzureStorageDomain(Uri)](inazurestoragedomain#inazurestoragedomainuri) | Validates if a URL is an Azure Storage endpoint. |
| [InAzureStorageDomain(string)](inazurestoragedomain#inazurestoragedomainstring) | Validates if a URL is an Azure Storage endpoint. |
| [InDomain(Uri, string)](indomain#indomainuri-string) | Validates if a URL belongs to a trusted domain. |
| [InDomain(string, string)](indomain#indomainstring-string) | Validates if a URL belongs to a trusted domain. |
| [InDomain(Uri, string[])](indomain#indomainuri-string-1) | Validates if a URL belongs to any of a list of trusted domains. |
| [InDomain(string, string[])](indomain#indomainstring-string-1) | Validates if a URL belongs to any of a list of trusted domains. |
