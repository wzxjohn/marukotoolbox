namespace XwWizard.WizardSteps
{
    partial class FormStep3
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
            this.panelStep3 = new System.Windows.Forms.Panel();
            this.btnChooseOutput = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelStep3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStep3
            // 
            this.panelStep3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(67)))), ((int)(((byte)(122)))));
            this.panelStep3.Controls.Add(this.btnChooseOutput);
            this.panelStep3.Controls.Add(this.label1);
            this.panelStep3.Location = new System.Drawing.Point(32, 41);
            this.panelStep3.Name = "panelStep3";
            this.panelStep3.Size = new System.Drawing.Size(720, 480);
            this.panelStep3.TabIndex = 2;
            // 
            // btnChooseOutput
            // 
            this.btnChooseOutput.AutoSize = true;
            this.btnChooseOutput.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnChooseOutput.FlatAppearance.BorderSize = 5;
            this.btnChooseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseOutput.Font = new System.Drawing.Font("微软雅黑", 44F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChooseOutput.ForeColor = System.Drawing.Color.White;
            this.btnChooseOutput.Location = new System.Drawing.Point(216, 191);
            this.btnChooseOutput.Name = "btnChooseOutput";
            this.btnChooseOutput.Size = new System.Drawing.Size(289, 98);
            this.btnChooseOutput.TabIndex = 1;
            this.btnChooseOutput.Text = "指定输出";
            this.btnChooseOutput.UseVisualStyleBackColor = true;
            this.btnChooseOutput.Click += new System.EventHandler(this.btnChooseOutput_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(235, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "输出到哪里？";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "*.flv";
            this.saveFileDialog1.Filter = "FLV|*.flv|MP4|*.mp4|MKV|*.mkv|All Files|*.*";
            // 
            // FormStep3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panelStep3);
            this.Name = "FormStep3";
            this.Text = "FormStep3";
            this.panelStep3.ResumeLayout(false);
            this.panelStep3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelStep3;
        private System.Windows.Forms.Button btnChooseOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}