---
layout: default
title: Node.js API Reference
nav_order: 4
description: "Complete API documentation for the AntiSSRF Node.js Library"
has_children: true
has_toc: false
---

# API Documentation

## AntiSSRF Node.js Library

The **AntiSSRF Node.js Library** is a library for JavaScript/TypeScript applications using Node.js that provides robust URL validation to prevent SSRF vulnerabilities in code. It is designed as an easy, drop-in library with a minimal impact on the engineering team, implemented both as [Node.js HTTP(S) agents](https://nodejs.org/api/http.html#class-httpagent) and a static URL validator, depending on use case.

## Usage Instructions

The AntiSSRF Library provides validation for different scenarios based on your trust requirements:

| Use Case | Description | Documentation Link |
| --- | --- | --- |
| **General Case** | The untrusted URL can belong to **any domain** or an **untrusted domain**. | [AntiSSRFPolicy](antissrfpolicy) |
| **Azure Key Vault Domain** | The untrusted URL must be an **Azure Key Vault endpoint**. | [URIValidator.inAzureKeyVaultDomain](urivalidator/inazurekeyvaultdomain) |
| **Azure Storage Domain** | The untrusted URL must be an **Azure Storage endpoint**. | [URIValidator.inAzureStorageDomain](urivalidator/inazurestoragedomain) |
| **Allowlist of Trusted Domains** | The untrusted URL must belong to a **specific, trusted domain**. | [URIValidator.inDomain](urivalidator/indomain) |

## Classes

| Class | Description |
| --- | --- |
| [AntiSSRFPolicy](antissrfpolicy) | Represents a customizable security policy and provides HTTP(S) agents to ensure all outgoing requests match the security policy. |
| [IPAddressRanges](../ipaddressranges) | Provides predefined IP address ranges for use with AntiSSRF policies. |
| [URIValidator](urivalidator) | Provides static methods for validating the hostname and protocol of URLs. |
