# .NET Unified Build (Product Construction v3) Documentation

Welcome! This directory serves as a location for general information about .NET's "Unified Build" effort, which is another iteration of "Product Construction".

## What is "Product Construction"?

Product Construction is the general term that Microsoft has used for how .NET is built and shipped from a collection of inter-related repositories and components. Over the lifetime of .NET, we have modified these methods to adapt to changing product needs, improve efficiency, and enable other organizations to build and ship .NET in their own OS distributions. There have been at least 5 major iterations in how we build and ship:

## What is Unified Build?

Unified Build for .NET is a combined effort to improve two problematic areas for .NET:

- **True upstream model** – Partners are efficient and can do the same things we can.
- **Product build and servicing complexity** – Taming our incredibly complex infrastructure.

Unified Build will eliminate Microsoft's current method of building the full .NET product, replacing it with source-build across all platforms. This will result in only needing to maintain one build system, shared with the community. The product will be built as an aggregate codebase (a monolithic repository), although individual repositories will still be used for day-to-day development. The goal is simplicity and consistency. We believe that this will not only make .NET easier to develop, build, and ship, but it will also enable .NET contributors to be more productive.