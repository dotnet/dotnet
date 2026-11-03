---
layout: default
title: Axios
parent: Samples
grand_parent: Node.js API Reference
description: "AntiSSRF agent with Axios"
---

# AntiSSRFPolicy with the Axios Library

## Introduction

[Axios](https://axios-http.com/docs/intro) is one of the most commonly used request libraries for JavaScript/TypeScript. It is very easy to use with the AntiSSRF Node.js Library, as shown in the examples below.

{: .important }
> Be careful of options that can bypass AntiSSRF protections:
> * Changing the `adapter` option could lead to requests without a Node.js agent, and therefore without the `AntiSSRFPolicy` applied.
> * Like the AntiSSRF agents themselves, using a custom `lookup` function will bypass the IP address validations from `AntiSSRFPolicy`.
> * Using `proxy` with AntiSSRF will largely not work, since the `AntiSSRFPolicy` will not be used once control is passed to the proxy.

## Axios with Axios Instance

Axios allows you to create an Axios instance with a custom `config` including the `AntiSSRFPolicy` agents. As long as you don't overwrite the request `config` agents, the instance will enforce the policy.

### Setup

Set up the `AntiSSRFPolicy`, then create an Axios instance with the AntiSSRF agents from the policy.

```js
import { AntiSSRFPolicy, PolicyConfigOptions } from '@microsoft/antissrf';
import axios from "axios";

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Use the policy with Axios
const secureClient = axios.create({
    httpAgent: policy.getHttpAgent(),
    httpsAgent: policy.getHttpsAgent({ keepAlive: true })
});
```

### Use the Axios Instance for Requests

Every request to an endpoint with untrusted input should use the `secureClient`.

```js
secureClient.get(
    "<some_https_url_constructed_with_untrusted_input>",
    {
        auth: {
            username: 'janedoe',
            password: 's00pers3cret'
        }
    })
    .then(function (res) {
        /**
         * Will get here if the untrusted URL does NOT direct the request to
         * an internal or special-use IP address
         */
    })
    .catch(function (err) {
        /**
         * Will get here if the untrusted URL directs the request to an
         * internal or special-use IP address
         */
    });
```

## Axios with Request Agents

Just like Node.js requests, Axios allows you to add agents to individual requests.


### Setup

Set up the `AntiSSRFPolicy`, then get the AntiSSRF agents from the policy.

```js
import { AntiSSRFPolicy, PolicyConfigOptions } from '@microsoft/antissrf';
import axios from "axios";

// Customize the policy
const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);

// Get the AntiSSRF agents
const httpAgent = policy.getHttpAgent();
const httpsAgent = policy.getHttpsAgent({ keepAlive: true });
```

### Use the AntiSSRF Agents for Requests

Every request to an endpoint with untrusted input should include the AntiSSRF agents.

```js
axios.get(
    "<some_https_url_constructed_with_untrusted_input>",
    {
        httpAgent: httpAgent,
        httpsAgent: httpsAgent,
        auth: {
            username: 'janedoe',
            password: 's00pers3cret'
        }
    })
    .then(function (res) {
        /**
         * Will get here if the untrusted URL does NOT direct the request to
         * an internal or special-use IP address
         */
    })
    .catch(function (err) {
        /**
         * Will get here if the untrusted URL directs the request to an
         * internal or special-use IP address
         */
    });
```

{: .note }
> If you want different examples or if you find any bug while using `AntiSSRFPolicy` with Axios, please let us know at [antissrf-oss@microsoft.com](mailto:antissrf-oss@microsoft.com).
