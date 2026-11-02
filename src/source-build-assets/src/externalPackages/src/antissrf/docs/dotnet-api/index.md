---
layout: default
title: .NET API Reference
nav_order: 3
description: "Complete API documentation for the AntiSSRF .NET Library"
has_children: true
has_toc: false
---

# API Documentation

## AntiSSRF .NET Library

The **AntiSSRF .NET Library** is a library for C# applications using .NET that provides robust URL validation and HTTP request protection to prevent SSRF vulnerabilities in code. It is designed as an easy, drop-in library with minimal impact on the engineering team, implemented both as an `HttpMessageHandler` for use with `HttpClient` and as a static URL validator, depending on use case.

## Usage Instructions

The AntiSSRF Library provides validation for different scenarios based on your trust requirements:

| Use Case | Description | Documentation Link |
| --- | --- | --- |
| **General Case** | The untrusted URL can belong to **any domain** or an **untrusted domain**. | [AntiSSRFPolicy](antissrfpolicy) |
| **Azure Key Vault Domain** | The untrusted URL must be an **Azure Key Vault endpoint**. | [URIValidator.InAzureKeyVaultDomain](urivalidator/inazurekeyvaultdomain) |
| **Azure Storage Domain** | The untrusted URL must be an **Azure Storage endpoint**. | [URIValidator.InAzureStorageDomain](urivalidator/inazurestoragedomain) |
| **Allowlist of Trusted Domains** | The untrusted URL must belong to a **specific, trusted domain**. | [URIValidator.InDomain](urivalidator/indomain) |

## Classes

| Class | Description |
| --- | --- |
| [AntiSSRFPolicy](antissrfpolicy) | Represents a customizable security policy and provides an `AntiSSRFHandler` to ensure all outgoing `HttpClient` requests match the security policy. |
| [AntiSSRFHandler](antissrfhandler) | An `HttpMessageHandler` that enforces the `AntiSSRFPolicy` on all outgoing requests. |
| [IPAddressRanges](../ipaddressranges) | Provides predefined IP address ranges for use with AntiSSRF policies. |
| [URIValidator](urivalidator/) | Provides static methods for validating the hostname and protocol of URLs. |