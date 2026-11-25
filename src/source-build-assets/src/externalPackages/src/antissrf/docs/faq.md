---
layout: default
title: FAQ
nav_order: 5
description: "Frequently asked questions about AntiSSRF"
---

# Frequently Asked Questions

## What IP addresses or subnets should my service block?

It is important to separate HTTP clients intended for internal vs. external requests.

Ideally, for internal requests or if the range of possible IP addresses is known, use the configuration `PolicyConfigOptions.InternalOnly` and ONLY allow the expected ranges.

For external requests, your service should AT LEAST block the internal and special-use IP addresses, included in the configuration `PolicyConfigOptions.ExternalOnlyLatest`.

## Does the library provide protections on redirects?

The `AntiSSRFPolicy` and its handler/agent DO re-evaluate all policy requirements for every redirect.

The `URIValidator` methods DO NOT apply protections on redirects. When using `InAzureKeyVaultDomain` or `InAzureStorageDomain`, this is okay, since they limit to only well-known, safe domains. When using `InDomain`, the lack of protections on redirects is part of the reason you must be sure that the `trustedDomains` argument truly only includes trusted domains.

## Does the library provide protections against DNS-rebinding based attacks?

The `AntiSSRFPolicy` and its handler/agent DO provide protections against DNS-rebinding based attacks. The IP address that is validated by the policy is EXACTLY the IP address used in the request, eliminating the potential TOCTOU problem.

The `URIValidator` methods DO NOT provide any protections for DNS-based attacks at all. This is okay for `InAzureKeyVaultDomain` and `InAzureStorageDomain`, since they limit to only well-known, safe domains. When using `InDomain`, the lack of DNS-based protections is part of the reason you must be sure that the `trustedDomains` argument truly only includes trusted domains.

## Does the AntiSSRF library support IPv6 functionality?

The `AntiSSRFPolicy` and its handler/agent DO support IPv6 addresses in all formats described by [RFC 4291](https://www.rfc-editor.org/rfc/rfc4291).

## My outgoing requests are being dropped at the destination because of an invalid X-Forwarded-For header. What should I do?

By default, the external-only configurations of AntiSSRF automatically add an `X-Forwarded-For: "true"` header to outgoing requests that don't already have this header. This is an important security feature that helps protect against access to sensitive, internal endpoints like IMDS which safely drop all requests with the `X-Forwarded-For` header.

However, some destination services may reject requests with this dummy value. If your destination service is rejecting requests because of the `X-Forwarded-For: "true"` header, you can disable this behavior:

```csharp
var policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
policy.AddXFFHeader = false; // Disables automatic X-Forwarded-For header addition
```

**Important:** Only disable this feature if you are absolutely certain that:
* Your destination service drops ALL requests with the `X-Forwarded-For` header. For example, you are intentionally accessing IMDS while blocking all external IP addresses.
* **OR** another component in your service stack reliably adds the `X-Forwarded-For` header to all outgoing requests. That way, the policy does not need to add the invalid dummy value to include the header.

For more information, see the [X-Forwarded-For security notes](../dotnet-api/antissrfpolicy/properties/addxffheader#security-notes) and `AddXFFHeader` property.

## Should Microsoft services be onboarding to this library?

If you are a Microsoft service with public code, YES! This is the Microsoft-recommended solution for mitigating SSRF vulnerabilities in Microsoft services.

If you are a Microsoft internal service, we have a separate version of the library for you. Please search for the internal version of the library or reach out to us at [antissrf-oss@microsoft.com](mailto:antissrf-oss@microsoft.com) for help!

## What languages and frameworks are supported?

| Language | Documentation | Notes |
| --- | --- | --- |
| C# | [AntiSSRF .NET Library](dotnet-api/) | For web clients using `HttpClient` objects |
| JavaScript/TypeScript | [AntiSSRF Node.js Library](nodejs-api/) | For requests using NodeJS HTTP(S) Agents |

Broader platform support is under development.
