call ../../set-nuget-key.bat

cd nuget
del *.nupkg
nuget pack Ministry.MultiHostRedirector.nuspec
nuget push *.nupkg
cd ../

pause