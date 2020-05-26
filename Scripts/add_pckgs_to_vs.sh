#!/bin/bash

match='\(<Project ToolsVersion.*\)'
insert='<ItemGroup> \n \
<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/> \n \
<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/> \n \
<Analyzer Include="packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\Google.Protobuf.dll" \/> \n \
<Analyzer Include="packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.CFG.dll" \/> \n \
<Analyzer Include="packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.CSharp.dll" \/> \n \
<Analyzer Include="packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.dll" \/> \n \
<\/ItemGroup> \n \
<PropertyGroup><CodeAnalysisRuleSet>StyleCop.ruleset<\/CodeAnalysisRuleSet><\/PropertyGroup> '

file='../AFKScape/ScriptsAssembly.csproj'

echo "--- Adding StyleCop ---"
sed -i "s/$match/\1\n$insert/" $file

echo "--- Done ---"
