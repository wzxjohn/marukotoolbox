// ------------------------------------------------------------------
// Copyright (C) 2011-2014 Maruko Toolbox Project
// 
//  Authors: komaruchan <sandy_0308@hotmail.com>
//           LunarShaddow <aflyhorse@hotmail.com>
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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace mp4box
{
    using Extension;
    using System.IO;
    public partial class WorkingForm : Form
    {
        /// <summary>
        /// Initialize a cmd output wrapper.
        /// </summary>
        /// <param name="commands">Proposing commands.
        ///     Expecting <see cref="Environment.NewLine"/> as line breaker.</param>
        /// <param name="workcount">The amount of roposing commands.
        ///     Single-pass work don't need to specify this parameter.</param>
        public WorkingForm(string commands, int workcount = 1)
        {
            InitializeComponent();
            Commands = commands;
            WorkQueued = workcount;
            this.Parent = this.Owner;
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Gets or sets the proposed commands. Expecting <see cref="Environment.NewLine"/> as line breaker.
        /// <exception cref="System.ArgumentException">
        ///     An exception is thrown if set it empty or null.</exception>
        /// </summary>
        public string Commands
        {
            get
            {
                return cmds;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Null or empty commands string.");
                else
                    cmds = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of proposed works.
        /// <exception cref="System.ArgumentException">
        ///     An exception is thrown if set it zero of negative.</exception>
        /// </summary>
        public int WorkQueued
        {
            get
            {
                return workQueued;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Negative number? Really?");
                else
                    workQueued = value;
            }
        }

        /// <summary>
        /// Makesure a file really exists.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="arg">Supplement info.</param>
        /// <returns>The file exist or not.</returns>
        public static bool CheckFileExist(string path, string info = "")
        {
            if (!System.IO.File.Exists(path))
                MessageBox.Show("找不到 " + path + " 了啦" + Environment.NewLine
                    + "其他信息：" + info);
            return System.IO.File.Exists(path);
        }

        #region Private Members Declaration

        /// <summary>
        /// Proposed commands.
        /// </summary>
        private string cmds;

        /// <summary>
        /// Queued works quantity.
        /// </summary>
        private int workQueued;

        /// <summary>
        /// Completed works quantity.
        /// </summary>
        private int workCompleted;

        /// <summary>
        /// Path to the batch file.
        /// </summary>
        private string batPath;

        /// <summary>
        /// Potential working process.
        /// </summary>
        private System.Diagnostics.Process proc;

        /// <summary>
        /// Frame count of current processing file via FFmpeg.
        /// </summary>
        private int frameCount;

        /// <summary>
        /// Path to the tools directory.
        /// </summary>
        private string workPath;

        /// <summary>
        /// true if the system is Win7 or later
        /// </summary>
        private bool win7supported;

        /// <summary>
        /// true if autoscroll is enabled
        /// </summary>
        private bool autoscroll;

        #endregion

        #region Regex Patterns
        /// <summary>
        /// Store const regex patterns.
        ///     The Regex objects are made to speed up matchup instead of
        ///     passing the pattern string as arguments from time to time.
        /// </summary>
        public static class Patterns
        {
            /// <summary>
            /// ffms splitter sample output:
            /// <para>[8.1%] 273/3357 frames, 56.22 fps, 17.99 kb/s, 25.01 KB, eta 0:00:54, est.size 307.54 KB</para>
            /// </summary>
            private const string ffmsRegStr = @"^\[(?<percent>\d+\.\d+)%\]";

            /// <summary>
            /// ffms splitter Regex.
            ///     Available patterns: percent.
            /// </summary>
            public static readonly Regex ffmsReg = new Regex(ffmsRegStr);

            /// <summary>
            /// lavf splitter sample output:
            /// <para>387 frames: 68.65 fps, 14.86 kb/s, 29.27 KB</para>
            /// </summary>
            private const string lavfRegStr = @"^(?<frame>\d+) frames:";

            /// <summary>
            /// lavf splitter Regex.
            ///     Available patterns: frame.
            /// </summary>
            public static readonly Regex lavfReg = new Regex(lavfRegStr);

            /// <summary>
            /// x264 execution sample output:
            /// <para>X:\toolDIR>"X:\toolDIR\tools\x264_32_tMod-8bit-420.exe" --crf 24 --preset 8 --demuxer ffms
            /// -r 3 --b 3 --me umh  -i 1 --scenecut 60 -f 1:1 --qcomp 0.5 --psy-rd 0.3:0 --aq-mode 2 --aq-strength 0.8
            /// -o "X:\workDIR\output.ext" "X:\workDIR\input.ext" --vf subtitles --sub "X:\workDIR\sub.ext"
            /// --acodec faac --abitrate 128</para>
            /// </summary>
            /// <remarks>
            /// Without double quote: ^.+>"(?<workDIR>[^"]+)\\x264.+ -o "(?<fileOut>[^"]+)" "(?<fileIn>[^"]+)"
            /// </remarks>
            private const string fileRegStr = @">""(?<workDIR>[^""]+)\\x264.+-o ""(?<fileOut>[^""]+)"" ""(?<fileIn>[^""]+)""";

            /// <summary>
            /// Filename and working directory Regex.
            ///     Available patterns: workDIR, fileOut, fileIn.
            /// </summary>
            public static readonly Regex fileReg = new Regex(fileRegStr, RegexOptions.Singleline);

            /// <summary>
            /// ffmpeg -i sample output:
            /// <para>Duration: 00:02:20.22, start: 0.000000, bitrate: 827 kb/s</para>
            /// <para>Stream #0:0: Video: h264 (High), yuv420p, 1280x720, 545 kb/s, 24.42 fps, 23.98 tbr, 1k tbn, 47.95 tbc</para>
            /// <para>Stream #0:1: Audio: aac, 48000 Hz, stereo, fltp, 128 kb/s</para>
            /// </summary>
            private const string ffmpegRegStr = @"\bDuration: (?<duration>\d{2}:\d{2}:\d{2}\.\d{2}), start: " +
                @".+: Video: .+ (?<tbr>\d+\.?\d+) tbr, ";

            /// <summary>
            /// ffmpeg output Regex.
            ///     Available patterns: duration, tbr.
            /// </summary>
            public static readonly Regex ffmpegReg = new Regex(ffmpegRegStr, RegexOptions.Singleline);
        }

        #endregion

        #region Native Functions
        /// <summary>
        /// Class hosting Native Win32 APIs
        /// </summary>
        private class NativeMethods
        {
            #region <WinUser.h>

            /// <summary>
            /// Specifies the type of scroll bar.
            /// </summary>
            public enum ScrollBarConsts : int
            {
                /// <summary>
                /// Window's standard horizontal scroll bar.
                /// </summary>
                SB_HORZ = 0,
                /// <summary>
                /// Window's standard vertical scroll bar.
                /// </summary>
                SB_VERT = 1,
                /// <summary>
                /// Retrieves the position of the scroll box in a scroll bar control.
                ///     The hWnd parameter must be the handle to the scroll bar control.
                /// </summary>
                SB_CTL = 2,
                /// <summary>
                /// Window's standard scroll bar.
                /// </summary>
                SB_BOTH = 3
            }

            /// <summary>
            /// Specifies the commands to scroll bar.
            /// </summary>
            public enum ScrollBarCmds : int
            {
                /// <summary>
                /// Scrolls one line down.
                /// </summary>
                SB_LINEUP = 0,
                /// <summary>
                /// Scrolls left by one unit.
                /// </summary>
                SB_LINELEFT = 0,
                /// <summary>
                /// Scrolls one line up.
                /// </summary>
                SB_LINEDOWN = 1,
                /// <summary>
                /// Scrolls right by one unit.
                /// </summary>
                SB_LINERIGHT = 1,
                /// <summary>
                /// Scrolls one page up.
                /// </summary>
                SB_PAGEUP = 2,
                /// <summary>
                /// Scrolls left by the width of the window.
                /// </summary>
                SB_PAGELEFT = 2,
                /// <summary>
                /// Scrolls one page down.
                /// </summary>
                SB_PAGEDOWN = 3,
                /// <summary>
                /// Scrolls right by the width of the window.
                /// </summary>
                SB_PAGERIGHT = 3,
                /// <summary>
                /// The user has dragged the scroll box (thumb) and released the mouse button.
                ///     The HIWORD indicates the position of the scroll box at the end of the drag operation.
                /// </summary>
                SB_THUMBPOSITION = 4,
                /// <summary>
                /// The user is dragging the scroll box. This message is sent repeatedly until the user releases the mouse button.
                ///     The HIWORD indicates the position that the scroll box has been dragged to.
                /// </summary>
                SB_THUMBTRACK = 5,
                /// <summary>
                /// Scrolls to the upper left.
                /// </summary>
                SB_TOP = 6,
                /// <summary>
                /// Scrolls to the upper left.
                /// </summary>
                SB_LEFT = 6,
                /// <summary>
                /// Scrolls to the lower right.
                /// </summary>
                SB_BOTTOM = 7,
                /// <summary>
                /// Scrolls to the lower right.
                /// </summary>
                SB_RIGHT = 7,
                /// <summary>
                /// Ends scroll.
                /// </summary>
                SB_ENDSCROLL = 8
            }

            /// <summary>
            /// Indicate the parameters to set or get.
            /// </summary>
            public enum ScrollBarFlags : uint
            {
                /// <summary>
                /// The nMin and nMax members contain the minimum and maximum values for the scrolling range.
                /// </summary>
                SIF_RANGE = 0x1,
                /// <summary>
                /// The nPage member contains the page size for a proportional scroll bar.
                /// </summary>
                SIF_PAGE = 0x2,
                /// <summary>
                /// The nPos member contains the scroll box position, which is not updated while the user drags the scroll box.
                /// </summary>
                SIF_POS = 0x4,
                /// <summary>
                /// This value is used only when setting a scroll bar's parameters.
                /// If the scroll bar's new parameters make the scroll bar unnecessary, disable the scroll bar instead of removing it.
                /// </summary>
                SIF_DISABLENOSCROLL = 0x8,
                /// <summary>
                /// The nTrackPos member contains the current position of the scroll box while the user is dragging it.
                /// </summary>
                SIF_TRACKPOS = 0x10,
                /// <summary>
                /// Combination of SIF_PAGE, SIF_POS, SIF_RANGE, and SIF_TRACKPOS.
                /// </summary>
                SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS)
            }

            /// <summary>
            /// The flash status.
            /// </summary>
            public enum FlashWindowFlags : uint
            {
                /// <summary>
                /// Stop flashing. The system restores the window to its original state.
                /// </summary>
                FLASHW_STOP = 0,
                /// <summary>
                /// Flash the window caption.
                /// </summary>
                FLASHW_CAPTION = 0x1,
                /// <summary>
                /// Flash the taskbar button.
                /// </summary>
                FLASHW_TRAY = 0x2,
                /// <summary>
                /// Flash both the window caption and taskbar button.
                /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
                /// </summary>
                FLASHW_ALL = (FLASHW_CAPTION | FLASHW_TRAY),
                /// <summary>
                /// Flash continuously, until the FLASHW_STOP flag is set.
                /// </summary>
                FLASHW_TIMER = 0x4,
                /// <summary>
                /// Flash continuously until the window comes to the foreground.
                /// </summary>
                FLASHW_TIMERNOFG = 0xC
            }

            /// <summary>
            /// Window Messages
            /// </summary>
            public enum Win32Msgs : uint
            {
                /// <summary>
                /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar.
                ///     This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
                /// </summary>
                WM_HSCROLL = 0x114,
                /// <summary>
                /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar.
                ///     This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
                /// </summary>
                WM_VSCROLL = 0x115,
                /// <summary>
                /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn.
                ///     By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.
                /// </summary>
                WM_CTLCOLORSCROLLBAR = 0x137
            }

            /// <summary>
            /// Contains scroll bar parameters to be set by the SetScrollInfo function, or retrieved by the GetScrollInfo function.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/bb787537(v=vs.85).aspx</para>
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SCROLLINFO
            {
                /// <summary>
                /// Specifies the size, in bytes, of this structure.
                /// </summary>
                public uint cbSize;
                /// <summary>
                /// Specifies the scroll bar parameters to set or retrieve.
                /// </summary>
                public ScrollBarFlags fMask;
                /// <summary>
                /// Specifies the minimum scrolling position. 
                /// </summary>
                public int nMin;
                /// <summary>
                /// Specifies the maximum scrolling position. 
                /// </summary>
                public int nMax;
                /// <summary>
                /// Specifies the page size, in device units.
                ///     A scroll bar uses this value to determine the appropriate size of the proportional scroll box. 
                /// </summary>
                public uint nPage;
                /// <summary>
                /// Specifies the position of the scroll box. 
                /// </summary>
                public int nPos;
                /// <summary>
                /// Specifies the immediate position of a scroll box that the user is dragging.
                ///     An application cannot set the immediate scroll position; the SetScrollInfo function ignores this member. 
                /// </summary>
                public int nTrackPos;
            }

            /// <summary>
            /// Contains the flash status for a window and the number of times the system should flash the window.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms679348(v=vs.85).aspx</para>
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct FLASHWINFO
            {
                /// <summary>
                /// The size of the structure, in bytes.
                /// </summary>
                public uint cbSize;
                /// <summary>
                /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
                /// </summary>
                public IntPtr hwnd;
                /// <summary>
                /// The Flash Status.
                /// </summary>
                public FlashWindowFlags dwFlags;
                /// <summary>
                /// The number of times to Flash the window.
                /// </summary>
                public uint uCount;
                /// <summary>
                /// The rate at which the Window is to be flashed, in milliseconds.
                ///     If Zero, the function uses the default cursor blink rate.
                /// </summary>
                public uint dwTimeout;
            }

            /// <summary>
            /// Creates a value for use as a wParam parameter in a message.
            ///     The macro concatenates the specified values.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms632664(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="wLow">The low-order word of the new value.</param>
            /// <param name="wHi">The high-order word of the new value.</param>
            /// <returns>The return value is a WPARAM value.</returns>
            public static UIntPtr MakeWParam(uint wLow, uint wHi)
            {
                return (UIntPtr)MakeLong(wLow, wHi);
            }

            /// <summary>
            /// Creates a value for use as an lParam parameter in a message.
            ///     The macro concatenates the specified values.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms632661(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="wLow">The low-order word of the new value.</param>
            /// <param name="wHi">The high-order word of the new value. </param>
            /// <returns>The return value is an LPARAM value. </returns>
            public static IntPtr MakeLParam(uint wLow, uint wHi)
            {
                return (IntPtr)MakeLong(wLow, wHi);
            }

            #endregion

            #region <WinDef.h>

            /// <summary>
            /// Retrieves the high-order word from the specified 32-bit value.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms632657(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="ptr">The value to be converted.</param>
            /// <returns>The return value is the high-order word of the specified value.</returns>
            public static uint HiWord(IntPtr ptr)
            {
                    return ((uint)ptr >> 16) & 0xFFFFu;
            }

            /// <summary>
            /// Retrieves the low-order word from the specified value.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms632659(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="ptr">The value to be converted.</param>
            /// <returns>The return value is the low-order word of the specified value.</returns>
            public static uint LoWord(IntPtr ptr)
            {
                return (uint)ptr & 0xFFFFu;
            }

            /// <summary>
            /// Creates a LONG value by concatenating the specified values.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms632660(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="wLow">The low-order word of the new value.</param>
            /// <param name="wHi">The high-order word of the new value.</param>
            /// <returns>The return value is a LONG value.</returns>
            public static uint MakeLong(uint wLow, uint wHi)
            {
                return (wLow & 0xFFFFu) | ((wHi & 0xFFFFu) << 16);
            }

            #endregion

            /// <summary>
            /// The GetScrollInfo function retrieves the parameters of a scroll bar,
            ///     including the minimum and maximum scrolling positions, the page size,
            ///     and the position of the scroll box (thumb).
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/bb787583(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="hWnd">Handle to a scroll bar control or a window with a standard scroll bar,
            ///     depending on the value of the fnBar parameter. </param>
            /// <param name="fnBar">Specifies the type of scroll bar for which to retrieve parameters.</param>
            /// <param name="lpsi">Pointer to a SCROLLINFO structure. Before calling GetScrollInfo, set the cbSize member to sizeof(SCROLLINFO),
            ///     and set the fMask member to specify the scroll bar parameters to retrieve.
            ///     Before returning, the function copies the specified parameters to the appropriate members of the structure.</param>
            /// <returns>If the function retrieved any values, the return value is nonzero.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetScrollInfo(
                IntPtr hWnd,
                ScrollBarConsts fnBar,
                ref SCROLLINFO lpsi);

            /// <summary>
            /// The SetScrollInfo function sets the parameters of a scroll bar, including the minimum and maximum scrolling positions,
            ///     the page size, and the position of the scroll box (thumb). The function also redraws the scroll bar, if requested.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/bb787595(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="hWnd">Handle to a scroll bar control or a window with a standard scroll bar,
            ///     depending on the value of the fnBar parameter. </param>
            /// <param name="fnBar">Specifies the type of scroll bar for which to set parameters.</param>
            /// <param name="lpsi">Pointer to a SCROLLINFO structure. Before calling SetScrollInfo, set the cbSize member of the structure to sizeof(SCROLLINFO),
            ///     set the fMask member to indicate the parameters to set, and specify the new parameter values in the appropriate members.</param>
            /// <param name="fRedraw">Specifies whether the scroll bar is redrawn to reflect the changes to the scroll bar.
            ///     If this parameter is TRUE, the scroll bar is redrawn, otherwise, it is not redrawn. </param>
            /// <returns>The current position of the scroll box.</returns>
            [DllImport("user32.dll")]
            public static extern int SetScrollInfo(
                IntPtr hWnd,
                ScrollBarConsts fnBar,
                ref SCROLLINFO lpsi,
                [MarshalAs(UnmanagedType.Bool)] bool fRedraw);

            /// <summary>
            /// Flashes the specified window. It does not change the active state of the window.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms679347(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="pwfi">A pointer to a FLASHWINFO structure.</param>
            /// <returns>The return value specifies the window's state before the call to the FlashWindowEx function.
            ///     If the window caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value is zero.</returns>
            [DllImport("user32.dll")]
            public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

            /// <summary>
            /// Places (posts) a message in the message queue associated with the thread that created
            ///     the specified window and returns without waiting for the thread to process the message.
            /// <para>http://msdn.microsoft.com/en-us/library/windows/desktop/ms644944(v=vs.85).aspx</para>
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
            /// <param name="Msg">The message to be posted.</param>
            /// <param name="wParam">Additional message-specific information.</param>
            /// <param name="lParam">Additional message-specific information.</param>
            /// <returns>If the function succeeds, the return value is nonzero.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PostMessage(
                IntPtr hWnd,
                Win32Msgs Msg,
                [MarshalAs(UnmanagedType.SysUInt)] UIntPtr wParam,
                [MarshalAs(UnmanagedType.SysInt)] IntPtr lParam);
        }
        #endregion

        #region UI Methods

        private void WorkingForm_Load(object sender, EventArgs e)
        {
            // save commands into a batch file
            batPath = System.IO.Path.Combine(
                Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine),
                "xiaowan" + DateTime.Now.ToFileTimeUtc().ToString() + ".bat");
            var encoder = Encoding.GetEncoding(0);
            var sw = new System.IO.StreamWriter(batPath, false, encoder);
            sw.WriteLine(Commands);
            sw.Close();
            // synchronize UI
            workCompleted = -1;
            richTextBoxOutput.Select();
            // check win7 supported
            win7supported = (Environment.OSVersion.Version.Major > 6 ||
                (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1));
            if (win7supported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            // validate the command string
            if (Commands.Equals(encoder.GetString(encoder.GetBytes(Commands))))
                ProcStart();
            else
            {
                MessageBox.Show("Path or filename contains invalid characters, please rename and retry."
                    + Environment.NewLine +
                    "路径或文件名含有不可识别的字符，请重命名后重试。");
                this.Close();
            }
            notifyIcon.Visible = true;
            MainForm main = (MainForm)this.Owner;
            workPath = main.workPath;
            autoscroll = true;
        }

        private void WorkingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // check remaining works
            if (!(proc == null || proc.HasExited))
            {
                // ask if user hit close button on any accident
                var result = MessageBox.Show("Processing is still undergoing, confirm exit?",
                    "Xiaowan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    // cancel event and return
                    e.Cancel = true;
                    return;
                }
                else
                    ProcAbort();
            }
            // clean up temp batch file
            System.IO.File.Delete(batPath);
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            ProcAbort();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // prohibit saving before work completion
            if (!proc.HasExited)
                MessageBox.Show("Saving before completion is not recommended." +
                    Environment.NewLine + "Please manually abort the process or wait patiently.");
            else
            {
                var savDlg = new SaveFileDialog();
                savDlg.FileName = "log_" + DateTime.Now.ToShortDateString().Replace('/', '-');
                savDlg.Filter = "Log files|*.log|All files|*.*";
                savDlg.FilterIndex = 1;
                savDlg.RestoreDirectory = true;
                if (savDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    File.WriteAllText(savDlg.FileName, richTextBoxOutput.Text);
                    //richTextBoxOutput.SaveFile(savDlg.FileName, RichTextBoxStreamType.UnicodePlainText);
            }
        }

        private void richTextBoxOutput_VScroll(object sender, EventArgs e)
        {
            var info = new NativeMethods.SCROLLINFO();
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.fMask = NativeMethods.ScrollBarFlags.SIF_ALL;
            NativeMethods.GetScrollInfo(richTextBoxOutput.Handle, NativeMethods.ScrollBarConsts.SB_VERT, ref info);
            autoscroll = (info.nTrackPos + richTextBoxOutput.Font.Height
                > info.nMax - richTextBoxOutput.ClientSize.Height);
        }

        private void WorkingForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                BalloonTip("丸子跑到这里了喔~", 75);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void richTextBoxOutput_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
   
        #endregion

        /// <summary>
        /// Start the working process.
        /// </summary>
        private void ProcStart()
        {
            // setup the process
            CheckFileExist(batPath);
            var processInfo = new System.Diagnostics.ProcessStartInfo(batPath, "2>&1");
            processInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            proc = System.Diagnostics.Process.Start(processInfo);
            // attach exit event handler
            proc.EnableRaisingEvents = true;
            proc.Exited += new EventHandler(ProcExit);
            // setup asynchronous reading
            proc.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(OutputHandler);
            proc.BeginOutputReadLine();
        }

        /// <summary>
        /// Process Exit event handler. Automatically called when process exit normally.
        /// </summary>
        private void ProcExit(object sender, EventArgs e)
        {
            // synchronize UI
            UpdateWorkCountUI();
            buttonAbort.InvokeIfRequired(() =>
                buttonAbort.Enabled = false);
            UpdateProgress(1);
            if (win7supported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            // wait a little bit for the last asynchronous reading
            System.Threading.Thread.Sleep(75);
            // append finish tag
            Print(Environment.NewLine + "Work Complete!");
            // fire a warning if something went wrong
            // this feature need %ERRORLEVEL% support in batch commands
            if (proc.ExitCode != 0)
            {
                Print(Environment.NewLine +
                    "Potential Error detected. Please double check the log.");
                Print(Environment.NewLine +
                    "Exit code is: " + proc.ExitCode.ToString());
            }
            // flash form and show balloon tips
            FlashForm();
            BalloonTip(@"完成了喵~ (= ω =)");

            // shutdown the system if required
            MainForm main = (MainForm)this.Owner;
            if (main.shutdownState)
            {
                System.Diagnostics.Process.Start("shutdown", "-s");
                // wait a bit to ensure synchronization
                System.Threading.Thread.Sleep(75);
                if (System.Windows.Forms.DialogResult.Cancel == MessageBox.Show(
                    "System will shutdown in 20 seconds. Click \"Cancel\" to stop countdown."
                    + Environment.NewLine +
                    "系统将在20秒后自动关机。点击“取消”停止倒计时。",
                    "Warning", MessageBoxButtons.OKCancel))
                {
                    System.Diagnostics.Process.Start("shutdown", "-a");
                }
            }
        }

        /// <summary>
        /// Manually call this method to abort all workings.
        /// </summary>
        private void ProcAbort()
        {
            // return value is useless when force aborted
            proc.CancelOutputRead();
            // exit process should be omitted too
            proc.Exited -= new EventHandler(ProcExit);
            // terminate threads
            killProcTree(proc.Id);
            // reset taskbar progress
            if (win7supported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            // Print abort message to log
            Print(Environment.NewLine + "Work is aborted by user.");
        }

        /// <summary>
        /// Asynchronous reading DataReceived event handler.
        /// </summary>
        private void OutputHandler(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                // convert and log first
                Print(e.Data + Environment.NewLine);
                // test if it is command
                Match result = Patterns.fileReg.Match(e.Data);
                if (result.Success)
                {
                    UpdateWorkCountUI();
                    UpdateProgress(0);
                    frameCount = EstimateFrame(workPath, result.Groups["fileIn"].Value);
                }
                // try ffms pattern
                result = Patterns.ffmsReg.Match(e.Data);
                if (result.Success)
                    UpdateProgress(Double.Parse(result.Groups["percent"].Value) / 100);
                // try lavf pattern
                result = Patterns.lavfReg.Match(e.Data);
                if (result.Success)
                    UpdateProgress(Double.Parse(result.Groups["frame"].Value) / frameCount);
            }
        }

        /// <summary>
        /// Kill a process as well as all childs by PID.
        /// </summary>
        /// <param name="pid">The PID of the target process.</param>
        private void killProcTree(int pid)
        {
            // recursive first
            foreach (var m in new System.Management.ManagementObjectSearcher(
                "Select * From Win32_Process Where ParentProcessID=" + pid).Get())
            {
                killProcTree(Convert.ToInt32(m["ProcessID"]));
            }
            // then suicide, usually threads throw exceptions when killed. Dump them silently.
            try
            {
                System.Diagnostics.Process.GetProcessById(pid).Kill();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Update Work Count UI on call. Each call will bump up completed work count by 1.
        /// </summary>
        private void UpdateWorkCountUI()
        {
            ++workCompleted;
            this.InvokeIfRequired(() =>
                this.Text = "Xiaowan (" + workCompleted + '/' + WorkQueued + ')');
            this.labelworkCount.InvokeIfRequired(() =>
                labelworkCount.Text = workCompleted.ToString() + '/' + WorkQueued + " Completed");
        }

        /// <summary>
        /// Update Progress Bar as well as the number on it.
        /// </summary>
        /// <param name="value">Progress expressed in decimal (0.00-1.00).</param>
        private void UpdateProgress(double value)
        {
            // Some kind of safeguard
            if (value < 0)
                value = 0;
            if (value > 1)
                value = 1;
            // Update UI
            progressBarX264.InvokeIfRequired(() =>
                progressBarX264.Value = Convert.ToInt32(value * progressBarX264.Maximum));
            labelProgress.InvokeIfRequired(() =>
                labelProgress.Text = value.ToString("P"));
            if (win7supported)
                TaskbarManager.Instance.SetProgressValue(
                Convert.ToInt32(value * progressBarX264.Maximum), progressBarX264.Maximum);
            notifyIcon.Text = "小丸工具箱" + Environment.NewLine +
                labelworkCount.Text + " - " + labelProgress.Text;
        }

        /// <summary>
        /// Get a rough estimation on frame counts via FFmpeg.
        ///     If failed, return <see cref="Int32.MaxValue"/> instead.
        /// </summary>
        /// <param name="workPath">Path to ffmpeg binary.</param>
        /// <param name="filePath">Path to target file.</param>
        /// <returns>Estimated frame count. 1% tolerance added.</returns>
        private int EstimateFrame(string workPath, string filePath)
        {
            string ffmpegPath = System.IO.Path.Combine(workPath, "ffmpeg.exe");
            CheckFileExist(ffmpegPath);
            var processInfo = new System.Diagnostics.ProcessStartInfo(ffmpegPath, "-i \"" + filePath + '"');
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            var ffproc = System.Diagnostics.Process.Start(processInfo);
            // log and append
            string mediaInfo = ffproc.StandardError.ReadToEnd();
            Print("Input file: " + filePath + Environment.NewLine);
            Print(mediaInfo + Environment.NewLine);
            ffproc.WaitForExit();
            var result = Patterns.ffmpegReg.Match(mediaInfo);
            if (!result.Success)
            {
                Print("Warning: Error detected on previous file. Estimatation may not work."
                        + Environment.NewLine);
                return Int32.MaxValue;
            }
            else
                // add a 1% tolerance to avoid unintentional overflow on progress bar
                return Convert.ToInt32(TimeSpan.Parse(result.Groups["duration"].Value).TotalSeconds
                    * Double.Parse(result.Groups["tbr"].Value) * 1.01);
        }

        /// <summary>
        /// Flash the current form.
        /// </summary>
        /// <returns>Whether the form need flash or not.</returns>
        private bool FlashForm()
        {
            var info = new NativeMethods.FLASHWINFO();
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            this.InvokeIfRequired(() =>
                info.hwnd = this.Handle);
            info.dwFlags = NativeMethods.FlashWindowFlags.FLASHW_ALL |
                NativeMethods.FlashWindowFlags.FLASHW_TIMERNOFG;
            // Flash 3 times
            info.uCount = 3;
            info.dwTimeout = 0;
            return NativeMethods.FlashWindowEx(ref info);
        }

        /// <summary>
        /// Pop a balloon tip.
        /// </summary>
        /// <param name="notes">The content to show.</param>
        /// <param name="timeout">Tip timeout.</param>
        private void BalloonTip(string notes, int timeout = 500)
        {
            MainForm main = (MainForm)this.Owner;
            if (main.trayMode)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(timeout, "小丸工具箱", notes, ToolTipIcon.Info);
            }
        }

        /// <summary>
        /// Append and scroll Textbox if needed.
        /// </summary>
        /// <param name="str">String to append.</param>
        private void Print(string str)
        {
            richTextBoxOutput.InvokeIfRequired(() =>
            {
                richTextBoxOutput.AppendText(str);
                if (autoscroll)
                {
                    var info = new NativeMethods.SCROLLINFO();
                    info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
                    info.fMask = NativeMethods.ScrollBarFlags.SIF_RANGE;
                    NativeMethods.GetScrollInfo(richTextBoxOutput.Handle, NativeMethods.ScrollBarConsts.SB_VERT, ref info);
                    NativeMethods.PostMessage(richTextBoxOutput.Handle, NativeMethods.Win32Msgs.WM_VSCROLL,
                        NativeMethods.MakeWParam((uint)NativeMethods.ScrollBarCmds.SB_THUMBPOSITION, (uint)info.nMax), IntPtr.Zero);
                }
            });
        }
    }
}
