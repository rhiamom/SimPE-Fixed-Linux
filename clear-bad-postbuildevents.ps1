# Clears PostBuildEvent blocks that contain hardcoded SimPeSource paths
$root = $PSScriptRoot
$projects = Get-ChildItem -Path $root -Recurse -Filter "*.csproj"

foreach ($proj in $projects) {
    $content = Get-Content $proj.FullName -Raw
    if ($content -notmatch 'SimPeSource') { continue }

    # Replace multi-line PostBuildEvent that contains SimPeSource with empty one
    $newContent = $content -replace '(?s)<PostBuildEvent>[^<]*SimPeSource[^<]*</PostBuildEvent>', '<PostBuildEvent />'

    if ($newContent -ne $content) {
        Set-Content -Path $proj.FullName -Value $newContent -NoNewline
        Write-Host "Cleared PostBuildEvent in: $($proj.Name)"
    }
}
Write-Host "Done."
