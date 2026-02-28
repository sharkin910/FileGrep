using System;
using System.Drawing;
using System.Text.Json;

namespace FileGrep
{
    public class AppSettings
    {
        public Size WindowSize { get; set; }
        public Point WindowLocation { get; set; }
        public string PathText { get; set; } = string.Empty;
        public string Extensions { get; set; } = "cs;tt";
        public string ExcludeFolders { get; set; } = "obj;bin;log;bak";
        public bool Recursively { get; set; } = true;
        public string SearchText { get; set; } = string.Empty;
        public bool NotInclude { get; set; }
        public bool IgnoreCase { get; set; }
        public bool IgnoreEmptyLine { get; set; }
        public bool IgnoreSpaceLine { get; set; }
        public bool AddPathName { get; set; }
        public bool AddLineNo { get; set; }

        public static string GetSettingsFilePath()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = System.IO.Path.Combine(folder, "FileGrep");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            return System.IO.Path.Combine(folder, "settings.json");
        }

        public static AppSettings Load()
        {
            try
            {
                var path = GetSettingsFilePath();
                if (System.IO.File.Exists(path))
                {
                    string json = System.IO.File.ReadAllText(path);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch
            {
                // ignore, return default
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                var path = GetSettingsFilePath();
                string json = JsonSerializer.Serialize(this);
                System.IO.File.WriteAllText(path, json);
            }
            catch
            {
                // ignore
            }
        }
    }
}
