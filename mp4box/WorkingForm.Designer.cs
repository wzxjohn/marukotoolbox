namespace mp4box
{
    partial class WorkingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkingForm));
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.labelworkCount = new System.Windows.Forms.Label();
            this.progressBarX264 = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // buttonAbort
            // 
            resources.ApplyResources(this.buttonAbort, "buttonAbort");
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // richTextBoxOutput
            // 
            resources.ApplyResources(this.richTextBoxOutput, "richTextBoxOutput");
            this.richTextBoxOutput.BackColor = System.Drawing.Color.White;
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.ReadOnly = true;
            this.richTextBoxOutput.VScroll += new System.EventHandler(this.richTextBoxOutput_VScroll);
            this.richTextBoxOutput.Enter += new System.EventHandler(this.richTextBoxOutput_Enter);
            // 
            // labelworkCount
            // 
            resources.ApplyResources(this.labelworkCount, "labelworkCount");
            this.labelworkCount.Name = "labelworkCount";
            // 
            // progressBarX264
            // 
            resources.ApplyResources(this.progressBarX264, "progressBarX264");
            this.progressBarX264.Maximum = 1000;
            this.progressBarX264.Name = "progressBarX264";
            // 
            // labelProgress
            // 
            resources.ApplyResources(this.labelProgress, "labelProgress");
            this.labelProgress.Name = "labelProgress";
            // 
            // notifyIcon
            // 
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // WorkingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.labelworkCount);
            this.Controls.Add(this.progressBarX264);
            this.Controls.Add(this.richTextBoxOutput);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonAbort);
            this.Name = "WorkingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkingForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkingForm_Load);
            this.Resize += new System.EventHandler(this.WorkingForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.ProgressBar progressBarX264;
        private System.Windows.Forms.Label labelworkCount;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}