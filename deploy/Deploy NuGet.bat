call ../../set-nuget-key.bat

cd ../nuget
del *.nupkg
nuget pack Ministry.MultiHostRedirector.nuspec
cd ../deploy

nuget push *.nupkg

pause