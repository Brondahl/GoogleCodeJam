Bunch of helper methods and scripts to save frameworking time for Code Jam.

For the new year:
* create the relevant top-level year-stamped folder
* edit the year stamp in the two .ps1 scripts.
* update the top-level shortcut to point at the new standalone folder.
  * (May not get created until after the first run of "CopyToSingleFile"?)

Then run CloneTemplate.ps1 with the name of the specific Question. This will:
    * create the project folder and files
    * updates the Entrypoint Main func to reference the new proj.
    * update the .sln and .csproj files to add/reference the new proj.
The solution should *JustWork*, once you've run it once.
Probably worth running it with "FirstTestInput<Year>", first to check it's all working properly, and doing what you want.
    
When you're ready to submit, run CopyToSingleFile, to create a single uploadable file, sitting in EntryPoint/StandAloneFiles_<Year>
(There's a shortcut link in the top folder)
    Note: The way the files and the judge are working, all usings MUST be **inside** the namespace definitions in each file.
    That's not what the IDE does by default, so it often gets confused and then makes the judge-compiler angry.   


To remove a project you've created, you need to:
* delete the project in VS.
* delete the folder in the year-stamped folder, in Wind.Expl.

To re-clone an existing project from scratch ...??? delete then re-clone,I think?