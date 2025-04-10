
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Timers;
using Werwolf.Workflow;
using Timer = System.Timers.Timer;

namespace Werwolf.ViewModel
{
    public partial class DiscussionTimerViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private Timer _timer;
        private int _secondsLeft;

        [ObservableProperty]
        private string timerDisplay = "02:00";

        public DiscussionTimerViewModel(GameManager gm)
        {
            _gameManager = gm;

            _secondsLeft = 120;
            UpdateTimerDisplay();

            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapsed!;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_secondsLeft > 0)
            {
                _secondsLeft--;
                UpdateTimerDisplay();
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerDisplay = $"{_secondsLeft / 60:D2}:{_secondsLeft % 60:D2}";
        }

        [RelayCommand]
        public void AddMinute()
        {
            if (_secondsLeft >= 1741)
            {
                return;
            }
            _secondsLeft += 60;
        }

        [RelayCommand]
        public void FinishUp()
        {
            _timer.Stop();
            _gameManager.NextPlayerVote();
        }
    }
}
