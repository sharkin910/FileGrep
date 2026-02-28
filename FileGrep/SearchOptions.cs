using System;

namespace FileGrep
{
    /// <summary>
    /// 検索オプションをまとめた値オブジェクト
    /// </summary>
    public class SearchOptions
    {
        public string[] Extensions { get; set; } = Array.Empty<string>();
        public string[] ExcludeFolders { get; set; } = Array.Empty<string>();
        public bool Recursively { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public bool Include { get; set; } = true;
        public bool AddPath { get; set; }
        public bool AddLineNo { get; set; }
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;
        public bool IgnoreEmptyLine { get; set; }
        public bool IgnoreSpaceLine { get; set; }
    }
}
