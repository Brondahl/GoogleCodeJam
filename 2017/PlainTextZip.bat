del /Q test.zip
rmdir /S /Q test

"C:\Program Files\7-Zip\7z.exe" a -tZip -xr!bin -xr!Release -xr!packages -xr!TempPE -xr!obj -xr!*.bat -xr!*.out -xr!*.in -xr!*.suo test.zip