using System;
using System.Collections.Generic;
using System.Text;

namespace IOC_Test02
{
    /// <summary>
    /// 用户播放媒体文件
    /// </summary>
    public class OperationMain02
    {
        public void PlayMedia()
        {
            IMediaFile _mtype = new MediaFile();
            IPlayer _player = new Player();

            _player.Play(_mtype);
        }
    }

    /// <summary>
    /// 播放器
    /// </summary>
    public interface IPlayer
    {
        void Play(IMediaFile file);
    }
    /// <summary>
    /// 默认播放器
    /// </summary>
    public class Player : IPlayer
    {
        public void Play(IMediaFile file)
        {
            Console.WriteLine(file.FilePath);
        }
    }
    /// <summary>
    /// 媒体文件
    /// </summary>
    public interface IMediaFile
    {
        string FilePath { get; set; }
    }
    /// <summary>
    /// 默认媒体文件
    /// </summary>
    public class MediaFile : IMediaFile
    {
        public string FilePath { get; set; }
    }

}
