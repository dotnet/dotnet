---
layout: default
title: node-fetch
parent: Samples
grand_parent: Node.js API Reference
description: "AntiSSRF agent with node-fetch"
---

# AntiSSRFPolicy with the node-fetch Library

## Introduction

The [node-fetch library](https://github.com/node-fetch/node-fetch/tree/2.x#readme) is a commonly used request library to extend Node.js http(s) functionality with a `window.fetch` compatible API. The example below shows how you can use the node-fetch library with the AntiSSRF Node.js library.

## Example

node-fetch allows you to make requests with the option `agent`, which is either a Node.js HTTP/S agent or a function to return a Node.js HTTP/S agent.

### Setup

Set up the `AntiSSRFPolicy`, then get the AntiSSRF agents from the policy.

```js
import { AntiSSRFPolicy, PolicyConfigOptions } from '@microsoft/antissrf';
import fetch from "node-fetch";

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Get the AntiSSRF agents
const httpAgent = policy.getHttpAgent();
const httpsAgent = policy.getHttpsAgent({ keepAlive: true });

const agentFn = (_parsedURL: URL) => {
    return _parsedURL.protocol === "https:" ? httpsAgent : httpAgent;
}
```

### Use the AntiSSRF Agents for Requests

Every request to an endpoint with untrusted input should include the AntiSSRF agents.

```js
fetch(
    "<some_https_url_constructed_with_untrusted_input>",
    {
        agent: agentFn
    })
    .then((res) => {
        /**
         * Will get here if the untrusted URL does NOT direct the request to
         * an internal or special-purpose IP address
         */
    })
    .catch((err) => {
        /**
         * Will get here if the untrusted URL directs the request to an internal
         * or special-purpose IP address
         */
    });
```

{: .note }
> If you want different examples or if you find any bug while using `AntiSSRFPolicy` with node-fetch, please let us know at [antissrf-oss@microsoft.com](mailto:antissrf-oss@microsoft.com).
