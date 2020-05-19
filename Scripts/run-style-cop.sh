#!/bin/bash

match='\(.*<Analyzer Include.*\)'
insert='    <Analyzer Include="packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/>\n    <Analyzer Include="packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/>'
file='./AFKScape/ScriptsAssembly.csproj'
style_cop_csproj='./AFKScape/ScriptsAssemblyStyleCop.csproj'

echo "Creating StyleCop csproj"
sed "s/$match/\1\n$insert/" $file > $style_cop_csproj

echo "Running Style Cop Analyzer"
dotnet build $style_cop_csproj

echo "Cleaning up"
rm $style_cop_csproj
echo "Done"
