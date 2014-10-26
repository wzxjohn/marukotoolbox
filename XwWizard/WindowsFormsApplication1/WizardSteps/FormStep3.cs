// ------------------------------------------------------------------
// Copyright (C) 2011-2014 Maruko Toolbox Project
// 
//  Authors: LYF <lyfjxymf@sina.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied.
// See the License for the specific language governing permissions
// and limitations under the License.
// -------------------------------------------------------------------
//

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
