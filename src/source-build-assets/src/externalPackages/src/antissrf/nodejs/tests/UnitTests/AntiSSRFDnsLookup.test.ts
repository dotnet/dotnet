// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import { ADDRCONFIG, ALL, lookup, LookupAddress, LookupOptions, V4MAPPED } from "dns";
import { isIP, isIPv4, isIPv6, LookupFunction } from "net";
import { promisify } from "util";

import { antiSSRFDnsLookup } from "../../src/Helpers/AntiSSRFDnsLookup";
import { AntiSSRFPolicy, AntiSSRFError, PolicyConfigOptions } from "../../src";

/**
 * Converts a callback-based lookup function to a promise-based lookup function
 * for easier testing.
 */
const customPromisify = (lookup: LookupFunction) => (hostname: string, options: LookupOptions) =>
    new Promise((resolve, reject) => {
        lookup(hostname, options, (err, address, family) => {
            if (err) {
                return reject(err);
            }
            if (family != null) {
                return resolve({ address: address as string, family });
            } else {
                return resolve(address as LookupAddress[]);
            }
        });
    });

const optionsToString = (options: LookupOptions | null | undefined) => {
    if (!options) {
        return options;
    }

    return (
        "{" +
        Object.entries(options)
            .map(([key, value]) => {
                return `${key}: ${value}`;
            })
            .join(", ") +
        "}"
    );
};

/**
 * Asserts that the result of AntiSSRFDnsLookup matches the result of dns.lookup
 * for the given hostname and options.
 */
const AssertMatchResult = async (policy: AntiSSRFPolicy, hostname: string, options: LookupOptions) => {
    let expected;
    try {
        expected = await customPromisify(lookup)(hostname, options);
    } catch (err) {
        assert.fail(
            `Expected dns.lookup to not error: hostname - ${hostname}, options - ${optionsToString(options)}, error - ${err}`
        );
    }

    let actual;
    try {
        actual = await customPromisify(antiSSRFDnsLookup(policy))(hostname, options);
    } catch (err) {
        assert.fail(
            `Expected AntiSSRFDnsLookup to not error: hostname - ${hostname}, options - ${optionsToString(options)}, error - ${err}`
        );
    }

    // If both are arrays, compare them as sets (same elements, order doesn't matter)
    if (Array.isArray(actual) && Array.isArray(expected)) {
        const sortedActual = [...actual].sort((a, b) => JSON.stringify(a).localeCompare(JSON.stringify(b)));
        const sortedExpected = [...expected].sort((a, b) => JSON.stringify(a).localeCompare(JSON.stringify(b)));
        assert.deepStrictEqual(
            sortedActual,
            sortedExpected,
            `Expected results to match: hostname - ${hostname}, options - ${optionsToString(options)}`
        );
    } else if (
        actual &&
        expected &&
        typeof actual === "object" &&
        typeof expected === "object" &&
        "address" in actual &&
        "family" in actual &&
        "address" in expected &&
        "family" in expected &&
        (actual as any).address !== (expected as any).address &&
        (actual as any).family === (expected as any).family
    ) {
        // If families match but addresses differ, warn instead of fail
        console.warn(
            `Address mismatch (family match): hostname - ${hostname}, expected address - ${(expected as any).address}, actual address - ${(actual as any).address}, family - ${(actual as any).family}, options - ${optionsToString(options)}`
        );
    } else {
        assert.deepStrictEqual(
            actual,
            expected,
            `Expected results to match: hostname - ${hostname}, options - ${optionsToString(options)}`
        );
    }
};

/**
 * Asserts that the error from AntiSSRFDnsLookup matches the error from
 * dns.lookup for the given hostname and options.
 */
const AssertMatchError = async (policy: AntiSSRFPolicy, hostname: string, options: LookupOptions) => {
    let expectedError: Error | null = null;
    try {
        await customPromisify(lookup)(hostname, options);
    } catch (err) {
        expectedError = err as Error;
    }
    if (!expectedError) {
        assert.fail(`Expected dns.lookup to error: hostname - ${hostname}, options - ${optionsToString(options)}`);
    }

    let actualError: Error | null = null;
    try {
        await customPromisify(antiSSRFDnsLookup(policy))(hostname, options);
    } catch (err) {
        actualError = err as Error;
    }
    if (!actualError) {
        assert.fail(
            `Expected AntiSSRFDnsLookup to error: hostname - ${hostname}, options - ${optionsToString(options)}`
        );
    }

    assert.deepStrictEqual(
        actualError,
        expectedError,
        `Expected errors to match: hostname - ${hostname}, options - ${optionsToString(options)}`
    );
};

