---
layout: default
title: addDeniedHeaders
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "addDeniedHeaders method documentation"
---

# AntiSSRFPolicy.addDeniedHeaders Method

## Definition

Adds headers to be explicitly blocked by the policy. Requests that include a denied header will be blocked.

```js
addDeniedHeaders(headers: string[]): void
```

{: .note }
> Both `requiredHeaders` and `deniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Parameters

`headers`: `string[]`

The list of headers for the policy to block.

### Errors

`AntiSSRFError`
* The `headers` argument is `null` or `undefined`.
* Some `header` in `headers` is `null`, `undefined`, or whitespace.

## Examples

```js
const { AntiSSRFPolicy, PolicyConfigOptions } = require('@microsoft/antissrf');
const https = require('https');

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
policy.addDeniedHeaders(['X-Real-IP', 'X-Forwarded-Host']);
const agent = policy.getHttpsAgent();

// This request will succeed (no denied headers)
const options = {
  hostname: '<some_untrusted_hostname>',
  path: '/public/data',
  headers: {
    'User-Agent': 'MyApp/1.0',
    'Accept': 'application/json'
  },
  agent: agent
};

https.get(options, (res) => {
  console.log('Request successful - no denied headers present');
});

// This request will be blocked (contains denied header)
const blockedOptions = {
  hostname: '<some_untrusted_hostname>',
  path: '/admin/endpoint',
  headers: {
    'X-Real-IP': '192.168.1.1', // This header is denied
    'Accept': 'application/json'
  },
  agent: agent
};

https.get(blockedOptions, (res) => {
  // This will not execute - request will be blocked
}).on('error', (err) => {
  console.log('Request blocked due to denied header:', err.message);
});
```
