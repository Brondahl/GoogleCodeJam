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

Write-Host "Updating .sln to include new project."

$slnFile = "GoogleCodeJam.sln"
$slnFileContent = [io.file]::ReadAllText($slnFile)

$newPrjGuid = (New-Guid).toString().toUpper()

$projectDefintionLocatorRegex = "(?smi)EndProject\r\nGlobal"
$newProjDefinition = 'Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "'+$newProjectName+'", "'+$year+'\'+$newProjectName+'\'+$newProjectName+'.csproj", "{'+$newPrjGuid+'}"'
$newProjectDefinitionBlock = @"
EndProject
$newProjDefinition
EndProject
Global
"@

$projectBuildConfigLocatorRegex = "(?smi)\tEndGlobalSection\r\n\tGlobalSection\(SolutionProperties\) = preSolution"
$newProjectBuildConfigBlock = @"
		{$newPrjGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{$newPrjGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{$newPrjGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{$newPrjGuid}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
"@

$guidRegex = "[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}"
$yearSolutionFolderLocatorRegex = '(?smi)Project\("\{2150E333-8FDC-42A3-9474-1A3956D46DE8\}"\) = "'+$year+'", "'+$year+'", "\{('+$guidRegex+')\}"'
$currentYearSolutionFolder = [regex]::match($slnFileContent,$yearSolutionFolderLocatorRegex).Groups[1].Value

$projectFolderConfigLocatorRegex = "(?smi)\tEndGlobalSection\r\n\tGlobalSection\(ExtensibilityGlobals\) = postSolution"
$newProjectFolderConfigBlock = @"
		{$newPrjGuid} = {$currentYearSolutionFolder}
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
"@

$slnFileContent `
    -replace $projectDefintionLocatorRegex, $newProjectDefinitionBlock `
    -replace $projectBuildConfigLocatorRegex, $newProjectBuildConfigBlock `
    -replace $projectFolderConfigLocatorRegex, $newProjectFolderConfigBlock `
    | Out-File -Encoding "UTF8" $slnFile


Write-Host "Updating EntryPoint Program.cs to target new Project"

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

Write-Host "Updating EntryPoint.csproj to reference new Project"

$csprojFile = ".\EntryPoint\EntryPoint.csproj"
$csprojFileContent = [io.file]::ReadAllText($csprojFile)

$projectReferenceLocatorRegex = '(?smi)(^\s*<ProjectReference Include="\.\.\\20\d\d\\\w*\\\w*\.csproj">.+?</Name>)'
$newProjectReferenceBlock = @"
    <ProjectReference Include="..\$year\$newProjectName\$newProjectName.csproj">
      <Project>{$newPrjGuid}</Project>
      <Name>$newProjectName</Name>
"@

#    <ProjectReference Include="..\2022\ASeDatAb\ASeDatAb.csproj">
#      <Project>{5a246888-8444-4c5a-a03d-ab60cf4c3a2d}</Project>
#      <Name>ASeDatAb</Name>
#    <ProjectReference Include="..\2022\FinalTest\FinalTest.csproj">
#      <Project>{fa59ef71-f9eb-4944-aa08-4ebe1216ddde}</Project>
#      <Name>FinalTest</Name>

$csprojFileContent `
    -replace $projectReferenceLocatorRegex, $newProjectReferenceBlock `
    | Out-File -Encoding "UTF8" $csprojFile


# Used for testing only!
#Write-Host "Copy Complete. Press any key to delete."
#[void][System.Console]::ReadKey($true)
#del $targetDir -Recurse

Write-Host "Complete."
