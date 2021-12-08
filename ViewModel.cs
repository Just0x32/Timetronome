using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Timetronome
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model model;

        public ViewModel(string fromViewTempo, string fromViewTime)
        {
            FromViewTempo = ParseString(fromViewTempo, FromViewTempo);
            FromViewTime = ParseString(fromViewTime, FromViewTime);

            model = new Model(FromViewTempo, FromViewTime);
            model.PropertyChanged += ModelNotify;
        }

        private int FromViewTempo { get; set; }
        private int FromViewTime { get; set; }

        public bool IsMetronomeRunned { get => model.IsMetronomeRunned; }
        public int SettedTempo { get => model.SettedTempo; }
        public int SettedTime { get => model.SettedTime; }
        public int EstimateTime { get => model.EstimateTime; }

        public bool IsMediaFailed { get => model.IsMediaFailed; }

        private int ParseString(string inputString, int previousIntVariableValue)
        {
            int parseResult;

            if (int.TryParse(inputString, out parseResult))
            {
                return parseResult;
            }
            else
            {
                return previousIntVariableValue;
            }
        }

        public void ToggleMetronomeState(string fromViewTempo, string fromViewTime)
        {
            FromViewTempo = ParseString(fromViewTempo, FromViewTempo);
            FromViewTime = ParseString(fromViewTime, FromViewTime);

            model.ToogleMetronomeState(FromViewTempo, FromViewTime);
        }

        public void CloseApp() => model.CloseApp();

        private void ModelNotify(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
