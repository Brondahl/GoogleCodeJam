Bunch of helper methods and scripts to save frameworking time for Code Jam.

For the new year create the relevant year-stamped folders, and edit the year stamp in the two .ps1 scripts.
Then run CloneTemplate.ps1 with the name of the specific Question. This creates the project and updates the Entrypoint Main func.
To make it all useable, Add the project to the solution, and add a reference from the EntryPoint project to the newly created project; then it should all JustWork.

When you're ready to submit, run CopyToSingleFile, to create a single uploadable file.