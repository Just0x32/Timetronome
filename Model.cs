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
        private int settedTempo;
        private int settedTime;
        private int estimateTime;

        Thread timerThread;
        Thread clickerThread;
        Thread stopMetronomeThread;

        public Model(int receivedTempo, int receivedTime)
        {
            SettedTempo = receivedTempo;
            SettedTime = receivedTime;

            EstimateTime = 0;

            IsMetronomeRunned = false;
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

        public bool IsMetronomeRunned { get; set; }

        public void RunMetronome()
        {
            clickerThread = new Thread(new ThreadStart(Clicker));
            timerThread = new Thread(new ThreadStart(Timer));

            clickerThread.Start();
            timerThread.Start();

            IsMetronomeRunned = true;
        }

        public void StopMetronome()
        {
            IsMetronomeRunned = false;

            if (clickerThread.IsAlive)
                clickerThread.Interrupt();

            if (EstimateTime > 0)
                timerThread.Interrupt();
        }

        private void Clicker()
        {
            int delay = 60000 / SettedTempo;

            while (IsMetronomeRunned)
            {
                //Click

                Thread.Sleep(delay);
            }
        }

        private void Timer()
        {
            EstimateTime = SettedTime;

            do
            {
                Thread.Sleep(60000);
                EstimateTime--;
            }
            while (IsMetronomeRunned && (EstimateTime > 0));

            IsMetronomeRunned = false;
            EstimateTime = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
