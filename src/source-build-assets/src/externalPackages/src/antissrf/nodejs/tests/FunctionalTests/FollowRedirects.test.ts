// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/**
 * Library: https://github.com/follow-redirects/follow-redirects
 *
 * Description: follow-redirects provides request and get methods that behave
 * identically to those found on the native http and https modules, with the
 * exception that they will seamlessly follow redirects.
 *
 * Notes: Used by Axios whenever maxRedirects != 0.
 */

import assert from "assert";
import { createServer, Server, Agent as NodeHttpAgent } from "http";
import { Agent as NodeHttpsAgent } from "https";
import { promises } from "dns";
import { http } from "follow-redirects";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("Follow-Redirects Library Tests", () => {
    describe("Redirect proxy", () => {
        let server: Server;
        let httpAgent: NodeHttpAgent;
        let httpsAgent: NodeHttpsAgent;

        before(async () => {
            // Set up server to redirect requests
            server = createServer((req, res) => {
                const { url } = req;
                assert.equal(req.headers["x-forwarded-for"], "true");
                res.writeHead(301, { Location: url?.substring(url.indexOf("=") + 1) ?? "" });
                res.end();
            });
            server.listen(3000);

            // Set up policy
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.allowPlainTextHttp = true;
            policy.addXFFHeader = true;
            policy.denyAllUnspecifiedIPs = true;

            // Set up allowed IPs
            const githubIPs = await promises.lookup("github.com", { family: 0, all: true });
            const googleIPs = await promises.lookup("google.com", { family: 0, all: true });
            const moreGoogleIPs = await promises.lookup("www.google.com", { family: 0, all: true });
            const portalAzureIPs = await promises.lookup("portal.azure.com", { family: 0, all: true });
            policy.addAllowedAddresses(
                [...githubIPs, ...googleIPs, ...moreGoogleIPs, ...portalAzureIPs].map((ip) => ip.address)
            );
            policy.addAllowedAddresses(["::1", "127.0.0.1"]);

            httpAgent = policy.getHttpAgent({ keepAlive: false });
            httpsAgent = policy.getHttpsAgent({ keepAlive: false });
        });

        const allowedRedirects = [
            "https://github.com/",
            "https://google.com/",
            "https://portal.azure.com/",
            "http://localhost:3000/?redirectTo=https://google.com/"
        ];
        allowedRedirects.forEach((url) => {
            it(`GET http://localhost:3000/?redirectTo=${url}`, (done) => {
                const req = http.get(
                    `http://localhost:3000/?redirectTo=${url}`,
                    { agents: { http: httpAgent, https: httpsAgent } },
                    (res) => {
                        res.on("data", () => {});
                        res.on("end", () => {
                            assert.equal(res.statusCode, 200);
                            if (!url.includes("google")) {
                                assert.equal(url, res.responseUrl);
                            }
                            done();
                        });
                    }
                );

                req.on("error", done);

                req.end();
            });
        });

        const disallowedRedirects = ["https://apple.com/", "https://cmu.edu/", "https://www.bing.com/"];
        disallowedRedirects.forEach((url) => {
            it(`GET http://localhost:3000/?redirectTo=${url}`, (done) => {
                const req = http.get(
                    `http://localhost:3000/?redirectTo=${url}`,
                    { trackRedirects: true, agents: { http: httpAgent, https: httpsAgent } },
                    (res) => {
                        res.on("data", () => {});
                        res.on("end", () => {
                            done("Expected error, but got response");
                        });
                    }
                );

                req.on("error", (err) => {
                    assert.equal(err.message, "IP address disallowed by policy");
                    done();
                });

                req.end();
            });
        });

        after(() => {
            server.close();
            httpAgent.destroy();
            httpsAgent.destroy();
        });
    });
});
