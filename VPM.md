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
- VPM packages are built from `MCPForUnity/` on the fork's stable `main` branch.
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

The fork checks the latest stable CoplayDev GitHub release every hour. A new
upstream release is merged into a temporary branch, packaged, and tested before
the validated merge is promoted to `main`. The matching VPM release and
repository listing are then published automatically.

Prereleases and the upstream `beta` branch are intentionally ignored. Merge
conflicts, test failures, and packaging failures stop promotion and create a
GitHub issue for maintainer review. The issue closes automatically after the
affected workflow recovers.

If Unity test credentials are configured, automatic promotion also runs the
upstream Unity test workflow. Without those optional secrets, synchronization
uses the stable upstream release as its compatibility baseline and still
requires VPM packaging, localization validation, and Python tests to pass.

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
