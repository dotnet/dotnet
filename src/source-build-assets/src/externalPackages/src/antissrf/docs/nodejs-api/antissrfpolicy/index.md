---
layout: default
title: AntiSSRFPolicy
parent: Node.js API Reference
description: "Main policy configuration class for SSRF protection"
nav_order: 1
has_children: true
has_toc: false
---

# AntiSSRFPolicy Class

## Use Case

Use this class whenever you are accessing a URL that can belong to **any domain** or **some untrusted domain**.

This use case addresses two distinct security scenarios. For requests to external endpoints, the policy enforces that IP addresses are not internal or special-use addresses, preventing URLs from being abused to gain access to internal resources. For requests to backend resources, the policy blocks all IP addresses except for specific ranges that you expect to see, ensuring that URLs cannot be used to exfiltrate data to unauthorized destinations.

{: .note }
> * If you instead expect the URL to be an **Azure Key Vault endpoint**, see [URIValidator.inAzureKeyVaultDomain](../urivalidator/inazurekeyvaultdomain).
> * If you instead expect the URL to be an **Azure Storage endpoint**, see [URIValidator.inAzureStorageDomain](../urivalidator/inazurestoragedomain).
> * If you instead expect the domain to be a **specific, trusted domain**, see [URIValidator.inDomain](../urivalidator/indomain).

## Definition

The `AntiSSRFPolicy` allows you to customize security requirements for headers, IP addresses, and protocols. You can configure the policy using built-in settings or define your own custom rules. The policy then provides Node.js HTTP(S) agents that automatically enforce these security requirements on all outgoing requests made using those agents.

## Constructors

| Constructor | Description |
| --- | --- |
| [AntiSSRFPolicy(PolicyConfigOptions)](constructor) | Initializes a new instance of the `AntiSSRFPolicy` class with the specified initial configuration. |

## Properties

| Property | Description |
| --- | --- |
| [addXFFHeader](properties/addxffheader) | Determines whether to automatically add the `X-Forwarded-For` header to outgoing requests that don't already include it. |
| [allowedAddresses](properties/allowedaddresses) | List of IP networks that are explicitly allowed by the policy. |
| [allowPlainTextHttp](properties/allowplaintexthttp) | Determines whether HTTPS is required or HTTP is allowed. |
| [deniedAddresses](properties/deniedaddresses) | List of IP networks that are explicitly blocked by the policy. |
| [deniedHeaders](properties/deniedheaders) | List of headers that are forbidden from being included in outgoing requests. |
| [denyAllUnspecifiedIPs](properties/denyallunspecifiedips) | Determines whether all IP addresses should be blocked by default or only `deniedAddresses` should be blocked. |
| [requiredHeaders](properties/requiredheaders) | List of headers that are required to be present in outgoing requests. |

## Policy Customization Methods

| Method | Description |
| --- | --- |
| [addAllowedAddresses(string[])](methods/addallowedaddresses) | Adds IP networks to be explicitly allowed by the policy. |
| [addDeniedAddresses(string[])](methods/adddeniedaddresses) | Adds IP networks to be explicitly blocked by the policy. |
| [addDeniedHeaders(string[])](methods/adddeniedheaders) | Adds headers to be explicitly blocked by the policy. |
| [addRequiredHeaders(string[])](methods/addrequiredheaders) | Adds headers to be explicitly required by the policy. |

## Policy Use Methods

| Method | Description |
| --- | --- |
| [getHttpAgent(any)](methods/gethttpagent) | Builds an `http.Agent` that will enforce the policy on all outgoing requests. |
| [getHttpsAgent(any)](methods/gethttpsagent) | Builds an `https.Agent` that will enforce the policy on all outgoing requests. |
