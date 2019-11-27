using System;
using System.Collections.Generic;
using System.Text;

namespace IOC_Test01
{
    /// <summary>
    /// 用户播放媒体文件
    /// </summary>
    public class OperationMain01
    {
        public void PlayMedia()
        {
            MediaFile mediaFile = new MediaFile();
            Player player = new Player();
            player.Play(mediaFile);
        }
    }

    /// <summary>
    /// 播放器
    /// </summary>
    public class Player
    {
        public void Play(MediaFile file)
        {
            Console.WriteLine(file.FilePath);
        }
    }

    /// <summary>
    /// 媒体文件
    /// </summary>
    public class MediaFile
    {
        public string FilePath { get; set; }
    }

}
