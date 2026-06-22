---
applyTo: "src/Microsoft.TestPlatform.CommunicationUtilities/**"
---

# CommunicationUtilities — IPC & Protocol Rules

This layer implements JSON-RPC communication between vstest.console and testhosts. Wire-format stability is paramount.

## Protocol Stability

- New protocol messages MUST be backward-compatible with older testhost/console versions.
- Document every wire-protocol version change — silent regressions here break the entire ecosystem.
- Never expose serializer-specific types (e.g., `JsonProperty` attributes) in public messaging contracts.
- Serializer migrations must verify wire-format compatibility on all supported TFMs.

## Error Handling

- Connection timeouts and broken pipes must be handled without crashing the host process.
- Protocol handlers must convert processing failures into deterministic error paths — never silently terminate.
- Surface process id, exit state, and captured error output when child process connections fail.

## Dependency Management

- Serializer library changes require verifying the full transitive dependency set ships correctly.
- Do not introduce new serializer dependencies without a migration plan from the existing one.
- Version conflicts here propagate to every test project — validate against `eng/expected-*.json`.

## Key Checks

- Interface extensions must not break existing consumers — prefer new interfaces over method additions.
- TranslationLayer methods must return or throw, never silently hang.
- Test both directions: newer console → older host AND older console → newer host.
