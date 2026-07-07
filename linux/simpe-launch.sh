#!/bin/bash
# Launches SimPE under Wine/Proton, in the same prefix as your Sims 2
# installation (whichever edition/launcher you use — Steam, Lutris, a
# manual Wine prefix, GOG, etc.) SimPE's own Settings dialog is where you
# point it at your actual game/Downloads folders once it's running —
# this script's only job is getting it launched correctly.
#
# Why not just double-click SimPE.Main.exe:
#  - Wine builds older than ~11.0 crash on any menu click: WinForms'
#    ToolStripManager unconditionally calls Win32 DPI-awareness-context
#    APIs that older wine doesn't implement.
#  - Even on wine >= 11.0, the .exe apphost's own .NET-runtime discovery
#    is unreliable under Wine — it can report "you must install .NET
#    Desktop Runtime" even when the runtime is correctly installed and
#    registered. Invoking dotnet.exe directly with the managed .dll
#    sidesteps the apphost's discovery logic entirely.
#
# Configuration (all optional — auto-detected if unset):
#   SIMPE_PREFIX   Path to the Wine prefix root (the directory containing
#                  drive_c). E.g. a Lutris prefix, a manual `winecfg`
#                  prefix, or a Steam compatdata/<appid>/pfx.
#   SIMPE_RUNNER   Path to the wine binary or Proton script to run it
#                  with. A "proton" in the filename is treated as a
#                  Proton compat-tool script (needs STEAM_COMPAT_* env);
#                  anything else is treated as a plain `wine` binary.
#
# If neither is set, this script looks for The Sims 2 Legacy Collection
# (Steam AppID 3314070) as a convenience default, since that's a common
# case — but it is *not* required. For any other edition or install
# method, set SIMPE_PREFIX (and SIMPE_RUNNER if auto-detection picks the
# wrong wine/proton build) to point at wherever your game already runs.
#
# Usage:
#   simpe-launch.sh                     launch SimPE
#   simpe-launch.sh --install-runtime   install .NET 8 Desktop Runtime
#                                        into the prefix (one-time, needed
#                                        before first launch)

set -euo pipefail

DOTNET_RUNTIME_URL="https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.28/windowsdesktop-runtime-8.0.28-win-x64.exe"
LEGACY_COLLECTION_APPID="3314070"

# --- locate this repo's build output -----------------------------------
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(dirname "$SCRIPT_DIR")"

SIMPE_DLL=""
for cfg in Debug Release; do
    if [ -f "$REPO_ROOT/bin/$cfg/SimPE.Main.dll" ]; then
        SIMPE_DLL="$REPO_ROOT/bin/$cfg/SimPE.Main.dll"
        break
    fi
done
if [ -z "$SIMPE_DLL" ]; then
    echo "error: SimPE.Main.dll not found under $REPO_ROOT/bin/{Debug,Release}" >&2
    echo "Build the solution first: dotnet build SimPE-Fixed.sln -c Debug -r win-x64 --self-contained false" >&2
    exit 1
fi

# --- resolve prefix + runner: explicit env vars, else Steam auto-detect -
PREFIX="${SIMPE_PREFIX:-}"
RUNNER="${SIMPE_RUNNER:-}"
STEAM_ROOT=""

