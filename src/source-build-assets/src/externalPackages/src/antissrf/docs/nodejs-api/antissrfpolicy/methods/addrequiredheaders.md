---
layout: default
title: addRequiredHeaders
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "addRequiredHeaders method documentation"
---

# AntiSSRFPolicy.addRequiredHeaders Method

## Definition

Adds headers to be explicitly required by the policy. Requests that are missing a required header will be blocked.

```js
addRequiredHeaders(headers: string[]): void
```

{: .note }
> Both `requiredHeaders` and `deniedHeaders` are considered when validating a request. If any header is in both lists, the request will always be blocked.

### Parameters

`headers`: `string[]`

The list of headers for the policy to require.

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
policy.addRequiredHeaders(['Authorization', 'X-API-Key']);
const agent = policy.getHttpsAgent();

// This request will succeed (all required headers present)
const options = {
  hostname: '<some_untrusted_hostname>',
  path: '/secure/data',
  headers: {
    'Authorization': 'Bearer token123',
    'X-API-Key': 'key456',
    'Accept': 'application/json'
  },
  agent: agent
};

https.get(options, (res) => {
  console.log('Request successful - all required headers present');
});

// This request will be blocked (missing required header)
const blockedOptions = {
  hostname: '<some_untrusted_hostname>',
  path: '/secure/admin',
  headers: {
    'Authorization': 'Bearer token123',
    'Accept': 'application/json'
  },
  agent: agent
};

https.get(blockedOptions, (res) => {
  // This will not execute - request will be blocked
}).on('error', (err) => {
  console.log('Request blocked due to missing required header:', err.message);
});
```
