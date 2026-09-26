# Experimental APIs

Some public API ships annotated `[Experimental]`. Referencing such API from code is a compile
error unless the diagnostic is suppressed. This is deliberate: it lets the platform publish an API
for feedback without committing to support it, and the annotation is removed once the API is
supported. Removing it is metadata only, so it is not a breaking change for callers.

Running a feature is unaffected. The diagnostic applies to compiling a reference to the API, not to
using the feature through configuration, the command line, or reflection.

## VSTEST001

Covers the xxHash128 test case id work:

- `Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities.EqtHash.GuidFromStringXxHash128(string)`
- `Microsoft.TestPlatform.AdapterUtilities.TestIdProviderXxHash128`
- `Microsoft.VisualStudio.TestPlatform.Extensions.TestIdsLogger.TestIdsLogger`

**This API is not supported.** It ships so that the new id algorithm can be evaluated while SHA1 is
still the default, and it may change or be removed. See
[`VSTEST_DISABLE_XXHASH128_TESTCASE_ID`](environment-variables.md#vstest_disable_xxhash128_testcase_id)
for the feature itself and [the test ids logger](test-ids-logger.md) for the reporting tool.

To reference it anyway, suppress the diagnostic:

```xml
<PropertyGroup>
  <NoWarn>$(NoWarn);VSTEST001</NoWarn>
</PropertyGroup>
```

Suppressing it opts out for everything the id covers, including API added to it later.
