Clear-Host

dotnet clean "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
dotnet msbuild "$PSScriptRoot\SqlServerDEID.Editor\SqlServerDEID.Editor.csproj" -property:Configuration=Release 
$buildPath = "$PSScriptRoot\SqlServerDEID.Editor\bin\Release\net4.8\"

Get-ChildItem $buildPath -Filter "Syncfusion*.dll" | Remove-Item -Verbose

if (Test-Path "$buildPath\Testfiles" -PathType Container) {
    Remove-Item "$buildPath\Testfiles" -Force -Recurse 
}

Remove-Item "$buildPath\..\Release.zip" -Force -ErrorAction SilentlyContinue
Get-ChildItem $buildPath | Compress-Archive -DestinationPath "$buildPath\..\Release.zip" -Update

