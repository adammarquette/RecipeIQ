[CmdletBinding()]
param(
    [string]$SourceRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
    [string]$ManifestPath = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")).Path "templates/agentic-foundation/template.manifest.json"),
    [string]$OutputRoot = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")).Path "templates/agentic-foundation/dist"),
    [string]$ProjectName = "RecipeIQ",
    [string]$RootNamespace = "MarqSpec.RecipeIQ",
    [string]$GitHubOwner = "adammarquette",
    [string]$RepositoryName = "RecipeIQ"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $ManifestPath -PathType Leaf)) {
    throw "Template manifest not found: $ManifestPath"
}

$manifest = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json

$templateRoot = Join-Path $OutputRoot "template"
if (Test-Path -LiteralPath $templateRoot) {
    Remove-Item -LiteralPath $templateRoot -Recurse -Force
}
New-Item -ItemType Directory -Path $templateRoot -Force | Out-Null

function Copy-RelativeItem {
    param(
        [Parameter(Mandatory = $true)] [string]$RelativePath,
        [Parameter(Mandatory = $true)] [bool]$IsDirectory
    )

    $sourcePath = Join-Path $SourceRoot $RelativePath
    if (-not (Test-Path -LiteralPath $sourcePath)) {
        Write-Warning "Skipping missing path: $RelativePath"
        return
    }

    $destinationPath = Join-Path $templateRoot $RelativePath
    $destinationParent = Split-Path -Path $destinationPath -Parent
    New-Item -ItemType Directory -Path $destinationParent -Force | Out-Null

    if ($IsDirectory) {
        Copy-Item -LiteralPath $sourcePath -Destination $destinationPath -Recurse -Force
    }
    else {
        Copy-Item -LiteralPath $sourcePath -Destination $destinationPath -Force
    }
}

foreach ($directory in $manifest.includeDirectories) {
    Copy-RelativeItem -RelativePath $directory -IsDirectory $true
}

foreach ($file in $manifest.includeFiles) {
    Copy-RelativeItem -RelativePath $file -IsDirectory $false
}

$textExtensions = @(
    ".md", ".yml", ".yaml", ".json", ".cs", ".csproj", ".props", ".targets", ".ps1", ".sh", ".txt"
)

$replacementRules = @(
    @{ From = "$GitHubOwner/$RepositoryName"; To = "__GITHUB_REPO_SLUG__" },
    @{ From = $RootNamespace; To = "__ROOT_NAMESPACE__" },
    @{ From = $ProjectName; To = "__PROJECT_NAME__" },
    @{ From = $GitHubOwner; To = "__GITHUB_OWNER__" },
    @{ From = $RepositoryName; To = "__REPOSITORY_NAME__" }
)

Get-ChildItem -Path $templateRoot -Recurse -File |
    Where-Object { $textExtensions -contains $_.Extension.ToLowerInvariant() } |
    ForEach-Object {
        $content = Get-Content -LiteralPath $_.FullName -Raw
        foreach ($rule in $replacementRules) {
            $escaped = [Regex]::Escape($rule.From)
            $content = [Regex]::Replace($content, $escaped, $rule.To)
        }
        Set-Content -LiteralPath $_.FullName -Value $content -Encoding utf8
    }

$metaPath = Join-Path $OutputRoot "template.metadata.json"
$meta = [ordered]@{
    generatedUtc = (Get-Date).ToUniversalTime().ToString("o")
    sourceRoot = $SourceRoot
    templateVersion = $manifest.templateVersion
    placeholders = $manifest.defaultPlaceholders
}
$meta | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $metaPath -Encoding utf8

Write-Host "Template exported to: $templateRoot"
Write-Host "Metadata written to: $metaPath"
