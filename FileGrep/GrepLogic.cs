using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileGrep
{
    /// <summary>
    /// 検索ロジックを提供するインスタンスサービス。
    /// UIや他のコンシューマーから非同期に呼び出せるように設計しています。
    /// </summary>
    public class GrepService
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
        public bool IsMatchPath(string filePath, string[] extentions, string[] excludeFolders)
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
        /// <returns>A string containing the matching lines, optionally prefixed with file path and line number.</returns>
        private string GrepFileInternal(
            string filePath,
            SearchOptions options,
            CancellationToken ct)
        {
            var sb = new StringBuilder();
            int lineNo = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                ct.ThrowIfCancellationRequested();
                lineNo++;
                if (options.IgnoreEmptyLine && string.IsNullOrEmpty(line)) continue;
                if (options.IgnoreSpaceLine && string.IsNullOrWhiteSpace(line)) continue;
                if (line.Contains(options.SearchText, options.Comparison) == options.Include)
                {
                    if (options.AddPath)
                    {
                        if (options.AddLineNo)
                        {
                            sb.Append($"\"{filePath}\"({lineNo}):");
                        }
                        else
                        {
                            sb.Append($"\"{filePath}\":");
                        }
                    }
                    else
                    {
                        if (options.AddLineNo)
                        {
                            sb.Append($"({lineNo}):");
                        }
                    }
                    sb.Append(line).Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Asynchronously searches a file for lines matching the specified search options.
        /// </summary>
        /// <param name="filePath">The path to the file to search.</param>
        /// <param name="options">The search options to apply.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching lines as a single
        /// string.</returns>
        public Task<string> GrepFileAsync(string filePath, SearchOptions options, CancellationToken ct)
        {
            return Task.Run(() => GrepFileInternal(filePath, options, ct), ct);
        }

        /// <summary>
        /// 指定されたテキストに一致する行をディレクトリ内のファイルで検索し、結果を返します。
        /// Searches files in a directory for lines matching the specified text and returns the results.
        /// </summary>
        /// <param name="path">The root directory to search.</param>
        /// <param name="options">The search options to apply.</param>
        /// <param name="progress"></param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>An enumerable collection of strings containing the search results.</returns>
        public async Task GrepFilesAsync(
            string path,
            SearchOptions options,
            IProgress<string>? progress,
            CancellationToken ct)
        {
            await Task.Run(() =>
            {
                var searchOpt = options.Recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                foreach (var file in Directory.EnumerateFiles(path, "*.*", searchOpt))
                {
                    ct.ThrowIfCancellationRequested();
                    if (IsMatchPath(file, options.Extensions, options.ExcludeFolders))
                    {
                        var log = GrepFileInternal(file, options, ct);
                        if (!string.IsNullOrEmpty(log))
                        {
                            progress?.Report(log);
                        }
                    }
                }
            }, ct).ConfigureAwait(false);
        }
    }
}