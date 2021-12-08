using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Timetronome
{
    public class Model : INotifyPropertyChanged
    {
        private bool isClosingApp = false;
        private bool isMediaFailed = false;
        private bool isMetronomeRunned = false;
        private int settedTempo;
        private int settedTime;
        private int estimateTime;
        private string toFilePath = "click.wav";

        Thread timerThread;
        Thread clickerThread;

        MediaPlayer mediaPlayer;
        MediaPlayer mediaChecker;

        public Model (int receivedTempo, int receivedTime)
        {
            SettedTempo = receivedTempo;
            SettedTime = receivedTime;

            clickerThread = new Thread(new ThreadStart(Clicker));
            timerThread = new Thread(new ThreadStart(Timer));

            //CheckMediaFile(toFilePath);

            clickerThread.Start();
            timerThread.Start();

            //CloseMediaChecker();
        }

        private bool IsClosingApp
        {
            get => isClosingApp;
            set => isClosingApp = value;
        }

        public bool IsMediaFailed
        {
            get => isMediaFailed;
            private set
            {
                isMediaFailed = value;
                OnPropertyChanged();
            }
        }

        public bool IsMetronomeRunned
        {
            get => isMetronomeRunned;
            private set
            {
                isMetronomeRunned = value;
                OnPropertyChanged();
            }
        }

        public int SettedTempo
        {
            get => settedTempo;
            private set
            {
                if (value > 300)
                {
                    settedTempo = 300;
                }
                else if (value < 20)
                {
                    settedTempo = 20;
                }
                else
                {
                    settedTempo = value;
                }
                
                OnPropertyChanged();
            }
        }

        public int SettedTime
        {
            get => settedTime;
            private set
            {
                if (value > 600)
                {
                    settedTime = 600;
                }
                else if (value < 1)
                {
                    settedTime = 1;
                }
                else
                {
                    settedTime = value;
                }

                OnPropertyChanged();
            }
        }

        public int EstimateTime
        {
            get => estimateTime;
            private set
            {
                estimateTime = value;
                OnPropertyChanged();
            }
        }

        public void ToogleMetronomeState(int receivedTempo, int receivedTime)
        {
            //CloseMediaChecker();

            if (!IsMetronomeRunned)
            {
                SettedTempo = receivedTempo;
                SettedTime = receivedTime;

                RunMetronome();
            }
            else
            {
                StopMetronome();
            }
        }

        private void RunMetronome()
        {
            IsMetronomeRunned = true;

            ThreadInterrupt(clickerThread);
            ThreadInterrupt(timerThread);
        }

        private void StopMetronome()
        {
            IsMetronomeRunned = false;

            ThreadInterrupt(clickerThread);
            ThreadInterrupt(timerThread);
        }

        private void Clicker()
        {


            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaFailed += NotifyMediaFailed;

            ThreadDelay(5000);

            mediaPlayer.Open(new Uri(toFilePath, UriKind.Relative));


            int delay;

            while (!IsClosingApp)
            {
                ThreadWaiting();

                delay = 60000 / SettedTempo;

                while (!IsClosingApp && IsMetronomeRunned)
                {
                    mediaPlayer.Stop();
                    mediaPlayer.Play();

                    ThreadDelay(delay);
                }
            }
        }

        private void Timer()
        {
            while (!IsClosingApp)
            {
                ThreadWaiting();

                EstimateTime = SettedTime;

                while (!IsClosingApp && IsMetronomeRunned && (EstimateTime > 0))
                {
                    ThreadDelay(60000);

                    EstimateTime--;
                }

                IsMetronomeRunned = false;
            }
        }

        private void ThreadDelay(int delay)
        {
            try
            {
                Thread.Sleep(delay);
            }
            catch (ThreadInterruptedException e) { }
        }

        private void ThreadWaiting()
        {
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException e) { }
        }

        private void ThreadInterrupt(Thread threadVariable)
        {
            if (threadVariable.IsAlive)
                threadVariable.Interrupt();
        }

        private void CheckMediaFile(string toFilePath)
        {
            mediaChecker = new MediaPlayer();

            mediaChecker.MediaFailed += NotifyMediaFailed;

            mediaChecker.Open(new Uri(toFilePath, UriKind.Relative));
        }

        private void CloseMediaChecker() => mediaChecker?.Close();

        public void CloseApp()
        {
            IsClosingApp = true;

            ThreadInterrupt(clickerThread);
            ThreadInterrupt(timerThread);
        }

        private void NotifyMediaFailed(object sender, ExceptionEventArgs e) => IsMediaFailed = true;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
