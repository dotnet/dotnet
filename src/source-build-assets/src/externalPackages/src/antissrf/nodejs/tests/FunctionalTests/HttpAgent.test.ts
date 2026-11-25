// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import http from "http";
import { lookup, LookupAddress, promises } from "dns";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("HttpAgent Tests - default policy", () => {
    let antiSSRFHttpAgent: http.Agent;
    let microsoftIP: string;

    before(async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest);
        policy.allowPlainTextHttp = true;
        antiSSRFHttpAgent = policy.getHttpAgent({ keepAlive: true });
        microsoftIP = await promises.lookup("microsoft.com", { family: 4 }).then((address) => address.address);
    });

    it("Successful lookup - get, URL", (done) => {
        const req = http.get("http://www.apple.com/", { agent: antiSSRFHttpAgent, family: 4 }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                assert.equal(res.statusCode, 301);
                done();
            });
        });

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - get, options", (done) => {
        const req = http.get(
            {
                agent: antiSSRFHttpAgent,
                host: microsoftIP,
                headers: { Host: "microsoft.com" }
            },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 307);
                    done();
                });
            }
        );

        req.on("error", done);

        req.end();
    });

    it("Successful lookup - request, URL", (done) => {
        const req = http.request(
            "http://twin-cities.umn.edu/academics-admissions/majors-programs",
            { agent: antiSSRFHttpAgent },
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

    it("Successful lookup - request, options", (done) => {
        const req = http.request(
            {
                agent: antiSSRFHttpAgent,
                hostname: "learn.microsoft.com",
                path: "/en-us/training/paths/describe-basic-concepts-of-cybersecurity/"
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

    it("Reject lookup - get, URL", (done) => {
        const req = http.get("http://[0::1]/", { agent: antiSSRFHttpAgent }, (res) => {
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
        const req = http.get({ agent: antiSSRFHttpAgent, hostname: "127.0.0.3", host: "google.com" }, (res) => {
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
        const req = http.request(
            "http://169.254.169.254:443",
            {
                agent: antiSSRFHttpAgent,
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
        const req = http.request(
            {
                agent: antiSSRFHttpAgent,
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
        const req = http.request("http://www.apple.com/", { agent: antiSSRFHttpAgent }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                assert.equal(res.statusCode, 301);
                done();
            });
        });

        req.on("error", done);

        req.end(() => {
            assert.equal(req.getHeader("X-Forwarded-For"), "true");
        });
    });

    it("Successful lookup - any, ensure XFF is not overwritten", (done) => {
        const req = http.request(
            "http://www.apple.com/",
            { agent: antiSSRFHttpAgent, headers: { "X-Forwarded-For": "127.0.0.1" } },
            (res) => {
                res.on("data", (data) => {});
                res.on("end", () => {
                    assert.equal(res.statusCode, 301);
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
        const req = http.get("http://google.com", { agent: antiSSRFHttpAgent, lookup: lookup }, (res) => {
            res.on("data", (data) => {});
            res.on("end", () => {
                done("Expected error, but got response");
            });
        });

        req.on("error", (err) => {
            assert.equal(err.message, "Cannot use AntiSSRFHttpAgent with custom lookup function");
            done();
        });

        req.end();
    });

    after(() => {
        antiSSRFHttpAgent.destroy();
    });
});

describe("HttpAgent Tests - custom policy", () => {
    let antiSSRFHttpAgent: http.Agent;
    let googleIPs: LookupAddress[];
    let appleIPs: LookupAddress[];

    before(async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.addRequiredHeaders(["test-required-header"]);
        policy.addDeniedHeaders(["test-denied-header"]);

        googleIPs = await promises.lookup("www.google.com", { family: 0, all: true });
        appleIPs = await promises.lookup("apple.com", { family: 0, all: true });
        policy.addDeniedAddresses([...googleIPs, ...appleIPs].map((address) => address.address));

        antiSSRFHttpAgent = policy.getHttpAgent();
    });

    it("Successful lookup - get, URL", (done) => {
        const req = http.get(
            "http://www.bing.com/",
            { agent: antiSSRFHttpAgent, headers: { "test-required-header": 25 } },
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

    it("Successful lookup - get, options", (done) => {
        const req = http.get(
            { agent: antiSSRFHttpAgent, hostname: "github.com", headers: { "test-required-header": 25 } },
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
        const req = http.get(
            "http://www.cmu.edu/",
            { agent: antiSSRFHttpAgent, headers: { "test-required-header": 25 } },
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

    it("Successful lookup - request, options", (done) => {
        const req = http.request(
            {
                agent: antiSSRFHttpAgent,
                hostname: "learn.microsoft.com",
                path: "/en-us/training/paths/describe-basic-concepts-of-cybersecurity/",
                headers: { "test-required-header": 25 }
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

    it("Reject lookup - get, URL", (done) => {
        const req = http.get(
            "http://apple.com",
            {
                agent: antiSSRFHttpAgent,
                host: "www.bing.com",
                headers: { "test-required-header": 25 }
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

    it("Reject lookup - get, options", (done) => {
        const req = http.get(
            {
                agent: antiSSRFHttpAgent,
                host: appleIPs.find((address) => address.family === 4)?.address,
                headers: { "test-required-header": 25 }
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
        const req = http.request(
            {
                agent: antiSSRFHttpAgent,
                host: "www.google.com",
                family: 0,
                headers: { "test-required-header": 25 }
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

    it("Reject lookup - any, missing required header", (done) => {
        const req = http.get("http://www.google.com/", { agent: antiSSRFHttpAgent }, (res) => {
            res.on("data", (data) => {});
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
        const req = http.get(
            "http://www.bing.com/",
            { agent: antiSSRFHttpAgent, headers: { "test-required-header": "true", "test-denied-header": "false" } },
            (res) => {
                res.on("data", (data) => {});
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
        antiSSRFHttpAgent.lookup = lookup;

        const req = http.get(
            "http://apple.com",
            {
                agent: antiSSRFHttpAgent,
                host: "www.google.com",
                headers: { "test-required-header": 25 }
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

    it("Bad agent construction", () => {
        assert.throws(() => {
            const newPolicy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            newPolicy.getHttpAgent({ lookup: lookup });
        });
    });

    after(() => {
        antiSSRFHttpAgent.destroy();
    });
});
