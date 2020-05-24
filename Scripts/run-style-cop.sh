#!/bin/bash

match='\(<Project ToolsVersion.*\)'
insert='<ItemGroup><Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/>\n<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/><\/ItemGroup>'
file='./AFKScape/ScriptsAssembly.csproj'
style_cop_csproj='./AFKScape/ScriptsAssemblyStyleCop.csproj'

echo "--- Creating StyleCop .csproj ---"
sed "s/$match/\1\n$insert/" $file > $style_cop_csproj

echo "--- Displaying csproj ---"
cat $style_cop_csproj

echo "--- Running Style Cop Analyzer ---"
export FrameworkPathOverride=/Library/Frameworks/Mono.framework/Versions/Current
dotnet build $style_cop_csproj --verbosity diag

echo "--- Cleaning up ---"
rm $style_cop_csproj
echo "--- Done ---"
