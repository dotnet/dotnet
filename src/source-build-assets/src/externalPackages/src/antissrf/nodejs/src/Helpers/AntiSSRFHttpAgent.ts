// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */

/**
 * Any changes in this file need to be copied into AntiSSRFHttpsAgent.ts as well.
 * The two files must always be the same, except that this one extends
 * http.Agent while AntiSSRFHttpsAgent extends https.Agent.
 *
 * The two parent Agents have different default options and implement
 * createConnection differently.
 */

import { Agent as HttpAgent, AgentOptions as HttpAgentOptions, ClientRequest } from "http";
import { isIP, LookupFunction } from "net";

import { antiSSRFDnsLookup } from "./AntiSSRFDnsLookup";
import { AntiSSRFError, AntiSSRFPolicy } from "../";

export class AntiSSRFHttpAgent extends HttpAgent {
    private _antiSSRFPolicy: AntiSSRFPolicy;
    private _antiSSRFLookup: LookupFunction;

    constructor(policy: AntiSSRFPolicy, options?: HttpAgentOptions) {
        // Ensure the user does not expect to use a different dns.lookup function
        if (options?.lookup != null) {
            throw new AntiSSRFError("Cannot use AntiSSRFHttpAgent with custom lookup function");
        }

        // Call the parent constructor
        super(options);

        // Set custom variables
        this._antiSSRFPolicy = policy;
        this._antiSSRFLookup = antiSSRFDnsLookup(policy);
    }

    /**
     * This function is a wrapper around the addAgent function in http.Agent.
     * It is used to check the headers portion of the policy and to add the
     * XFF header if required.
     */
    addRequest = (req: ClientRequest, ...args: any[]) => {
        // Check if the request headers are allowed by the policy, and add the
        // XFF header if required.
        // Check if plaintext requests are allowed by the policy.
        if (!this._antiSSRFPolicy._isHttpRequestAllowed(req)) {
            process.nextTick(() => {
                req.emit("error", new AntiSSRFError("Request headers or protocol disallowed by policy"));
            });
            return;
        }

        // @ts-expect-error 'addRequest' isn't defined in '@types/node'
        return super.addRequest(req, ...args); // eslint-disable-line
    };

    /**
     * This function is a wrapper around the createConnection function in
     * http.Agent. It is used to check if the host is an IP address, and if so,
     * to check if it is allowed by the policy. Then, it sets the lookup
     * function for the request to our AntiSSRFDnsLookup function.
     *
     * This wrapper function is needed because the createConnection function in
     * http.Agent first checks if isIP(host), and if so, it skips our
     * policy-based lookup function. To make sure the policy is still checked
     * when the host is an IP address, this wrapper function explicitly checks
     * if the host is an allowed IP address before letting the http.Agent
     * createConnection function to take over.
     *
     * Note: NodeJS has known, inconsistent type documentation for the Agent
     * createConnection methods. In the Agent code, createConnection is only
     * used with this signature.
     */
    createConnection = (options: any, callback: any) => {
        // Ensure the user does not expect to use a different dns.lookup function
        if (options?.lookup != null) {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-return, @typescript-eslint/no-unsafe-call
            return callback(new AntiSSRFError("Cannot use AntiSSRFHttpAgent with custom lookup function"), null);
        }

        // http.request host is supposed to default to localhost. We want to
        // ensure we know what the host is ahead of time for the policy check,
        // so we are explicitly setting the default host if it is not already
        // set.
        // Not sure it is possible to get here, excluding from test cases
        /* istanbul ignore next 5 */
        if (options == null) {
            options = { host: "localhost" };
        } else if (options.host == null) {
            options.host = "localhost";
        }

        // If host is an IP address, check if it is allowed by the policy.
        if (isIP(options.host) && !this._antiSSRFPolicy._isNetworkConnectionAllowed([options.host])) {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-return, @typescript-eslint/no-unsafe-call
            return callback(new AntiSSRFError("IP address disallowed by policy"), null);
        }

        // Set the lookup function for the request to the AntiSSRFDnsLookup
        // function. Since we created this agent with lookup = null and we
        // checked that options.lookup = null, we can safely set it to our
        // custom lookup function.
        options.lookup = this._antiSSRFLookup;

        return super.createConnection(options, callback);
    };
}
