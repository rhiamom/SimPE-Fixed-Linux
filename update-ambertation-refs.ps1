# Updates ambertation.3D and ambertation.utilities DLL references to project references
$root = $PSScriptRoot
$projects = Get-ChildItem -Path $root -Recurse -Filter "*.csproj" |
    Where-Object { $_.FullName -notmatch "Ambertation\.3D" -and $_.FullName -notmatch "Ambertation\.Utilities" }

# All projects are one level deep from root
$rel3D   = "..\Ambertation.3D\ambertation.3D.csproj"
$relUtil = "..\Ambertation.Utilities\ambertation.utilities.csproj"

foreach ($proj in $projects) {
    $content = Get-Content $proj.FullName -Raw
    $original = $content

    $changed3D   = $original -match 'ambertation\.3D[^.]'
    $changedUtil = $original -match 'ambertation\.utilities'

    if (-not $changed3D -and -not $changedUtil) { continue }

    # Remove ambertation.3D reference block (plain or versioned Include)
    $content = $content -replace '(?s)\s*<Reference Include="ambertation\.3D[^"]*"[^>]*>.*?</Reference>', ""
    # Remove ambertation.utilities reference block
    $content = $content -replace '(?s)\s*<Reference Include="ambertation\.utilities[^"]*"[^>]*>.*?</Reference>', ""

    # Build the ProjectReference block to insert
    $refs = ""
    if ($changed3D)   { $refs += "    <ProjectReference Include=`"$rel3D`" />`r`n" }
    if ($changedUtil) { $refs += "    <ProjectReference Include=`"$relUtil`" />`r`n" }
    $newGroup = "`r`n  <ItemGroup>`r`n$refs  </ItemGroup>`r`n"

    # Only replace the LAST </Project> in the file
    $lastIdx = $content.LastIndexOf("</Project>")
    if ($lastIdx -ge 0) {
        $content = $content.Substring(0, $lastIdx) + $newGroup + "</Project>"
    }

    Set-Content -Path $proj.FullName -Value $content -NoNewline
    Write-Host "Updated: $($proj.Name)"
}
Write-Host "Done."
