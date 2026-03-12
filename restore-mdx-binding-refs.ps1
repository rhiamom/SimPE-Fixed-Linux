# Re-adds ambertation.3D.mdx.binding DLL reference to projects that have the ambertation.3D ProjectReference
# (these projects had the mdx.binding reference removed by mistake)
$root = $PSScriptRoot
$projects = Get-ChildItem -Path $root -Recurse -Filter "*.csproj" |
    Where-Object { $_.FullName -notmatch "Ambertation\.3D" -and $_.FullName -notmatch "Ambertation\.Utilities" }

foreach ($proj in $projects) {
    $content = Get-Content $proj.FullName -Raw

    # Only process projects that have the ambertation.3D ProjectReference but NOT the mdx.binding reference
    if ($content -notmatch 'ambertation\.3D\.csproj') { continue }
    if ($content -match 'mdx\.binding') { continue }

    # Compute relative path to lib\ambertation.3D.mdx.binding.dll
    $projDir = $proj.Directory.FullName
    # All projects are one level deep - relative path is always ..\lib\
    $hintPath = "..\lib\ambertation.3D.mdx.binding.dll"

    $mdxRef = "  <ItemGroup>`r`n    <Reference Include=`"ambertation.3D.mdx.binding`">`r`n      <HintPath>$hintPath</HintPath>`r`n    </Reference>`r`n  </ItemGroup>`r`n"

    # Insert before last </Project>
    $lastIdx = $content.LastIndexOf("</Project>")
    if ($lastIdx -ge 0) {
        $content = $content.Substring(0, $lastIdx) + $mdxRef + "</Project>"
    }

    Set-Content -Path $proj.FullName -Value $content -NoNewline
    Write-Host "Restored mdx.binding ref in: $($proj.Name)"
}
Write-Host "Done."
