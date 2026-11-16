// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";
import axios from "axios";

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

describe("AntiSSRFPolicy AddXFFHeader Tests", () => {
    const TEST_DOMAIN = "ambitious-flower-0611c910f.2.azurestaticapps.net";

    it("check defaults", () => {
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.InternalOnly).addXFFHeader, false);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyV1).addXFFHeader, true);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.ExternalOnlyLatest).addXFFHeader, true);
        assert.strictEqual(new AntiSSRFPolicy(PolicyConfigOptions.None).addXFFHeader, false);
    });

    it("on true", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addXFFHeader = true;
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        const res = await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Forwarded-For`);
        assert.strictEqual(res.status, 200);
        assert.ok(res.data.headerValue, "Expected server to observe the X-Forwarded-For header added by policy");
    });

    it("does not overwrite header", async () => {
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.addXFFHeader = true;
        const instance = axios.create({
            httpAgent: policy.getHttpAgent(),
            httpsAgent: policy.getHttpsAgent(),
            validateStatus: () => true
        });

        const res = await instance.get(`https://${TEST_DOMAIN}/api/header-check?header=X-Forwarded-For`, {
            headers: {
                "X-Forwarded-For": "1.2.3.4"
            }
        });
        assert.strictEqual(res.status, 200);
        assert.strictEqual(res.data.headerValue.includes("1.2.3.4"), true);
    });
});
