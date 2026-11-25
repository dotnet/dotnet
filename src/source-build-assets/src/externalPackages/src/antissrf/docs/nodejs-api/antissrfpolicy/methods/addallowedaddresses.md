---
layout: default
title: addAllowedAddresses
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "addAllowedAddresses method documentation"
---

# AntiSSRFPolicy.addAllowedAddresses Method

## Definition

Adds IP networks to be explicitly allowed by the policy.

```js
addAllowedAddresses(networks: string[]): void
```

{: .note }
> `allowedAddresses` takes precedence over `deniedAddresses`. If an IP address matches both, it will be considered allowed by the policy.

### Parameters

`networks`: `string[]`

The list of IP networks to be explicitly allowed by the policy.

Networks can be:
* IPv4 addresses in dotted-quad notation
    * ex. `127.0.0.1`
* IPv6 addresses in expanded notation `x:x:x:x:x:x:x:x`, where the `x`s are one to four hexadecimal digits
    * ex. `ABCD:EF01:2345:6789:ABCD:EF01:2345:6789`
* IPv6 addresses in compressed notation, where one group of consecutive 0s is represented with `::`
    * ex. `ABCD::`, `::1`, `ABCD:EF01::2345:6789`
* IPv6 in mixed notation `x:x:x:x:x:x:d.d.d.d`, where the `x`s are hexadecimal values and the `d`s are decimal
    * ex. `::FFFF:127.0.0.1`
* Any of the above addresses with a decimal prefix length `<ip-address>/<prefix-length>` 
    * ex. `192.0.2.0/24`, `2001:db8::/32`

### Errors

`AntiSSRFError`
* The `networks` argument is `null` or `undefined`.
* Some `network` in `networks` is not a valid format.

## Examples

```javascript
const { AntiSSRFPolicy, PolicyConfigOptions } = require('@microsoft/antissrf');
const https = require('https');

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
policy.denyAllUnspecifiedIPs = true;
policy.addAllowedAddresses(["1.2.3.4"]);

const options = {
  hostname: '<some_untrusted_hostname>',
  path: '/public/data',
  agent: policy.getHttpsAgent()
};

const req = https.request(options, (res) => {
  // If the untrusted hostname directs to 1.2.3.4,
  // the request will succeed here
});

req.on('error', (err) => {
  // If untrusted hostname directs to anything besides 1.2.3.4,
  // the request will fail here with an AntiSSRFError 
});

req.end();
```
