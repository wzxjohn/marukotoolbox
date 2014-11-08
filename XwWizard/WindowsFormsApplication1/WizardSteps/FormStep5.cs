using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XwWizard.WizardSteps
{
    public partial class FormStep5 : Form
    {
        public FormStep5()
        {
            InitializeComponent();
        }

        private void btnChooseOutput_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("\"" + FormWizard.OutputFilePath + "\"");
            }
            catch { }
        }
    }
}
