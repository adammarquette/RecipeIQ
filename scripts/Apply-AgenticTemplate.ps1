[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$TargetRoot,
    [string]$TemplateRoot = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")).Path "templates/agentic-foundation/dist/template"),
    [string]$ProjectName = "MyProject",
    [string]$RootNamespace = "Company.MyProject",
    [string]$GitHubOwner = "your-org",
    [string]$RepositoryName = "my-repo",
    [switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $TemplateRoot -PathType Container)) {
    throw "Template root not found: $TemplateRoot. Run Export-AgenticTemplate.ps1 first."
}

if (-not (Test-Path -LiteralPath $TargetRoot -PathType Container)) {
    throw "Target root does not exist: $TargetRoot"
}

$templateFiles = Get-ChildItem -Path $TemplateRoot -Recurse -File
$copiedFiles = New-Object System.Collections.Generic.List[string]
foreach ($file in $templateFiles) {
    $relativePath = $file.FullName.Substring($TemplateRoot.Length).TrimStart([char[]]@([char]92, [char]47))
    $destinationPath = Join-Path $TargetRoot $relativePath
    $destinationParent = Split-Path -Path $destinationPath -Parent

    if (-not (Test-Path -LiteralPath $destinationParent)) {
        New-Item -ItemType Directory -Path $destinationParent -Force | Out-Null
    }

    if ((Test-Path -LiteralPath $destinationPath) -and (-not $Force.IsPresent)) {
        Write-Warning "Skipping existing file (use -Force to overwrite): $relativePath"
        continue
    }

    Copy-Item -LiteralPath $file.FullName -Destination $destinationPath -Force
    $copiedFiles.Add($destinationPath) | Out-Null
}

$textExtensions = @(
    ".md", ".yml", ".yaml", ".json", ".cs", ".csproj", ".props", ".targets", ".ps1", ".sh", ".txt"
)

$replacements = [ordered]@{
    "__PROJECT_NAME__" = $ProjectName
    "__ROOT_NAMESPACE__" = $RootNamespace
    "__GITHUB_OWNER__" = $GitHubOwner
    "__REPOSITORY_NAME__" = $RepositoryName
    "__GITHUB_REPO_SLUG__" = "$GitHubOwner/$RepositoryName"
}

$copiedFiles |
    ForEach-Object { Get-Item -LiteralPath $_ } |
    Where-Object { $textExtensions -contains $_.Extension.ToLowerInvariant() } |
    ForEach-Object {
        $content = Get-Content -LiteralPath $_.FullName -Raw
        $updated = $false

        foreach ($entry in $replacements.GetEnumerator()) {
            if ($content.Contains($entry.Key)) {
                $content = $content.Replace($entry.Key, $entry.Value)
                $updated = $true
            }
        }

        if ($updated) {
            Set-Content -LiteralPath $_.FullName -Value $content -Encoding utf8
        }
    }

Write-Host "Template applied to: $TargetRoot"
