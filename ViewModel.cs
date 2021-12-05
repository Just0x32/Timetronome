using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Timetronome
{
    class ViewModel /*: INotifyPropertyChanged*/
    {
        Model model;

        private int toModelTempo = 120;
        private int toModelTime = 5;

        public ViewModel()
        {
            model = new Model();
        }

        public void RunMetronome(string fromViewTempo, string fromViewTime)
        {
            toModelTempo = int.Parse(fromViewTempo);
            toModelTime = int.Parse(fromViewTime);

            model.RunMetronome(toModelTempo, toModelTime);
        }

        public void StopMetronome()
        {
            model.StopMetronome();
        }



        //public bool IsMetronomeRunned { get; set; }


        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
