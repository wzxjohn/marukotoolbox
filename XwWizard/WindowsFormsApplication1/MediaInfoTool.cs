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
using System.IO;
using MediaInfoLib;

namespace XwWizard
{
    class MediaInfoTool
    {
        public static bool IsVideo(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            MediaInfo MI = new MediaInfo();
            MI.Open(filePath);

            //string vid = MI.Get(StreamKind.Video, 0, "ID");
            string video = MI.Get(StreamKind.Video, 0, "Format");

            MI.Close();

            if (string.IsNullOrEmpty(GetVideoFormat(filePath)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsAudio(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(GetAudioFormat(filePath)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string GetVideoFormat(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            MediaInfo MI = new MediaInfo();
            MI.Open(filePath);

            //string vid = MI.Get(StreamKind.Video, 0, "ID");
            string video = MI.Get(StreamKind.Video, 0, "Format");

            MI.Close();

            return video;
        }

        public static string GetAudioFormat(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            MediaInfo MI = new MediaInfo();
            MI.Open(filePath);

            //string aid = MI.Get(StreamKind.Audio, 0, "ID");
            string audio = MI.Get(StreamKind.Audio, 0, "Format");

            MI.Close();

            return audio;
        }

        public static string GetVideoBitrate(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(filePath, "filePath");
            }

            MediaInfo MI = new MediaInfo();
            MI.Open(filePath);

            string vBitRate = MI.Get(StreamKind.Video, 0, "BitRate/String");

            MI.Close();

            return vBitRate;
        }

        public static string GetContainer(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(filePath, "filePath");
            }

            MediaInfo MI = new MediaInfo();
            MI.Open(filePath);

            string container = MI.Get(StreamKind.General, 0, "Format");

            MI.Close();

            return container;
        }

        public static bool NeedEncoding(string filePath)
        {
            //TODO: 判断是否需要转码
            //H264 + (AAC/MP3)则不需要

            string vBitrate = GetVideoBitrate(filePath);
            if (!string.IsNullOrEmpty(vBitrate))
            {
                vBitrate = vBitrate.Trim().ToLower();
                if (vBitrate.Contains("mbps"))
                {
                    System.Diagnostics.Debug.WriteLine("码率过高" + vBitrate);
                    return true;
                }
                if (vBitrate.Contains("kbps"))
                {
                    double kbps = 0.0;
                    if (double.TryParse(vBitrate.Replace("kbps", "").Replace(" ", ""), out kbps))
                    {
                        if (kbps >= 2500)
                        {
                            System.Diagnostics.Debug.WriteLine("码率过高" + vBitrate);
                            return true;
                        }
                    }
                }
            }
            //码率过高

            string audio = GetAudioFormat(filePath);
            string video = GetVideoFormat(filePath);

            Func<string,bool> CheckAudio = ((string audioFormat) =>
                {
                    if(string.IsNullOrEmpty(audioFormat))
                    {
                        return false;
                    }

                    return audioFormat.Equals("AAC") ||
                        audioFormat.Equals("MPEG Audio") ||
                        audioFormat.Equals("PCM");
                });

            Func<string, bool> CheckVideo = ((string videoFormat) =>
                {
                    if (string.IsNullOrEmpty(videoFormat))
                    {
                        return false;
                    }

                    return videoFormat.Equals("AVC");
                });

            bool bCheckAudio = CheckAudio(audio);
            bool bCheckVideo = CheckVideo(video);

            bool audioOk = false;
            bool videoOk = false;

            if (bCheckAudio)//符合AAC编码
            {
                audioOk = true;
            }
            else if (string.IsNullOrEmpty(audio) && bCheckVideo)//没有音频流，但是视频流符合标准
            {
                audioOk = true;
            }

            if (bCheckVideo)//符合标准
            {
                videoOk = true;
            }
            else if (string.IsNullOrEmpty(video) && bCheckAudio)//没有视频流，但是音频流符合标准
            {
                videoOk = true;
            }

            return !(audioOk && videoOk);
        }

        public static bool NeedMux(string filePath)
        {
            string container = GetContainer(filePath);
            if (!string.IsNullOrEmpty(container))
            {
                if (container.Equals("Flash Video"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