if [ -z "$PREFIX" ] || [ -z "$RUNNER" ]; then
    STEAM_CANDIDATES=(
        "$HOME/.steam/root"
        "$HOME/.steam/steam"
        "$HOME/.local/share/Steam"
        "$HOME/.var/app/com.valvesoftware.Steam/.steam/root"
    )
    for c in "${STEAM_CANDIDATES[@]}"; do
        if [ -d "$c/steamapps" ]; then
            STEAM_ROOT="$(cd "$c" && pwd)"
            break
        fi
    done

    if [ -n "$STEAM_ROOT" ] && [ -z "$PREFIX" ]; then
        mapfile -t LIBRARY_PATHS < <(
            { echo "$STEAM_ROOT"; grep -oP '"path"\s*"\K[^"]+' "$STEAM_ROOT/steamapps/libraryfolders.vdf" 2>/dev/null; } \
                | sed 's/\\\\/\//g' | sort -u
        )
        for lib in "${LIBRARY_PATHS[@]}"; do
            if [ -f "$lib/steamapps/appmanifest_$LEGACY_COLLECTION_APPID.acf" ]; then
                candidate="$lib/steamapps/compatdata/$LEGACY_COLLECTION_APPID/pfx"
                [ -d "$candidate" ] && PREFIX="$candidate"
                break
            fi
        done
    fi

    if [ -n "$STEAM_ROOT" ] && [ -z "$RUNNER" ]; then
        best_dir="" best_ver=0
        for dir in "$STEAM_ROOT/steamapps/common"/Proton* "$STEAM_ROOT/compatibilitytools.d"/*; do
            [ -x "$dir/files/bin/wine" ] || continue
            ver="$("$dir/files/bin/wine" --version 2>/dev/null | grep -oP '(?<=wine-)[0-9]+' | head -1)"
            [ -n "$ver" ] || continue
            if [ "$ver" -gt "$best_ver" ]; then
                best_ver="$ver"
                best_dir="$dir"
            fi
        done
        [ -n "$best_dir" ] && RUNNER="$best_dir/proton"
    fi
fi

if [ -z "$PREFIX" ] || [ ! -d "$PREFIX" ]; then
    echo "error: couldn't determine a Wine prefix to use." >&2
    echo "       Set SIMPE_PREFIX to your game's Wine prefix (the folder containing drive_c)." >&2
    echo "       Example: SIMPE_PREFIX=~/Games/sims2/prefix $0" >&2
    exit 1
fi
if [ -z "$RUNNER" ]; then
    echo "error: couldn't find a Proton/wine build to run SimPE with." >&2
    echo "       Set SIMPE_RUNNER to a wine binary or Proton script." >&2
    echo "       Example: SIMPE_RUNNER=/usr/bin/wine $0" >&2
    exit 1
fi

# best-effort wine-version sanity check, non-fatal either way
wine_bin="$RUNNER"
[[ "$RUNNER" == *proton* ]] && wine_bin="$(dirname "$RUNNER")/files/bin/wine"
if [ -x "$wine_bin" ]; then
    ver="$("$wine_bin" --version 2>/dev/null | grep -oP '(?<=wine-)[0-9]+' | head -1 || true)"
    if [ -n "$ver" ] && [ "$ver" -lt 11 ]; then
        echo "warning: $wine_bin reports wine-$ver.x — SimPE may crash on menu clicks (needs wine >= 11 for DPI-awareness API support)." >&2
    fi
fi

# --- optional: install .NET 8 Desktop Runtime into the prefix -----------
if [ "${1:-}" = "--install-runtime" ]; then
    tmp_installer="$(mktemp --suffix=.exe)"
    echo "Downloading .NET 8 Desktop Runtime..."
    if command -v wget >/dev/null; then
        wget -q "$DOTNET_RUNTIME_URL" -O "$tmp_installer"
    else
        curl -sSL "$DOTNET_RUNTIME_URL" -o "$tmp_installer"
    fi
    echo "Installing into $PREFIX ..."
    if [[ "$RUNNER" == *proton* ]]; then
        export STEAM_COMPAT_DATA_PATH="$(dirname "$PREFIX")"
        export STEAM_COMPAT_CLIENT_INSTALL_PATH="$STEAM_ROOT"
        "$RUNNER" run "$tmp_installer" /install /quiet /norestart
    else
        WINEPREFIX="$PREFIX" "$RUNNER" "$tmp_installer" /install /quiet /norestart
    fi
    rm -f "$tmp_installer"
    echo "Done."
    exit 0
fi

if [ ! -f "$PREFIX/drive_c/Program Files/dotnet/dotnet.exe" ]; then
    echo "error: .NET Desktop Runtime isn't installed in this prefix yet." >&2
    echo "       Run: $0 --install-runtime" >&2
    exit 1
fi

# Windows path form of the DLL, via Wine's Z: -> / mapping.
SIMPE_DLL_WIN="Z:$(echo "$SIMPE_DLL" | sed 's/\//\\\\/g')"

if [[ "$RUNNER" == *proton* ]]; then
    export STEAM_COMPAT_DATA_PATH="$(dirname "$PREFIX")"
    export STEAM_COMPAT_CLIENT_INSTALL_PATH="$STEAM_ROOT"
    exec "$RUNNER" run "C:\\Program Files\\dotnet\\dotnet.exe" "$SIMPE_DLL_WIN"
else
    export WINEPREFIX="$PREFIX"
    exec "$RUNNER" "C:\\Program Files\\dotnet\\dotnet.exe" "$SIMPE_DLL_WIN"
fi
