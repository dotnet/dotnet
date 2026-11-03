// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { isIPv4, isIPv6, SocketAddress } from "net";

import { AntiSSRFError } from "..";

// Exactly 1 zero OR a non-zero decimal digit followed by 0-2 decimal digits (max 3 chars)
const decRegex = /^(0|[1-9][0-9]{0,2})$/;

export class CIDRBlock {
    private _address: string;
    private _prefix: number;

    public getAddress(): string {
        return this._address;
    }

    public getPrefix(): number {
        return this._prefix;
    }

    /**
     * @internal
     * @throws AntiSSRFError If arguments are null or prefix is invalid
     */
    constructor(address: string, prefix: number) {
        if (address == null || prefix == null) {
            throw new AntiSSRFError("Null argument");
        }

        if (isNaN(prefix) || prefix < 0 || prefix > 128) {
            throw new AntiSSRFError("Invalid prefix");
        }

        if (!isIPv6(address)) {
            throw new AntiSSRFError(`Invalid IPv6 address: ${address}`);
        }

        this._address = address;
        this._prefix = prefix;
    }

    /**
     * @internal
     * @throws AntiSSRFError If the IP address is null or invalid
     */
    public static _parseIPAddress(ipaddress: string): [string, 4 | 6] {
        if (ipaddress == null) {
            throw new AntiSSRFError("Null argument");
        }

        if (ipaddress.includes(":")) {
            return [this._parseIPv6(ipaddress), 6];
        } else {
            return [this._parseIPv4(ipaddress), 4];
        }
    }

    /**
     * @internal
     * Returns a tuple with (IP address mapped to IPv6, the original IP version)
     * @throws AntiSSRFError If the CIDR string is null, malformed, or contains invalid components
     */
    public static _parseCIDR(cidr: string): CIDRBlock {
        if (cidr == null) {
            throw new AntiSSRFError("Null argument");
        }

        const parts = cidr.split("/");
        try {
            const [ipv6, oldVersion] = this._parseIPAddress(parts[0]);

            if (parts.length == 1) {
                return new CIDRBlock(ipv6, 128);
            } else if (parts.length == 2) {
                if (decRegex.test(parts[1])) {
                    const prefixLength = parseInt(parts[1], 10);
                    if (oldVersion === 4) {
                        // IPv4-mapped IPv6 address, adjust the prefix length accordingly
                        return new CIDRBlock(ipv6, prefixLength + 96);
                    } else {
                        return new CIDRBlock(ipv6, prefixLength);
                    }
                } else {
                    throw new AntiSSRFError(`Invalid prefix length: ${parts[1]}`);
                }
            } else {
                throw new AntiSSRFError(`Invalid CIDR block: ${cidr}`);
            }
        } catch {
            throw new AntiSSRFError(`Invalid CIDR block: ${cidr}`);
        }
    }

    /**
     * 1. x:x:x:x:x:x:x:x, where the 'x's are 1-4 hexadecimal digits
     * 2. The same as (1) with exactly 1 :: to compress 1+ consecutive groups
     *    of 0s.
     * 3. x:x:x:x:x:x:d.d.d.d, with or without compression in the xs, where the
     *    d.d.d.d is in standard IPv4 representation without leading 0s.
     * 4. [IPv6], to support IPv6 as hostnames
     */
    private static _parseIPv6(address: string): string {
        // Strip brackets if the address is in the form [IPv6]
        const len = address.length;
        if (len > 2 && address[0] == "[" && address[len - 1] == "]") {
            address = address.substring(1, len - 1);
        }

        if (isIPv6(address)) {
            try {
                const socketAddress = new SocketAddress({ address, family: "ipv6" });
                return socketAddress.address;
            } catch {
                throw new AntiSSRFError(`Invalid IPv6 address: ${address}`);
            }
        } else {
            throw new AntiSSRFError(`Invalid IPv6 address: ${address}`);
        }
    }

    /**
     * Node.js requires IPv4 addresses to be in dotted-quad notation, with
     * exactly 4 sections, without any leading 0s.
     */
    private static _parseIPv4(address: string): string {
        if (isIPv4(address)) {
            return `::FFFF:${address}`;
        } else {
            throw new AntiSSRFError(`Invalid IPv4 address: ${address}`);
        }
    }
}
