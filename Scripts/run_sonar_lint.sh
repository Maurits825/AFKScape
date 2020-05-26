#!/bin/bash

match='\(<Project ToolsVersion.*\)'
insert='<ItemGroup> \n \
<Analyzer Include="Packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\Google.Protobuf.dll" \/> \n \
<Analyzer Include="Packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.CFG.dll" \/> \n \
<Analyzer Include="Packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.CSharp.dll" \/> \n \
<Analyzer Include="Packages\\SonarAnalyzer.CSharp.8.7.0.17535\\analyzers\\SonarAnalyzer.dll" \/> \n \
<\/ItemGroup> \n \
<PropertyGroup><CodeAnalysisRuleSet>AnalyzerRules.ruleset<\/CodeAnalysisRuleSet><\/PropertyGroup> '

file='./AFKScape/ScriptsAssembly.csproj'
sonar_lint_csproj='./AFKScape/ScriptsAssemblySonarLint.csproj'

echo "--- Creating Sonar Lint .csproj ---"
sed "s/$match/\1\n$insert/" $file > $sonar_lint_csproj

echo "--- Running Sonar Lint ---"
export FrameworkPathOverride=/Library/Frameworks/Mono.framework/Versions/Current
log_file='log.txt'
dotnet build $sonar_lint_csproj --verbosity m > $log_file

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
rm $sonar_lint_csproj
rm $log_file
echo "--- Done ---"

exit $exit_status
