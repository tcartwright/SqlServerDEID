Clear-Host

dotnet clean "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
dotnet msbuild "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
$buildPath = "$PSScriptRoot\SqlServerDEID.Editor\bin\Release\net4.8\"
$releasePath = "$PSScriptRoot\SqlServerDEID.Editor\bin\Release\"

# Get-ChildItem $buildPath -Filter "Syncfusion*.dll" | Remove-Item -Verbose

if (Test-Path "$buildPath\Testfiles" -PathType Container) {
    Remove-Item "$buildPath\Testfiles" -Force -Recurse 
}

Remove-Item "$releasePath\SqlServerDEID.zip" -Force -ErrorAction SilentlyContinue

Get-ChildItem $buildPath | Compress-Archive -DestinationPath "$releasePath\SqlServerDEID.zip" -Update

. explorer.exe $releasePath