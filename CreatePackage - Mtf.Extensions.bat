nuget pack Mtf.Extensions.nuspec
REM dotnet pack --include-symbols --include-source
powershell.exe -ExecutionPolicy Bypass -File ".\Mtf.Extensions.IncrementNugetPackageVersion.ps1"
pause