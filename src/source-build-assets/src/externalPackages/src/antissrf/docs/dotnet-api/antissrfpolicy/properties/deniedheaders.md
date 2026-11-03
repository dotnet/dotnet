---
layout: default
title: DeniedHeaders
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "DeniedHeaders property documentation"
---

# AntiSSRFPolicy.DeniedHeaders Property

## Definition

Gets a read-only view of headers that are forbidden from being included in outgoing requests.

```csharp
public IReadOnlyList<string> DeniedHeaders { get; }
```

{: .note }
> Both `RequiredHeaders` and `DeniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Property Value

`IReadOnlyList<string>`

A read-only list of header names that are blocked by the policy.
