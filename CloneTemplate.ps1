$targetProjectName = ""

while($targetProjectName -eq "") {
    Write-Host "You must provide a non-empty Project Name!"
    $targetProjectName = Read-Host -Prompt "Name of new Project?"
    $targetDir = ".\2018\$targetProjectName"
}

$sourceDir = ".\2018\TemplateProject"

#Clone the TemplateFolder
robocopy $sourceDir $targetDir /E /XD dirs obj bin /NJH /NJS /NP /NS /NDL

Write-Host
    
Get-ChildItem $targetDir -Filter * | Foreach-Object {
    $filePath = $_.FullName
        
    "Replacing Contents of:  $filePath"
    $updatedContent = Get-Content $filePath | ForEach-Object { $_ -replace "TemplateProject", "$newProjectName" }
    $updatedContent | Set-Content $filePath
    
    if($filePath -like "*TemplateProject*") {
        $newFilePath = ($filePath -replace "TemplateProject", $newProjectName)
        "Renaming File to :      $newFilePath"
        ren $_.FullName $newFilePath
    }
}
Write-Host

Write-Host "Updating EntryPoint to target new Project"

$programEntryPointFile = ".\EntryPoint\Program.cs"

"namespace GoogleCodeJam"              | Set-Content $programEntryPointFile
"{"                                    | Add-Content $programEntryPointFile
"  using $targetProjectName;"          | Add-Content $programEntryPointFile
""                                     | Add-Content $programEntryPointFile
"  class Program"                      | Add-Content $programEntryPointFile
"  {"                                  | Add-Content $programEntryPointFile
"    static void Main(string[] args)"  | Add-Content $programEntryPointFile
"    {"                                | Add-Content $programEntryPointFile
"      CaseSolver.Run();"              | Add-Content $programEntryPointFile
"    }"                                | Add-Content $programEntryPointFile
"  }"                                  | Add-Content $programEntryPointFile
"}"                                    | Add-Content $programEntryPointFile
""                                     | Add-Content $programEntryPointFile

# Used for testing only!
#Write-Host "Copy Complete. Press any key to delete."
#[void][System.Console]::ReadKey($true)
#del $targetDir -Recurse

Write-Host "Complete. Press any key to close."
[void][System.Console]::ReadKey($true)

