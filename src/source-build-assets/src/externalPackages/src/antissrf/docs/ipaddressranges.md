---
layout: default
title: IP Address Ranges
nav_order: 2
description: "Predefined IP address ranges class"
---

# IPAddressRanges

## Definition

Provides predefined IP address ranges for internal and special-purpose addresses for use with AntiSSRF policies.

This list is consistent and shared across all the languages and frameworks.

## Example

To block all address ranges except for the IPv4 Private-Use ranges:

```cs
const policy = new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly);
policy.addAllowedAddresses(IPAddressRanges.privateUse);
```

## Special-Purpose Ranges

| Special Purpose | Variable Name | IP Address Ranges |
| --- | --- | --- |
| Automatic Multicast Tunneling | `IPAddressRanges.amt` | `192.52.193.0/24`, `2001:3::/32` |
| AS112 Service | `IPAddressRanges.as112` | `192.31.196.0/24`, `192.175.48.0/24`, `2001:4:112::/48`, `2620:4f:8000::/48` |
| Benchmarking | `IPAddressRanges.benchmarking` | `198.18.0.0/15`, `2001:2::/48` |
| Broadcast | `IPAddressRanges.broadcast` | `255.255.255.255/32` |
| Deprecated | `IPAddressRanges.deprecated` | `192.88.99.0/24`, `2001:10::/28` |
| Drone Remote ID Protocol Entity Tags (DETs) Prefix | `IPAddressRanges.detsPrefix` | `2001:30::/28` |
| Discard-Only | `IPAddressRanges.discardOnly` | `100::/64` |
| Documentation | `IPAddressRanges.documentation` | `192.0.2.0/24`, `198.51.100.0/24`, `203.0.113.0/24`, `2001:db8::/32`, `3fff::/20` |
| Dummy | `IPAddressRanges.dummy` | `192.0.0.8/32`, `100:0:0:1::/64` |
| IETF Protocol Assignments | `IPAddressRanges.ietfProtocol` | `192.0.0.0/24`, `2001::/23` |
| IMDS | `IPAddressRanges.imds` | `169.254.169.254/32` |
| IPv4/IPv6 Translation | `IPAddressRanges.ipv4Ipv6Translat` | `64:ff9b::/96`, `64:ff9b:1::/48` |
| IPv4 Service Continuity | `IPAddressRanges.ipv4ServiceContinuity` | `192.0.0.0/29` |
| Link-Local | `IPAddressRanges.linkLocal` | `169.254.0.0/16`, `fe80::/10` |
| Loopback | `IPAddressRanges.loopback` | `127.0.0.0/8`, `::1/128` |
| Multicast | `IPAddressRanges.multicast` | `224.0.0.0/4`, `ff00::/8` |
| ORCHIDv2 | `IPAddressRanges.orchidv2` | `2001:20::/28` |
| Private-Use | `IPAddressRanges.privateUse` | `10.0.0.0/8`, `172.16.0.0/12`, `192.168.0.0/16` |
| Reserved | `IPAddressRanges.reserved` | `240.0.0.0/4` |
| Shared Address Space | `IPAddressRanges.sharedAddressSpace` | `100.64.0.0/10` |
| Site-Local | `IPAddressRanges.siteLocal` | `fec0::/10` |
| 6to4 | `IPAddressRanges.sixto4` | `2002::/16` |
| Segment Routing (SRv6) SIDs | `IPAddressRanges.srv6Sid` | `5f00::/16` |
| Teredo | `IPAddressRanges.teredo` | `2001::/32` |
| Unique-Local | `IPAddressRanges.uniqueLocal` | `fc00::/7` |
| Unspecified | `IPAddressRanges.unspecified` | `0.0.0.0/8`, `::/128` |
| Wireserver | `IPAddressRanges.wireserver` | `168.63.129.16/32` |
| Recommended V1 | `IPAddressRanges.recommendedV1` | See Recommended Ranges V1 |
| Recommended Latest | `IPAddressRanges.recommendedLatest` | See Recommended Latest |

## IMDS (Instance Metadata Service)

The `IPAddressRanges.imds` range (`169.254.169.254/32`) provides access to Azure's Instance Metadata Service, which exposes sensitive information about the virtual machine including access tokens, compute metadata, and network configuration. If not blocked, attackers could use SSRF vulnerabilities to retrieve Azure credentials and escalate privileges within the cloud environment. 

Please ensure you add the `X-Forwarded-For` header on all outgoing requests whenever possible, either manually or using `addXFFHeader`. Requests to IMDS that include the `X-Forwarded-For` header will be safely dropped by the service as a security measure.

## Wireserver

The `IPAddressRanges.wireserver` range (`168.63.129.16/32`) is Azure's internal communication endpoint used for VM provisioning, health monitoring, and DNS resolution. If not blocked, attackers could potentially interfere with Azure's internal services, access configuration data, or disrupt VM operations through SSRF attacks.

## Recommended Ranges V1

The `IPAddressRanges.recommendedV1` contains the address ranges used by the constructor `AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1)`. This includes ALL internal and special-purpose addresses from above, simplified for efficiency when ranges overlap.

**IPv4 Ranges:**
- `0.0.0.0/8`
- `10.0.0.0/8`
- `100.64.0.0/10`
- `127.0.0.0/8`
- `168.63.129.16/32`
- `169.254.0.0/16`
- `172.16.0.0/12`
- `192.0.0.0/24`
- `192.0.2.0/24`
- `192.31.196.0/24`
- `192.52.193.0/24`
- `192.88.99.0/24`
- `192.168.0.0/16`
- `192.175.48.0/24`
- `198.18.0.0/15`
- `198.51.100.0/24`
- `203.0.113.0/24`
- `224.0.0.0/4`
- `240.0.0.0/4`

**IPv6 Ranges:**
- `::1/128`
- `::/128`
- `64:ff9b::/96`
- `64:ff9b:1::/48`
- `100::/64`
- `100:0:0:1::/64`
- `2001::/23`
- `2001:db8::/32`
- `2002::/16`
- `2620:4f:8000::/48`
- `3fff::/20`
- `5f00::/16`
- `fc00::/7`
- `fe80::/10`
- `fec0::/10`
- `ff00::/8`

## Recommended Ranges Latest

The `IPAddressRanges.recommendedLatest` contains the address ranges used by the constructor `AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest)`. 

{: .important }
> This policy operates independently of semantic versioning and ensures continuous alignment with current recommendations.

## For More Details

- [IANA IPv4 Special-Purpose Address Registry](https://www.iana.org/assignments/iana-ipv4-special-registry/iana-ipv4-special-registry.xhtml)
- [IANA IPv6 Special-Purpose Address Registry](https://www.iana.org/assignments/iana-ipv6-special-registry/iana-ipv6-special-registry.xhtml)
- [IANA IPv4 Multicast Address Space](https://www.iana.org/assignments/multicast-addresses/multicast-addresses.xhtml)
- [IANA IPv6 Multicast Address Space](https://www.iana.org/assignments/ipv6-multicast-addresses/ipv6-multicast-addresses.xhtml)
- [Microsoft Learn Azure Instance Metadata Service](https://learn.microsoft.com/en-us/azure/virtual-machines/instance-metadata-service?tabs=windows)
- [Microsoft Learn Azure IP Address 168.63.129.16 Overview](https://learn.microsoft.com/en-us/azure/virtual-network/what-is-ip-address-168-63-129-16?tabs=windows)
- [Microsoft Learn IPv6 Link-local and Site-local Addresses](https://learn.microsoft.com/en-us/windows/win32/winsock/link-local-and-site-local-addresses-2)