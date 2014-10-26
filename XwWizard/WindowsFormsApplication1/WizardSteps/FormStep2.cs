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
    public partial class FormStep2 : Form
    {
        public FormStep2(FormWizard wizardParent)
        {
            this.WizardParent = wizardParent;
            InitializeComponent();
        }

        private FormWizard WizardParent = null;

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: 弹出对话框
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show("文件不存在！", "骚年你是在玩我", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                else
                {
                    FormWizard.VideoFilePath = openFileDialog1.FileName;

                    this.CheckFile();
                }
            }
        }

        /// <summary>
        /// 判断文件
        /// </summary>
        public void CheckFile()
        {
            if (!IsVideo())
            {
                MessageBox.Show(FormWizard.VideoFilePath + "\n不是视频！", "你又在玩我",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                FormWizard.VideoFilePath = null;
                return;
            }

            FormWizard.NeedEncode = NeedEncode();
            if (FormWizard.NeedEncode)
            {
                FormWizard.NeedMux = false;
            }
            else
            {
                FormWizard.NeedMux = NeedMux();
            }
            //if (!FormWizard.NeedEncode)//不需要转码，直接直接封装
            //{
            //    WizardParent.SwitchToNextStep();
            //    return;
            //}

            ////Need Encoding
            //WizardParent.SwitchToNextStep();

            WizardParent.SwitchToNextStep();
        }

        /// <summary>
        /// 判断这个文件是否是视频文件
        /// </summary>
        /// <returns></returns>
        private bool IsVideo()
        {
            //FormWizard.VideoFilePath
            //判断这个文件是否是视频文件（是否存在视频流）
            return MediaInfoTool.IsVideo(FormWizard.VideoFilePath);
        }

        /// <summary>
        /// 判断是否需要转码
        /// </summary>
        /// <returns></returns>
        private bool NeedEncode()
        {
            //判断是否需要转码
            //H264 + (AAC/MP3)则不需要
            return MediaInfoTool.NeedEncoding(FormWizard.VideoFilePath);
        }

        private bool NeedMux()
        {
            return MediaInfoTool.NeedMux(FormWizard.VideoFilePath);
        }
    }
}
