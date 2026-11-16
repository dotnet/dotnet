---
layout: default
title: addXFFHeader
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "addXFFHeader property documentation"
---

# AntiSSRFPolicy.addXFFHeader Property

## Definition

Determines whether to automatically add the `X-Forwarded-For` header to outgoing requests that don’t already include it.

{: .important }
> The header is added with the dummy value `"true"`. If your end service requires this header to be a valid IP address, you will have to add the header manually.

```js
addXFFHeader: boolean { get; set; }
```

### Property Value

`boolean`

* `true` if the `X-Forwarded-For` header should be added to requests that don’t already include it.
* `false` if the `X-Forwarded-For` header should not be added.

### Errors

`AntiSSRFError`
The value passed cannot be `null` or `undefined`.

## Security Notes

The `X-Forwarded-For` header can be an important defense-in-depth strategy against SSRF vulnerabilities. Some services, including IMDS, will drop all incoming requests with the `X-Forwarded-For` header present. By ensuring that the header is added to all outgoing requests, your service can be sure that it will never have an SSRF vulnerability that leaks data from IMDS.
