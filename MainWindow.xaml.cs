using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Timetronome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ViewModel viewModel;

        private string startStopButtonText;

        public MainWindow()
        {
            InitializeComponent();

            StartStopButtonText = "Start";

            viewModel = new ViewModel("120", "5");

            DataContext = viewModel;
            viewModel.PropertyChanged += ViewModelNotify;
        }

        public string StartStopButtonText
        {
            get => startStopButtonText;
            private set
            {
                startStopButtonText = value;
                OnPropertyChanged();
            }
        }

        private void ViewModelNotify(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsMetronomeRunned" || e.PropertyName == "EstimateTime")
                ChangeStartStopButtonText();
        }

        private void StartStopButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.ToggleMetronomeState(TempoTextBox.Text, TimerTextBox.Text);
        }

        private void ChangeStartStopButtonText()
        {
            if (viewModel.IsMetronomeRunned)
                StartStopButtonText = "Est. time:" + Environment.NewLine + viewModel.EstimateTime + " min";
            else
                StartStopButtonText = "Start";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged ([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
