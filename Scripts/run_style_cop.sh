#!/bin/bash

match='\(<Project ToolsVersion.*\)'
insert='<ItemGroup> \n \
<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.CodeFixes.dll" \/> \n \
<Analyzer Include="Packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll" \/> \n \
<\/ItemGroup> \n \
<PropertyGroup><CodeAnalysisRuleSet>AnalyzerRules.ruleset<\/CodeAnalysisRuleSet><\/PropertyGroup> '

file='./AFKScape/ScriptsAssembly.csproj'
style_cop_csproj='./AFKScape/ScriptsAssemblyStyleCop.csproj'

echo "--- Creating StyleCop .csproj ---"
sed "s/$match/\1\n$insert/" $file > $style_cop_csproj

echo "--- Running Style Cop Analyzer ---"
export FrameworkPathOverride=/Library/Frameworks/Mono.framework/Versions/Current
log_file='log.txt'
dotnet build $style_cop_csproj --verbosity m > $log_file

echo "--- Showing Results ---"
cat $log_file

warnings=$(sed -n -e 's/ *\(.*\) Warning(s)/\1/p' $log_file)
errors=$(sed -n -e 's/ *\(.*\) Error(s)/\1/p' $log_file)

if [[ $warnings -eq 0 && $errors -eq 0 ]]; then
    exit_status='0'
else
    exit_status='1'
fi

echo "--- Cleaning up ---"
rm $style_cop_csproj
rm $log_file
echo "--- Done ---"

exit $exit_status
