---
layout: default
title: allowPlainTextHttp
parent: Properties
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "allowPlainTextHttp property documentation"
---

# AntiSSRFPolicy.allowPlainTextHttp Property

{: .warning }
> Changing an `AntiSSRFPolicy` instance to allow plaintext HTTP means you will be able to send HTTP requests without the recommended TLS encryption.

## Definition

Determines whether HTTPS is required or HTTP is allowed.

```js
allowPlainTextHttp: boolean { get; set; }
```

{: .note }
> With ALL configuration options, HTTP is disallowed unless `allowPlainTextHttp` is explicitly set to `true`.

### Property Value

`boolean`

* `true` if HTTP should be allowed.
* `false` if HTTPS should be required.

### Errors

`AntiSSRFError`
The value passed cannot be `null` or `undefined`.
