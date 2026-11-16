---
layout: default
title: AllowPlainTextHttp
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: .NET API Reference
description: "AllowPlainTextHttp property documentation"
---

# AntiSSRFPolicy.AllowPlainTextHttp Property

{: .warning }
> Changing an `AntiSSRFPolicy` instance to allow plaintext HTTP means you will be able to send HTTP requests without the recommended TLS encryption.

## Definition

Determines whether HTTPS is required or HTTP is allowed.

```csharp
public bool AllowPlainTextHttp { get; set; }
```

{: .note }
> With ALL configuration options, HTTP is disallowed unless `AllowPlainTextHttp` is explicitly set to `true`.

### Property Value

`bool`

* `true` if HTTP should be allowed.
* `false` if HTTPS should be required.

### Exceptions

`AntiSSRFException`
Thrown when attempting to change the property after the policy has been used to create a handler via `GetHandler()`.
