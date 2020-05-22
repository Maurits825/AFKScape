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

#fix slashes
sed -i 's/\\/\//g' $style_cop_csproj

#set paths
file_path='C:\/Program Files\/Unity\/Hub\/Editor\/2019.3.7f1'
linux_path='\/opt\/Unity'
sed -i "s/$file_path/$linux_path/" $style_cop_csproj

file_path='A:\/repo\/AFKScape\/AFKScape\/Library\/ScriptAssemblies\/UnityEditor.UI.dll'
inux_path='\/opt\/Unity\/Editor\/Data\/Resources\/PackageManager\/ProjectTemplates\/libcache\/com.unity.template.universal-7.1.8\/ScriptAssemblies\/UnityEditor.UI.dll'
sed -i "s/$file_path/$linux_path/" $style_cop_csproj
file_path='A:\/repo\/AFKScape\/AFKScape\/Library\/ScriptAssemblies\/UnityEngine.UI.dll'
linux_path='\/opt\/Unity\/Editor\/Data\/Resources\/PackageManager\/ProjectTemplates\/libcache\/com.unity.template.universal-7.1.8\/ScriptAssemblies\/UnityEngine.UI.dll'
sed -i "s/$file_path/$linux_path/" $style_cop_csproj

#remove unused dlls
dll_name='UnityEditor.WindowsStandalone.Extensions'
sed -i "1N;$!N;s/.*$dll_name.*\n.*\n.*//;P;D" $style_cop_csproj
dll_name='SyntaxTree.VisualStudio.Unity.Bridge'
sed -i "1N;$!N;s/.*$dll_name.*\n.*\n.*//;P;D" $style_cop_csproj
dll_name='Microsoft.Unity.Analyzers.dll'
sed -i "s/.*$dll_name.*//" $style_cop_csproj

repo_path='A:\/repo\/AFKScape\/'
sed -i "s/$repo_path//" $style_cop_csproj

echo "Running Style Cop Analyzer"
dotnet build $style_cop_csproj

echo "Cleaning up"
rm $style_cop_csproj
echo "Done"
