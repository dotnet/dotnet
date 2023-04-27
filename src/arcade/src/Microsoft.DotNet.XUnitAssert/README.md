# Custom Version of Xunit Assert

## Origin/Attribution

This is a fork of the code in https://github.com/xunit/assert.xunit for building the
`Microsoft.DotNet.XUnitAssert` NuGet package.  The original authors of this code are Brad Wilson and
Oren Novotny.  See `../../THIRD-PARTY-NOTICES.TXT` for the license for this code.

## Purpose

This copy of assert.xunit is intended to be AOT-compatible and contains breaking changes from the
original code. In general, code which relied on reflection or dynamic code generation has been
removed in this fork.
