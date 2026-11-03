// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import https from "https";
import { lookup, LookupAddress, promises } from "dns";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("HttpsAgent Tests - default policy", () => {
    let antiSSRFHttpsAgent: https.Agent;
    let microsoftIP: string;

    before(async () => {
        antiSSRFHttpsAgent = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).getHttpsAgent({
            keepAlive: true
        });
        microsoftIP = await promises.lookup("microsoft.com", { family: 4 }).then((address) => address.address);
    });

    it("Successful lookup - get, URL", (done) => {
        const req = https.get("https://www.apple.com/", { agent: antiSSRFHttpsAgent, family: 4 }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                assert.equal(res.statusCode, 200);
                done();
            });
        });

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - get, options", (done) => {
        const req = https.get(
            {
                agent: antiSSRFHttpsAgent,
                host: microsoftIP,
                servername: "microsoft.com",
                headers: { Host: "microsoft.com" }
            },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 301);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - request, URL", (done) => {
        const req = https.request(
            "https://twin-cities.umn.edu/academics-admissions/majors-programs",
            { agent: antiSSRFHttpsAgent },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - request, options", (done) => {
        const req = https.request(
            {
                agent: antiSSRFHttpsAgent,
                hostname: "learn.microsoft.com",
                path: "/en-us/training/paths/describe-basic-concepts-of-cybersecurity/"
            },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Reject lookup - get, URL", (done) => {
        const req = https.get("https://[0::1]/", { agent: antiSSRFHttpsAgent }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                done("Expected error, but got response");
            });
        });

        req.on("error", (err) => {
            assert.equal(err.message, "IP address disallowed by policy");
            done();
        });

        req.end();
    });

    it("Reject lookup - get, options", (done) => {
        const req = https.get({ agent: antiSSRFHttpsAgent, hostname: "127.0.0.3", host: "google.com" }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                done("Expected error, but got response");
            });
        });

        req.on("error", (err) => {
            assert.equal(err.message, "IP address disallowed by policy");
            done();
        });

        req.end();
    });

    it("Reject lookup - request, URL", (done) => {
        const req = https.request(
            "https://169.254.169.254:443",
            {
                agent: antiSSRFHttpsAgent,
                host: "www.google.com"
            },
            (res) => {
                res.on("data", (data) => {});
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

    it("Reject lookup - request, options", (done) => {
        const req = https.request(
            {
                agent: antiSSRFHttpsAgent,
                host: "www.imds.michaelhendrickx.com"
            },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    done("Expected error, but got response");
                });
            }
        );

        req.on("error", (err) => {
            assert.equal(err.message, "getaddrinfo ENOTFOUND www.imds.michaelhendrickx.com");
            done();
        });

        req.end();
    });

    it("Successful lookup - any, ensure XFF is added", (done) => {
        const req = https.request("https://www.apple.com/", { agent: antiSSRFHttpsAgent }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                assert.equal(res.statusCode, 200);
                done();
            });
        });

        req.on("error", done);

        req.end(() => {
            assert.equal(req.getHeader("X-Forwarded-For"), "true");
        });
    });

    it("Successful lookup - any, ensure XFF is not overwritten", (done) => {
        const req = https.request(
            "https://www.apple.com/",
            { agent: antiSSRFHttpsAgent, headers: { "X-Forwarded-For": "127.0.0.1" } },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end(() => {
            assert.equal(req.getHeader("x-forwarded-for"), "127.0.0.1");
        });
    });

    it("Reject lookup - tried to add lookup to request", (done) => {
        const req = https.get("https://google.com", { agent: antiSSRFHttpsAgent, lookup: lookup }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                done("Expected error, but got response");
            });
        });

        req.on("error", (err) => {
            assert.equal(err.message, "Cannot use AntiSSRFHttpsAgent with custom lookup function");
            done();
        });

        req.end();
    });

    after(() => {
        antiSSRFHttpsAgent.destroy();
    });
});

