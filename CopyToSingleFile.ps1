$targetProjectName = ""

while(($targetProjectName -eq "") -Or (!(Test-Path $targetDir -PathType Container))) {
    Write-Host "You must provide a non-empty Project Name, whose folder exists!"
    $targetProjectName = Read-Host -Prompt "Name of Project to map to a single File? "
    $targetDir = ".\2018\$targetProjectName"
}

$targetDir = ".\2018\$targetProjectName"
$commonCodeDir = ".\Common"
$programEntryPointFile = ".\EntryPoint\Program.cs"
$destinationFile = ".\EntryPoint\StandAloneFiles\$targetProjectName.cs"

Write-Host

Write-Host "Writing initial entry point:"

"namespace GoogleCodeJam"              | Set-Content $destinationFile
"{"                                    | Add-Content $destinationFile
"  using $targetProjectName;"          | Add-Content $destinationFile
""                                     | Add-Content $destinationFile
"  class Program"                      | Add-Content $destinationFile
"  {"                                  | Add-Content $destinationFile
"    static void Main(string[] args)"  | Add-Content $destinationFile
"    {"                                | Add-Content $destinationFile
"      CaseSolver.Run();"              | Add-Content $destinationFile
"    }"                                | Add-Content $destinationFile
"  }"                                  | Add-Content $destinationFile
"}"                                    | Add-Content $destinationFile
""                                     | Add-Content $destinationFile

Write-Host

$projectSpecificFiles = Get-ChildItem $targetDir -Filter *.cs
$commonFiles = Get-ChildItem $commonCodeDir -Filter *.cs
$projectSpecificFiles + $commonFiles | Foreach-Object {
    $filePath = $_.FullName
    if(-Not ($filePath -like "*Test*"))
    {
        Write-Host "Copying Contents of:  $filePath"
        Get-Content $filePath | Add-Content $destinationFile
    }
    else
    {
        Write-Host "Skipping:  $filePath"
    }
}

Write-Host

Write-Host "Complete. Press any key to close."
[void][System.Console]::ReadKey($true)

