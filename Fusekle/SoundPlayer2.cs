using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Fusekle.SoundPlayer2;


namespace Fusekle
{
    public class SoundPlayer2
    {

        MediaPlayer mediaPlayer;
        Sound currentSound;

        public double Volume { get { return mediaPlayer.Volume; } set { mediaPlayer.Volume = value; } }

        public SoundPlayer2(Sound sound, double volume)
        {
            Init(volume);
            Play(sound);
        }

        public SoundPlayer2(double volume)
        {
            Init(volume);
        }

        private void Init(double volume)
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Volume = volume;
        }

        public void Play(Sound sound)
        {
            mediaPlayer.Open(new Uri(ResolveSound(sound), UriKind.Relative));
            mediaPlayer.Play();
        }

        public void PlayBackgroundMusic(SoundBackground soundBackground)
        {
            currentSound = GetFirstSound(soundBackground);

            Play(currentSound);

            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Open(new Uri(ResolveSound(GetNextSound(currentSound)), UriKind.Relative));
            mediaPlayer.Play();
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        private Sound GetFirstSound(SoundBackground soundBackground)
        { 
            switch (soundBackground) 
            {
                case SoundBackground.Dramatic:
                    return Sound.Dramatic1;
                case SoundBackground.Epic:
                    return Sound.Epic1;
                case SoundBackground.Relax:
                    return Sound.Relax1;
                case SoundBackground.MenuMusic:
                    return Sound.MenuMusic;
                default:
                    return Sound.Dramatic1;
            }
        }

        private Sound GetNextSound (Sound sound)
        {
           
            switch (sound)
            {
                case Sound.Dramatic1:
                    return Sound.Dramatic2;
                case Sound.Dramatic2:
                    return Sound.Dramatic3;
                case Sound.Dramatic3:
                    return Sound.Dramatic1;

                case Sound.Epic1:
                    return Sound.Epic2;
                case Sound.Epic2:
                    return Sound.Epic3;
                case Sound.Epic3:
                    return Sound.Epic4;
                case Sound.Epic4:
                    return Sound.Epic1;

                case Sound.Relax1:
                    return Sound.Relax2;
                case Sound.Relax2:
                    return Sound.Relax3;
                case Sound.Relax3:
                    return Sound.Relax1;

                case Sound.MenuMusic:
                    return Sound.MenuMusic;

                default: return Sound.Dramatic1;
            }
        }

        

        private string ResolveSound(Sound sound)
        {
            switch (sound)
            {
                case Sound.Dramatic1:
                    return "Sounds\\GameplayMusic\\dramatic1.mp3";
                case Sound.Dramatic2:
                    return "Sounds\\GameplayMusic\\dramatic2.mp3";
                case Sound.Dramatic3:
                    return "Sounds\\GameplayMusic\\dramatic3.mp3";

                case Sound.Epic1:
                    return "Sounds\\GameplayMusic\\epic1.mp3";
                case Sound.Epic2:
                    return "Sounds\\GameplayMusic\\epic2.mp3";
                case Sound.Epic3:
                    return "Sounds\\GameplayMusic\\epic3.mp3";
                case Sound.Epic4:
                    return "Sounds\\GameplayMusic\\epic4.mp3";

                case Sound.Relax1:
                    return "Sounds\\GameplayMusic\\relax1.mp3";
                case Sound.Relax2:
                    return "Sounds\\GameplayMusic\\relax2.mp3";
                case Sound.Relax3:
                    return "Sounds\\GameplayMusic\\relax3.mp3";

                case Sound.MenuMusic:
                    return "Sounds\\menumusic.mp3";

                case Sound.NewGame:
                    return "Sounds\\newgame.mp3";
                case Sound.GoodStep:
                    return "Sounds\\goodstep.mp3";
                case Sound.WrongStep:
                    return "Sounds\\wrongstep.mp3";
                case Sound.DontTouch:
                    return "Sounds\\donttouch.mp3";
                case Sound.WinApplause:
                    return "Sounds\\winapplause.mp3";

                default: return string.Empty;
            }

        }


        public enum Sound
        {
            Dramatic1,
            Dramatic2,
            Dramatic3,
            Epic1,
            Epic2,
            Epic3,
            Epic4,
            Relax1,
            Relax2,
            Relax3,
            MenuMusic,
            NewGame,
            GoodStep,
            WrongStep,
            DontTouch,
            WinApplause
        }

        
    }

    public enum SoundBackground
    {
        Dramatic,
        Epic,
        Relax,
        MenuMusic
    }



}

    


