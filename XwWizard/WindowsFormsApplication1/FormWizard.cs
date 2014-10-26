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
using System.Threading;
using System.Windows.Forms;
using XwWizard.WizardSteps;

namespace XwWizard
{
    public partial class FormWizard : Form
    {
        public FormWizard()
        {
            NeedEncode = NeedMux = NeedReduceBitrate = true;
            InitializeComponent();

            StepPanels.Add(new FormStep1().panelStep1);
            StepPanels.Add(new FormStep2(this).panelStep2);
            StepPanels.Add(new FormStep3(this).panelStep3);
            StepPanels.Add(new FormStep4(this).panelStep4);
            StepPanels.Add(new FormStep5().panelStep5);

            StepOptions.Add(new StepOpt(true, true, true));
            StepOptions.Add(new StepOpt(true, true, false));
            StepOptions.Add(new StepOpt(true, true, false));
            StepOptions.Add(new StepOpt(true, false, false));
            StepOptions.Add(new StepOpt(false, false, true));

            StepShownActions.Add(null);
            StepShownActions.Add(null);
            StepShownActions.Add(null);
            StepShownActions.Add(new Action(()=>
                {
                    Thread t = new Thread(() =>
                        {
                            bool succ = FormStep4.EncodeFile();
                            if (!succ)
                            {
                                MessageBox.Show("转码失败");
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    this.Close();
                                }));
                            }
                            this.Invoke(new MethodInvoker(delegate()
                            {
                                SwitchToNextStep();
                            }));
                        });
                    t.Start();
                }));
            StepShownActions.Add(null);

            SwitchToNextStep();
        }

        public List<Panel> StepPanels = new List<Panel>();
        public List<StepOpt> StepOptions = new List<StepOpt>();
        public List<Action> StepShownActions = new List<Action>();

        private int CurrentStep = -1;

        /// <summary>
        /// 用户所选中文件的路径
        /// </summary>
        public static string VideoFilePath { get; set; }
        /// <summary>
        /// 用户选择的输出文件路径
        /// </summary>
        public static string OutputFilePath { get; set; }
        /// <summary>
        /// 是否要转码
        /// </summary>
        public static bool NeedEncode { get; set; }
        /// <summary>
        /// 是否要封装
        /// </summary>
        public static bool NeedMux { get; set; }
        /// <summary>
        /// 是否要后黑
        /// </summary>
        public static bool NeedReduceBitrate { get; set; }

        private void FormWizard_Load(object sender, EventArgs e)
        {
            //new FormStep1().Show();
        }

        public void SwitchToNextStep()
        {
            CurrentStep++;
            if (CurrentStep > StepPanels.Count - 1)
            {
                //完成了
                this.Close();
                return;
            }
            else if (CurrentStep == StepPanels.Count - 1)
            {
                //最后一步
                this.btnNext.Text = "完成";
            }
            SetOpt();
            StepPanels[CurrentStep].Location = new Point(0, 0);
            this.Controls.Add(StepPanels[CurrentStep]);
            StepPanels[CurrentStep].BringToFront();
            btnCancel.BringToFront();
            btnPrev.BringToFront();
            btnNext.BringToFront();
            RunAction();
        }

        public void SwitchToPrevStep()
        {
            CurrentStep--;
            if (CurrentStep < 0)
            {
                //第一步
                return;
            }
            SetOpt();
            this.btnNext.Text = "下一步";
            StepPanels[CurrentStep].Location = new Point(0, 0);
            //this.Controls.Add(StepPanels[CurrentStep]);
            StepPanels[CurrentStep].BringToFront();
            btnCancel.BringToFront();
            btnPrev.BringToFront();
            btnNext.BringToFront();
            RunAction();
        }

        private void SetOpt()
        {
            StepOpt s = StepOptions[CurrentStep];
            btnCancel.Enabled = s.Cancel;
            btnNext.Enabled = s.Next;
            btnPrev.Enabled = s.Prev;
        }

        private void RunAction()
        {
            Action a = StepShownActions[CurrentStep]; ;
            if (a != null)
            {
                a();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SwitchToNextStep();
        }

        private void Motion(Control cNew,int toX,Control cOld)
        {
            int timeMS = 2000;
            //int acMS = 250;
            //int dcMS = 250;

            int refreshRate = 1000 / 60;

            //float accumulation=
            //for (int i = 0; i < acMS / refreshRate; i++)
            //{
            //    c.Location=new Point(
            //        c.Location.X
            //}

            float addXperms = (float)timeMS / (toX - cNew.Location.X);
            int addXperrefresh = (int)(addXperms * refreshRate);

            Thread t = new Thread(() =>
                {
                    for (int i = 0; i < timeMS / refreshRate; i++)
                    {
                        int cNewX=0;
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            cNew.Location = new Point(
                                cNew.Location.X + addXperrefresh, cNew.Location.Y);
                            cNewX = cNew.Location.X;
                        }));
                        Thread.Sleep(refreshRate);

                        if (cNew.Location.X < toX)
                        {
                            break;
                        }
                    }
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        cNew.Location = new Point(toX, cNew.Location.Y);
                    }));
                });
            t.Start();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            SwitchToPrevStep();
        }
    }
}
