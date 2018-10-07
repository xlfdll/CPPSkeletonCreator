# C/C++ Header / Code Skeleton Creator
A simple program to create empty C++ header and code files with preprocessor guards

## System Requirements
* .NET Framework 2.0

[Runtime configuration](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-configure-an-app-to-support-net-framework-4-or-4-5) is needed for running in .NET Framework 4.0+.

## Usage
```
CPPSkeletonCreator [/?] | [<HeaderFilePath> <CodeFilePath>] [/overwrite]
```
* **\<no arguments\>** - Interactive mode (file save dialogs will open)
* **/?** - Show usage
* **\<HeaderFilePath\> \<CodeFilePath\>** - Silent mode. Create skeleton files based on given paths
* **/overwrite** - In both interactive and silent modes, suppresses the prompt and overwrites all existing files automatically

## Development Prerequisites
* Visual Studio 2012+
