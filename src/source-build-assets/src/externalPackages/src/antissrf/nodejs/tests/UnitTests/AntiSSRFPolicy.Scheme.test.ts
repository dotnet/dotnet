// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import axios from "axios";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("AntiSSRFPolicy Scheme Tests", () => {
    const BAD_SCHEME_MESSAGE = "Request headers or protocol disallowed by policy";
    const TEST_DOMAIN = "ambitious-flower-0611c910f.2.azurestaticapps.net";

    it("check defaults", () => {
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly).allowPlainTextHttp, false);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).allowPlainTextHttp, false);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).allowPlainTextHttp, false);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.None).allowPlainTextHttp, false);
    });

    it("on true", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.doesNotReject(async () => {
            await instance.get(`http://${TEST_DOMAIN}`);
        });

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}`);
        });
    });

    it("on false", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = false;
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        await assert.rejects(
            async () => {
                await instance.get(`http://${TEST_DOMAIN}`);
            },
            (err: Error) => err.message === BAD_SCHEME_MESSAGE,
            "Expected HTTP request to be rejected when allowPlainTextHttp is false"
        );

        await assert.doesNotReject(async () => {
            await instance.get(`https://${TEST_DOMAIN}`);
        });
    });

    it("rejects non-http schemes", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        const nonHttpSchemes = [
            "ws://example.com",
            "wss://example.com",
            "ftp://example.com",
            "gopher://example.com",
            "file:///etc/passwd",
            "ldap://example.com",
            "ldaps://example.com",
            "mailto:test@example.com",
            "tel:+1234567890",
            // "data:text/plain;base64,SGVsbG8=", Axios handles data: separately
            "javascript:alert('xss')",
            "custom://example.com"
        ];

        for (const url of nonHttpSchemes) {
            await assert.rejects(async () => {
                await instance.get(url);
            }, `Expected request with non-HTTP scheme to be rejected: ${url}`);
        }
    });
});
