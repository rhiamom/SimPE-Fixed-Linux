$root = "c:\Users\rhiam\source\repos\SimPE-VSCode\SimPE-Fixed"
$projects = @(
    "SimPe GMDC Exporter\Plugin.GMDCExporter.csproj",
    "SimPe More Plugins\simpeMoreplugin.csproj",
    "SimPe Toolbox Scenegraph\Plugin.ToolboxScenegraph.csproj",
    "SimPE.Downloads\Plugin.Downloads.csproj",
    "SimPE.Filehandlers\SimPE.Filehandlers.csproj",
    "SimPE.GMDCExporterbase\SimPE.GMDCExporterbase.csproj",
    "SimPE.GraphControl\SimPE.GraphControl.csproj",
    "SimPE.HGBH\SimPE.HGBH.csproj",
    "SimPE.Main\SimPE.Main.csproj",
    "SimPE.NameProvider\SimPE.NameProvider.csproj",
    "SimPE.PluginDockBox\Plugin.Dockbox.csproj",
    "SimPE.RCOL\SimPE.RCOL.csproj",
    "SimPE.Sims\SimPE.Sims.csproj",
    "SimPE.Splash\SimPE.Splash.csproj",
    "SimPE.ToolBoxWorkshops\SimPE.ToolBoxWorkshops.csproj",
    "SimPE.WorkSpaceHelper\SimPE.WorkSpaceHelper.csproj",
    "Wizards of SimPe\Wizards of SimPe.csproj"
)

foreach ($proj in $projects) {
    $path = Join-Path $root $proj
    $content = Get-Content $path -Raw -Encoding UTF8

    $content = [regex]::Replace($content, '<Reference Include="xsi\.lib"[^>]*/>', '', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    $content = [regex]::Replace($content, '<Reference Include="xsi\.lib"[^>]*>.*?</Reference>', '', [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if ($content -notmatch 'xsi\.lib\.csproj') {
        $content = $content -replace '</Project>', "  <ItemGroup>`r`n    <ProjectReference Include=""..\xsi.lib\xsi.lib.csproj"" />`r`n  </ItemGroup>`r`n</Project>"
    }

    Set-Content $path $content -Encoding UTF8 -NoNewline
    Write-Host "Updated: $proj"
}