describe("HttpsAgent Tests - custom policy", () => {
    let antiSSRFHttpsAgent: https.Agent;
    let googleIPs: LookupAddress[];
    let appleIPs: LookupAddress[];

    before(async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addRequiredHeaders(["test-required-header"]);
        policy.addDeniedHeaders(["test-denied-header"]);

        googleIPs = await promises.lookup("www.google.com", { family: 0, all: true });
        appleIPs = await promises.lookup("apple.com", { family: 0, all: true });
        policy.addDeniedAddresses([...googleIPs, ...appleIPs].map((address) => address.address));

        antiSSRFHttpsAgent = policy.getHttpsAgent();
    });

    it("Successful lookup - get, URL", (done) => {
        const req = https.get(
            "https://www.bing.com/",
            { agent: antiSSRFHttpsAgent, port: 443, headers: { "test-required-header": 25 } },
            (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - get, options", (done) => {
        const req = https.get(
            { agent: antiSSRFHttpsAgent, hostname: "github.com", headers: { "test-required-header": 25 } },
            (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    if (res.statusCode == 200) {
                        done();
                    } else {
                        done(new Error(`Expected 200, got ${res.statusCode}`));
                    }
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - request, URL", (done) => {
        const req = https.get(
            "https://www.cmu.edu/",
            { agent: antiSSRFHttpsAgent, headers: { "test-required-header": 25 } },
            (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - request, options", (done) => {
        const req = https.request(
            {
                agent: antiSSRFHttpsAgent,
                hostname: "learn.microsoft.com",
                path: "/en-us/training/paths/describe-basic-concepts-of-cybersecurity/",
                headers: { "test-required-header": 25 }
            },
            (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Reject lookup - get, URL", (done) => {
        const req = https.get(
            "https://apple.com",
            {
                agent: antiSSRFHttpsAgent,
                host: "www.bing.com",
                headers: { "test-required-header": 25 }
            },
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

    it("Reject lookup - get, options", (done) => {
        const req = https.get(
            {
                agent: antiSSRFHttpsAgent,
                host: appleIPs.find((address) => address.family === 4)?.address,
                headers: { "test-required-header": 25 },
                servername: "apple.com"
            },
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

    it("Reject lookup - request, options", (done) => {
        const req = https.request(
            {
                agent: antiSSRFHttpsAgent,
                host: "www.google.com",
                family: 0,
                headers: { "test-required-header": 25 }
            },
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

    it("Reject lookup - any, missing required header", (done) => {
        const req = https.get("https://www.google.com/", { agent: antiSSRFHttpsAgent }, (res) => {
            res.on("data", () => {});
            res.on("end", () => {
                done("Expected error, but got response");
            });
        });

        req.on("error", (err) => {
            assert.equal(err.message, "Request headers or protocol disallowed by policy");
            done();
        });

        req.end();
    });

    it("Reject lookup - any, include denied header", (done) => {
        const req = https.get(
            "https://www.bing.com/",
            { agent: antiSSRFHttpsAgent, headers: { "test-required-header": "true", "test-denied-header": "false" } },
            (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done("Expected error, but got response");
                });
            }
        );

        req.on("error", (err) => {
            assert.equal(err.message, "Request headers or protocol disallowed by policy");
            done();
        });

        req.end();
    });

    it("Reject lookup - any, tried to overwrite lookup", (done) => {
        // @ts-expect-error - trying to overwrite lookup should cause error
        antiSSRFHttpsAgent.lookup = lookup;

        const req = https.get(
            "https://apple.com",
            {
                agent: antiSSRFHttpsAgent,
                host: "www.google.com",
                headers: { "test-required-header": 25 }
            },
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

    it("Bad agent construction", () => {
        assert.throws(() => {
            const newPolicy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            newPolicy.getHttpsAgent({ lookup: lookup });
        });
    });

    after(() => {
        antiSSRFHttpsAgent.destroy();
    });
});

describe("HttpsAgent Tests - other methods", () => {
    const testUrl = "https://ambitious-flower-0611c910f.2.azurestaticapps.net/api/method";
    let allowAgent: https.Agent;
    let disallowAgent: https.Agent;

    before(async () => {
        const allowPolicy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
        allowAgent = allowPolicy.getHttpsAgent();

        const disallowPolicy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
        const disallowedIPs = await promises.lookup(new URL(testUrl).hostname, { family: 0, all: true });
        disallowPolicy.addDeniedAddresses(disallowedIPs.map((addr) => addr.address));
        disallowAgent = disallowPolicy.getHttpsAgent();
    });

    const methods = ["GET", "POST", "PUT", "DELETE", "HEAD", "PATCH", "OPTIONS"];
    methods.map((method) => {
        it(`${method} allowed`, (done) => {
            const req = https.request(testUrl, { method, agent: allowAgent }, (res) => {
                let responseData = "";
                res.on("data", (chunk) => {
                    responseData += chunk.toString();
                });
                res.on("end", () => {
                    if (method === "HEAD") {
                        assert.equal(res.statusCode, 200);
                        return done();
                    }

                    try {
                        assert.equal(res.statusCode, 200);
                        const parsedData = JSON.parse(responseData);
                        assert.equal(parsedData.method, method);
                        done();
                    } catch (error) {
                        done(error);
                    }
                });
            });

            req.on("error", done);

            req.end();
        });

        it(`${method} disallowed`, (done) => {
            const req = https.request(testUrl, { method, agent: disallowAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done("Expected error, but got response");
                });
            });

            req.on("error", () => {
                done();
            });

            req.end();
        });
    });

    after(() => {
        allowAgent.destroy();
        disallowAgent.destroy();
    });
});

describe("HttpsAgent Tests - certificates", () => {
    let antiSSRFHttpsAgent: https.Agent;

    before(() => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
        antiSSRFHttpsAgent = policy.getHttpsAgent();
    });

    describe("Valid Certificate Tests", () => {
        it("Valid certificate should succeed", (done) => {
            const req = https.get("https://www.google.com", { agent: antiSSRFHttpsAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 200);
                    done();
                });
            });

            req.on("error", done);

            req.end();
        });
    });

    describe("Expired Certificate Tests", () => {
        it("Expired certificate should fail with certificate verification enabled", (done) => {
            const req = https.get("https://expired.badssl.com/", { agent: antiSSRFHttpsAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done(new Error("Expected SSL error, but got response"));
                });
            });

            req.on("error", (err: any) => {
                assert.ok(err.message.includes("certificate") || err.code === "CERT_HAS_EXPIRED");
                done();
            });

            req.end();
        });

        it("Expired certificate should succeed with certificate verification disabled", (done) => {
            const req = https.get(
                "https://expired.badssl.com/",
                { agent: antiSSRFHttpsAgent, rejectUnauthorized: false },
                (res) => {
                    res.on("data", () => {});
                    res.on("end", () => {
                        assert.equal(res.statusCode, 200);
                        done();
                    });
                }
            );

            req.on("error", done);

            req.end();
        });
    });

    describe("Wrong Host Certificate Tests", () => {
        it("Wrong host certificate should fail with certificate verification enabled", (done) => {
            const req = https.get("https://wrong.host.badssl.com/", { agent: antiSSRFHttpsAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done(new Error("Expected SSL error, but got response"));
                });
            });

            req.on("error", (err: any) => {
                assert.ok(
                    err.message.includes("certificate") ||
                        err.message.includes("Hostname/IP does not match") ||
                        err.code === "ERR_TLS_CERT_ALTNAME_INVALID"
                );
                done();
            });

            req.end();
        });

        it("Wrong host certificate should succeed with certificate verification disabled", (done) => {
            const req = https.get(
                "https://wrong.host.badssl.com/",
                { agent: antiSSRFHttpsAgent, rejectUnauthorized: false },
                (res) => {
                    res.on("data", () => {});
                    res.on("end", () => {
                        assert.equal(res.statusCode, 200);
                        done();
                    });
                }
            );

            req.on("error", done);

            req.end();
        });
    });

    describe("Self-Signed Certificate Tests", () => {
        it("Self-signed certificate should fail with certificate verification enabled", (done) => {
            const req = https.get("https://self-signed.badssl.com/", { agent: antiSSRFHttpsAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done(new Error("Expected SSL error, but got response"));
                });
            });

            req.on("error", (err: any) => {
                assert.ok(
                    err.message.includes("certificate") ||
                        err.message.includes("self-signed") ||
                        err.code === "DEPTH_ZERO_SELF_SIGNED_CERT"
                );
                done();
            });

            req.end();
        });

        it("Self-signed certificate should succeed with certificate verification disabled", (done) => {
            const req = https.get(
                "https://self-signed.badssl.com/",
                { agent: antiSSRFHttpsAgent, rejectUnauthorized: false },
                (res) => {
                    res.on("data", () => {});
                    res.on("end", () => {
                        assert.equal(res.statusCode, 200);
                        done();
                    });
                }
            );

            req.on("error", done);

            req.end();
        });
    });

    describe("Untrusted Root Certificate Tests", () => {
        it("Untrusted root certificate should fail with certificate verification enabled", (done) => {
            const req = https.get("https://untrusted-root.badssl.com/", { agent: antiSSRFHttpsAgent }, (res) => {
                res.on("data", () => {});
                res.on("end", () => {
                    done(new Error("Expected SSL error, but got response"));
                });
            });

            req.on("error", (err: any) => {
                assert.ok(
                    err.message.includes("certificate") ||
                        err.message.includes("unable to verify") ||
                        err.code === "UNABLE_TO_VERIFY_LEAF_SIGNATURE"
                );
                done();
            });

            req.end();
        });

        it("Untrusted root certificate should succeed with certificate verification disabled", (done) => {
            const req = https.get(
                "https://untrusted-root.badssl.com/",
                { agent: antiSSRFHttpsAgent, rejectUnauthorized: false },
                (res) => {
                    res.on("data", () => {});
                    res.on("end", () => {
                        assert.equal(res.statusCode, 200);
                        done();
                    });
                }
            );

            req.on("error", done);

            req.end();
        });
    });

    after(() => {
        antiSSRFHttpsAgent.destroy();
    });
});
