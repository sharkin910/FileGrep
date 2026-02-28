namespace FileGrep
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            labelPath = new Label();
            textBoxPath = new TextBox();
            buttonBrowseFolder = new Button();
            buttonBrowseFile = new Button();
            textBoxLog = new TextBox();
            buttonProcess = new Button();
            checkBoxNotInclude = new CheckBox();
            label1 = new Label();
            textBoxSearchText = new TextBox();
            checkBoxIgnoreCase = new CheckBox();
            checkBoxIgnoreEmptyLine = new CheckBox();
            checkBoxIgnoreSpaceLine = new CheckBox();
            labelExtention = new Label();
            textBoxExtention = new TextBox();
            checkBoxRecursively = new CheckBox();
            labelExcludeFolders = new Label();
            textBoxExcludeFolders = new TextBox();
            checkBoxAddPathName = new CheckBox();
            checkBoxAddLineNo = new CheckBox();
            SuspendLayout();
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(15, 16);
            labelPath.Margin = new Padding(4, 0, 4, 0);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(109, 15);
            labelPath.TabIndex = 0;
            labelPath.Text = "ファイル・フォルダーパス";
            // 
            // textBoxPath
            // 
            textBoxPath.AllowDrop = true;
            textBoxPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPath.Location = new Point(131, 12);
            textBoxPath.Margin = new Padding(4);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.Size = new Size(564, 23);
            textBoxPath.TabIndex = 1;
            textBoxPath.DragDrop += textBoxPath_DragDrop;
            textBoxPath.DragEnter += textBoxPath_DragEnter;
            // 
            // buttonBrowseFolder
            // 
            buttonBrowseFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseFolder.Location = new Point(703, 11);
            buttonBrowseFolder.Margin = new Padding(4);
            buttonBrowseFolder.Name = "buttonBrowseFolder";
            buttonBrowseFolder.Size = new Size(100, 23);
            buttonBrowseFolder.TabIndex = 2;
            buttonBrowseFolder.Text = "フォルダー参照...";
            buttonBrowseFolder.UseVisualStyleBackColor = true;
            buttonBrowseFolder.Click += buttonBrowseFolder_Click;
            // 
            // buttonBrowseFile
            // 
            buttonBrowseFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseFile.Location = new Point(811, 11);
            buttonBrowseFile.Margin = new Padding(4);
            buttonBrowseFile.Name = "buttonBrowseFile";
            buttonBrowseFile.Size = new Size(100, 23);
            buttonBrowseFile.TabIndex = 3;
            buttonBrowseFile.Text = "ファイル参照...";
            buttonBrowseFile.UseVisualStyleBackColor = true;
            buttonBrowseFile.Click += buttonBrowseFile_Click;
            // 
            // textBoxLog
            // 
            textBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxLog.Location = new Point(6, 143);
            textBoxLog.Margin = new Padding(4);
            textBoxLog.Multiline = true;
            textBoxLog.Name = "textBoxLog";
            textBoxLog.ScrollBars = ScrollBars.Both;
            textBoxLog.Size = new Size(913, 415);
            textBoxLog.TabIndex = 4;
            // 
            // buttonProcess
            // 
            buttonProcess.Location = new Point(18, 104);
            buttonProcess.Margin = new Padding(4);
            buttonProcess.Name = "buttonProcess";
            buttonProcess.Size = new Size(88, 29);
            buttonProcess.TabIndex = 5;
            buttonProcess.Text = "実行";
            buttonProcess.UseVisualStyleBackColor = true;
            buttonProcess.Click += buttonProcess_Click;
            // 
            // checkBoxNotInclude
            // 
            checkBoxNotInclude.AutoSize = true;
            checkBoxNotInclude.Location = new Point(131, 110);
            checkBoxNotInclude.Margin = new Padding(4);
            checkBoxNotInclude.Name = "checkBoxNotInclude";
            checkBoxNotInclude.Size = new Size(228, 19);
            checkBoxNotInclude.TabIndex = 6;
            checkBoxNotInclude.Text = "検索文字列を含まない行のほうを出力する";
            checkBoxNotInclude.UseVisualStyleBackColor = true;
            checkBoxNotInclude.UseWaitCursor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 80);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 7;
            label1.Text = "検索文字列";
            // 
            // textBoxSearchText
            // 
            textBoxSearchText.Location = new Point(131, 77);
            textBoxSearchText.Margin = new Padding(4);
            textBoxSearchText.Name = "textBoxSearchText";
            textBoxSearchText.Size = new Size(317, 23);
            textBoxSearchText.TabIndex = 8;
            // 
            // checkBoxIgnoreCase
            // 
            checkBoxIgnoreCase.AutoSize = true;
            checkBoxIgnoreCase.Location = new Point(473, 79);
            checkBoxIgnoreCase.Name = "checkBoxIgnoreCase";
            checkBoxIgnoreCase.Size = new Size(134, 19);
            checkBoxIgnoreCase.TabIndex = 9;
            checkBoxIgnoreCase.Text = "大文字小文字同一視";
            checkBoxIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreEmptyLine
            // 
            checkBoxIgnoreEmptyLine.AutoSize = true;
            checkBoxIgnoreEmptyLine.Location = new Point(368, 110);
            checkBoxIgnoreEmptyLine.Name = "checkBoxIgnoreEmptyLine";
            checkBoxIgnoreEmptyLine.Size = new Size(121, 19);
            checkBoxIgnoreEmptyLine.TabIndex = 10;
            checkBoxIgnoreEmptyLine.Text = "空の行を出力しない";
            checkBoxIgnoreEmptyLine.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreSpaceLine
            // 
            checkBoxIgnoreSpaceLine.AutoSize = true;
            checkBoxIgnoreSpaceLine.Location = new Point(498, 110);
            checkBoxIgnoreSpaceLine.Name = "checkBoxIgnoreSpaceLine";
            checkBoxIgnoreSpaceLine.Size = new Size(175, 19);
            checkBoxIgnoreSpaceLine.TabIndex = 11;
            checkBoxIgnoreSpaceLine.Text = "空白文字だけの行を出力しない";
            checkBoxIgnoreSpaceLine.UseVisualStyleBackColor = true;
            // 
            // labelExtention
            // 
            labelExtention.AutoSize = true;
            labelExtention.Location = new Point(15, 48);
            labelExtention.Name = "labelExtention";
            labelExtention.Size = new Size(91, 15);
            labelExtention.TabIndex = 12;
            labelExtention.Text = "検索対象拡張子";
            // 
            // textBoxExtention
            // 
            textBoxExtention.Location = new Point(131, 45);
            textBoxExtention.Name = "textBoxExtention";
            textBoxExtention.Size = new Size(110, 23);
            textBoxExtention.TabIndex = 13;
            textBoxExtention.Text = "cs;tt";
            // 
            // checkBoxRecursively
            // 
            checkBoxRecursively.AutoSize = true;
            checkBoxRecursively.Checked = true;
            checkBoxRecursively.CheckState = CheckState.Checked;
            checkBoxRecursively.Location = new Point(271, 47);
            checkBoxRecursively.Name = "checkBoxRecursively";
            checkBoxRecursively.Size = new Size(126, 19);
            checkBoxRecursively.TabIndex = 14;
            checkBoxRecursively.Text = "下位フォルダーも検索";
            checkBoxRecursively.UseVisualStyleBackColor = true;
            // 
            // labelExcludeFolders
            // 
            labelExcludeFolders.AutoSize = true;
            labelExcludeFolders.Location = new Point(416, 48);
            labelExcludeFolders.Name = "labelExcludeFolders";
            labelExcludeFolders.Size = new Size(74, 15);
            labelExcludeFolders.TabIndex = 15;
            labelExcludeFolders.Text = "除外フォルダー";
            // 
            // textBoxExcludeFolders
            // 
            textBoxExcludeFolders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxExcludeFolders.Location = new Point(496, 45);
            textBoxExcludeFolders.Name = "textBoxExcludeFolders";
            textBoxExcludeFolders.Size = new Size(415, 23);
            textBoxExcludeFolders.TabIndex = 16;
            textBoxExcludeFolders.Text = "obj;bin;log;bak";
            // 
            // checkBoxAddPathName
            // 
            checkBoxAddPathName.AutoSize = true;
            checkBoxAddPathName.Location = new Point(683, 110);
            checkBoxAddPathName.Name = "checkBoxAddPathName";
            checkBoxAddPathName.Size = new Size(123, 19);
            checkBoxAddPathName.TabIndex = 17;
            checkBoxAddPathName.Text = "結果にパス名を付与";
            checkBoxAddPathName.UseVisualStyleBackColor = true;
            // 
            // checkBoxAddLineNo
            // 
            checkBoxAddLineNo.AutoSize = true;
            checkBoxAddLineNo.Location = new Point(816, 110);
            checkBoxAddLineNo.Name = "checkBoxAddLineNo";
            checkBoxAddLineNo.Size = new Size(95, 19);
            checkBoxAddLineNo.TabIndex = 18;
            checkBoxAddLineNo.Text = "行番号を付与";
            checkBoxAddLineNo.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(924, 561);
            Controls.Add(checkBoxAddLineNo);
            Controls.Add(checkBoxAddPathName);
            Controls.Add(textBoxExcludeFolders);
            Controls.Add(labelExcludeFolders);
            Controls.Add(checkBoxRecursively);
            Controls.Add(textBoxExtention);
            Controls.Add(labelExtention);
            Controls.Add(checkBoxIgnoreSpaceLine);
            Controls.Add(checkBoxIgnoreEmptyLine);
            Controls.Add(checkBoxIgnoreCase);
            Controls.Add(textBoxSearchText);
            Controls.Add(label1);
            Controls.Add(checkBoxNotInclude);
            Controls.Add(buttonProcess);
            Controls.Add(textBoxLog);
            Controls.Add(buttonBrowseFile);
            Controls.Add(buttonBrowseFolder);
            Controls.Add(textBoxPath);
            Controls.Add(labelPath);
            Margin = new Padding(4);
            Name = "FormMain";
            Text = "FileGrep - 指定した文字列を含む・含まない行をファイルから抽出する";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowseFolder;
        private System.Windows.Forms.Button buttonBrowseFile;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.CheckBox checkBoxNotInclude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearchText;
        private CheckBox checkBoxIgnoreCase;
        private CheckBox checkBoxIgnoreEmptyLine;
        private CheckBox checkBoxIgnoreSpaceLine;
        private Label labelExtention;
        private TextBox textBoxExtention;
        private CheckBox checkBoxRecursively;
        private Label labelExcludeFolders;
        private TextBox textBoxExcludeFolders;
        private CheckBox checkBoxAddPathName;
        private CheckBox checkBoxAddLineNo;
    }
}

