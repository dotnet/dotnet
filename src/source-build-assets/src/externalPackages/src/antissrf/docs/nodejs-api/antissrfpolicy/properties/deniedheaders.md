---
layout: default
title: deniedHeaders
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "deniedHeaders property documentation"
---

# AntiSSRFPolicy.deniedHeaders Property

## Definition

A read-only array of headers that are forbidden from being included in outgoing requests.

```js
deniedHeaders: readonly string[] { get; }
```

{: .note }
> Both `requiredHeaders` and `deniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Property Value

`readonly string[]`

A read-only array of header names that are blocked by the policy.
