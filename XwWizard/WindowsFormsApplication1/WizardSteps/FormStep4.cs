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
