# VRCLearn VPM distribution

This fork adds VPM packaging and a VRChat Creator Companion repository to MCP
for Unity.

## Installation

Add this community repository to VRChat Creator Companion:

```text
https://vrclearn.github.io/unity-mcp/index.json
```

Then add **MCP for Unity** (`com.coplaydev.unity-mcp`) to the desired project.
VPM installs the Unity Editor plugin only. Python 3.10+, `uv`, and an MCP client
are still required; finish setup from **Window → MCP for Unity** inside Unity.

## Fork policy

- The complete upstream repository and Git history are retained.
- Upstream changes are merged and reviewed instead of replacing fork files.
- The fork's `main` branch follows upstream `main`, including unreleased changes.
- VPM packages use an exact upstream GitHub Release plus the fork overlay; they
  are never built implicitly from the current `main` tree.
- Package authorship remains attributed to CoplayDev.
- The upstream MIT license is included in every VPM archive.

The Unity Editor interface is available in English, Japanese, Traditional
Chinese, and Simplified Chinese. The initial language follows Unity's system
language and can be changed from the **Advanced** tab. MCP protocol identifiers,
tool schemas, configuration data, and technical logs remain in English for
compatibility.

## Versioning

The versions in `MCPForUnity/package.json`, `Server/pyproject.toml`, and the root
`manifest.json` must match. Stable VPM releases use the tag `vpm-<version>` and
are immutable.

Any fork change intended for release must receive a new stable version across
all three files. An existing release archive is never overwritten.

## Automated upstream updates

The fork checks CoplayDev's `main` branch every hour. Ordinary upstream changes
are merged into a temporary branch, packaged, and tested before the validated
merge is promoted to the fork's `main`. Upstream workflow changes require a
maintainer to use GitHub's **Sync fork → Update branch** button because the
workflow token cannot modify workflow files. That update pushes `main` and
immediately triggers the same checks again.

After `main` is synchronized successfully, the workflow checks the latest
stable CoplayDev GitHub Release. If the matching `vpm-<version>` release is
missing, it constructs a temporary snapshot from the exact upstream release
tag and reapplies the effective fork changes. This keeps VPM releases aligned
with upstream releases even when upstream `main` already contains newer,
unreleased commits. The snapshot is packaged and tested before publication;
its temporary branch is removed afterward.

Prereleases and the upstream `beta` branch are intentionally ignored. Merge,
overlay, test, and packaging failures stop the affected track and create a
GitHub issue for maintainer review. Repeated runs with the same failure do not
append duplicate notifications. The issue closes automatically after the
affected workflow recovers.

If Unity test credentials are configured, both synchronization tracks also run
the upstream Unity test workflow. Without those optional secrets, both tracks
still require VPM packaging, localization validation, and Python tests to pass.

## Generated package metadata

The release script puts the contents of `MCPForUnity/` at the ZIP root. During
staging, it adds the following VPM metadata to `package.json` without changing
the tracked source manifest:

- the release asset URL;
- the MIT SPDX license identifier;
- an empty `vpmDependencies` object;
- the fork repository and exact Git revision;
- the VPM release page as the changelog URL.

The root `LICENSE` is included as `LICENSE.md`.
