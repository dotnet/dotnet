// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import axios from "axios";

import { AntiSSRFError, AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("AntiSSRFPolicy Header Tests", () => {
    const BAD_HEADER_MESSAGE = "Request headers or protocol disallowed by policy";
    const TEST_DOMAIN = "ambitious-flower-0611c910f.2.azurestaticapps.net";

    it("bad inputs", () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1);

        // Invalid arrays
        assert.throws(
            () => policy.addDeniedHeaders(null as any),
            (err: AntiSSRFError) => err.message === "Null argument",
            "Expected addDeniedHeaders(null) to throw 'Null argument'"
        );
        assert.throws(
            () => policy.addRequiredHeaders(null as any),
            (err: AntiSSRFError) => err.message === "Null argument",
            "Expected addRequiredHeaders(null) to throw 'Null argument'"
        );

        // Invalid array elements
        assert.throws(
            () => policy.addDeniedHeaders([""]),
            (err: AntiSSRFError) => err.message === "Headers cannot be an empty string",
            "Expected addDeniedHeaders(['']) to throw empty-header validation error"
        );
        assert.throws(
            () => policy.addRequiredHeaders([""]),
            (err: AntiSSRFError) => err.message === "Headers cannot be an empty string",
            "Expected addRequiredHeaders(['']) to throw empty-header validation error"
        );
        assert.throws(
            () => policy.addDeniedHeaders(["X-Valid-Header", null as any, "Another-Header"]),
            (err: AntiSSRFError) => err.message === "Headers cannot be null or undefined",
            "Expected addDeniedHeaders(['X-Valid-Header', null, 'Another-Header']) to throw null-header validation error"
        );
        assert.throws(
            () => policy.addRequiredHeaders([null as any, "X-Test-Header"]),
            (err: AntiSSRFError) => err.message === "Headers cannot be null or undefined",
            "Expected addRequiredHeaders([null, 'X-Test-Header']) to throw null-header validation error"
        );
    });

    it("check defaults", () => {
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly).requiredHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly).deniedHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).requiredHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).deniedHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).deniedHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).requiredHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).deniedHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.None).requiredHeaders.length, 0);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.None).deniedHeaders.length, 0);
    });

    it("required header", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addRequiredHeaders(["X-Test-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.rejects(
            async () => {
                await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                    headers: {
                        "Not-X-Test-Header": "test-value"
                    }
                });
            },
            (err: Error) => err.message === BAD_HEADER_MESSAGE,
            "Expected request missing required header to be rejected"
        );

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                headers: {
                    "X-Test-Header": "test-value"
                }
            });
        });
    });

    it("denied header", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addDeniedHeaders(["X-Test-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.rejects(
            async () => {
                await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                    headers: {
                        "X-Test-Header": "test-value"
                    }
                });
            },
            (err: Error) => err.message === BAD_HEADER_MESSAGE,
            "Expected request containing denied header to be rejected"
        );

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                headers: {
                    "Not-X-Test-Header": "test-value"
                }
            });
        });
    });

    it("both required and denied", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addRequiredHeaders(["X-Test-Header"]);
        policy.addDeniedHeaders(["X-Test-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.rejects(
            async () => {
                await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                    headers: {
                        "X-Test-Header": "test-value"
                    }
                });
            },
            (err: Error) => err.message === BAD_HEADER_MESSAGE,
            "Expected request to be rejected when same header is both required and denied"
        );

        await assert.rejects(
            async () => {
                await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                    headers: {
                        "Not-X-Test-Header": "test-value"
                    }
                });
            },
            (err: Error) => err.message === BAD_HEADER_MESSAGE,
            "Expected request to be rejected when required header is missing and denied header config also exists"
        );
    });

    it("with XFF header", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addXFFHeader = true;
        policy.addRequiredHeaders(["X-Forwarded-For", "X-Test-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Forwarded-For`, {
                headers: {
                    "X-Test-Header": "test-value"
                }
            });
        });

        const policy2 = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy2.addXFFHeader = true;
        policy2.addDeniedHeaders(["X-Forwarded-For", "Not-X-Test-Header"]);
        const instance2 = axios.create({
            httpAgent: policy2.getHttpAgent(),
            httpsAgent: policy2.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.rejects(
            async () => {
                await instance2.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Forwarded-For`, {
                    headers: {
                        "X-Test-Header": "test-value"
                    }
                });
            },
            (err: Error) => err.message === BAD_HEADER_MESSAGE,
            "Expected request to be rejected when X-Forwarded-For is denied and addXFFHeader is enabled"
        );
    });

    it("holds on redirect", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addRequiredHeaders(["X-Required-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}/api/redirect?num=3`, {
                headers: {
                    "X-Required-Header": "test-value"
                }
            });
        });
    });

    it("case insensitive headers", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addRequiredHeaders(["X-Test-Header"]);
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Test-Header`, {
                headers: {
                    "x-test-header": "test-value"
                }
            });
        });
    });
});
