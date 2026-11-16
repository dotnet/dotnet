// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { ClientRequest, AgentOptions as HttpAgentOptions } from "http";
import { AgentOptions as HttpsAgentOptions } from "https";
import { BlockList } from "net";

import { AntiSSRFError, IPAddressRanges } from ".";
import { CIDRBlock } from "./Helpers/CIDRBlock";
import { AntiSSRFHttpsAgent } from "./Helpers/AntiSSRFHttpsAgent";
import { AntiSSRFHttpAgent } from "./Helpers/AntiSSRFHttpAgent";

export enum PolicyConfigOptions {
    InternalOnly = "InternalOnly",
    ExternalOnlyV1 = "ExternalOnlyV1",
    ExternalOnlyLatest = "ExternalOnlyLatest",
    None = "None"
}

export class AntiSSRFPolicy {
    // IP address related variables
    private _allowedAddresses: BlockList; // maintains all networks are in IPv6
    private _deniedAddresses: BlockList; // maintains all networks are in IPv6
    private _denyAllUnspecifiedIPs: boolean;

    // Headers related variables
    private _requiredHeaders: string[]; // maintains all headers are lowercase
    private _deniedHeaders: string[]; // maintains all headers are lowercase
    private _addXFFHeader: boolean = false;
    private _allowPlainTextHttp: boolean = false;

    /**
     * Creates a new AntiSSRF policy with the specified default configuration.
     *
     * @param config The policy configuration:
     * - `InternalOnly`: Denies all connections by default. Use `addAllowedAddresses()` to permit specific ranges.
     * - `ExternalOnlyV1`: Blocks recommendedV1 IP ranges. Adds the `X-Forwarded-For` header to requests when missing.
     * - `ExternalOnlyLatest`: Blocks recommendedLatest IP ranges. Adds the `X-Forwarded-For` header to requests when missing.
     * - `None`: No restrictions.
     */
    constructor(config: PolicyConfigOptions) {
        if (config == null) {
            throw new AntiSSRFError("Null argument");
        }

        this._allowedAddresses = new BlockList();
        this._deniedAddresses = new BlockList();
        this._denyAllUnspecifiedIPs = false;

        this._requiredHeaders = [];
        this._deniedHeaders = [];
        this._addXFFHeader = false;
        this._allowPlainTextHttp = false;

        switch (config) {
            // Block all IPs by default. Users must add their intended internal ranges.
            case PolicyConfigOptions.InternalOnly:
                this._denyAllUnspecifiedIPs = true;
                break;
            // Block recommendedV1 IPs. Blocks IMDS, so add XFF.
            case PolicyConfigOptions.ExternalOnlyV1:
                this.addDeniedAddresses(IPAddressRanges.recommendedV1);
                this._addXFFHeader = true;
                break;
            // Block recommendedLatest IPs. Blocks IMDS, so add XFF.
            case PolicyConfigOptions.ExternalOnlyLatest:
                this.addDeniedAddresses(IPAddressRanges.recommendedLatest);
                this._addXFFHeader = true;
                break;
            // No restrictions.
            case PolicyConfigOptions.None:
                break;
            default:
                throw new AntiSSRFError("Argument must be a valid PolicyConfigOptions value");
        }
    }

    /**
     * ===== The IP addresses related functionality =====
     */

    /**
     * Gets or sets whether all unspecified IPs are denied by default.
     * When true, only explicitly allowed addresses can connect.
     * When false, addresses are evaluated against the denied addresses list.
     */

    get denyAllUnspecifiedIPs(): boolean {
        return this._denyAllUnspecifiedIPs;
    }

    set denyAllUnspecifiedIPs(value: boolean) {
        if (value == null) {
            throw new AntiSSRFError("Null argument");
        }
        this._denyAllUnspecifiedIPs = value;
    }

    /**
     * Gets the allowed IP addresses BlockList (readonly).
     */
    get allowedAddresses(): Readonly<BlockList> {
        return this._allowedAddresses;
    }

    /**
     * Adds the specified IP addresses and/or range of IP addresses to the
     * collection of allowed addresses.
     *
     * @param networks List of IPv4 and/or IPv6 addresses and/or subnets in
     *                 CIDR notation
     * @throws AntiSSRFError on improperly formatted network
     */
    public addAllowedAddresses(networks: string[]): void {
        if (networks == null) {
            throw new AntiSSRFError("Null argument");
        }

        const parsedNetworks = networks.map((n) => CIDRBlock._parseCIDR(n));

        for (const network of parsedNetworks) {
            this._allowedAddresses.addSubnet(network.getAddress(), network.getPrefix(), "ipv6");
        }
    }

    /**
     * Gets the denied IP addresses BlockList (readonly).
     */
    get deniedAddresses(): Readonly<BlockList> {
        return this._deniedAddresses;
    }

    /**
     * Adds the specified IP addresses and/or range subnets to the collection
     * of denied addresses.
     *
     * @param networks List of IPv4 and/or IPv6 addresses and/or subnets in
     *                 CIDR notation
     * @throws AntiSSRFError on improperly formatted network
     */
    public addDeniedAddresses(networks: string[]): void {
        if (networks == null) {
            throw new AntiSSRFError("Null argument");
        }

        if (this._denyAllUnspecifiedIPs) {
            throw new AntiSSRFError("Can't add denied networks when denyAllUnspecifiedIPs is true");
        }

        const parsedNetworks = networks.map((n) => CIDRBlock._parseCIDR(n));

        for (const network of parsedNetworks) {
            this._deniedAddresses.addSubnet(network.getAddress(), network.getPrefix(), "ipv6");
        }
    }

