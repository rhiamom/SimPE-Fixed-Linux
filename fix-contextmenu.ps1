# Replaces deprecated ContextMenu/MenuItem with ContextMenuStrip/ToolStripMenuItem
$root = $PSScriptRoot
$files = Get-ChildItem -Path $root -Recurse -Filter "*.cs" |
    Where-Object { $_.FullName -notmatch "\\obj\\" }

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -notmatch 'System\.Windows\.Forms\.ContextMenu\b|System\.Windows\.Forms\.MenuItem\b') { continue }

    $original = $content

    # Remove TODO comment lines for ContextMenu/MenuItem
    $content = $content -replace '(?m)[ \t]*// TODO ContextMenu is no longer supported\..*\r?\n', ''
    $content = $content -replace '(?m)[ \t]*// TODO MenuItem is no longer supported\..*\r?\n', ''

    # Type declarations and instantiations
    $content = $content -replace 'System\.Windows\.Forms\.ContextMenu\b', 'System.Windows.Forms.ContextMenuStrip'
    $content = $content -replace 'System\.Windows\.Forms\.MenuItem\b', 'System.Windows.Forms.ToolStripMenuItem'

    # Property assignments
    $content = $content -replace '\.ContextMenu\s*=', '.ContextMenuStrip ='

    # MenuItems collection -> Items (for ContextMenuStrip)
    $content = $content -replace '\.MenuItems\.AddRange\(new System\.Windows\.Forms\.ToolStripMenuItem\[\]', '.Items.AddRange(new System.Windows.Forms.ToolStripItem[]'
    $content = $content -replace '\.MenuItems\.AddRange\(new System\.Windows\.Forms\.ToolStripItem\[\]', '.Items.AddRange(new System.Windows.Forms.ToolStripItem[]'
    $content = $content -replace '\.MenuItems\.Add\(', '.Items.Add('
    $content = $content -replace '\.MenuItems\b', '.Items'

    # Remove .Index = N; lines (not valid on ToolStripMenuItem)
    $content = $content -replace '(?m)[ \t]*this\.\w+\.Index = \d+;\r?\n', ''

    # Remove .DefaultItem = ... lines
    $content = $content -replace '(?m)[ \t]*this\.\w+\.DefaultItem = [^;]+;\r?\n', ''

    # Shortcut mappings
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.CtrlC\b', '.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.CtrlV\b', '.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.CtrlX\b', '.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.CtrlA\b', '.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.CtrlZ\b', '.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.Ins\b', '.ShortcutKeys = System.Windows.Forms.Keys.Insert'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.Del\b', '.ShortcutKeys = System.Windows.Forms.Keys.Delete'
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.F5\b', '.ShortcutKeys = System.Windows.Forms.Keys.F5'
    # Any remaining Shortcut assignments - convert generically
    $content = $content -replace '\.Shortcut = System\.Windows\.Forms\.Shortcut\.([^;]+);', '.ShortcutKeys = System.Windows.Forms.Keys.$1;'

    if ($content -ne $original) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.Name)"
    }
}
Write-Host "Done."