/**
 * Asserts that the error from AntiSSRFDnsLookup matches the error from
 * dns.lookup for the given hostname and options OR asserts that the result of
 * AntiSSRFDnsLookup matches the result of dns.lookup for the given hostname and
 * options.
 *
 * Only used for hostnames/options that behave difference between local and
 * Azure pipeline tests.
 */
const AssertMatchResultOrError = async (policy: AntiSSRFPolicy, hostname: string, options: LookupOptions) => {
    let expectedResult;
    let expectedError: Error | null = null;
    try {
        expectedResult = await customPromisify(lookup)(hostname, options);
    } catch (err) {
        expectedError = err as Error;
    }

    let actualResult;
    let actualError: Error | null = null;
    try {
        actualResult = await customPromisify(antiSSRFDnsLookup(policy))(hostname, options);
    } catch (err) {
        actualError = err as Error;
    }

    if (expectedError) {
        assert.deepStrictEqual(
            actualError,
            expectedError,
            `Expected errors to match: hostname - ${hostname}, options - ${optionsToString(options)}`
        );
    } else {
        if (Array.isArray(actualResult) && Array.isArray(expectedResult)) {
            const sortedActual = [...actualResult].sort((a, b) => JSON.stringify(a).localeCompare(JSON.stringify(b)));
            const sortedExpected = [...expectedResult].sort((a, b) =>
                JSON.stringify(a).localeCompare(JSON.stringify(b))
            );
            assert.deepStrictEqual(
                sortedActual,
                sortedExpected,
                `Expected results to match: hostname - ${hostname}, options - ${optionsToString(options)}`
            );
        } else if (
            actualResult &&
            expectedResult &&
            typeof actualResult === "object" &&
            typeof expectedResult === "object" &&
            "address" in actualResult &&
            "family" in actualResult &&
            "address" in expectedResult &&
            "family" in expectedResult &&
            (actualResult as any).address !== (expectedResult as any).address &&
            (actualResult as any).family === (expectedResult as any).family
        ) {
            // If families match but addresses differ, warn instead of fail
            console.warn(
                `Address mismatch (family match): hostname - ${hostname}, expected address - ${(expectedResult as any).address}, actual address - ${(actualResult as any).address}, family - ${(actualResult as any).family}, options - ${optionsToString(options)}`
            );
        } else {
            assert.deepStrictEqual(
                actualResult,
                expectedResult,
                `Expected results to match: hostname - ${hostname}, options - ${optionsToString(options)}`
            );
        }
    }
};

