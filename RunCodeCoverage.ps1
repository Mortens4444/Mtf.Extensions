param(
    [switch]$NoBrowser
)

$ErrorActionPreference = "Stop"
$repoRoot = $PSScriptRoot
$resultsDir = Join-Path $repoRoot "CoverageResults"
$reportDir = Join-Path $resultsDir "Report"

# Resolve the dotnet-tools.json manifest from the repo root regardless of the caller's cwd.
Push-Location $repoRoot
try {

Write-Host "Restoring local dotnet tools..." -ForegroundColor Cyan
dotnet tool restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "dotnet tool restore failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

if (Test-Path $resultsDir) {
    Remove-Item $resultsDir -Recurse -Force
}
New-Item -ItemType Directory -Path $resultsDir | Out-Null

$testProjects = @(
    "Mtf.Extensions.Tests\Mtf.Extensions.Tests.csproj",
    "Mtf.Windows.Forms.Extensions.Tests\Mtf.Windows.Forms.Extensions.Tests.csproj"
)

foreach ($project in $testProjects) {
    Write-Host "Running tests with coverage: $project" -ForegroundColor Cyan
    dotnet test (Join-Path $repoRoot $project) -c Debug --collect:"XPlat Code Coverage" --results-directory $resultsDir
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Tests failed for $project" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

$coberturaFiles = Get-ChildItem -Path $resultsDir -Filter "coverage.cobertura.xml" -Recurse | ForEach-Object { $_.FullName }
if ($coberturaFiles.Count -eq 0) {
    Write-Host "No coverage files were produced." -ForegroundColor Red
    exit 1
}

$reportInput = [string]::Join(";", $coberturaFiles)

Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
dotnet tool run reportgenerator `
    "-reports:$reportInput" `
    "-targetdir:$reportDir" `
    "-reporttypes:Html;TextSummary" `
    "-title:Mtf.Extensions Code Coverage"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Report generation failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

$summaryFile = Join-Path $reportDir "Summary.txt"
if (Test-Path $summaryFile) {
    Write-Host ""
    Get-Content $summaryFile | Write-Host
}

$indexFile = Join-Path $reportDir "index.html"
Write-Host ""
Write-Host "Coverage report: $indexFile" -ForegroundColor Green

if (-not $NoBrowser) {
    Start-Process $indexFile
}

}
finally {
    Pop-Location
}
