---
layout: default
title: denyAllUnspecifiedIPs
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "denyAllUnspecifiedIPs property documentation"
---

# AntiSSRFPolicy.denyAllUnspecifiedIPs Property

## Definition

Determines whether all IP addresses should be blocked by default or only `deniedAddresses` should be blocked.

{: .note }
> To allow specific addresses, see [addAllowedAddresses](../methods/addallowedaddresses). 

```js
denyAllUnspecifiedIPs: boolean { get; set; }
```

### Property Value

`boolean`

* `true` if all IP addresses NOT specified by `addAllowedAddresses` should be blocked.
* `false` if only addresses in `deniedAddresses` should be blocked.

### Errors

`AntiSSRFError`
The value passed cannot be `null` or `undefined`.
