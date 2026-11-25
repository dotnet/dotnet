---
layout: default
title: getHttpAgent
parent: Methods
grand_parent: AntiSSRFPolicy
ancestor: Node.js API Reference
description: "getHttpAgent method documentation"
---

# AntiSSRFPolicy.getHttpAgent Method

## Definition

Builds an `http.Agent` that will enforce the policy on all outgoing requests.

{: .warning }
> **HTTP is insecure**: HTTP requests send data in plaintext over the network. Using this HTTP agent when your policy does not allow plaintext connections will cause all requests to fail. For secure communications, use [`getHttpsAgent()`](gethttpsagent) instead.

```js
getHttpAgent(options?: http.AgentOptions): http.Agent
```

### Parameters

`options`: `http.AgentOptions`

The optional `http.AgentOptions` to pass to the new agent.

### Errors

`AntiSSRFError`

The function `lookup` is included in `options`.

## Examples

```js
const { AntiSSRFPolicy, PolicyConfigOptions } = require('@microsoft/antissrf');
const http = require('http');

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
policy.allowPlainTextHttp = true;

// Get HTTP agent with the configured policy
const httpAgent = policy.getHttpAgent();

const options = {
  hostname: '<some_untrusted_hostname>',
  port: 80,
  path: '/public/data',
  method: 'GET',
  agent: httpAgent
};

const req = http.request(options, (res) => {
  // If the untrusted hostname directs to an external address using HTTP,
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
