---
layout: default
title: requiredHeaders
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "requiredHeaders property documentation"
---

# AntiSSRFPolicy.requiredHeaders Property

## Definition

A read-only array of headers that are required to be present in outgoing requests.

```js
requiredHeaders: readonly string[] { get; }
```

{: .note }
> Both `requiredHeaders` and `deniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Property Value

`readonly string[]`

A read-only array of header names that are required by the policy.
