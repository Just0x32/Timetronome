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
        private bool isEnabledTextBox;

        public MainWindow()
        {
            InitializeComponent();
            
            StartStopButtonText = "Start";
            IsEnabledTextBox = true;

            viewModel = new ViewModel("120", "10");
            DataContext = viewModel;

            this.Closing += MainWindowClosing;
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

        public bool IsEnabledTextBox
        {
            get => isEnabledTextBox;
            private set
            {
                isEnabledTextBox = value;
                OnPropertyChanged();
            }
        }

        private void ViewModelNotify(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EstimateTime")
                ChangeStartStopButtonText();

            if (e.PropertyName=="IsMetronomeRunned")
            {
                ChangeStartStopButtonText();

                IsEnabledTextBox = !viewModel.IsMetronomeRunned;
            }

            if (e.PropertyName == "IsMediaFailed" && viewModel.IsMediaFailed)
                ShowMediaFailedMessage();
        }

        private void StartStopButtonClick(object sender, RoutedEventArgs e) => viewModel.ToggleMetronomeState(TempoTextBox.Text, TimerTextBox.Text);

        private void ChangeStartStopButtonText()
        {
            if (viewModel.IsMetronomeRunned)
                StartStopButtonText = "Est. time:" + Environment.NewLine + viewModel.EstimateTime + " min";
            else
                StartStopButtonText = "Start";
        }

        private void ShowMediaFailedMessage() => MessageBox.Show("click.wav is absent or broken!");

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e) => viewModel.CloseApp();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged ([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
