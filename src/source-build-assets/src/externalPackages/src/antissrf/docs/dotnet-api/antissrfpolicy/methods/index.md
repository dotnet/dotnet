---
layout: default
title: Methods
parent: AntiSSRFPolicy
grand_parent: .NET API Reference
nav_order: 3
description: "AntiSSRFPolicy methods documentation"
has_children: true
nav_fold: true
has_toc: false
---

## Policy Customization Methods

| Method | Description |
| --- | --- |
| [AddAllowedAddresses(string[])](addallowedaddresses) | Adds IP networks to be explicitly allowed by the policy. |
| [AddDeniedAddresses(string[])](adddeniedaddresses) | Adds IP networks to be explicitly blocked by the policy. |
| [AddDeniedHeaders(string[])](adddeniedheaders) | Adds headers to be explicitly blocked by the policy. |
| [AddRequiredHeaders(string[])](addrequiredheaders) | Adds headers to be explicitly required by the policy. |

## Policy Use Method

| Method | Description |
| --- | --- |
| [GetHandler()](gethandler) | Creates and returns a new `AntiSSRFHandler` that will enforce the policy on all outgoing requests. |
