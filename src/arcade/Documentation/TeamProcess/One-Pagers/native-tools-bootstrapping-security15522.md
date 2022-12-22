# Deprecating Native Tools Bootstrapping

## Purpose
The whole thrust behind native tools bootstrapping was that build machines should be
clean and repos should bring their dependencies to the machines at build times. However,
this presents a major security problem: how do we ensure that all these repos are keeping
the executables they're installing on the build machines up to date? The current answer
is that we simply aren't, but that needs to change.

Going forward, **we will be installing all artifacts on the build machines directly via
dotnet-helix-machines** (the machine prep process), except for the ones installed via package
feeds. This is a departure from the "keep build machines clean" philosophy and from our work
with folks on bootstrapping native dependencies, but it makes sense from a security and
maintainability perspective.

## The Current State of the World
Based on [a search of the dotnet org](https://github.com/search?l=JSON&q=org%3Adotnet+native-tools&type=Code),
there are currently 10 non-archived repos using native tools bootstrapping. The artifacts
they're bootstrapping are as follows:

| Artifact | Version(s) | Latest Patch Version(s) | Repos Using |
|----------|------------|-------------------------|-------------|
| cmake | 3.11.1, 3.14.5, 3.16.4, 3.21.0 | 3.11.4, 3.14.7, 3.16.9, 3.21.5 | arcade-validation, deployment-tools, installer, msquic, runtime, winforms |
| cmake-test (fake artifact for testing) | 3.11.1 | N/A | arcade-validation |
| dotnet-api-docs_netcoreapp3.0 | 0.0.0.2 | N/A | wpf-test |
| dotnet-api-docs_net5.0 | 0.0.0.3 | N/A | winforms, wpf |
| msvcurt-c1xx | 0.0.0.8 | N/A | wpf-test |
| net-framework-48-ref-assemblies | 0.0.0.1 | N/A | wpf, wpf-test |
| perl | 5.32.1.1 | 5.32.1.1 | fsharp |
| python3 | 3.7.1 | 3.7.12 | deployment-tools, performance, runtime |
| strawberry-perl | 5.28.1.1-1 | 5.28.2.1 | wpf, wpf-test |

This indicates that four main executables are being bootstrapped:
* CMake (multiple different minor versions)
* Perl
* Python 3
* Strawberry Perl

Additionally, Winforms and WPF rely on some zip files that probably should come directly from
framework targeting packs, etc.

Evidence of the necessity of this work can be found in the fact that Python 3.7.1 has
[multiple CVEs](https://cve.mitre.org/cgi-bin/cvekey.cgi?keyword=python+3.7) against it.

## Removing Native Tools Bootstrapping Direct Downloading
The most straightforward way to deal with this problem is to simply stop natively bootstrapping artifacts
onto the machines. This approach has two main advantages: first, it brings the security of the machines fully
under the purview of the .NET Engineering Services team (which makes sense as we manage the machines
in every other respect) and allows us to make sure these artifacts are kept up-to-date, and second,
it simplifies the task of maintaining these artifacts for vendors (as there will be one central repository
for the artifacts and one place to check for component governance alerts).

It also has its drawbacks, however, which will be mentioned later in the document. However, it is the
belief of the .NET Engineering Services team that these drawbacks do not outweigh the benefits of
consolidating artifacts in dotnet-helix-machines.

The basic outline of this process would be:

1.  Take stock of what native tools are currently being used, create artifacts for them, and install
    and add those artifacts to the build queues.
2.  Alert teams that we will be decommissioning native tools bootstrapping and installing tools like
    CMake and Perl on the build machines and give them time to migrate their builds to not use
    native tools bootstrapping anymore.
3.  Rework native tools scripts in Arcade to put the appropriate version of an artifact on the machine
    (e.g., a specific minor version of CMake) on the path.

In the future, when a team needs a specific artifact, they can request it be added to the machines. We will
vet the artifact through component governance before adding it to machines in production. If the artifact
is going to be installed side-by-side along with other versions of the artifact (e.g., if they want to install
CMake 3.21 and 3.11 is already on the machines), the requester will specify the version they want to use
in the global.json under "native-tools" just like they do today. The bootstrapping scripts will then elevate
the specific version of a tool to the path. However, when possible, we should attempt to consolidate on a
single version to obviate the need for the use of these scripts.

### Takebacks/Downsides
This solution is, unfortunately, a takeback from previous solutions that involved investment in native
tools bootstrapping (see our epic for [Harmonizing Arcade and Runtime Repos](https://github.com/dotnet/arcade/issues/6560)).
Most obviously, this worsens the local dev experience which previously relied on native tools bootstrapping
to make specific dependencies less necessary. Going forward, repos will instead have to specifically note
which dependencies are required to build the product in their readme or COMPILING.md. The .NET Engineering
Services team will send out reminders to update the versioning of these dependencies every time there is a
version bump of an artifact. Eventually the plan is to use the matrix of truth (which knows which versions
of artifacts are installed on images) to flow the exact versions being used on build machines to READMEs automatedly.

This also reduces the control that repos have over their dependencies. The hope is that by allowing for side-by-side
installation and minor version selection, repos will still be able to have control over minor versions of
the tools they use while staying up to date with the latest patches.

In order to avoid build breaks while still maintaining security, once we have settled on a minor version for an artifact,
we will only be bumping its patch version as that is updated. This will restrict artifacts to security updates and bugfixes,
minimizing build breakage. The only time when a minor or major version bump will be required is when a particular version
reaches end-of-life and thus will no longer be maintained.

## Conclusion
While this is a significant departure from our previous thinking in the space, deprecating direct downloads via
native tools bootstrapping and moving to a machine-only artifacts world will significantly improve the
security and maintainability of machine artifacts going forward.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnative-tools-bootstrapping-security15522.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnative-tools-bootstrapping-security15522.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cnative-tools-bootstrapping-security15522.md)</sub>
<!-- End Generated Content-->
