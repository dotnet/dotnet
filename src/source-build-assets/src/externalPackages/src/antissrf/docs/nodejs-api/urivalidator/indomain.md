---
layout: default
title: inDomain
parent: URIValidator
grand_parent: Node.js API Reference
description: "Check if a URI belongs to specified domain(s)"
---

# URIValidator.inDomain Method

## Use Case

The code is making requests to a URL constructed using untrusted inputs, where an input is considered untrusted if it comes from *user input* or *other services*.

**AND**

The URL is expected to belong to a **specific set of trusted domains**.

{: .note }
> * If you instead expect the domain to be in **any domain** or **an untrusted domain**, see [AntiSSRFPolicy](../antissrfpolicy).
> * If you instead expect the URL to be an **Azure Key Vault endpoint**, see [inAzureKeyVaultDomain](inazurekeyvaultdomain).
> * If you instead expect the URL to be an **Azure Storage endpoint**, see [inAzureStorageDomain](inazurestoragedomain).

{: .important }
> If your untrusted URL needs to belong to a specific domain, but you do not fully control all subdomains of the domain, you can use BOTH `inDomain` AND `AntiSSRFPolicy` to be protected. If the untrusted URL belongs to a domain that cannot be fully trusted, at least `AntiSSRFPolicy` is required for full protection.

## Definition

Validates if a URL belongs to any of a list of trusted domains.

## Overloads

| Method | Description |
| --------------- | --------------- |
| [inDomain(URL \| string, string): boolean](#indomainurl-url--string-domain-boolean) | Validates if a URL belongs to a trusted domain. |
| [inDomain(URL \| string, string[]): boolean](#indomainurl-url--string-domains-string-boolean) | Validates if a URL belongs to any of a list of trusted domains. |

## inDomain(URL | string, string): boolean

```js
inDomain(untrustedUrl: URL | string, trustedDomain: string): boolean
```

### Parameters

`untrustedUrl`: `URL | string`

The URL to be evaluated.

`trustedDomain`: `string`

The domain name that `untrustedUrl` will be compared against.

### Returns

* `true` if `untrustedUrl` belongs to `trustedDomain`.
* `false` if `untrustedUrl` does not belong to `trustedDomain`, if `untrustedUrl` cannot be converted to a valid `URL`, if protocol is not HTTP/S or WS/S, or if either argument is invalid.


## inDomain(URL | string, string[]): boolean

```js
inDomain(untrustedUrl: URL | string, trustedDomains: string[]): boolean
```

### Parameters

`untrustedUrl`: `URL | string`

The URL to be evaluated.

`trustedDomains`: `string[]`

The list of domain names that `untrustedUrl` will be compared against.

### Returns

* `true` if `untrustedUrl` belongs to any domain in `trustedDomains`.
* `false` if `untrustedUrl` does not belong to any domain in `trustedDomains`, if `untrustedUrl` cannot be converted to a valid `URL`, if protocol is not HTTP/S or WS/S, or if either argument is invalid.

## Examples

```javascript
const { URIValidator } = require('@microsoft/antissrf');

URIValidator.inDomain('https://api.mycompany.com/data', 'mycompany.com');
// → true

URIValidator.inDomain('https://api.mycompany.com/data', ['mycompany.com', 'trusted.com']);
// → true

URIValidator.inDomain('https://evil.com/secrets', 'mycompany.com');
// → false
```