    /**
     * @internal
     * This method is intended for internal use by AntiSSRF agents only.
     * Use getHttpAgent() or getHttpsAgent() for public API.
     */
    public _isNetworkConnectionAllowed(ipaddresses: string[]): boolean {
        if (ipaddresses == null) {
            return false;
        }

        for (const ipaddress of ipaddresses) {
            if (ipaddress == null) {
                return false;
            }

            try {
                const [ipv6] = CIDRBlock._parseIPAddress(ipaddress);

                if (this._allowedAddresses.check(ipv6, "ipv6")) {
                    // If the address is in the allow list, it is allowed
                    continue;
                }

                if (this._denyAllUnspecifiedIPs || this._deniedAddresses.check(ipv6, "ipv6")) {
                    // If the address is not in an allow list, it's not allowed
                    return false;
                }
            } catch {
                return false;
            }
        }

        // No IP address was denied
        return true;
    }

    /**
     * ===== The headers policy related functionality =====
     */

    /**
     * Gets the list of required headers (readonly copy).
     */
    get requiredHeaders(): readonly string[] {
        return [...this._requiredHeaders];
    }

    /**
     * Adds headers to the collection of required headers.
     *
     * @param headers List of headers to require
     * @throws AntiSSRFError on null/undefined headers array and on any
     *         null/undefined or empty string header
     */
    public addRequiredHeaders(headers: string[]): void {
        if (headers == null) {
            throw new AntiSSRFError("Null argument");
        }

        for (const header of headers) {
            if (header == null) {
                throw new AntiSSRFError("Headers cannot be null or undefined");
            }
            if (header.trim() === "") {
                throw new AntiSSRFError("Headers cannot be an empty string");
            }
        }

        this._requiredHeaders.push(...headers.map((h) => h.toLowerCase()));
    }

    /**
     * Gets the list of denied headers (readonly copy).
     */
    get deniedHeaders(): readonly string[] {
        return [...this._deniedHeaders];
    }

    /**
     * Adds headers to the collection of denied headers.
     *
     * @param headers List of headers to deny
     * @throws AntiSSRFError on null/undefined headers array and on any
     *         null/undefined or empty string header
     */
    public addDeniedHeaders(headers: string[]): void {
        if (headers == null) {
            throw new AntiSSRFError("Null argument");
        }

        for (const header of headers) {
            if (header == null) {
                throw new AntiSSRFError("Headers cannot be null or undefined");
            }
            if (header.trim() === "") {
                throw new AntiSSRFError("Headers cannot be an empty string");
            }
        }

        this._deniedHeaders.push(...headers.map((h) => h.toLowerCase()));
    }

    /**
     * Gets or sets whether to add the XFF header to all requests.
     * True to add the XFF header to all requests, false otherwise.
     */

    get addXFFHeader(): boolean {
        return this._addXFFHeader;
    }

    set addXFFHeader(value: boolean) {
        if (value == null) {
            throw new AntiSSRFError("Null argument");
        }
        this._addXFFHeader = value;
    }

    /**
     * Gets or sets whether to allow http or to require https.
     * True to allow http, false to require https.
     */

    get allowPlainTextHttp(): boolean {
        return this._allowPlainTextHttp;
    }

    set allowPlainTextHttp(value: boolean) {
        if (value == null) {
            throw new AntiSSRFError("Null argument");
        }
        this._allowPlainTextHttp = value;
    }

    /**
     * @internal
     * This method is intended for internal use by AntiSSRF agents only.
     * Use getHttpAgent() or getHttpsAgent() for public API.
     */
    public _isHttpRequestAllowed(req: ClientRequest): boolean {
        if (req == null || req.protocol == null) {
            return false;
        }

        // Node URL protocol property returns the protocol with the ':'
        // Check if the protocol is plain text AND plain text is disallowed
        if (!(req.protocol.toLowerCase() === "https:" || this._allowPlainTextHttp)) {
            return false;
        }

        // Node URL protocol property returns the protocol with the ':'
        // Ensure the protocol is http(s)
        if (!(req.protocol.toLowerCase() === "http:" || req.protocol.toLowerCase() === "https:")) {
            return false;
        }

        // Add the XFF header if required
        if (this._addXFFHeader && !req.getHeaderNames().includes("x-forwarded-for")) {
            req.setHeader("X-Forwarded-For", "true");
        }

        // Ensure none of the denied headers are present
        for (const header of this._deniedHeaders) {
            if (req.hasHeader(header)) {
                return false;
            }
        }

        // Ensure all of the required headers are present
        for (const header of this._requiredHeaders) {
            if (!req.hasHeader(header)) {
                return false;
            }
        }

        return true;
    }

    /**
     * ===== The Agent for easy policy enforcement =====
     */

    public getHttpsAgent(options?: HttpsAgentOptions) {
        return new AntiSSRFHttpsAgent(this, options);
    }

    public getHttpAgent(options?: HttpAgentOptions) {
        return new AntiSSRFHttpAgent(this, options);
    }
}
