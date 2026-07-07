#!/bin/bash
# One-time setup: makes `simpe` runnable from anywhere and adds a SimPE
# entry to your application launcher (KDE/GNOME/etc). Safe to re-run.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(dirname "$SCRIPT_DIR")"
LAUNCHER="$SCRIPT_DIR/simpe-launch.sh"
ICON="$REPO_ROOT/Resources/SimPE.ico"

chmod +x "$LAUNCHER"

BIN_DIR="$HOME/.local/bin"
mkdir -p "$BIN_DIR"
ln -sf "$LAUNCHER" "$BIN_DIR/simpe"
echo "Linked $BIN_DIR/simpe -> $LAUNCHER"

APPS_DIR="$HOME/.local/share/applications"
mkdir -p "$APPS_DIR"
cat > "$APPS_DIR/simpe.desktop" <<EOF
[Desktop Entry]
Type=Application
Name=SimPE
Comment=Sims 2 package editor
Exec=$LAUNCHER
Icon=$ICON
Terminal=false
Categories=Game;Utility;
StartupNotify=true
EOF
echo "Installed $APPS_DIR/simpe.desktop"

command -v update-desktop-database >/dev/null && update-desktop-database "$APPS_DIR" 2>/dev/null || true

case ":$PATH:" in
    *":$BIN_DIR:"*) ;;
    *)
        echo
        echo "Note: $BIN_DIR isn't on your PATH yet. Add this to your ~/.bashrc (or ~/.zshrc):"
        echo "  export PATH=\"\$HOME/.local/bin:\$PATH\""
        ;;
esac

echo
echo "Setup complete. Run 'simpe' from a terminal, or find SimPE in your application launcher."
echo "First-time only: if .NET Desktop Runtime isn't installed in the game's Wine prefix yet, run:"
echo "  $LAUNCHER --install-runtime"
echo
echo "By default this looks for The Sims 2 Legacy Collection via Steam. Using a different"
echo "edition or install method (Lutris, a manual Wine prefix, GOG, etc.)? Point the launcher"
echo "at it directly instead:"
echo "  export SIMPE_PREFIX=/path/to/your/wineprefix   # the folder containing drive_c"
echo "  export SIMPE_RUNNER=/path/to/wine-or-proton"
echo "Add those to your ~/.bashrc to make them permanent."
