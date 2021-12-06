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

        private bool isMetronomeRunned = false;

        private int actualModelTempo;
        private int actualModelTime;

        public ViewModel(string fromViewTempo, string fromViewTime)
        {
            ActualModelTempo = ParseString(fromViewTempo, ActualModelTempo);
            ActualModelTime = ParseString(fromViewTime, ActualModelTime);

            model = new Model(ActualModelTempo, ActualModelTime);
            model.PropertyChanged += FromModelNotify;
        }

        public int ActualModelTempo
        {
            get => actualModelTempo;
            set => actualModelTempo = value;
        }

        public int ActualModelTime
        {
            get => actualModelTime;
            set => actualModelTime = value;
        }

        public int SettedTempo
        {
            get => model.SettedTempo;
        }

        public int SettedTime
        {
            get => model.SettedTime;
        }

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
            ActualModelTempo = ParseString(fromViewTempo, ActualModelTempo);
            ActualModelTime = ParseString(fromViewTime, ActualModelTime);

            if (!isMetronomeRunned)
                model.RunMetronome(ActualModelTempo, ActualModelTime);
            else
            {
                model.StopMetronome();
            }

            isMetronomeRunned = !isMetronomeRunned;
        }

        public void FromModelNotify(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
