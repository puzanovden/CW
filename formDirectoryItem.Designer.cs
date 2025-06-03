namespace CW
{
    partial class formDirectoryItem
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
            labelCode = new Label();
            textBoxCode = new TextBox();
            labelName = new Label();
            textBoxName = new TextBox();
            buttonOk = new Button();
            buttonClose = new Button();
            SuspendLayout();
            // 
            // labelCode
            // 
            labelCode.AutoSize = true;
            labelCode.Location = new Point(10, 15);
            labelCode.Name = "labelCode";
            labelCode.Size = new Size(35, 20);
            labelCode.TabIndex = 0;
            labelCode.Text = "Код";
            labelCode.Click += labelCode_Click;
            // 
            // textBoxCode
            // 
            textBoxCode.Location = new Point(50, 10);
            textBoxCode.Name = "textBoxCode";
            textBoxCode.ReadOnly = true;
            textBoxCode.Size = new Size(109, 27);
            textBoxCode.TabIndex = 1;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(165, 15);
            labelName.Name = "labelName";
            labelName.Size = new Size(117, 20);
            labelName.TabIndex = 2;
            labelName.Text = "Найменування:";
            labelName.Click += labelName_Click;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(285, 10);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(388, 27);
            textBoxName.TabIndex = 3;
            // 
            // buttonOk
            // 
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOk.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            buttonOk.Location = new Point(537, 44);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(136, 39);
            buttonOk.TabIndex = 4;
            buttonOk.Text = "Зберегти";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            buttonClose.Location = new Point(395, 44);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(136, 39);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Закрити";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // formDirectoryItem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(679, 86);
            Controls.Add(buttonClose);
            Controls.Add(buttonOk);
            Controls.Add(textBoxName);
            Controls.Add(labelName);
            Controls.Add(textBoxCode);
            Controls.Add(labelCode);
            Name = "formDirectoryItem";
            Text = "DirectoryItemForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelCode;
        private TextBox textBoxCode;
        private Label labelName;
        private TextBox textBoxName;
        private Button buttonOk;
        private Button buttonClose;
    }
}