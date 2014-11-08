// ------------------------------------------------------------------
// Copyright (C) 2011-2014 Maruko Toolbox Project
// 
//  Authors: komaruchan <sandy_0308@hotmail.com>
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
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace mp4box
{
    using Extension;
    public partial class SplashForm : Form
    {
        public static class ClassStyles
        {
            public static readonly Int32
            CS_BYTEALIGNCLIENT = 0x1000,
            CS_BYTEALIGNWINDOW = 0x2000,
            CS_CLASSDC = 0x0040,
            CS_DBLCLKS = 0x0008,
            CS_DROPSHADOW = 0x00020000,
            CS_GLOBALCLASS = 0x4000,
            CS_HREDRAW = 0x0002,
            CS_NOCLOSE = 0x0200,
            CS_OWNDC = 0x0020,
            CS_PARENTDC = 0x0080,
            CS_SAVEBITS = 0x0800,
            CS_VREDRAW = 0x0001;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= ClassStyles.CS_DROPSHADOW;
                return cp;
            }
        }   

        //int iCount = 0;
        public SplashForm()
        {
            InitializeComponent();
            //去掉外框
            this.FormBorderStyle = FormBorderStyle.None;
            //屏幕中央
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Opacity = 0.1;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            ArtTextLabel TitleLabel = new ArtTextLabel();
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("微软雅黑", 21.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            TitleLabel.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(241)))), ((int)(((byte)(191)))));
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.Location = new Point(300, 110);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(208, 29);
            TitleLabel.TabIndex = 2;
            TitleLabel.Text = "小丸工具箱";
            TitleLabel.Parent = pictureBox;

            LuneartTextLabel.Parent = pictureBox;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.InvokeIfRequired(() =>
                this.Opacity += 0.1);

            //if (iCount > 800)
            //{
            //    //启动动画完了之后..
            //    this.Close();
            //    return;
            //}
            //iCount += 10;
            ////窗体透明度减小
            //this.Opacity = 1 - Convert.ToDouble(iCount) / 1000;
        }

        private void SplashForm_Paint(object sender, PaintEventArgs e)
        {
            List<Point> list = new List<Point>();
            int width = this.Width;
            int height = this.Height;
            #region 四个圆角

            //左上
            list.Add(new Point(0, 5));
            list.Add(new Point(1, 5));
            list.Add(new Point(1, 3));
            list.Add(new Point(2, 3));
            list.Add(new Point(2, 2));
            list.Add(new Point(3, 2));
            list.Add(new Point(3, 1));
            list.Add(new Point(5, 1));
            list.Add(new Point(5, 0));
            //右上
            list.Add(new Point(width - 5, 0));
            list.Add(new Point(width - 5, 1));
            list.Add(new Point(width - 3, 1));
            list.Add(new Point(width - 3, 2));
            list.Add(new Point(width - 2, 2));
            list.Add(new Point(width - 2, 3));
            list.Add(new Point(width - 1, 3));
            list.Add(new Point(width - 1, 5));
            list.Add(new Point(width - 0, 5));
            //右下
            list.Add(new Point(width - 0, height - 5));
            list.Add(new Point(width - 1, height - 5));
            list.Add(new Point(width - 1, height - 3));
            list.Add(new Point(width - 2, height - 3));
            list.Add(new Point(width - 2, height - 2));
            list.Add(new Point(width - 3, height - 2));
            list.Add(new Point(width - 3, height - 1));
            list.Add(new Point(width - 5, height - 1));
            list.Add(new Point(width - 5, height - 0));
            //左下
            list.Add(new Point(5, height - 0));
            list.Add(new Point(5, height - 1));
            list.Add(new Point(3, height - 1));
            list.Add(new Point(3, height - 2));
            list.Add(new Point(2, height - 2));
            list.Add(new Point(2, height - 3));
            list.Add(new Point(1, height - 3));
            list.Add(new Point(1, height - 5));
            list.Add(new Point(0, height - 5));
            #endregion
            Point[] points = list.ToArray();
            GraphicsPath shape = new GraphicsPath();
            shape.AddPolygon(points);
            this.Region = new System.Drawing.Region(shape);
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Close();
        }

        private void Type(Control sender, int p_1, double p_2)
        {
            GraphicsPath oPath = new GraphicsPath();
            oPath.AddClosedCurve(
                new Point[] {
            new Point(0, sender.Height / p_1),
            new Point(sender.Width / p_1, 0), 
            new Point(sender.Width - sender.Width / p_1, 0), 
            new Point(sender.Width, sender.Height / p_1),
            new Point(sender.Width, sender.Height - sender.Height / p_1), 
            new Point(sender.Width - sender.Width / p_1, sender.Height), 
            new Point(sender.Width / p_1, sender.Height),
            new Point(0, sender.Height - sender.Height / p_1) },

                (float)p_2);

            sender.Region = new Region(oPath);
        }
    }



    class ArtTextLabel : Label
    {
        int _borderSize = 3;
        Color _borderColor = Color.Gray;
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            PointF pt = new PointF(0, 0);
            RenderRelievoText(pe.Graphics, pt);
        }


        //边框
        private void RenderBordText(Graphics g, PointF point)
        {
            using (Brush brush = new SolidBrush(_borderColor))
            {
                for (int i = 1; i <= _borderSize; i++)
                {
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X - i,
                        point.Y);
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X,
                        point.Y - i);
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X + i,
                        point.Y);
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X,
                        point.Y + i);
                }
            }
            using (Brush brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(
                    base.Text, base.Font, brush, point);
            }
        }

        //印版
        private void RenderFormeText(Graphics g, PointF point)
        {
            using (Brush brush = new SolidBrush(_borderColor))
            {
                for (int i = 1; i <= _borderSize; i++)
                {
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X - i,
                        point.Y + i);
                }
            }
            using (Brush brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(
                    base.Text, base.Font, brush, point);
            }
        }


        //浮雕。
        private void RenderRelievoText(Graphics g, PointF point)
        {
            using (Brush brush = new SolidBrush(_borderColor))
            {
                for (int i = 1; i <= _borderSize; i++)
                {
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X + i,
                        point.Y);
                    g.DrawString(
                        base.Text,
                        base.Font,
                        brush,
                        point.X,
                        point.Y + i);
                }
            }
            using (Brush brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(
                    base.Text, base.Font, brush, point);
            }
        }
    }
}
