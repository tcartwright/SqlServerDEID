Clear-Host

# the dotnet sdk is required as a minimum to run this file

dotnet clean "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
dotnet msbuild "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
$releasePath = "$PSScriptRoot\SqlServerDEID.Editor\bin\Release\"
$buildPath = "$($releasePath)net4.8\"

if (Test-Path "$buildPath\Testfiles" -PathType Container) {
    Remove-Item "$buildPath\Testfiles" -Force -Recurse 
}

Remove-Item "$releasePath\SqlServerDEID*.zip" -Force -ErrorAction SilentlyContinue

$version = (Get-Item "$($buildPath)SqlServerDEID.Editor.exe")

Get-ChildItem $buildPath | Compress-Archive -DestinationPath "$releasePath\SqlServerDEID-$($version.VersionInfo.FileVersion).zip" -Update

. explorer.exe $releasePath