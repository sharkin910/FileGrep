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
        private readonly GrepService _grepService = new();
        private CancellationTokenSource? _cts;

        public FormMain()
        {
            InitializeComponent();
            // キャンセルボタンは初期では無効化
            buttonCancel.Enabled = false;
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
            using FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "フォルダーを選択";
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxPath.Text = folderBrowserDialog.SelectedPath;
                CheckPath(folderBrowserDialog.SelectedPath);
            }
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            openFileDialog.Title = "ファイルを選択";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxPath.Text = openFileDialog.FileName;
                CheckPath(openFileDialog.FileName);
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

        private async void buttonProcess_Click(object sender, EventArgs e)
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

            // 検索オプションを構築
            var options = new SearchOptions
            {
                Extensions = extentions,
                ExcludeFolders = excludeFolders,
                Recursively = checkBoxRecursively.Checked,
                SearchText = textBoxSearchText.Text,
                Include = !checkBoxNotInclude.Checked,
                AddPath = checkBoxAddPathName.Checked,
                AddLineNo = checkBoxAddLineNo.Checked,
                Comparison = checkBoxIgnoreCase.Checked ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal,
                IgnoreEmptyLine = checkBoxIgnoreEmptyLine.Checked,
                IgnoreSpaceLine = checkBoxIgnoreSpaceLine.Checked,
            };

            // UIをブロックしないよう非同期で実行
            buttonProcess.Enabled = false;
            buttonCancel.Enabled = true;
            _cts = new System.Threading.CancellationTokenSource();
            var progress = new Progress<string>(s => AppendLog(s));

            try
            {
                if (File.Exists(filePath))
                {
                    var log = await _grepService.GrepFileAsync(filePath, options, _cts.Token);
                    AppendLog(log);
                }
                else
                {
                    await _grepService.GrepFilesAsync(filePath, options, progress, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                AppendLog("キャンセルされました。");
            }
            catch (Exception ex)
            {
                AppendLog($"エラー: {ex}");
            }
            finally
            {
                buttonProcess.Enabled = true;
                try { buttonCancel.Enabled = false; } catch { }
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => _cts?.Cancel());
            buttonCancel.Enabled = false;
        }
    }
}
