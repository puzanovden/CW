
namespace CW
{
    partial class formMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panelContainer = new Panel();
            SuspendLayout();
            // 
            // panelContainer
            // 
            panelContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContainer.Location = new Point(12, 68);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1017, 339);
            panelContainer.TabIndex = 4;
            panelContainer.Paint += panelContainer_Paint;
            // 
            // formMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1041, 419);
            Controls.Add(panelContainer);
            Name = "formMain";
            Text = "Каталог автозапчастин";
            FormClosing += formMain_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
        }


        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panelContainer;
    }
}
