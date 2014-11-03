@ECHO OFF

ECHO Preparing NuGet...
CALL ..\..\set-nuget-key.bat
del lib\net40\* /Q
mkdir lib
mkdir lib\net40
copy ..\Ministry.MultiHostRedirector\bin\Release\*.dll lib
del *.nupkg
pause

ECHO Publishing to NuGet...
nuget pack Ministry.MultiHostRedirector.nuspec
nuget push *.nupkg

pause