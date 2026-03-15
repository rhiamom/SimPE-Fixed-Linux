$root = "c:\Users\rhiam\source\repos\SimPE-VSCode\SimPE-Fixed"
$projects = @(
    "AdvancedForms\AdvancedForms.csproj",
    "ColorBinningTool\ColorBinningTool.csproj",
    "SimPe FAMH\SimPe famhPlugin.csproj",
    "SimPe More Plugins\simpeMoreplugin.csproj",
    "SimPe Toolbox Scenegraph\Plugin.ToolboxScenegraph.csproj",
    "SimPE.Downloads\Plugin.Downloads.csproj",
    "SimPE.Filehandlers\SimPE.Filehandlers.csproj",
    "SimPE.GMDCExporterbase\SimPE.GMDCExporterbase.csproj",
    "SimPE.GraphControl\SimPE.GraphControl.csproj",
    "SimPE.Helper\SimPE.Helper.csproj",
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

    $content = [regex]::Replace($content, '<Reference Include="NetDocks"[^>]*/>', '', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    $content = [regex]::Replace($content, '<Reference Include="NetDocks"[^>]*>.*?</Reference>', '', [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if ($content -notmatch 'NetDocks\.csproj') {
        $content = $content -replace '</Project>', "  <ItemGroup>`r`n    <ProjectReference Include=""..\NetDocks\NetDocks.csproj"" />`r`n  </ItemGroup>`r`n</Project>"
    }

    Set-Content $path $content -Encoding UTF8 -NoNewline
    Write-Host "Updated: $proj"
}
