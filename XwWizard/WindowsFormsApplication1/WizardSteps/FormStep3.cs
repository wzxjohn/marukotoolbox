using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace XwWizard.WizardSteps
{
    public partial class FormStep3 : Form
    {
        public FormStep3(FormWizard wParent)
        {
            this.WizardParent = wParent;
            InitializeComponent();
        }

        private FormWizard WizardParent = null;

        private void btnChooseOutput_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(saveFileDialog1.FileName))
                {
                    MessageBox.Show(FormWizard.VideoFilePath + "请更换个文件名或目录","",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                FormWizard.OutputFilePath = saveFileDialog1.FileName;
                WizardParent.SwitchToNextStep();
            }
        }

        
    }
}
