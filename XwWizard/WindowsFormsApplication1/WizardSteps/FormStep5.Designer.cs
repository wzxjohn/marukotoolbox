namespace XwWizard.WizardSteps
{
    partial class FormStep5
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
            this.panelStep5 = new System.Windows.Forms.Panel();
            this.btnChooseOutput = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStep5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStep5
            // 
            this.panelStep5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(67)))), ((int)(((byte)(122)))));
            this.panelStep5.Controls.Add(this.btnChooseOutput);
            this.panelStep5.Controls.Add(this.label1);
            this.panelStep5.Location = new System.Drawing.Point(32, 41);
            this.panelStep5.Name = "panelStep5";
            this.panelStep5.Size = new System.Drawing.Size(720, 480);
            this.panelStep5.TabIndex = 4;
            // 
            // btnChooseOutput
            // 
            this.btnChooseOutput.AutoSize = true;
            this.btnChooseOutput.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnChooseOutput.FlatAppearance.BorderSize = 5;
            this.btnChooseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseOutput.Font = new System.Drawing.Font("微软雅黑", 44F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChooseOutput.ForeColor = System.Drawing.Color.White;
            this.btnChooseOutput.Location = new System.Drawing.Point(216, 222);
            this.btnChooseOutput.Name = "btnChooseOutput";
            this.btnChooseOutput.Size = new System.Drawing.Size(289, 98);
            this.btnChooseOutput.TabIndex = 2;
            this.btnChooseOutput.Text = "打开看看";
            this.btnChooseOutput.UseVisualStyleBackColor = true;
            this.btnChooseOutput.Click += new System.EventHandler(this.btnChooseOutput_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(140, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(440, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "恭喜你，视频处理成功！";
            // 
            // FormStep5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panelStep5);
            this.Name = "FormStep5";
            this.Text = "FormStep5";
            this.panelStep5.ResumeLayout(false);
            this.panelStep5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelStep5;
        private System.Windows.Forms.Button btnChooseOutput;
        private System.Windows.Forms.Label label1;
    }
}