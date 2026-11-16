---
layout: default
title: getHttpsAgent
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "getHttpsAgent method documentation"
---

# AntiSSRFPolicy.getHttpsAgent Method

## Definition

Builds an `https.Agent` that will enforce the policy on all outgoing requests.

```js
getHttpsAgent(options?: https.AgentOptions): https.Agent
```

### Parameters

`options`: `https.AgentOptions`

The optional `https.AgentOptions` to pass to the new agent.

### Errors

`AntiSSRFError`

The function `lookup` is included in `options`.

## Examples

```js
const { AntiSSRFPolicy, PolicyConfigOptions } = require('@microsoft/antissrf');
const https = require('https');

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Get HTTPS agent with the configured policy
const httpsAgent = policy.getHttpsAgent();

const options = {
  hostname: '<some_untrusted_hostname>',
  port: 443,
  path: '/public/data',
  method: 'GET',
  agent: httpsAgent
};

const req = https.request(options, (res) => {
  // If the untrusted hostname directs to an external address using HTTPS,
  // the request will succeed here
});

req.on('error', (err) => {
  // If untrusted hostname directs to an internal or special-use address,
  // the request will fail here with an AntiSSRF error
});

req.end();
```

## Security Notes
* The agent utilizes the `lookup` function to apply the policy. Attempts to overwrite the `lookup` function will result in errors.
* While not explicitly blocked, any use of proxies, such as `proxyEnv` in `options` or in clients that use the agent, will bypass the protections provided by the AntiSSRF library.
