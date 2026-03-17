# SimPE Architecture Guide

A technical overview for new maintainers. SimPE is a resource editor for
The Sims 2 game files. It reads, displays, and writes binary `.package` files
using a type-driven wrapper/plugin system.

---

## Table of Contents

1. [The Package File Format](#1-the-package-file-format)
2. [Type Codes — The Four-Letter System](#2-type-codes)
3. [The Wrapper System](#3-the-wrapper-system)
4. [The UI Handler System](#4-the-ui-handler-system)
5. [The Plugin System](#5-the-plugin-system)
6. [The File Index](#6-the-file-index)
7. [Project Map](#7-project-map)
8. [Application Startup Flow](#8-application-startup-flow)
9. [Resource Selection Flow](#9-resource-selection-flow)
10. [Neighborhood Files vs. Object Packages](#10-neighborhood-files-vs-object-packages)
11. [Major Subsystems](#11-major-subsystems)
12. [Tools Menu vs. Wrappers](#12-tools-menu-vs-wrappers)
13. [Build Output Layout](#13-build-output-layout)
14. [Known Platform Notes (.NET 8)](#14-known-platform-notes-net-8)

---

## 1. The Package File Format

A `.package` file is a binary container. It holds a flat list of resources, each
identified by three numbers: **Type**, **Group**, and **Instance** (TGI). The file
also carries a header with an index offset and hole index (deleted space).

### Key classes

| Class | Project | Purpose |
|---|---|---|
| `IPackageFile` | SimPE.Interfaces | Interface for an open package |
| `GeneratableFile` | SimPE.Packages | Concrete read/write implementation |
| `IPackedFileDescriptor` | SimPE.Interfaces | Metadata for one resource (TGI + offset + size) |
| `PackedFileDescriptor` | SimPE.Packages | Concrete descriptor |
| `IPackedFile` | SimPE.Interfaces | The actual bytes of one resource (may be compressed) |
| `PackedFile` | SimPE.Packages | Handles zlib decompression on read |

### TGI — Type / Group / Instance

```
Type     (uint32) — What kind of resource this is (OBJD, TXMT, BHAV, …)
Group    (uint32) — Namespace. 0x1C050000 = custom CC, 0x1C0532FA = Maxis global
Instance (uint32) — Unique ID within this type+group
SubType  (uint32) — Extra discriminator, only in package format 1.1+
```

A resource is uniquely addressed by its full TGI. SimPE displays them grouped
by type in the Resource Tree, and listed individually in the Resource List.

### Package I/O flow

```
GeneratableFile.LoadFromFile(path)
  ├─ Read header  (version, index count, index offset)
  ├─ Read index   (one PackedFileDescriptor per resource)
  └─ Resources are lazy-loaded: bytes read only when ProcessData() is called

GeneratableFile.IncrementalBuild(path)
  ├─ Append new/changed resources
  ├─ Rewrite index at end
  └─ Write new header
```

---

## 2. Type Codes

Every resource type has a **uint32 type code**. The human-readable four-letter
names (OBJD, BHAV, TXMT …) are the ASCII encoding of those four bytes.

All constants live in `SimPE.Helper/MetaData.cs`.

### Most important type codes

| Name | Hex constant | Description |
|---|---|---|
| OBJD | `0x4F424A44` | Object definition — catalog info, sort flags, EP flags |
| BHAV | `0x42484156` | Behavior script (bytecode, like compiled Java class) |
| BCON | `0x42434F4E` | Behavior constants (variables used by BHAV) |
| GLOB | `0x474C4F42` | Global behavior (shared BHAV library) |
| STR# | `0x53545223` | String table |
| CTSS | `0x43545353` | Catalog strings (object name, description) |
| TXMT | `0x49596978` | Texture material definition (references TXTR) |
| TXTR | `0x1C4A276C` | Texture image (DDS or RLE compressed) |
| GMDC | `0xAC4F8687` | 3D geometry mesh |
| SHPE | `0xFC6EB1F7` | Shape (skeleton / mesh container) |
| CRES | `0xE519C933` | Colour resource (links shape to material) |
| GZPS | `0xEBCF3E27` | Property set (old-style recolor, pre-AL) |
| MMAT | `0x4C697E5A` | Material override (new-style recolor, AL+) |
| 3IDR | `0xAC598EAC` | Resource ID list (links recolor to mesh) |
| CPF  | `0xEBCF3E27` | Compressed property file (Sim traits, etc.) |
| SDSC | `0xAACE2EFB` | Sim description (one per Sim in neighborhood) |
| SDNA | `0xEBFEE33F` | Sim DNA / genetics |
| FAMI | `0x8C870743` | Family / household data |
| HOUS | `0x484F5553` | Neighborhood/lot header |
| NREF | `0x6B943B43` | Neighborhood reference |
| TTAB | `0x54544142` | Test tree (BHAV function call table) |
| ANIM | `0xFB00791E` | Animation |

---

## 3. The Wrapper System

A **wrapper** is a class that knows how to parse and re-serialize one resource
type. The type code is what connects a resource to its wrapper.

### Interfaces

```
IWrapper                    — base marker interface
  └─ IFileWrapper           — adds AssignableTypes[], FileSignature[]
       └─ IPackedFileWrapper — adds ProcessData(), RefreshUI(), UIHandler
            └─ AbstractWrapper (abstract class in SimPE.Interfaces)
                 — subclass this to implement a wrapper
```

### AbstractWrapper — what to implement

```csharp
class MyWrapper : AbstractWrapper, IFileWrapper
{
    // Which type codes this wrapper handles
    public uint[] AssignableTypes => new uint[] { MetaData.MY_TYPE };

    // Optional: match by file header bytes instead of type code
    public byte[] FileSignature => new byte[0];

    // Parse binary → object model
    protected override void Unserialize(BinaryReader reader) { … }

    // Write object model → binary
    protected override void Serialize(BinaryWriter writer) { … }

    // Create the WinForms editor panel
    protected override IPackedFileUI CreateDefaultUIHandler() => new MyForm();
}
```

### Factory — how wrappers register themselves

Every plugin DLL must expose a class implementing `IWrapperFactory`:

```csharp
public class MyFactory : AbstractWrapperFactory, IWrapperFactory
{
    public IWrapper[] KnownWrappers => new IWrapper[] { new MyWrapper() };
}
```

At startup, `LoadFileWrappersExt.LoadWrapperFactory(dllPath)` uses reflection to
find the first `IWrapperFactory` in the assembly and registers all its wrappers
with `FileTable.WrapperRegistry`.

### Finding a wrapper at runtime

```
user clicks resource (type = 0x49596978)
└─ FileTable.WrapperRegistry.FindHandler(0x49596978)
   └─ returns TxmtWrapper
   if nothing found:
└─ reads first 64 bytes, tries FileTable.WrapperRegistry.FindHandler(bytes)
   └─ matches by FileSignature
   if still nothing:
└─ returns DefaultWrapper (hex dump display)
```

---

## 4. The UI Handler System

Each wrapper optionally provides an editor panel — a WinForms `Control` that
lets the user view and edit the resource.

### Interfaces

```
IPackedFileUI
  ├─ Control GUIHandle        — the WinForms control to embed
  └─ void UpdateGUI(wrapper)  — refresh display when data changes
```

### Lifecycle

```
wrapper.UIHandler property getter
  ├─ if null → calls CreateDefaultUIHandler() → stores result
  └─ returns cached IPackedFileUI

ResourceLoader.Present(fii, wrapper)
  ├─ panel = wrapper.UIHandler.GUIHandle
  ├─ Creates a WeifenLuo DockContent window
  ├─ Embeds panel in it
  └─ doc.Show(dc, DockState.Document)   ← dc is the main document DockPanel
```

When the user edits something in the panel, the wrapper's properties update.
Calling `wrapper.SynchronizeUserData()` re-serializes the wrapper to binary and
stores the result in `pfd.UserData`, marking the file dirty. On Save, all dirty
resources are written back to disk.

---

## 5. The Plugin System

SimPE discovers plugins at runtime by scanning the `Plugins/` folder for DLL
files matching two patterns:

- `*.plugin.dll` — resource wrappers and/or tool menu items
- `*.wizard.dll` — step-based wizard tools

### What a plugin DLL must contain

To add **resource wrappers**: a `public` class implementing `IWrapperFactory`.

To add **Tools menu items**: a `public` class implementing `IToolFactory`:

```csharp
public class MyToolFactory : AbstractWrapperFactory, IToolFactory
{
    public IToolPlugin[] KnownTools => new IToolPlugin[] { new MyTool() };
}

public class MyTool : AbstractTool, ITool
{
    public bool IsEnabled(IPackedFileDescriptor pfd, IPackageFile package) => true;

    public IToolResult ShowDialog(ref IPackedFileDescriptor pfd,
                                  ref IPackageFile package)
    {
        new MyToolForm().ShowDialog();
        return new ToolResult(false, false);
    }

    public override string ToString() => "MyMenu\\My Tool Name…";
}
```

`ToString()` controls the menu placement. Backslash creates submenus.
Example: `"Wizards\\BSOK Wizard…"` → Tools > Wizards > BSOK Wizard…

### Loading sequence (PluginManager.cs)

```
LoadDynamicWrappers()
  for each *.plugin.dll and *.wizard.dll in Plugins/:
    LoadWrapperFactory(file)  → registers IWrapperFactory
    LoadToolFactory(file)     → registers IToolFactory / ITool

LoadStaticWrappers()
  → registers built-in factories directly (SimFactory, DefaultWrapperFactory, …)

LoadMenuItems()
  → iterates FileTable.ToolRegistry.Tools
  → creates ToolMenuItemExt for each
  → calls AddMenuItem() to place in Tools menu tree
```

---

## 6. The File Index

The **Scenegraph File Index** is an in-memory catalogue of every resource across
all open packages plus the game's own data files.

### Why it exists

Many resources reference other resources by TGI. For example, a TXMT material
references a TXTR texture. Those two files might be in different packages — the
file index allows cross-package lookups.

### Key classes

| Class | Purpose |
|---|---|
| `SimPe.FileTable` (static) | Service locator — holds WrapperRegistry, ToolRegistry, FileIndex, etc. |
| `IScenegraphFileIndex` | Interface for the index |
| `IScenegraphFileIndexItem` | One entry: a (IPackedFileDescriptor, IPackageFile) pair |

### PJSE File Table (separate)

The PJSE tools maintain their own file table (`pjse.FileTable.GFT`) that indexes
the game's global STR# and BHAV files separately. It loads its data from
`Plugins/pjse.coder.plugin/GlobalStrings.package` (bundled in the repo).

---

## 7. Project Map

```
SimPE-Fixed/
│
├─ SimPE.Interfaces/        Core interfaces only — no implementations.
│                           IPackageFile, IFileWrapper, IWrapperFactory, etc.
│                           Everything else depends on this.
│
├─ SimPE.Helper/            Utilities and constants.
│                           MetaData.cs — all type code constants
│                           Helper.cs   — paths, version, registry access
│                           Localization — string lookup
│
├─ SimPE.Packages/          Binary package I/O.
│                           GeneratableFile — reads and writes .package files
│                           PackedFile      — handles compression
│
├─ SimPE.WorkSpaceHelper/   High-level package management.
│                           FileTable       — static service locator
│                           LoadedPackage   — wraps IPackageFile with UI events
│                           ResourceLoader  — manages editor panels
│                           PluginManager   — plugin discovery and loading
│                           LoadFileWrappersExt — reflection-based DLL loader
│
├─ SimPE.Filehandlers/      Built-in resource wrappers (the big one).
│                           One wrapper per major type: OBJD, BHAV, CPF,
│                           GLOB, CREG, FAMI, STR#, CTSS, SDSC, HOUS, …
│
├─ SimPE.Plugin/            Scenegraph and 3D resource wrappers.
│                           TXMT, TXTR, GMDC, SHPE, CRES, ANIM, MMAT, GZPS, 3IDR
│                           ScenegraphWrapperFactory — registers all of these
│
├─ SimPE.Scenegraph/        3D rendering.
│                           Uses Ambertation.3D (decompiled from lib/).
│                           Renders meshes and textures in a preview panel.
│
├─ SimPE.HGBH/              Neighborhood file handling.
│                           Parses NGBH (neighborhood) packages.
│                           Builds as simpe.ngbh.dll AND simpe.ngbh.plugin.dll
│                           (the .plugin copy is how the plugin loader finds it).
│
├─ SimPE.Main/              The application executable.
│                           MainForm        — main window
│                           Main.Designer   — layout (WeifenLuo DockPanel for dc)
│                           Main.Setup      — initialization sequence
│                           Main.Theme      — dock theme, layout management
│                           PluginManager   — orchestrates startup
│
├─ Ambertation.3D/          Decompiled from lib/ambertation.3D.dll
├─ Ambertation.Utilities/   Decompiled from lib/ambertation.utilities.dll
├─ Ambertation.3D.Gl.Binding/ Replaces the old DirectX/MDX binding with OpenTK
│
├─ _PJSE/                   PJSE tools (optional, advanced)
│   ├─ pjse Coder/          BHAV editor, GUID tool, BCON/TTAB editors
│   │                       Requires Plugins/pjse.coder.plugin/GlobalStrings.package
│   ├─ pjOBJDTool/          Object data editor (catalog sort, EP flags)
│   ├─ pjObjKeyTool/        Resource key fixer for old-style GZPS recolors
│   ├─ pjBodyMeshTool/      Body mesh extraction and linking
│   └─ pjHoodTool/          Neighborhood export (superseded by Mootilda's tools)
│
└─ __NonCore/               Older optional plugins
    ├─ WOSimPe - BSOK/      Body shape override kit wizard (GPL, Chris Hatch)
    ├─ WOSimPe - Wardrobecleaner/  Wardrobe management wizard
    └─ WOSimPe - Recolor/   Recolor helper tools
```

---

## 8. Application Startup Flow

```
Program.Main()
  └─ Application.Run(new MainForm())

MainForm..ctor()
  ├─ InitializeComponent()       — WinForms designer-generated layout
  └─ MainForm_Load()
       └─ Setup() [Main.Setup.cs]
            ├─ Splash screen shown
            ├─ LoadedPackage created  (event sink for the open package)
            ├─ PluginManager created
            │    ├─ TypeRegistry created
            │    ├─ FileTable.WrapperRegistry = TypeRegistry
            │    ├─ LoadDynamicWrappers()  — scans Plugins/*.plugin.dll
            │    ├─ LoadStaticWrappers()   — registers built-ins
            │    └─ LoadMenuItems()        — populates Tools menu
            ├─ InitTheme()
            │    └─ dc.Theme = VS2015LightTheme  (required by WeifenLuo)
            └─ ReloadLayout()  — positions docked panels
```

After startup, three docked panels are visible by default:
- **Resource Tree** (left) — resources grouped by type
- **Resource List** (centre-left) — individual files in selected type
- **Plugin View** (right) — editor panel for selected resource

---

## 9. Resource Selection Flow

```
User clicks a row in Resource List
└─ ResourceListViewExt fires SelectResource event
   └─ MainForm.lv_SelectResource()
      └─ ResourceLoader.AddResource(IScenegraphFileIndexItem, overload=false)
         ├─ GetWrapper(fii)
         │    ├─ WrapperRegistry.FindHandler(pfd.Type)
         │    └─ if none: WrapperRegistry.FindHandler(first 64 bytes)
         ├─ wrapper.ProcessData(pfd, package)   — parse binary
         ├─ wrapper.LoadUI()
         │    └─ wrapper.UIHandler.GUIHandle    — creates WinForms control
         ├─ Present(fii, wrapper)
         │    ├─ Creates WeifenLuo DockContent
         │    ├─ Embeds GUIHandle in it
         │    └─ doc.Show(dc, DockState.Document)
         └─ wrapper.RefreshUI()                 — populate fields from data
```

---

## 10. Neighborhood Files vs. Object Packages

### Object package

A `.package` file for a custom object or recolor. Resources inside:
- **OBJD** — catalog definition (price, sort flags, EP requirements)
- **CTSS / STR#** — localized name and description
- **TXMT + TXTR** — material and texture
- **GMDC + SHPE + CRES** — 3D mesh
- **BHAV / BCON / TTAB / GLOB** — scripted behavior
- **GZPS + 3IDR** (old style) or **MMAT** (new style) — recolor data

### Neighborhood file

A large `.package` file in `Neighborhoods/N###/N###_Neighborhood.package`.
Resources inside:
- **HOUS** — neighborhood header
- **SDSC** — one per Sim (description, aspiration, personality)
- **SDNA** — Sim genetics and appearance
- **FAMI** — family and household groupings
- **SREL / CHAR** — relationships between Sims
- Lot sub-packages embedded by reference

SimPE opens neighborhoods to give access to Sim data. Object packages give
access to the catalog object system and scripting.

---

## 11. Major Subsystems

### BHAV Scripting (SimPe BHAV / pjse Coder)

The Sims 2 uses a bytecode scripting system:
- **BHAV** — compiled behavior functions (like Java .class files)
- **BCON** — named constants referenced by BHAV
- **TTAB** — interaction table (which BHAVs are available as Sim interactions)
- **GLOB** — semi-global libraries shared across objects

The PJSE Coder adds a disassembler/decompiler that turns raw BHAV opcodes into
readable pseudocode, and a wizard-based operand editor.

### 3D Scenegraph (SimPE.Scenegraph / SimPE.Plugin)

Renders object previews using:
- **GMDC** — vertex/triangle geometry
- **SHPE** — shape container (links mesh parts to materials)
- **CRES** — colour resource (connects SHPE to TXMT subsets)
- **TXMT** — material (references TXTR for each texture slot)
- **TXTR** — DDS or proprietary RLE-compressed image

The old DirectX/MDX binding was replaced with OpenTK (`Ambertation.3D.Gl.Binding`).

### Recolor Systems (two generations)

**Old style (pre-Apartment Life):**
- **GZPS** property set + **3IDR** resource ID list
- ObjKey Tool (PJSE) can fix broken resource key linkages in these

**New style (Apartment Life+):**
- **MMAT** Material Override — simpler, self-contained
- Most modern CC uses this format

### Wizards (__NonCore / WOSimPe)

Step-based tools that walk the user through a multi-step process:
- Implement `IWizardForm` for each step
- `WizardHostForm` hosts the steps and handles Back/Next navigation
- Deployed as `*.wizard.dll` and loaded alongside `*.plugin.dll`

**BSOK Wizard** — sets up body shape override kits (EP-specific body shapes).
**Wardrobe Wrangler** — cleans up clothing conflicts in a neighborhood.

---

## 12. Tools Menu vs. Wrappers

These are two distinct extension points:

| | Wrappers | Tools |
|---|---|---|
| **Purpose** | View/edit a selected resource inline | Perform an operation on demand |
| **Interface** | `IFileWrapper` / `AbstractWrapper` | `ITool` / `AbstractTool` |
| **Factory** | `IWrapperFactory` | `IToolFactory` |
| **Triggered by** | Clicking a resource in the list | Clicking a menu item in Tools |
| **UI lives** | Embedded in document dock area | Separate dialog/form |
| **Enabled when** | Type code matches | `IsEnabled(pfd, package)` returns true |

A plugin DLL can implement both factories.

---

## 13. Build Output Layout

All projects output to a unified folder:

```
bin/Debug/
  SimPE.Main.exe          — the application
  *.dll                   — all library assemblies
  Plugins/
    *.plugin.dll          — resource wrappers and tool plugins
    *.wizard.dll          — wizard plugins
    pjse.coder.plugin/
      GlobalStrings.package   — PJSE catalog label strings (bundled data)
```

`Directory.Build.targets` controls this: any project whose `AssemblyName` ends
in `.plugin` or `.wizard` is automatically copied to both `bin/Debug/` and
`bin/Debug/Plugins/`.

---

## 14. Known Platform Notes (.NET 8)

Things fixed during the .NET 2.0 → .NET 8 migration that a maintainer should
know about:

- **`ProgressBar.BackColor = Color.Transparent`** throws on .NET 8. Removed.
- **`TabPage` added to `Panel`** throws on .NET 8. Changed `TabPage` to `Panel`.
- **`Process.Start(url)`** requires `UseShellExecute = true` on .NET 8.
- **`AppDomain.DefineDynamicAssembly`** removed — replaced with
  `AssemblyBuilder.DefineDynamicAssembly`.
- **`BinaryFormatter`** disabled by default — resource `.resx` files with
  serialized `CheckedListBox` labels use it; suppressed at build level.
- **`Microsoft.NET.Sdk.WindowsDesktop`** is now `Microsoft.NET.Sdk` — all
  projects updated.
- **CA1416** (Windows platform compatibility) suppressed globally in
  `Directory.Build.targets` — the entire solution targets `net8.0-windows`.
- **WeifenLuo DockPanelSuite** requires `dc.Theme` to be set before any content
  can be shown — set to `VS2015LightTheme` in `Main.Theme.cs:InitTheme()`.
- **`Environment.Exit(0)`** added at end of `Program.Main()` — plugin threads
  and stream handles otherwise keep the process alive after close, locking DLLs
  for the next build.
