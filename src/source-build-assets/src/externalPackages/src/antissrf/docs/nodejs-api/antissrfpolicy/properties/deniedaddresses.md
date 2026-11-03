---
layout: default
title: deniedAddresses
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "deniedAddresses property documentation"
---

# AntiSSRFPolicy.deniedAddresses Property

## Definition

The [`BlockList`](https://nodejs.org/api/net.html#class-netblocklist) of IP networks explicitly blocked by the policy.

```js
deniedAddresses: ReadOnly<BlockList> { get; }
```

{: .note }
> `allowedAddresses` takes precedence over `deniedAddresses`. If an IP address matches both, it will be considered allowed by the policy.

{: .note }
> `denyAllUnspecifiedIPs` takes precedence over `deniedAddresses`. If `denyAllUnspecifiedIPs` is `true`, `deniedAddresses` will not be considered when determining if an IP address is allowed or blocked by the policy.

### Property Value

`ReadOnly<BlockList>`

The `ReadOnly` version of the `net.BlockList` storing the denied IP networks.
