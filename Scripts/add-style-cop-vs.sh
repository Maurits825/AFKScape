#!/bin/bash

match='\(<Project ToolsVersion.*\)'
insert='<ItemGroup><Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/> \n \
<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/><\/ItemGroup> \n \
<PropertyGroup><CodeAnalysisRuleSet>StyleCop.ruleset<\/CodeAnalysisRuleSet><\/PropertyGroup> '

file='../AFKScape/ScriptsAssembly.csproj'

echo "--- Adding StyleCop ---"
sed -i "s/$match/\1\n$insert/" $file

echo "--- Done ---"
