
namespace CW
{
    partial class formDirectoryList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            directoryList = new DataGridView();
            buttonAdd = new Button();
            buttonEdit = new Button();
            buttonRemove = new Button();
            label1 = new Label();
            textBoxSearch = new TextBox();
            buttonLoad = new Button();
            buttonSave = new Button();
            filterPanel = new Panel();
            buttonTempl = new Button();
            ((System.ComponentModel.ISupportInitialize)directoryList).BeginInit();
            SuspendLayout();
            // 
            // directoryList
            // 
            directoryList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            directoryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            directoryList.Location = new Point(12, 45);
            directoryList.Name = "directoryList";
            directoryList.RowHeadersWidth = 51;
            directoryList.Size = new Size(703, 393);
            directoryList.TabIndex = 0;
            directoryList.CellDoubleClick += directoryList_CellDoubleClick;
            directoryList.ColumnHeaderMouseClick += directoryList_ColumnHeaderMouseClick;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(12, 4);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(127, 35);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Додати";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(142, 4);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(127, 35);
            buttonEdit.TabIndex = 2;
            buttonEdit.Text = "Змінити";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(271, 4);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(127, 35);
            buttonRemove.TabIndex = 3;
            buttonRemove.Text = "Видалити";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(403, 11);
            label1.Name = "label1";
            label1.Size = new Size(58, 20);
            label1.TabIndex = 4;
            label1.Text = "Пошук:";
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(467, 8);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(248, 27);
            textBoxSearch.TabIndex = 5;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(721, 4);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(133, 35);
            buttonLoad.TabIndex = 6;
            buttonLoad.Text = "Завантажити";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(876, 4);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(143, 35);
            buttonSave.TabIndex = 7;
            buttonSave.Text = "Вивантажити";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // filterPanel
            // 
            filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            filterPanel.Location = new Point(723, 45);
            filterPanel.Name = "filterPanel";
            filterPanel.Size = new Size(296, 393);
            filterPanel.TabIndex = 8;
            // 
            // buttonTempl
            // 
            buttonTempl.Location = new Point(854, 4);
            buttonTempl.Name = "buttonTempl";
            buttonTempl.Size = new Size(21, 34);
            buttonTempl.TabIndex = 9;
            buttonTempl.Text = "T";
            buttonTempl.UseVisualStyleBackColor = true;
            buttonTempl.Click += buttonTempl_Click;
            // 
            // formDirectoryList
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 450);
            Controls.Add(buttonTempl);
            Controls.Add(filterPanel);
            Controls.Add(buttonSave);
            Controls.Add(buttonLoad);
            Controls.Add(textBoxSearch);
            Controls.Add(label1);
            Controls.Add(buttonRemove);
            Controls.Add(buttonEdit);
            Controls.Add(buttonAdd);
            Controls.Add(directoryList);
            Name = "formDirectoryList";
            Text = "DirectoryListForm";
            ((System.ComponentModel.ISupportInitialize)directoryList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private DataGridView directoryList;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonRemove;
        private Label label1;
        private TextBox textBoxSearch;
        private Button buttonLoad;
        private Button buttonSave;
        private Panel filterPanel;
        private Button buttonTempl;
    }
}