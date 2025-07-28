namespace MiscUtil
{
    partial class App
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
            btnVBtoCSharp = new Button();
            btnJsonPayload = new Button();
            btnDocX = new Button();
            SuspendLayout();
            // 
            // btnVBtoCSharp
            // 
            btnVBtoCSharp.Location = new Point(34, 36);
            btnVBtoCSharp.Name = "btnVBtoCSharp";
            btnVBtoCSharp.Size = new Size(171, 58);
            btnVBtoCSharp.TabIndex = 0;
            btnVBtoCSharp.Text = "VB 2 C#";
            btnVBtoCSharp.UseVisualStyleBackColor = true;
            btnVBtoCSharp.Click += btnVBtoCSharp_Click;
            // 
            // btnJsonPayload
            // 
            btnJsonPayload.Location = new Point(245, 36);
            btnJsonPayload.Name = "btnJsonPayload";
            btnJsonPayload.Size = new Size(197, 58);
            btnJsonPayload.TabIndex = 1;
            btnJsonPayload.Text = "Json Payload Generator";
            btnJsonPayload.UseVisualStyleBackColor = true;
            btnJsonPayload.Click += btnJsonPayload_Click;
            // 
            // btnDocX
            // 
            btnDocX.Location = new Point(472, 36);
            btnDocX.Name = "btnDocX";
            btnDocX.Size = new Size(197, 58);
            btnDocX.TabIndex = 2;
            btnDocX.Text = "DocX Report Testing Tool";
            btnDocX.UseVisualStyleBackColor = true;
            btnDocX.Click += btnDocX_Click;
            // 
            // App
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDocX);
            Controls.Add(btnJsonPayload);
            Controls.Add(btnVBtoCSharp);
            Name = "App";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "App";
            ResumeLayout(false);
        }

        #endregion

        private Button btnVBtoCSharp;
        private Button btnJsonPayload;
        private Button btnDocX;
    }
}