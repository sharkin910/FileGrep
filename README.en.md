<table><thead><tr>
<th style="text-align:center">English</th>
<th style="text-align:center"><a href="README.md">日本語</a></th>
</tr></thead></table>

# FileGrep

A Windows application for extracting **lines that match or do not match a specified string** from text files in a selected folder.

![screenshot](screenshot.png)

### Overview

FileGrep provides the following features:

- **Text search**: Lists lines in files that match the specified search string
- **Multiple file support**: Searches multiple files under the target folder in one operation
- **File selector**: Filters target files by extension and supports excluded folders
- **Negative search**: Extracts only lines that do not match
- **Line-based result display**: Shows the file name, line number, and matching line text
- **Saved settings**: Restores the options specified on the screen the next time the app starts

### Usage

1. Launch the application.
1. Specify the folder or file to search.
1. Specify file filtering conditions.
    - Target extensions to search, with multiple values separated by `|`
    - Whether to search subfolders
    - Excluded folders, with multiple values separated by `|`
1. Enter the string to search for. Enable case-insensitive matching if needed.
1. Specify search options.
    - Output lines that do not contain the search string
    - Do not output empty lines
    - Do not output lines that contain only whitespace
    - Add path names to results
    - Add line numbers
1. Press the execute button to display the results.
1. Use the results as needed, such as by copying them.

## System Requirements

- Windows 10 or later
- .NET 8.0 RC or later

## Build

### Requirements

- .NET 8.0 SDK or later

### Build Instructions

Build with Visual Studio 2022 or later.
Alternatively, run the `publish.bat` script in an environment with the dotnet CLI.

### License

[LICENSE](LICENSE)
