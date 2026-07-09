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

## Run it in the same prefix as your Sims 2 install

SimPE needs to share a Wine prefix with your Sims 2 install (whichever
edition/launcher — Steam, Lutris, GOG, a manual prefix) so it can see the
same drive mappings and registry entries the game uses. SimPE's own
Settings dialog is where you point it at your actual game/Downloads
folders once it's running.

**Manual / any prefix:**

```
WINEPREFIX=/path/to/your/prefix wine "bin/Release/SimPE.Main.exe"
```

(For a Proton prefix, use the matching `proton run` invocation with
`STEAM_COMPAT_DATA_PATH` and `STEAM_COMPAT_CLIENT_INSTALL_PATH` set, the
same way you'd launch any other non-Steam Windows exe through Proton.)

**Steam:** add `SimPE.Main.exe` as a non-Steam game, then set its Proton
compat-tool version to match Sims 2's, and point `STEAM_COMPAT_DATA_PATH`
at the same `compatdata/<appid>` folder your Sims 2 install already uses.

**Lutris:** add `SimPE.Main.exe` as an extra executable/runner entry in
your existing Sims 2 game configuration, so it reuses that game's prefix.

There's no single install method that covers every edition, so this is
left as configuration rather than an auto-detecting script.
