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
    public class Model : INotifyPropertyChanged
    {
        private bool isMetronomeRunned;
        private int settedTempo;
        private int settedTime;
        private int estimateTime;

        Thread timerThread;
        Thread clickerThread;

        public Model (int receivedTempo, int receivedTime)
        {
            SettedTempo = receivedTempo;
            SettedTime = receivedTime;

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

        public bool IsMetronomeRunned
        {
            get
            {
                return isMetronomeRunned;
            }
            set
            {
                isMetronomeRunned = value;
                OnPropertyChanged();
            }
        }

        public void RunMetronome(int receivedTempo, int receivedTime)
        {
            SettedTempo = receivedTempo;
            SettedTime = receivedTime;

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

            if (timerThread.IsAlive)
                timerThread.Interrupt();
        }

        private void Clicker()
        {
            int delay = 60000 / SettedTempo;

            while (IsMetronomeRunned)
            {
                Console.Beep(4000, 100);

                try
                {
                    Thread.Sleep(delay);
                }
                catch (ThreadInterruptedException e) { }
            }
        }

        private void Timer()
        {
            EstimateTime = SettedTime;

            do
            {
                try
                {
                    Thread.Sleep(60000);
                }
                catch (ThreadInterruptedException e) { }

                EstimateTime--;
            }
            while (IsMetronomeRunned && (EstimateTime > 0));

            IsMetronomeRunned = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
