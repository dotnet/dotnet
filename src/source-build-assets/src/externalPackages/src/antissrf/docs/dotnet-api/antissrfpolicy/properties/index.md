---
layout: default
title: Properties
parent: AntiSSRFPolicy
grand_parent: .NET API Reference
nav_order: 2
description: "AntiSSRFPolicy properties documentation"
has_children: true
nav_fold: true
has_toc: false
---

## AntiSSRFPolicy Properties

| Property | Description |
| --- | --- |
| [AddXFFHeader](addxffheader) | Determines whether to automatically add the `X-Forwarded-For` header to outgoing requests that don't already include it. |
| [AllowedAddresses](allowedaddresses) | List of IP networks that are explicitly allowed by the policy. |
| [AllowPlainTextHttp](allowplaintexthttp) | Determines whether HTTPS is required or HTTP is allowed. |
| [DeniedAddresses](deniedaddresses) | List of IP networks that are explicitly blocked by the policy. |
| [DeniedHeaders](deniedheaders) | List of headers that are forbidden from being included in outgoing requests. |
| [DenyAllUnspecifiedIPs](denyallunspecifiedips) | Determines whether all IP addresses should be blocked by default or only `deniedAddresses` should be blocked. |
| [RequiredHeaders](requiredheaders) | List of headers that are required to be present in outgoing requests. |
