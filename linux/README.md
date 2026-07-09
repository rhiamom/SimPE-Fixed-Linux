# Running SimPE on Linux

SimPE runs under Wine/Proton. `SimPE.Main.exe` is self-contained (it ships
its own .NET 8 runtime), so there's no separate runtime to install and
nothing that needs to be registered inside a Wine prefix — you can launch
the exe directly.

Build it:

```
dotnet build SimPE-Fixed.sln -c Release
```

Output lands at `bin/Release/SimPE.Main.exe`.

## Requirements

- **Wine >= 11.0** (or a Proton build on the same base). Older Wine builds
  crash on any menu click — WinForms' `ToolStripManager` unconditionally
  calls Win32 DPI-awareness-context APIs that older Wine doesn't implement.

## It does not need to share a prefix with Sims 2

Unlike the game itself, SimPE doesn't rely on the Windows registry to find
your game files. It ships its own Game Root scanner
(`GameRootAutoScanner.cs`) that walks a folder you point it at on disk,
looking for `TSData` subfolders to identify the base game and each
installed expansion/stuff pack, and remembers the result in its own
`GameRoot.cfg` — no registry involved. (There's a legacy registry lookup
for auto-detecting installed EPs, used only as an optional shortcut; it
already falls back to the Game Root scanner when that registry key isn't
present, which is the norm for Origin/Ultimate Collection installs and any
other prefix that doesn't match your game's.)

Since every Wine prefix maps the whole host filesystem under `Z:` by
default, SimPE can browse to and read/write wherever your Sims 2 files
actually live on disk — inside the game's own prefix, or anywhere else —
regardless of which prefix SimPE itself runs in. So the simplest setup is
a plain, dedicated Wine prefix just for SimPE:

```
WINEPREFIX=~/.simpe wine "bin/Release/SimPE.Main.exe"
```

On first run, use SimPE's Settings / Game Root scanner to point it at
your Sims 2 install folder (e.g. the `compatdata/<appid>/pfx/drive_c/...`
path from your Steam/Lutris/GOG prefix) and your Downloads folder. SimPE's
own settings and cache then live in `~/.simpe`, kept separate from the
game's prefix.

Running SimPE inside the same prefix as the game works too, if that's more
convenient for you — nothing above is a requirement, just the path of
least setup. If you do want that, add `SimPE.Main.exe` as a non-Steam game
(Steam) or an extra executable/runner entry (Lutris) in your existing Sims
2 configuration, pointed at that install's `WINEPREFIX`/`STEAM_COMPAT_DATA_PATH`.

(For a Proton prefix, use the matching `proton run` invocation with
`STEAM_COMPAT_DATA_PATH` and `STEAM_COMPAT_CLIENT_INSTALL_PATH` set, the
same way you'd launch any other non-Steam Windows exe through Proton.)
