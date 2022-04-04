[Environment]::CurrentDirectory = (Get-Location -PSProvider FileSystem).ProviderPath
$newProjectName = ""
$year = "2022"

while(($newProjectName -eq "") -Or ((Test-Path $targetDir -PathType Container))) {
    Write-Host "Cloning template to create project in $year folder"
    Write-Host "You must provide a non-empty Project Name, which does not already exist!"
    $newProjectName = Read-Host -Prompt "Name of new Project?"
    $targetDir = ".\$year\$newProjectName"
}

$sourceDir = ".\TemplateFolder\TemplateProject"

#Clone the TemplateFolder
robocopy $sourceDir $targetDir /E /XD dirs obj bin /NJH /NJS /NP /NS /NDL

Write-Host
    
Get-ChildItem $targetDir -Depth 9 -File | Foreach-Object {
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

$contents = @"
namespace GoogleCodeJam
{
  using $newProjectName;
  using Common;
  // See README.txt in sln root!!
 
  // Remember to add the new csproj,
  // and to add the proj ref and the
  // one-off fild to the EP project.
  class Program
  {
    static void Main(string[] args)
    {
      CaseSolver.Run();
    }
  }
}
"@

[System.IO.File]::WriteAllText($programEntryPointFile, $contents)

# Used for testing only!
#Write-Host "Copy Complete. Press any key to delete."
#[void][System.Console]::ReadKey($true)
#del $targetDir -Recurse

Write-Host "Complete. Press any key to close."
[void][System.Console]::ReadKey($true)

