#!/bin/bash

echo "Check env"
mono --version
ls

match='\(.*<Analyzer Include.*\)'
insert='    <Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/>\n    <Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/>'
file='./AFKScape/ScriptsAssembly.csproj'
style_cop_csproj='./AFKScape/ScriptsAssemblyStyleCop.csproj'

echo "Creating StyleCop csproj"
sed "s/$match/\1\n$insert/" $file > $style_cop_csproj

file_path='C:\/Program Files\/Unity\/Hub\/Editor\/2019.3.7f1'
linux_path='\/opt\/Unity'
sed -i "s/$file_path/$linux_path/" $style_cop_csproj

echo "Running Style Cop Analyzer"
dotnet build $style_cop_csproj

echo "Cleaning up"
rm $style_cop_csproj
echo "Done"
