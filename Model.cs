using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Timetronome
{
    class Model : INotifyPropertyChanged
    {
        private static object locker = new object();

        private int settedTempo;
        private int settedTime;
        private int estimateTime;

        public Model(int receivedTempo, int receivedTime)
        {
            SettedTempo = receivedTempo;
            SettedTime = receivedTime;
        }

        public int SettedTempo
        {
            get
            {
                return settedTempo;
            }
            set
            {
                if (value > 300)
                {
                    settedTempo = 300;
                }
                else if (value < 0)
                {
                    settedTempo = 0;
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
            get
            {
                return settedTime;
            }
            set
            {
                if (value > 600)
                {
                    settedTime = 600;
                }
                else if (value < 0)
                {
                    settedTime = 0;
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
            get
            {
                return estimateTime;
            }
            private set
            {
                estimateTime = value;

                OnPropertyChanged();
            }
        }

        public bool IsStoppingMetronome { get; set; }

        public void Run()
        {
            IsStoppingMetronome = false;

            Thread clickerThread = new Thread(new ThreadStart(Clicker));
            Thread timerThread = new Thread(new ThreadStart(Timer));

            clickerThread.Start();
            timerThread.Start();
        }

        public void Stop()
        {

        }

        private void Clicker()
        {

        }

        private void Timer()
        {
            for (EstimateTime = SettedTime; EstimateTime > 0; EstimateTime--)
            {
                Thread.Sleep(60000);
            }

            Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