describe("AntiSSRFDnsLookup", () => {
    describe("Bad inputs", () => {
        it("Null hostname", async () => {
            // Different behavior across versions of NodeJS
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            await AssertMatchResultOrError(policy, null as unknown as string, {});
            await AssertMatchResultOrError(policy, null as unknown as string, { all: false });
            await AssertMatchResultOrError(policy, null as unknown as string, { all: true });
            await AssertMatchResultOrError(policy, null as unknown as string, { family: 0 });
            await AssertMatchResultOrError(policy, null as unknown as string, { family: 6 });
        });

        it("Undefined hostname", async () => {
            // Different behavior across versions of NodeJS
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            await AssertMatchResultOrError(policy, undefined as unknown as string, {});
            await AssertMatchResultOrError(policy, undefined as unknown as string, { all: false });
            await AssertMatchResultOrError(policy, undefined as unknown as string, { all: true });
            await AssertMatchResultOrError(policy, undefined as unknown as string, { family: 4 });
            await AssertMatchResultOrError(policy, undefined as unknown as string, { family: 6 });
            await AssertMatchResultOrError(policy, undefined as unknown as string, { family: 0, all: true });
        });

        it("Generally bad hostname", async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            await AssertMatchError(policy, "hello", {});
            await AssertMatchError(policy, "https://google.com", { all: false });
            await AssertMatchError(policy, "https://www.google.com", null as any);
            await AssertMatchError(policy, "google.com:60", { all: true });
            await AssertMatchError(policy, "google.com/path", { family: 4 });
            await AssertMatchError(policy, "google.com/search?q=hi", { family: 6 });
            await AssertMatchError(policy, "username@sup.com", { family: 0, all: true });
            await AssertMatchError(policy, "#fragment", null as any);
        });

        it("Bad options", async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            const hostname = "bing.com";

            // options.all must be true or false
            await AssertMatchError(policy, hostname, { all: 1 as unknown as boolean });

            // options.family must by 0, 4, 6, "IPv4", or "IPv6"
            await AssertMatchError(policy, hostname, { family: 3 as unknown as 0 });
            await AssertMatchError(policy, hostname, { family: "IPv0" as unknown as "IPv4" });

            // options.hints can only be specific flags
            await AssertMatchError(policy, hostname, { hints: -1 });

            // options.order can only be "verbatim", "ipv4first", or "ipv6first"
            // Behavior different across environments
            await AssertMatchResultOrError(policy, hostname, { order: "NotAnOrder" as unknown as "verbatim" });

            // options.verbatim can only be true or false
            await AssertMatchError(policy, hostname, { verbatim: 1 as unknown as boolean });
        });
    });

    const hostnames = ["google.com", "bing.com", "learn.microsoft.com"];

    it("Lookup options - order", async () => {
        // order: verbatim, ipv4first, ipv6first, undefined
        // checking ipv4first and ipv6first work as expected.
        // only checking verbatim and undefined have the right elements.
        const promisified = promisify(antiSSRFDnsLookup(new AntiSSRFPolicy(PolicyConfigOptions.None)));

        for (const hostname of hostnames) {
            const addresses_ipv4first = await promisified(hostname, { all: true, order: "ipv4first" }) as LookupAddress[];
            assert.ok(
                addresses_ipv4first.every((addr, i, a) => i === 0 || a[i - 1].family <= addr.family),
                `Addresses should be sorted with IPv4 first, but got ${JSON.stringify(addresses_ipv4first, null, 2)}`
            );

            const addresses_ipv6first = await promisified(hostname, { all: true, order: "ipv6first" }) as LookupAddress[];
            assert.ok(
                addresses_ipv6first.every((addr, i, a) => i === 0 || a[i - 1].family >= addr.family),
                `Addresses should be sorted with IPv6 first, but got ${JSON.stringify(addresses_ipv6first, null, 2)}`
            );

            const addresses_verbatim = await promisified(hostname, { all: true, order: "verbatim" }) as LookupAddress[];
            const addresses_undefined = await promisified(hostname, { all: true, order: undefined as unknown as "verbatim" }) as LookupAddress[];

            const sorted_ipv4first = addresses_ipv4first.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_ipv6first = addresses_ipv6first.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_verbatim = addresses_verbatim.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_undefined = addresses_undefined.map((addr) => `${addr.address}|${addr.family}`).sort();

            assert.deepStrictEqual(sorted_ipv4first, sorted_ipv6first, `${hostname}: ipv4first and ipv6first returned different address sets`);
            assert.deepStrictEqual(sorted_ipv4first, sorted_verbatim, `${hostname}: ipv4first and verbatim returned different address sets`);
            assert.deepStrictEqual(sorted_ipv4first, sorted_undefined, `${hostname}: ipv4first and undefined-order returned different address sets`);
        }
    });

    it("Lookup options - verbatim", async () => {
        // verbatim: true, false, undefined
        // only checking they all have the right elements.
        const promisified = promisify(antiSSRFDnsLookup(new AntiSSRFPolicy(PolicyConfigOptions.None)));

        for (const hostname of hostnames) {
            const addresses_true = await promisified(hostname, { all: true, verbatim: true }) as LookupAddress[];
            const addresses_false = await promisified(hostname, { all: true, verbatim: false }) as LookupAddress[];
            const addresses_undefined = await promisified(hostname, { all: true, verbatim: undefined as unknown as boolean }) as LookupAddress[];

            const sorted_true = addresses_true.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_false = addresses_false.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_undefined = addresses_undefined.map((addr) => `${addr.address}|${addr.family}`).sort();

            assert.deepStrictEqual(sorted_true, sorted_false, `${hostname}: verbatim=true and verbatim=false returned different address sets`);
            assert.deepStrictEqual(sorted_true, sorted_undefined, `${hostname}: verbatim=true and verbatim=undefined returned different address sets`);
        }
    });

    it("Lookup options - family", async () => {
        // family: 0, 4, 6, IPv4, IPv6, undefined
        // checking:
        // 1) family=0 includes both families when family=4/family=6 resolve (DNS may rotate exact IPs)
        // 2) family=undefined matches family=0
        // 3) family=4 and family=IPv4 match, all IPv4 addresses
        // 4) family=6 and family=IPv6 match, all IPv6 addresses
        const promisified = promisify(antiSSRFDnsLookup(new AntiSSRFPolicy(PolicyConfigOptions.None)));

        for (const hostname of hostnames) {
            const addresses_0 = await promisified(hostname, { all: true, family: 0, hints: ALL }) as LookupAddress[];
            const addresses_4 = await promisified(hostname, { all: true, family: 4, hints: ALL }) as LookupAddress[];
            const addresses_6 = await promisified(hostname, { all: true, family: 6, hints: ALL | V4MAPPED }) as LookupAddress[];
            const addresses_ipv4 = await promisified(hostname, {
                all: true,
                family: "IPv4",
                hints: ALL
            }) as LookupAddress[];
            const addresses_ipv6 = await promisified(hostname, {
                all: true,
                family: "IPv6",
                hints: ALL | V4MAPPED
            }) as LookupAddress[];
            const addresses_undefined = await promisified(hostname, {
                all: true,
                family: undefined as unknown as 0,
                hints: ALL
            }) as LookupAddress[];

            assert.ok(
                addresses_4.every((addr) => addr.family === 4 && isIPv4(addr.address)),
                `${hostname}: family=4 returned non-IPv4 address(es): ${JSON.stringify(addresses_4, null, 2)}`
            );
            assert.ok(
                addresses_6.every((addr) => addr.family === 6 && isIPv6(addr.address)),
                `${hostname}: family=6 returned non-IPv6 address(es): ${JSON.stringify(addresses_6, null, 2)}`
            );

            const sorted_0 = addresses_0.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_4 = addresses_4.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_6 = addresses_6.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_ipv4 = addresses_ipv4.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_ipv6 = addresses_ipv6.map((addr) => `${addr.address}|${addr.family}`).sort();
            const sorted_undefined = addresses_undefined.map((addr) => `${addr.address}|${addr.family}`).sort();

            const has_ipv4_in_0 = addresses_0.some((addr) => addr.family === 4);
            const has_ipv6_in_0 = addresses_0.some((addr) => addr.family === 6);

            if (addresses_4.length > 0) {
                assert.ok(has_ipv4_in_0, `${hostname}: family=0 should include IPv4 results when family=4 resolves`);
            }
            if (addresses_6.length > 0) {
                assert.ok(has_ipv6_in_0, `${hostname}: family=0 should include IPv6 results when family=6 resolves`);
            }

            assert.deepStrictEqual(sorted_0, sorted_undefined, `${hostname}: family=0 and family=undefined returned different address sets`);
            assert.deepStrictEqual(sorted_4, sorted_ipv4, `${hostname}: family=4 and family=IPv4 returned different address sets`);
            assert.deepStrictEqual(sorted_6, sorted_ipv6, `${hostname}: family=6 and family=IPv6 returned different address sets`);
        }
    });

    it("Lookup options - all", async () => {
        // all: true, false, undefined
        // checking:
        // 1) all=false returns a single IP string and is contained in all=true results
        // 2) all=undefined returns a single IP string and is contained in all=true results
        // 3) all=false and all=undefined do not need to return the same address
        const promisified = promisify(antiSSRFDnsLookup(new AntiSSRFPolicy(PolicyConfigOptions.None)));

        for (const hostname of hostnames) {
            const addresses_true = await promisified(hostname, { all: true }) as LookupAddress[];
            const address_false = await promisified(hostname, { all: false }) as string;
            const address_undefined = await promisified(hostname, {
                all: undefined as unknown as boolean
            }) as string;

            assert.ok(
                addresses_true.some(
                    (addr) => addr.address === address_false && addr.family === isIP(address_false)
                ),
                `${hostname}: all=false address ${address_false} (family ${isIP(address_false)}) was not found in all=true results ${JSON.stringify(addresses_true, null, 2)}`
            );

            assert.ok(
                addresses_true.some(
                    (addr) => addr.address === address_undefined && addr.family === isIP(address_undefined)
                ),
                `${hostname}: all=undefined address ${address_undefined} (family ${isIP(address_undefined)}) was not found in all=true results ${JSON.stringify(addresses_true, null, 2)}`
            );
        }
    });

    /**
     * All addresses are allowed, so dns.lookup and AntiSSRFDnsLookup should
     * always return the same result or throw the same error.
     */
    describe("Lookup with accepting policy", () => {
        // If all addresses are allowed, dns.lookup and AntiSSRFDnsLookup should be the same
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);

        const OPT_HINTS: number[] = [
            V4MAPPED, // 2048
            ALL, // 256
            ADDRCONFIG, // 1024
            V4MAPPED | ALL,
            V4MAPPED | ADDRCONFIG,
            ALL | ADDRCONFIG,
            V4MAPPED | ALL | ADDRCONFIG,
            undefined as unknown as number
        ];

        const hostnames = ["google.com", "bing.com", "learn.microsoft.com", "azure.com", "github.com"];
        for (const hostname of hostnames) {
            it(`Common domain tests - ${hostname}`, async () => {
                for (const hints of OPT_HINTS) {
                    await AssertMatchResult(policy, hostname, {
                        hints
                    });
                }
            });
        }
    });

    describe("Lookup with policy functionality", () => {
        it("Default policy", async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
            const promisifiedAntiSSRFLookup = customPromisify(antiSSRFDnsLookup(policy));

            // Allowed by policy
            await AssertMatchResult(policy, "google.com", { family: 4 });
            await AssertMatchResult(policy, "yAhOo.com", { family: 4, all: false });

            // Disallowed by policy - IMDS
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("169.254.169.254", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("0xA9.0Xfe.0xA9.0xFe", { family: 4 }),
                (err: Error) => {
                    if (process.platform === 'win32') {
                        return err.message.includes("getaddrinfo ENOTFOUND 0xA9.0Xfe.0xA9.0xFe");
                    } else {
                        return err.message === "IP address disallowed by policy";
                    }
                }
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("169.254.169.254", { family: 6 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("::FFFF:169.254.169.254", { family: 4 }),
                AntiSSRFError
            );

            // Rejects with different error in local vs Azure pipeline
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("imds.michaelhendrickx.com", { family: 0, all: true })
            );

            // Disallowed by policy - WireServer
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("168.63.129.16", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("0xA8.0X3F.0x81.0x10", { family: 4 }),
                (err: Error) => {
                    if (process.platform === 'win32') {
                        return err.message.includes("getaddrinfo ENOTFOUND 0xA8.0X3F.0x81.0x10");
                    } else {
                        return err.message === "IP address disallowed by policy";
                    }
                }
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("168.63.129.16", { family: 6 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("::FFFF:168.63.129.16", { family: 4 }),
                AntiSSRFError
            );

            // Disallowed by policy - localhost
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("localhost", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("localhost", { family: 6 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("127.0.0.1", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("127.0.0.1", { family: 6 }),
                AntiSSRFError
            );

            // Disallowed by policy - other
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("100.64.0.10", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("100.64.0.10", { family: 6 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("::FFFF:100.64.0.10", { family: 4 }),
                AntiSSRFError
            );
            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("::FFFF:100.64.0.10", { family: 6 }),
                AntiSSRFError
            );

            // More allowed by policy
            await AssertMatchResult(policy, "bing.com", { family: 4 });
            await AssertMatchResult(policy, "microsoft.com", { family: 0 });
            await AssertMatchResult(policy, "docs.github.com", { family: 0, all: true });
            await AssertMatchResult(policy, "223.6.7.8", { family: 0, all: true });
            await AssertMatchResult(policy, "::fffF:223.6.7.8", { family: 0, all: true });
        });

        it("addAllowedAddresses", async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
            const promisifiedAntiSSRFLookup = customPromisify(antiSSRFDnsLookup(policy));

            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("169.254.0.2", { all: true }),
                AntiSSRFError
            );

            policy.addAllowedAddresses(["169.254.0.2"]);

            await AssertMatchResult(policy, "169.254.0.2", null as any);
            await AssertMatchResult(policy, "google.com", null as any);
        });

        it("addDeniedAddresses", async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            const promisifiedAntiSSRFLookup = customPromisify(antiSSRFDnsLookup(policy));

            await AssertMatchResult(policy, "192.168.0.0", null as any);

            policy.addDeniedAddresses(["192.168.0.0"]);

            await assert.rejects(
                async () => await promisifiedAntiSSRFLookup("192.168.0.0", null as any),
                AntiSSRFError
            );
            await AssertMatchResult(policy, "google.com", null as any);
        });
    });
});
