using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Fusekle
{
    public static class SoundPlayer
    {
        private static string _command;
        private static bool isOpen;
        private static string thisFileName;
        private static bool thisLoop;
        private static int volume;
        [DllImport("winmm.dll")]

        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public enum Sounds
        {
            MenuMusic,
            NewGame,
            GoodStep,
            WrongStep,
            DontTouch,
            WinApplause
        }

        static SoundPlayer()
        {
            Volume = 500;
        }

        public static void Close()
        {
            _command = "close MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
            isOpen = false;
        }

        static void Open(string sFileName)
        {
            _command = "open \"" + sFileName + "\" type mpegvideo alias MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
            isOpen = true;
            thisFileName = sFileName;
        }

        static void Play(bool loop)
        {
            if (!isOpen)
                Open(thisFileName);

            if (isOpen)
            {
                _command = "play MediaFile";
                if (loop)
                    _command += " REPEAT";
                mciSendString(_command, null, 0, IntPtr.Zero);
            }
        }

        public static void Play(Sounds sound, bool loop = false)
        {
            if (isOpen) Close();

            switch (sound)
            {
                case Sounds.MenuMusic:
                    Open("Sounds\\menumusic.mp3");
                    loop = true;
                    break;
                case Sounds.NewGame:
                    Open("Sounds\\newgame.mp3");
                    break;
                case Sounds.GoodStep:
                    Open("Sounds\\goodstep.mp3");
                    break;
                case Sounds.WrongStep:
                    Open("Sounds\\wrongstep.mp3");
                    break;
                case Sounds.DontTouch:
                    Open("Sounds\\donttouch.mp3");
                    break;
                case Sounds.WinApplause:
                    Open("Sounds\\winapplause.mp3");
                    break;
            }

            thisLoop = loop;

            Play(thisLoop);
        }

        public static int Volume
        {
            set
            {
                if ((value >= 0 && value <= 1000))
                {
                    volume = value;
                    string command = string.Format("setaudio MediaFile volume to {0}", value);
                    mciSendString(command, null, 0, IntPtr.Zero);
                }
            }

            get { return volume; }
           
        }

      
    }
}
