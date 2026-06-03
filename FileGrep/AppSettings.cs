using System.Text.Json;

namespace FileGrep
{
    public class AppSettings
    {
        public static string SettingItemDelimiter { get; } = "|";
        public Size WindowSize { get; set; }
        public Point WindowLocation { get; set; }
        public string PathText { get; set; } = string.Empty;
        public string Extensions { get; set; } = "cs|tt";
        public string ExcludeFolders { get; set; } = "obj|bin|log|bak";
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
            folder = Path.Combine(folder, "FileGrep");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, "settings.json");
        }

        public static AppSettings Load()
        {
            try
            {
                var path = GetSettingsFilePath();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var setting = JsonSerializer.Deserialize<AppSettings>(json);
                    if (setting is not null)
                    {
                        setting.Extensions = setting.Extensions.Replace(";", SettingItemDelimiter);
                        setting.ExcludeFolders = setting.ExcludeFolders.Replace(";", SettingItemDelimiter);
                        return setting;
                    }
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
                File.WriteAllText(path, json);
            }
            catch
            {
                // ignore
            }
        }
    }
}
