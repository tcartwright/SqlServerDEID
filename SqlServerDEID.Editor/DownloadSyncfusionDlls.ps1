

$webClient = New-Object System.Net.WebClient

$webClient.DownloadFile("https://www.nuget.org/api/v2/package/Syncfusion.SfDataGrid.WinForms/latest", "$PSScriptRoot\Syncfusion.SfDataGrid.WinForms.zip")