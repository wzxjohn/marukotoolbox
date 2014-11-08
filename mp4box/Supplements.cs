// ------------------------------------------------------------------
// Copyright (C) 2011-2014 Maruko Toolbox Project
// 
//  Authors: LunarShaddow <aflyhorse@hotmail.com>
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
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace mp4box
{
    namespace Extension
    {
        /// <summary>
        /// A wrapper to make Invoke more easy by using Method Extension.
        /// </summary>
        static class Invoker
        {
            public static void InvokeIfRequired(this ISynchronizeInvoke control, MethodInvoker action)
            {
                if (control.InvokeRequired)
                    control.Invoke(action, null);
                else
                    action();
            }
        }

        /// <summary>
        /// Handmade progress bar. Currently not in use.
        /// </summary>
        class ProgressBarEx : ProgressBar
        {
            public ProgressBarEx()
            {
                // Modify the ControlStyles flags
                //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
                // Initialize Property
                CustomText = "";
                Format = new StringFormat(StringFormatFlags.NoWrap);
                Format.Alignment = StringAlignment.Center;
                Format.LineAlignment = StringAlignment.Center;
            }

            public String CustomText { get; set; }

            private StringFormat Format { get; set; }

            protected override void OnPaint(PaintEventArgs e)
            {
                // let the system do base work first
                base.OnPaint(e);
                // grab some tools
                var rect = ClientRectangle;
                var g = e.Graphics;
                // draw the background
                ProgressBarRenderer.DrawHorizontalBar(g, rect);
                rect.Inflate(-3, -3);
                // then chunk
                if (Value > 0)
                {
                    Rectangle clip = new Rectangle(rect.X, rect.Y, Value * rect.Width / Maximum, rect.Height);
                    ProgressBarRenderer.DrawHorizontalChunks(g, clip);
                }
                // append text
                g.DrawString(CustomText, SystemFonts.DefaultFont, SystemBrushes.ControlText,
                    this.DisplayRectangle, Format);
            }
        }
    }
}
