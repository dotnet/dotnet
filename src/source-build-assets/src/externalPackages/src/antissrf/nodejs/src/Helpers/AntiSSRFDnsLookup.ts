// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { lookup, LookupAddress, LookupAllOptions, LookupOneOptions } from "dns";
import { LookupFunction } from "net";

import { AntiSSRFError, AntiSSRFPolicy } from "..";

class LookupWithPolicy {
    private _policy: AntiSSRFPolicy;

    constructor(policy: AntiSSRFPolicy) {
        this._policy = policy;
    }

    private _lookupAll = (
        hostname: string,
        options: LookupAllOptions, // options.all == true
        callback: (err: NodeJS.ErrnoException | null, addresses: LookupAddress[]) => void
    ) => {
        return lookup(hostname, options, (err, addresses) => {
            // Errored in dns.lookup, forward error to callback
            if (err != null) {
                return callback(err, []);
            }

            // This case should error in dns.lookup, so we should never hit this
            /* istanbul ignore next */
            if (addresses == null) {
                return callback(new AntiSSRFError("Error in DNS lookup"), []);
            }

            // This case should never happen, since we only get to this function
            // if options.all == true, but we are including this to satisfy
            // typescript typechecking issues
            /* istanbul ignore next */
            if (!(addresses instanceof Array)) {
                return callback(new AntiSSRFError("Error in DNS lookup"), []);
            }

            // Handle the case where no valid address was found
            // [] instead of error for dns.lookup backwards compatibility
            if (addresses.length === 0) {
                return callback(null, []);
            }

            // If all addresses are allowed by policy, forward to callback
            if (this._policy._isNetworkConnectionAllowed(addresses.map((address) => address.address))) {
                return callback(null, addresses);
            }

            // If any address is disallowed by policy, return error
            return callback(new AntiSSRFError("IP address disallowed by policy"), []);
        });
    };

    private _lookupOne = (
        hostname: string,
        options: LookupOneOptions,
        callback: (err: NodeJS.ErrnoException | null, address: string | LookupAddress[], family: number) => void
    ) => {
        return lookup(hostname, options, (err, address, family) => {
            // Errored in dns.lookup, forward error to callback
            if (err != null) {
                return callback(err, null, family);
            }

            // Handle the case where no valid address was found
            // null instead of error for dns.lookup backwards compatibility
            if (address == null) {
                return callback(null, null, family);
            }

            // If the address is allowed by policy, forward to callback
            if (this._policy._isNetworkConnectionAllowed([address])) {
                return callback(null, address, family);
            }

            // If the address is disallowed by policy, return error
            return callback(new AntiSSRFError("IP address disallowed by policy"), null, family);
        });
    };

    /**
     * @internal
     */
    public _lookup: LookupFunction = (hostname, options, callback) => {
        if (options?.all == true) {
            return this._lookupAll(hostname, options as LookupAllOptions, callback);
        } else {
            return this._lookupOne(hostname, options as LookupOneOptions, callback);
        }
    };
}

/**
 * @internal
 * Intended for internal use only. Use AntiSSRFPolicy agents for public use.
 */
export function antiSSRFDnsLookup(policy: AntiSSRFPolicy): LookupFunction {
    if (policy == null) {
        throw new AntiSSRFError("Null argument");
    }

    const lookupWithPolicy = new LookupWithPolicy(policy);
    return lookupWithPolicy._lookup;
}
