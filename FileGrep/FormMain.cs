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
                bool include = !checkBoxNotInclude.Checked;
                bool addPath = checkBoxAddPathName.Checked;
                bool addLineNo = checkBoxAddLineNo.Checked;
                StringComparison comparison = checkBoxIgnoreCase.Checked
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                if (File.Exists(filePath))
                {
                    var log = GrepLogic.GrepFile(filePath,
                        textBoxSearchText.Text,
                        include,
                        addPath,
                        addLineNo,
                        comparison,
                        checkBoxIgnoreEmptyLine.Checked,
                        checkBoxIgnoreSpaceLine.Checked);
                    AppendLog(log);
                }
                else
                {
                    foreach (var log in GrepLogic.GrepFiles(
                        filePath,
                        extentions,
                        excludeFolders,
                        checkBoxRecursively.Checked,
                        textBoxSearchText.Text,
                        include,
                        addPath,
                        addLineNo,
                        comparison,
                        checkBoxIgnoreEmptyLine.Checked,
                        checkBoxIgnoreSpaceLine.Checked))
                    {
                        AppendLog(log);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"エラー: {ex}");
            }
        }
    }
}
