namespace MiscUtil
{
    partial class frmDocXReporter
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
            btnProcess = new Button();
            txtSource = new TextBox();
            SuspendLayout();
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(1114, 12);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(147, 46);
            btnProcess.TabIndex = 0;
            btnProcess.Text = "Process";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // txtSource
            // 
            txtSource.Location = new Point(8, 8);
            txtSource.Multiline = true;
            txtSource.Name = "txtSource";
            txtSource.ScrollBars = ScrollBars.Both;
            txtSource.Size = new Size(1100, 788);
            txtSource.TabIndex = 1;
            // 
            // frmDocXReporter
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1273, 788);
            Controls.Add(txtSource);
            Controls.Add(btnProcess);
            Font = new Font("Calibri", 10.2F);
            Name = "frmDocXReporter";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DocX Reporter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnProcess;
        private TextBox txtSource;
    }
}