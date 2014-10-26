namespace XwWizard.WizardSteps
{
    partial class FormStep2
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
            this.panelStep2 = new System.Windows.Forms.Panel();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelStep2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStep2
            // 
            this.panelStep2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(67)))), ((int)(((byte)(122)))));
            this.panelStep2.Controls.Add(this.btnChooseFile);
            this.panelStep2.Controls.Add(this.label1);
            this.panelStep2.Location = new System.Drawing.Point(32, 41);
            this.panelStep2.Name = "panelStep2";
            this.panelStep2.Size = new System.Drawing.Size(720, 480);
            this.panelStep2.TabIndex = 1;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.AutoSize = true;
            this.btnChooseFile.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnChooseFile.FlatAppearance.BorderSize = 5;
            this.btnChooseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseFile.Font = new System.Drawing.Font("微软雅黑", 44F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChooseFile.ForeColor = System.Drawing.Color.White;
            this.btnChooseFile.Location = new System.Drawing.Point(216, 191);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(289, 98);
            this.btnChooseFile.TabIndex = 1;
            this.btnChooseFile.Text = "选择文件";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(121, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "那么，少年，我们开始吧！";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "视频文件|*.*";
            // 
            // FormStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panelStep2);
            this.Name = "FormStep2";
            this.Text = "FormStep2";
            this.panelStep2.ResumeLayout(false);
            this.panelStep2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelStep2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}