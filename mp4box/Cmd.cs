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
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace mp4box
{
    class Cmd
    {
        
        /// <summary>
        /// 自动加引号
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(string path)
        {
            if (string.IsNullOrEmpty(path)) { return null; }
            string ret = null;
            ret = path.Replace("\"", "");
            if (ret.Contains(" ")) { ret = "\"" + ret + "\""; }
            return ret;
        }

        /// <summary>
        /// concat:D:\一二三123.png 可以防止FFMpeg不认中文名输入文件的Bug
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConcatProtocol(string path)
        {
            return FormatPath("concat:" + path);
        }

        /// <summary>
        /// 防止文件或目录重名
        /// </summary>
        /// <param name="oriPath_">原路径</param>
        /// <returns>不会重名的路径</returns>
        public static string GetSuitablePath(string oriPath_)
        {
            if (string.IsNullOrEmpty(oriPath_)) { return null; }
            string oriPath = oriPath_.Replace("\"", string.Empty);//去掉引号

            if ((!File.Exists(oriPath)) && (!Directory.Exists(oriPath)))
            {
                return oriPath;
            }
            int i = 0;
            string ext = Path.GetExtension(oriPath);
            string str = oriPath.Remove(oriPath.Length - ext.Length) + "_" + i;
            while (File.Exists(str + ext) || Directory.Exists(str + ext))
            {
                i++;
                str = oriPath + "_" + i;
            }

            return str + ext;
        }
        /// <summary>
        /// <para></para>输入：目标文件的 路径或所在目录；
        /// <para></para>输入：原始文件的路径；
        /// <para></para>输出：输出文件的路径。
        /// <para></para>用目标目录或文件路径（取自tbOutput），和输入的文件，返回一个供输出的文件路径。
        /// </summary>
        /// <param name="DestDirOrFile_">目标目录或文件路径</param>
        /// <param name="SrcFile_">输入的文件（若DestDirOrFile是文件，则忽略此项）</param>
        /// <param name="dotExtension">换扩展名</param>
        /// <returns></returns>
        public static string GetSimilarFilePath(string DestDirOrFile_, string SrcFile_, string dotExtension)
        {
            if (string.IsNullOrEmpty(DestDirOrFile_)) { return null; }
            if (string.IsNullOrEmpty(SrcFile_)) { return null; }
            string DestDirOrFile = DestDirOrFile_.Replace("\"", string.Empty);
            string SrcFile = SrcFile_.Replace("\"", string.Empty);//去掉引号

            if (DestDirOrFile.EndsWith("\\"))//目录
            {
                if (string.IsNullOrEmpty(dotExtension))//没有指定扩展名
                {
                    return DestDirOrFile + Path.GetFileName(SrcFile);
                }
                else//指定了扩展名
                {
                    return DestDirOrFile + Path.GetFileNameWithoutExtension(SrcFile) + dotExtension;
                }
            }
            else
            {
                //单文件，已经设置好了输出
                //DestDirOrFile是文件路径

                if (string.IsNullOrEmpty(dotExtension))//没有指定扩展名
                {
                    return DestDirOrFile;
                }
                else//指定了扩展名
                {
                    return ChangeExt(DestDirOrFile, dotExtension);//换扩展名
                }
            }
        }
        /// <summary>
        /// 用目标目录或文件路径（取自tbOutput），和输入的文件，返回一个供输出的文件路径。
        /// </summary>
        /// <param name="DestDirOrFile">目标目录或文件路径</param>
        /// <param name="SrcFile">输入的文件</param>
        /// <returns></returns>
        public static string GetSimilarFilePath(string DestDirOrFile, string SrcFile)
        {
            return GetSimilarFilePath(DestDirOrFile, SrcFile, null);
        }
        /// <summary>
        /// 更换扩展名（保留绝对路径）
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string ChangeExt(string srcFile, string ext)
        {
            return GetDir(srcFile) + Path.GetFileNameWithoutExtension(srcFile) + ext;
        }
        /// <summary>
        /// 获取文件目录，带“\”
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetDir(string path)
        {
            string fileDir = Path.GetDirectoryName(path);
            if (fileDir.EndsWith("\\"))
            {
                return fileDir;
            }
            else
            {
                return fileDir + "\\";
            }
        }

    }
}
