---
layout: default
title: RequiredHeaders
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "RequiredHeaders property documentation"
---

# AntiSSRFPolicy.RequiredHeaders Property

## Definition

Gets a read-only view of headers that are required to be present in outgoing requests.

```csharp
public IReadOnlyList<string> RequiredHeaders { get; }
```

{: .note }
> Both `RequiredHeaders` and `DeniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Property Value

`IReadOnlyList<string>`

A read-only list of header names that are required by the policy.
