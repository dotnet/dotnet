// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/**
 * The Axios library allows you to create an instance with custom configuration,
 * which can include custom headers, base URLs, Agents, timeouts, interceptors,
 * and more. MaxRedirects controls if Axios should use the http/https libraries
 * or the follow-redirects library.
 *
 * This test suite create an Axios instance with an AntiSSRFPolicy and tests
 * various scenarios, including absolute addresses with and without redirects,
 * absolute addresses that are redirected through a local HTTP server, and
 * relative addresses that use the base URL feature.
 */

import axios, { AxiosInstance } from "axios";
import assert from "assert";
import { createServer, Server } from "http";
import { lookup, promises } from "dns";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

const allowedAddressesNoRedirect = ["https://github.com/"];

const allowedAddressesWithRedirect = [
    "https://google.com/",
    "http://localhost:3000/?redirectTo=https://github.com/",
    "http://localhost:3000/?redirectTo=https://google.com/"
];

const deniedAddressesNoRedirect = [
    "https://apple.com/",
    "https://www.facebook.com",
    "https://www.bing.com",
    "https://outlook.live.com",
    "https://www.etsy.com",
    "https://169.254.169.254/"
];

const deniedAddressesWithRedirect = [
    "http://localhost:3000/?redirectTo=https://apple.com/",
    "http://localhost:3000/?redirectTo=https://www.facebook.com",
    "http://localhost:3000/?redirectTo=https://www.bing.com",
    "http://localhost:3000/?redirectTo=https://outlook.live.com",
    "http://localhost:3000/?redirectTo=https://www.etsy.com",
    "http://localhost:3000/?redirectTo=https://169.254.169.254/"
];

describe("Axios Instance tests", () => {
    let server: Server;
    let instance: AxiosInstance;

    /**
     * Set up policy:
     * - Add XFF header
     * - Require header "test-required-header"
     * - Deny header "test-denied-header"
     * - Deny all unspecifieid addresses
     * - Allow addresses from GitHub, Google, and NYT
     *
     * - Allow plain text HTTP required for local HTTP server
     * - Allow localhost required for local HTTP server
     */
    before(async () => {
        // Set up policy
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.addXFFHeader = true;
        policy.addRequiredHeaders(["test-required-header"]);
        policy.addDeniedHeaders(["test-denied-header"]);
        policy.denyAllUnspecifiedIPs = true;

        // Set up allowed IPs
        const githubIPs = await promises.lookup("github.com", { family: 0, all: true });
        const googleIPs = await promises.lookup("google.com", { family: 0, all: true });
        const moregoogleIPs = await promises.lookup("www.google.com", { family: 0, all: true });
        policy.addAllowedAddresses([...githubIPs, ...googleIPs, ...moregoogleIPs].map((ip) => ip.address));
        policy.addAllowedAddresses(["::1", "127.0.0.1"]);

        // Set up HTTP server to redirect requests
        server = createServer((req, res) => {
            const { url } = req;
            // Ensure the request has the XFF header
            assert.equal(req.headers["x-forwarded-for"], "true");
            res.writeHead(301, { Location: url?.substring(url.indexOf("=") + 1) ?? "" });
            res.end();
        });
        server.listen(3000);

        // Create the Axios instance
        instance = axios.create({
            headers: { "test-required-header": "true" },
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            baseURL: "https://google.com"
        });
    });

    describe("Allowed addresses that don't cause redirects", () => {
        allowedAddressesNoRedirect.forEach((url) => {
            it(`GET ${url} - maxRedirects = 0`, async () => {
                try {
                    const res = await instance.get(url, {
                        maxRedirects: 0
                    });
                    assert.ok(res.status == 200);
                } catch (err) {
                    assert.fail(`Unexpected error for ${url}: ${(err as Error).message}\n${(err as Error).stack}`);
                }
            });

            it(`GET ${url} - no maxRedirects specified`, async () => {
                try {
                    const res = await instance.get(url);
                    assert.ok(res.status == 200);
                } catch (err) {
                    assert.fail(`Unexpected error for ${url}: ${(err as Error).message}\n${(err as Error).stack}`);
                }
            });
        });
    });

    describe("Allowed addresses that cause redirects", () => {
        allowedAddressesWithRedirect.forEach((url) => {
            it(`GET ${url} - maxRedirects = 0`, async () => {
                try {
                    await instance.get(url, {
                        maxRedirects: 0
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, "Request failed with status code 301");
                }
            });

            it(`GET ${url} - no maxRedirects`, async () => {
                try {
                    const res = await instance.get(url);
                    assert.ok(res.status == 200);
                } catch (err) {
                    assert.fail(`Unexpected error for ${url}: ${(err as Error).message}\n${(err as Error).stack}`);
                }
            });
        });
    });

    describe("Denied addresses that don't cause redirects", () => {
        deniedAddressesNoRedirect.forEach((url) => {
            it(`GET ${url} - maxRedirects = 0`, async () => {
                try {
                    await instance.get(url, {
                        maxRedirects: 0
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, "IP address disallowed by policy");
                }
            });

            it(`GET ${url} - no maxRedirects`, async () => {
                try {
                    await instance.get(url);
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, "IP address disallowed by policy");
                }
            });
        });
    });

    describe("Denied addresses that cause redirects", () => {
        deniedAddressesWithRedirect.forEach((url) => {
            it(`GET ${url} - maxRedirects = 0`, async () => {
                try {
                    await instance.get(url, {
                        maxRedirects: 0
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, "Request failed with status code 301");
                }
            });

            it(`GET ${url} - no maxRedirects`, async () => {
                try {
                    await instance.get(url);
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, "IP address disallowed by policy");
                }
            });
        });
    });

    it("Contains denied header", async () => {
        try {
            await instance.get("https://github.com", {
                headers: { "test-required-header": "true", "test-denied-header": "true" }
            });
            assert.fail("Expected error, but got response");
        } catch (err) {
            assert.equal((err as Error).message, "Request headers or protocol disallowed by policy");
        }
    });

    it("Tries to overwrite lookup", async () => {
        try {
            await instance.get("https://github.com", {
                headers: { "test-required-header": "true" },
                lookup: lookup
            });
            assert.fail("Expected error, but got response");
        } catch (err) {
            assert.equal((err as Error).message, "Cannot use AntiSSRFHttpsAgent with custom lookup function");
        }
    });

    it("Uses baseURL", async () => {
        try {
            const res = await instance.get("/", {
                headers: { "test-required-header": "true" }
            });
            assert.ok(res.status == 200);
        } catch (err) {
            assert.fail(`Unexpected error for baseURL test: ${(err as Error).message}\n${(err as Error).stack}`);
        }
    });

    it("Incorrectly uses baseURL", async () => {
        try {
            await instance.get("www.bing.com", {
                headers: { "test-required-header": "true" }
            });
            assert.fail("Expected error, but got response");
        } catch (err) {
            assert.equal((err as Error).message, "Request failed with status code 404", `Unexpected error message: ${(err as Error).message}\n${(err as Error).stack}`);
        }
    });

    after(() => {
        server.close();
    });
});
