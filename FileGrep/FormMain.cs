using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileGrep
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void textBoxPath_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files) return;

            var file = files[0];
            if (CheckPath(file))
            {
                textBoxPath.Text = file;
            }
            else
            {
                textBoxPath.Text = "";
            }
        }

        private void textBoxPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
            }
        }

        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "フォルダーを選択";
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxPath.Text = folderBrowserDialog.SelectedPath;
                    CheckPath(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "ファイルを選択";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxPath.Text = openFileDialog.FileName;
                    CheckPath(openFileDialog.FileName);
                }
            }
        }

        private bool CheckPath(string path)
        {
            if (File.Exists(path))
            {
                labelExtention.Enabled = false;
                textBoxExtention.Enabled = false;
                checkBoxRecursively.Enabled = false;
                labelExcludeFolders.Enabled = false;
                textBoxExcludeFolders.Enabled = false;
                buttonProcess.Enabled = true;
                return true;
            }
            labelExtention.Enabled = true;
            textBoxExtention.Enabled = true;
            checkBoxRecursively.Enabled = true;
            labelExcludeFolders.Enabled = true;
            textBoxExcludeFolders.Enabled = true;
            if (Directory.Exists(path))
            {
                buttonProcess.Enabled = true;
                return true;
            }
            buttonProcess.Enabled = false;
            return false;
        }

        private void AppendLog(string message)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new Action(() => AppendLog(message)));
            }
            else
            {
                if (message.EndsWith(Environment.NewLine))
                {
                    textBoxLog.AppendText(message);
                }
                else
                {
                    textBoxLog.AppendText(message + Environment.NewLine);
                }
                textBoxLog.SelectionStart = textBoxLog.Text.Length;
                textBoxLog.ScrollToCaret();
            }
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            var filePath = textBoxPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show(this, "パス名が指定されていません。");
                return;
            }
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
            {
                MessageBox.Show(this, "指定されたパス名が存在しません。");
                return;
            }

            var extention = textBoxExtention.Text.Trim();
            var extentions = extention.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.StartsWith('.') ? x : "." + x)
                .ToArray();

            var excludeFolders = textBoxExcludeFolders.Text.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.StartsWith('\\') ? x : "\\" + x)
                .Select(x => x.EndsWith('\\') ? x : x + "\\")
                .ToArray();

            textBoxLog.Clear();

            try
            {
                if (File.Exists(filePath))
                {
                    var log = GrepFile(filePath);
                    AppendLog(log);
                }
                else
                {
                    GrepFiles(filePath, extentions, excludeFolders);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"エラー: {ex}");
            }
        }

        private void GrepFiles(string path, string[] extentions, string[] excludeFolders)
        {
            var searchOpt = checkBoxRecursively.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var file in Directory.EnumerateFiles(path, "*.*", searchOpt))
            {
                if (IsMatchPath(file, extentions, excludeFolders))
                {
                    var log = GrepFile(file);
                    if (log.Length > 0)
                    {
                        AppendLog(log);
                    }
                }
            }
        }

        private bool IsMatchPath(string filePath, string[] extentions, string[] excludeFolders)
        {
            // 指定拡張子以外はスキップ
            bool hit = false;
            if (extentions.Length == 0)
            {
                // 対象となる拡張子が指定されていないときは全部が対象
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

            // 指定フォルダーが含まれていたらスキップ
            foreach (var folder in excludeFolders)
            {
                if (filePath.Contains(folder, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private string GrepFile(string filePath)
        {
            string lines = "";
            string searchText = textBoxSearchText.Text;
            bool include = !checkBoxNotInclude.Checked;
            bool addPath = checkBoxAddPathName.Checked;
            bool addLineNo = checkBoxAddLineNo.Checked;
            StringComparison comparison = checkBoxIgnoreCase.Checked
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            int lineNo = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                lineNo++;
                if (checkBoxIgnoreEmptyLine.Checked && string.IsNullOrEmpty(line)) continue;
                if (checkBoxIgnoreSpaceLine.Checked && string.IsNullOrWhiteSpace(line)) continue;
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
    }
}
