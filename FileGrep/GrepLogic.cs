using System;
using System.Collections.Generic;
using System.IO;

namespace FileGrep
{
    public static class GrepLogic
    {
        /// <summary>
        /// ファイルパスが、指定された拡張子のいずれかに一致して、かつ除外されたフォルダを含まないかどうかを判定する。
        /// Determines whether a file path matches any of the specified extensions and does not contain any of the
        /// excluded folders.
        /// </summary>
        /// <param name="filePath">評価するファイルパス。The file path to evaluate.</param>
        /// <param name="extentions">照合するファイル拡張子の配列。An array of file extensions to match against.</param>
        /// <param name="excludeFolders">一致から除外するフォルダ名の配列。An array of folder names to exclude from matching.</param>
        /// <returns>
        /// ファイルパスが指定された拡張子に一致し、除外されたフォルダーを含まない場合はtrue、それ以外の場合はfalse。
        /// true if the file path matches the specified extensions and does not contain any excluded folders; otherwise, false.
        /// </returns>
        public static bool IsMatchPath(string filePath, string[] extentions, string[] excludeFolders)
        {
            // 拡張子判定
            bool hit = false;
            if (extentions.Length == 0)
            {
                hit = true;
            }
            else
            {
                foreach (var ext in extentions)
                {
                    if (filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        hit = true;
                        break;
                    }
                }
            }
            if (!hit) return false;

            foreach (var folder in excludeFolders)
            {
                if (filePath.Contains(folder, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 指定されたテキストに一致する行をファイル内で検索し、オプションでファイルパスや行番号の情報と共に結果を返します。
        /// Searches a file for lines matching the specified text and returns the results with optional file path and
        /// line number information.
        /// </summary>
        /// <param name="filePath">The path of the file to search.</param>
        /// <param name="searchText">The text to search for in each line.</param>
        /// <param name="include">true to include lines containing the search text; false to exclude them.</param>
        /// <param name="addPath">true to include the file path in the output.</param>
        /// <param name="addLineNo">true to include the line number in the output.</param>
        /// <param name="comparison">The string comparison option to use when searching for text.</param>
        /// <param name="ignoreEmptyLine">true to ignore empty lines.</param>
        /// <param name="ignoreSpaceLine">true to ignore lines containing only whitespace.</param>
        /// <returns>A string containing the matching lines, optionally prefixed with file path and line number.</returns>
        public static string GrepFile(
            string filePath,
            string searchText,
            bool include,
            bool addPath,
            bool addLineNo,
            StringComparison comparison,
            bool ignoreEmptyLine,
            bool ignoreSpaceLine)
        {
            string lines = "";
            int lineNo = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                lineNo++;
                if (ignoreEmptyLine && string.IsNullOrEmpty(line)) continue;
                if (ignoreSpaceLine && string.IsNullOrWhiteSpace(line)) continue;
                if (line.Contains(searchText, comparison) == include)
                {
                    if (addPath)
                    {
                        if (addLineNo)
                        {
                            lines += $"\"{filePath}\"({lineNo}):";
                        }
                        else
                        {
                            lines += $"\"{filePath}\":";
                        }
                    }
                    else
                    {
                        if (addLineNo)
                        {
                            lines += $"({lineNo}):";
                        }
                    }
                    lines += line + Environment.NewLine;
                }
            }
            return lines;
        }

        /// <summary>
        /// 指定されたテキストに一致する行をディレクトリ内のファイルで検索し、結果を返します。
        /// Searches files in a directory for lines matching the specified text and returns the results.
        /// </summary>
        /// <param name="path">The root directory to search.</param>
        /// <param name="extentions">The file extensions to include in the search.</param>
        /// <param name="excludeFolders">The folders to exclude from the search.</param>
        /// <param name="recursively">true to search subdirectories; otherwise, false.</param>
        /// <param name="searchText">The text to search for within files.</param>
        /// <param name="include">true to include lines containing the search text; false to exclude them.</param>
        /// <param name="addPath">true to include the file path in the result; otherwise, false.</param>
        /// <param name="addLineNo">true to include line numbers in the result; otherwise, false.</param>
        /// <param name="comparison">The string comparison option to use when searching.</param>
        /// <param name="ignoreEmptyLine">true to ignore empty lines; otherwise, false.</param>
        /// <param name="ignoreSpaceLine">true to ignore lines containing only whitespace; otherwise, false.</param>
        /// <returns>An enumerable collection of strings containing the search results.</returns>
        public static IEnumerable<string> GrepFiles(
            string path,
            string[] extentions,
            string[] excludeFolders,
            bool recursively,
            string searchText,
            bool include,
            bool addPath,
            bool addLineNo,
            StringComparison comparison,
            bool ignoreEmptyLine,
            bool ignoreSpaceLine)
        {
            var searchOpt = recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var file in Directory.EnumerateFiles(path, "*.*", searchOpt))
            {
                if (IsMatchPath(file, extentions, excludeFolders))
                {
                    var log = GrepFile(file, searchText, include, addPath, addLineNo, comparison, ignoreEmptyLine, ignoreSpaceLine);
                    if (log.Length > 0)
                    {
                        yield return log;
                    }
                }
            }
        }
    }
}