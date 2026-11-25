---
layout: default
title: allowedAddresses
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "allowedAddresses property documentation"
---

# AntiSSRFPolicy.allowedAddresses Property

## Definition

The [`BlockList`](https://nodejs.org/api/net.html#class-netblocklist) of IP networks explicitly allowed by the policy.

```js
allowedAddresses: ReadOnly<BlockList> { get; }
```

{: .note }
> `allowedAddresses` takes precedence over `deniedAddresses`. If an IP address matches both, it will be considered allowed by the policy.

### Property Value

`ReadOnly<BlockList>`

The `ReadOnly` version of the `net.BlockList` storing the allowed IP networks.
