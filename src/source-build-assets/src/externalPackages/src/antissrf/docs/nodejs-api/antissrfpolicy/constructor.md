---
layout: default
title: Constructor
parent: AntiSSRFPolicy
grand_parent: Node.js API Reference
nav_order: 1
description: "AntiSSRFPolicy constructor documentation"
uid: constructor
---

# AntiSSRFPolicy Constructor

## Definition

Creates a new [AntiSSRFPolicy](../) instance with a predefined security configuration.

```js
AntiSSRFPolicy(config: PolicyConfigOptions)
```

### Parameters

`config`: `PolicyConfigOptions`

Choose from the following predefined policy configurations:

### PolicyConfigOptions

| Option | Use Case | Behavior |
| --- | --- | --- |
| **InternalOnly** | Making requests to internal, non-public addresses only **OR** restricting requests to specific allowed addresses | Blocks all IP addresses by default. Only allows addresses explicitly added via [addAllowedAddresses](../methods/addallowedaddresses). |
| **ExternalOnlyV1** | Making requests to external APIs while blocking internal access | Blocks internal and special-purpose IP addresses per [IPAddressRanges.recommendedV1](../../ipaddressranges#recommendedrangesv1). Automatically adds `X-Forwarded-For` header to requests. |
| **ExternalOnlyLatest** | Currently the same as `ExternalOnlyV1` with automatic security updates | Always stays up to date with the latest `ExternalOnly` version, independent of semantic versioning. |
| **None** | Custom policy configuration | No restrictions applied. Requires manual configuration via policy methods. |

{: .important }
> `PolicyConfigOptions.ExternalOnlyLatest` does NOT follow semantic versioning. It will always stay up-to-date with the latest recommended addresses with no code changes required by the user.

## Security Notes

To prevent SSRF vulnerabilities, it is a best practice to ensure that your code can make requests to external endpoints or to internal endpoints, but never both.

If your service needs to make requests to external endpoints, you need to make sure that it cannot be abused to access internal resources. In this case, we recommend using `PolicyConfigOptions.ExternalOnlyLatest`, which takes care of blocking internal and special-purpose addresses automatically.

If your service needs to make requests to backend services, you must prevent data exfiltration by ensuring it cannot be used to send data to external endpoints. In this case, we recommend using `PolicyConfigOptions.InternalOnly` to block all unspecified addresses, then use `addAllowedAddresses(...)` with the specific internal IP addresses that your service might need to access.

For more about the `X-Forwarded-For` header, see [addXFFHeader](../properties/addxffheader).
