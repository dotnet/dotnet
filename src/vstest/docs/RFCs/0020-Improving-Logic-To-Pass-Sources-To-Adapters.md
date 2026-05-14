# 0020 Improving Logic To Pass Sources To Adapters

## Summary
This note outlines the proposed solution for improving logic to pass test sources to adapter by adding assembly type filter (in addition to existing file extensions based filter).

## Motivation
Every adapter specifies file extensions supported by it. Test sources are passed to adapters based on their supported file extensions.

In case when extension is an assembly (.dll or .exe), we require additional filter based on which we can pass test source to an adapter. The reason is, an assembly can be either `native` or `managed`. Its possible that an adapter supports only native or only managed or both types of assemblies.

Currently even though an adapter supports one type of assembly, there is no way for it to specify the same and thus we end up giving all type of assemblies to that adapter, which might cause overhead in test discovery time.

Proposed solution allows adapters to specify which assembly type is supported, and thus in result:
1. Native assemblies will not be passed to adapters which don't support native assemblies.
2. Managed assemblies will not be passed to adapters which don't support managed assemblies.

## Proposed Solution
1. System.ComponentModel.CategoryAttribute will be used for passing native and managed support. Adapter can specify native assemblies support by adding `[Category("native")]` in their discoverer. Similarly for managed support, `[Category("managed")]` can be specified.
2. `[Category("native")]` and `[Category("managed")]` will be ignored if adapter doesn't support assemblies i.e. none of `[FileExtension(".dll")]` or `[FileExtension(".exe")]` is present in adapter discoverer.
3. `Category` value other than `[Category("native")]` and `[Category("managed")]` will be ignored.

## Changes required by adapters
Adapter can use `CategoryAttribute` to specify native or managed assemblies support.
1. For managed assemblies support, `[Category("managed")]` should be specified.
2. For native assemblies support, `[Category("native")]` should be specified.
3. For both native and managed assemblies support, `Category` attribute is not required.

Proposed changes are optional for adapters. In case `Category` attribute is not added by adapter which supports assemblies, both native and managed assemblies are passed to it.

**Example 1**: If adapter supports native .dll and .appx, then adapter discoverer has following attributes:
```
    [DefaultExecutorUri(<ExecutorUriValue>)]
    [FileExtension(".dll")]
    [FileExtension(".appx")]
    [Category("native")]
```

**Example 2**: If adapter supports managed .dll, managed .exe and .xap, then adapter discoverer has following attributes:
```
    [DefaultExecutorUri(<ExecutorUriValue>)]
    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [FileExtension(".xap")]
    [Category("managed")]
```

**Example 3**: If adapter supports native .dll, managed .dll, .js and .xap, then adapter has following attributes:
```
    [DefaultExecutorUri(<ExecutorUriValue>)]
    [FileExtension(".dll")]
    [FileExtension(".js")]
    [FileExtension(".xap")]
```
