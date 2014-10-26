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

namespace XwWizard.WizardSteps
{
    public partial class FormStep4 : Form
    {
        public FormStep4(FormWizard p)
        {
            this.WizardParent = p;
            InitializeComponent();
        }

        private FormWizard WizardParent = null;

        /// <summary>
        /// 处理文件
        /// </summary>
        public static bool EncodeFile()
        {
            //TODO: 处理文件方法
            if (FormWizard.NeedEncode)
            {
                //转码……
                //FormWizard.VideoFilePath;
                //FormWizard.OutputFilePath;

                FormWizard.NeedMux = false;//这个不要去掉，转码时直接转成FLV，不用再封装
            }

            if (FormWizard.NeedMux)
            {
                //封装……
            }

            //if (FormWizard.NeedReduceBitrate)
            //{
            //}

            //然后判断码率，进行后黑

            #region 测试区域
            System.Threading.Thread.Sleep(2000);
            #endregion

            return true;//true表示转码成功
        }
    }
}
