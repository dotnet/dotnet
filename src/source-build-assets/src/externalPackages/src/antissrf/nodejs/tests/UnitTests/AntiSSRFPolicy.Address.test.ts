// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import axios from "axios";
import { promises } from "dns";

import { AntiSSRFError, AntiSSRFPolicy, IPAddressRanges, PolicyConfigOptions } from "../../src";

describe("AntiSSRFPolicy Address Tests", function () {
    this.timeout(30000);
    const BAD_IP_MESSAGE = "IP address disallowed by policy";
    const TEST_DOMAIN = "ambitious-flower-0611c910f.2.azurestaticapps.net";

    let instance1: axios.AxiosInstance;
    let instance2: axios.AxiosInstance;
    let instance3: axios.AxiosInstance;
    let instance4: axios.AxiosInstance;

    before(() => {
        const policy1 = new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly);
        policy1.allowPlainTextHttp = true;
        instance1 = axios.create({
            httpAgent: policy1.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy1.getHttpsAgent({ keepAlive: false })
        });

        const policy2 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
        policy2.allowPlainTextHttp = true;
        instance2 = axios.create({
            httpAgent: policy2.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy2.getHttpsAgent({ keepAlive: false })
        });

        const policy3 = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
        policy3.allowPlainTextHttp = true;
        instance3 = axios.create({
            httpAgent: policy3.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy3.getHttpsAgent({ keepAlive: false })
        });

        const policy4 = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy4.allowPlainTextHttp = true;
        instance4 = axios.create({
            httpAgent: policy4.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy4.getHttpsAgent({ keepAlive: false }),
            timeout: 1,
            signal: AbortSignal.timeout(1)
        });
    });

    it("bad inputs", () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.denyAllUnspecifiedIPs = true;
        assert.throws(
            () => policy.addDeniedAddresses(["1.2.3.4"]),
            AntiSSRFError,
            "Expected addDeniedAddresses to throw when denyAllUnspecifiedIPs is true"
        );

        // Test null arrays
        const policy2 = new AntiSSRFPolicy(PolicyConfigOptions.None);
        assert.throws(
            () => policy2.addAllowedAddresses(null as any),
            AntiSSRFError,
            "Expected addAllowedAddresses(null) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addAllowedAddresses([null as any]),
            AntiSSRFError,
            "Expected addAllowedAddresses([null]) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addAllowedAddresses(undefined as any),
            AntiSSRFError,
            "Expected addAllowedAddresses(undefined) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addAllowedAddresses([undefined as any]),
            AntiSSRFError,
            "Expected addAllowedAddresses([undefined]) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy2.addDeniedAddresses(null as any),
            AntiSSRFError,
            "Expected addDeniedAddresses(null) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addDeniedAddresses([null as any]),
            AntiSSRFError,
            "Expected addDeniedAddresses([null]) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addDeniedAddresses(undefined as any),
            AntiSSRFError,
            "Expected addDeniedAddresses(undefined) to throw AntiSSRFError"
        );
        assert.throws(
            () => policy.addDeniedAddresses([undefined as any]),
            AntiSSRFError,
            "Expected addDeniedAddresses([undefined]) to throw AntiSSRFError"
        );

        // Test empty arrays
        policy2.addAllowedAddresses([]);
        policy2.addDeniedAddresses([]);

        // Test invalid IP address formats
        assert.throws(
            () => policy2.addDeniedAddresses(["invalid.ip.address"]),
            AntiSSRFError,
            "Expected addDeniedAddresses to throw for invalid IP format"
        );
        assert.throws(
            () => policy2.addDeniedAddresses(["256.256.256.256/24"]),
            AntiSSRFError,
            "Expected addDeniedAddresses to throw for out-of-range IPv4 values"
        );
        assert.throws(
            () => policy2.addDeniedAddresses(["192.168.1.1/33"]),
            AntiSSRFError,
            "Expected addDeniedAddresses to throw for invalid IPv4 prefix length"
        );
        assert.throws(
            () => policy2.addAllowedAddresses(["not-an-ip"]),
            AntiSSRFError,
            "Expected addAllowedAddresses to throw for invalid IP format"
        );

        // Test array containing null addresses
        const policy3 = new AntiSSRFPolicy(PolicyConfigOptions.None);
        assert.throws(
            () => policy3.addDeniedAddresses(["192.168.1.0/24", null!, "10.0.0.0/8"]),
            AntiSSRFError,
            "Expected addDeniedAddresses to throw when address list contains null"
        );
        assert.throws(
            () => policy3.addAllowedAddresses([null!]),
            AntiSSRFError,
            "Expected addAllowedAddresses to throw when address list contains null"
        );
    });

    it("check defaults IMDS", async () => {
        const urls = [
            "https://169.254.169.254/latest/meta-data/",
            "https://0xA9.0xFE.0xA9.0xFE/latest/meta-data/",
            "https://[::ffff:169.254.169.254]/latest/meta-data/",
            "https://[::ffff:A9FE:A9FE]/latest/meta-data/",
            `https://${TEST_DOMAIN}/api/imds-ip?code=301`,
            `https://${TEST_DOMAIN}/api/imds-ip?code=302`,
            `https://${TEST_DOMAIN}/api/imds?redirectNum=3`
        ];

        for (const url of urls) {
            await assert.rejects(
                async () => {
                    await instance1.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance2.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance3.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            try {
                await instance4.get(url);
            } catch (err) {
                assert.notEqual(
                    (err as Error).message,
                    BAD_IP_MESSAGE,
                    `Expected non-AntiSSRFError for URL: ${url} but got ${err}`
                );
            }
        }
    });

    it("check defaults wireserver", async () => {
        const urls = [
            "http://168.63.129.16/",
            "http://0xA8.0x3F.0x81.0x10/",
            "http://[::ffff:168.63.129.16]/",
            "http://[::ffff:A83F:8110]/",
            `https://${TEST_DOMAIN}/api/wireserver`
        ];

        for (const url of urls) {
            await assert.rejects(
                async () => {
                    await instance1.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance2.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance3.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            try {
                await instance4.get(url);
            } catch (err) {
                assert.notEqual(
                    (err as Error).message,
                    BAD_IP_MESSAGE,
                    `Expected non-AntiSSRFError for URL: ${url} but got ${err}`
                );
            }
        }
    });

    it("check defaults localhost", async () => {
        const urls = [
            "http://127.0.0.1/",
            "http://0x7F.0x0.0x0.0x1/",
            "http://[::ffff:127.0.0.1]/",
            "http://[::ffff:7F00:1]/",
            `https://${TEST_DOMAIN}/api/localhost`,
            "http://localhost/"
        ];

        for (const url of urls) {
            await assert.rejects(
                async () => {
                    await instance1.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance2.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            await assert.rejects(
                async () => {
                    await instance3.get(url);
                },
                (err: Error) => {
                    return err.message === BAD_IP_MESSAGE;
                },
                `Expected AntiSSRFError for URL: ${url}`
            );
            try {
                await instance4.get(url);
            } catch (err) {
                assert.notEqual(
                    (err as Error).message,
                    BAD_IP_MESSAGE,
                    `Expected non-AntiSSRFError for URL: ${url} but got ${err}`
                );
            }
        }
    });

    it("default with IpAddressRanges", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
        policy.addAllowedAddresses([
            ...IPAddressRanges.imds,
            ...IPAddressRanges.wireserver,
            ...IPAddressRanges.loopback
        ]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true,
            timeout: 1,
            signal: AbortSignal.timeout(1)
        });

        try {
            await instance.get(`http://${TEST_DOMAIN}/api/localhost`);
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://${TEST_DOMAIN}/api/localhost but got ${err}`
            );
        }

        try {
            await instance.get(`http://${TEST_DOMAIN}/api/wireserver`);
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://${TEST_DOMAIN}/api/wireserver but got ${err}`
            );
        }

        try {
            await instance.get(`http://${TEST_DOMAIN}/api/imds`);
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://${TEST_DOMAIN}/api/imds but got ${err}`
            );
        }

        try {
            await instance.get("http://127.0.0.1");
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://127.0.0.1 but got ${err}`
            );
        }

        try {
            await instance.get("http://168.63.129.16");
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://168.63.129.16 but got ${err}`
            );
        }

        try {
            await instance.get("http://169.254.169.254");
        } catch (err) {
            assert.notEqual(
                (err as Error).message,
                BAD_IP_MESSAGE,
                `Expected non-AntiSSRFError for URL: http://169.254.169.254 but got ${err}`
            );
        }
    });

    it("allow IPv4 addresses", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.denyAllUnspecifiedIPs = true;
        const testIpArr = await promises.lookup(TEST_DOMAIN, { family: 4, all: true });
        policy.addAllowedAddresses(testIpArr.map((ip) => ip.address));
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Allowed IPv4 - allowed by policy
        await assert.doesNotReject(async () => {
            await instance.get(`http://${testIpArr[0].address}`);
        });

        // Allowed IPv4-mapped IPv6 - allowed by policy but might fail on some systems due to IPv6 handling
        try {
            await instance.get(`http://[::ffff:${testIpArr[0].address}]:80`);
        } catch (err) {
            assert.notEqual((err as Error).message, BAD_IP_MESSAGE);
        }

        // Disallowed IPv4 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("http://1.2.3.4");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected request to disallowed IPv4 address to be rejected"
        );

        // Disallowed IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("http://[1:2:3:4:5:6:7:8]");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected request to disallowed IPv6 address to be rejected"
        );
    });

    it("allow IPv4-mapped IPv6 addresses", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.denyAllUnspecifiedIPs = true;
        const testIpArr = await promises.lookup(TEST_DOMAIN, { family: 4, all: true });
        policy.addAllowedAddresses(testIpArr.map((ip) => `::ffff:${ip.address}`));
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Allowed IPv4 - allowed by policy
        await assert.doesNotReject(async () => {
            await instance.get("http://" + testIpArr[0].address);
        });

        // Allowed IPv4-mapped IPv6 - allowed by policy but might fail on some systems due to IPv6 handling
        try {
            await instance.get(`http://[::ffff:${testIpArr[0].address}]:80`);
        } catch (err) {
            assert.notEqual((err as Error).message, BAD_IP_MESSAGE);
        }

        // Disallowed IPv4 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("http://1.2.3.4");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected request to disallowed IPv4 address to be rejected"
        );

        // Disallowed IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("http://[1:2:3:4:5:6:7:8]");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected request to disallowed IPv6 address to be rejected"
        );
    });

    it("allow IPv6 addresses", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.denyAllUnspecifiedIPs = true;
        const testIPv6 = "::1";
        policy.addAllowedAddresses([testIPv6]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Allowed IPv6 - allowed by policy but might fail on some systems due to IPv6 handling
        try {
            await instance.get(`http://[${testIPv6}]`);
        } catch (err) {
            assert.notEqual((err as Error).message, BAD_IP_MESSAGE);
        }

        // Disallowed IPv4 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("https://1.2.3.4");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected disallowed IPv4 request to be rejected when only IPv6 is allowed"
        );

        // Disallowed different IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get("https://[2606:4700:4700::1111]");
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            "Expected disallowed IPv6 request to be rejected when only ::1 is allowed"
        );
    });

    it("deny IPv4 address", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        const testIpArr = await promises.lookup(TEST_DOMAIN, { family: 4, all: true });
        policy.addDeniedAddresses(testIpArr.map((ip) => ip.address));
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Denied IPv4 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get(`https://${testIpArr[0].address}`);
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            `Expected denied IPv4 request to be rejected: ${testIpArr[0].address}`
        );

        // Denied IPv4-mapped IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get(`https://[::ffff:${testIpArr[0].address}]`);
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            `Expected denied IPv4-mapped IPv6 request to be rejected: ::ffff:${testIpArr[0].address}`
        );

        // Allowed different IPv4 - allowed by policy
        await assert.doesNotReject(async () => {
            await instance.get("https://github.com");
        });

        // Allowed IPv6 - allowed by policy but might fail on some systems due to IPv6 handling
        try {
            await instance.get("https://ipv6.google.com");
        } catch (err) {
            assert.notEqual((err as Error).message, BAD_IP_MESSAGE);
        }
    });

    it("deny IPv4-mapped IPv6 address", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        const testIpArr = await promises.lookup(TEST_DOMAIN, { family: 4, all: true });
        policy.addDeniedAddresses(testIpArr.map((ip) => ip.address));
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Denied IPv4 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get(`http://${testIpArr[0].address}`);
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            `Expected denied IPv4 request to be rejected over HTTP: ${testIpArr[0].address}`
        );

        // Denied IPv4-mapped IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get(`http://[::ffff:${testIpArr[0].address}]`);
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            `Expected denied IPv4-mapped IPv6 request to be rejected over HTTP: ::ffff:${testIpArr[0].address}`
        );

        // Allowed different IPv4 - allowed by policy
        await assert.doesNotReject(async () => {
            await instance.get("https://github.com");
        });

        // Allowed IPv6 - allowed by policy but might fail on some systems due to IPv6 handling
        try {
            await instance.get("https://ipv6.google.com");
        } catch (err) {
            assert.notEqual((err as Error).message, BAD_IP_MESSAGE);
        }
    });

    it("deny IPv6 address", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        const testIPv6 = "2001:4860:4860::8888";
        policy.addDeniedAddresses([testIPv6]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        // Denied IPv6 - not allowed by policy
        await assert.rejects(
            async () => {
                await instance.get(`http://[${testIPv6}]`);
            },
            (err: Error) => {
                return err.message === BAD_IP_MESSAGE;
            },
            `Expected denied IPv6 request to be rejected: ${testIPv6}`
        );

        // Allowed IPv4 - allowed by policy
        await assert.doesNotReject(async () => {
            await instance.get("https://github.com");
        });
    });

    it("both allow and deny", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        const testIps = await promises.lookup(TEST_DOMAIN, { all: true });
        policy.addDeniedAddresses(testIps.map((ip) => ip.address));
        policy.addAllowedAddresses(testIps.map((ip) => ip.address));
        const instance = axios.create({
            httpAgent: policy.getHttpAgent({ keepAlive: false }),
            httpsAgent: policy.getHttpsAgent({ keepAlive: false }),
            validateStatus: () => true
        });

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}`);
        });
    });

    describe("direct IPs - wireserver", () => {
        // IPv4: 168.63.129.16 = 0xA83F8110, IPv6: N/A
        const wireServerIPs = [
            "168.63.129.16",

            // IPv4-mapped-IPv6
            "::FFFF:168.63.129.16",
            "[0:0:0:0:0:FFFF:A83F:8110]"
        ];

        const badWireserverIPs = [
            // dddd
            "0xA83f8110", // hex
            "2822734096", // dec
            "025017700420", // oct

            // d.ddd
            "0xA8.0x3F8110", // hex
            "168.4161808", // dec
            "0250.017700420", // oct
            "168.017700420", // mixed
            "0250.4161808",

            // d.d.dd
            "0xA8.0x3F.0x8110", // hex
            "168.63.33040", // dec
            "0250.077.0100420", // oct
            "168.077.33040", // mixed
            "0250.0x3f.0100420",

            // d.d.d.d
            "0xA8.0x3F.0x81.0x10", // hex
            "0250.077.0201.020", // oct
            "168.077.0x81.16", // mixed
            "0250.0x3f.0201.020"
        ];

        it("defaults", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);

            for (const ip of wireServerIPs) {
                assert.equal(policy._isNetworkConnectionAllowed([ip]), false);
            }

            for (const ip of badWireserverIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });

        it("explicit deny", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.addDeniedAddresses(["168.63.129.16"]);

            for (const ip of wireServerIPs) {
                assert.equal(policy._isNetworkConnectionAllowed([ip]), false);
            }

            for (const ip of badWireserverIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });

        it("explicit allow", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            policy.addAllowedAddresses(["168.63.129.16"]);

            for (const ip of wireServerIPs) {
                assert.equal(policy._isNetworkConnectionAllowed([ip]), true);
            }

            for (const ip of badWireserverIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });
    });

    describe("direct IPs - IMDS", () => {
        // IPv4: 169.254.169.254 = 0xA9FEA9FE, IPv6: N/A

        const imdsIPs = [
            "169.254.169.254",

            // IPv4-mapped-IPv6
            "::FFFF:169.254.169.254",
            "[0:0:0:0:0:FFFF:A9FE:A9FE]"
        ];

        const badImdsIPs = [
            // dddd
            "0xA9FEA9FE", // hex
            "2852039166", // dec
            "025177524776", // oct

            // d.ddd
            "0xA9.0xFEA9FE", // hex
            "169.16689662", // dec
            "0251.077524776", // oct
            "169.077524776", // mixed
            "0251.16689662",

            // d.d.dd
            "0xA9.0xFE.0xA9FE", // hex
            "169.254.43518", // dec
            "0251.0376.0124776", // oct
            "169.0376.43518", // mixed
            "0251.0xfe.0124776",

            // d.d.d.d
            "0xA9.0xFE.0xA9.0xFE", // hex
            "0251.0376.0251.0376", // oct
            "169.0376.0xA9.254", // mixed
            "0251.0xfe.0251.0376"
        ];

        it("defaults", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);

            for (const ip of imdsIPs) {
                assert.equal(policy._isNetworkConnectionAllowed([ip]), false);
            }

            for (const ip of badImdsIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });

        it("explicit deny", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.addDeniedAddresses(["169.254.169.254"]);

            for (const ip of imdsIPs) {
                assert.equal(policy._isNetworkConnectionAllowed([ip]), false);
            }

            for (const ip of badImdsIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });

        it("explicit allow", () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);
            policy.addAllowedAddresses(["169.254.169.254"]);

            for (const ip of imdsIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    true,
                    `Expected IP ${ip} to be allowed by policy`
                );
            }

            for (const ip of badImdsIPs) {
                assert.equal(
                    policy._isNetworkConnectionAllowed([ip]),
                    false,
                    `Expected IP ${ip} to be denied by policy as invalid IP`
                );
            }
        });
    });
});
