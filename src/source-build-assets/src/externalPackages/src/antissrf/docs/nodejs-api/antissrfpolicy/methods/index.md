---
layout: default
title: Methods
parent: AntiSSRFPolicy
grand_parent: Node.js API Reference
nav_order: 3
description: "AntiSSRFPolicy methods documentation"
has_children: true
nav_fold: true
has_toc: false
---

## Policy Customization Methods

| Method | Description |
| --- | --- |
| [addAllowedAddresses(string[])](addallowedaddresses) | Adds IP networks to be explicitly allowed by the policy. |
| [addDeniedAddresses(string[])](adddeniedaddresses) | Adds IP networks to be explicitly blocked by the policy. |
| [addDeniedHeaders(string[])](adddeniedheaders) | Adds headers to be explicitly blocked by the policy. |
| [addRequiredHeaders(string[])](addrequiredheaders) | Adds headers to be explicitly required by the policy. |

## Policy Use Methods

| Method | Description |
| --- | --- |
| [getHttpAgent(any)](gethttpagent) | Builds an `http.Agent` that will enforce the policy on all outgoing requests. |
| [getHttpsAgent(any)](gethttpsagent) | Builds an `https.Agent` that will enforce the policy on all outgoing requests. |
