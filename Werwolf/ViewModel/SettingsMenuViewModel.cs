using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class SettingsMenuViewModel : ObservableObject
    {
        private GameManager _gameManager;
        private const string SoundKey = "sound_enabled";

        [ObservableProperty]
        bool isSoundEnabled;

        [ObservableProperty]
        bool isNotificationEnabled;

        [ObservableProperty]
        bool isAvatarEnabled;

        public SettingsMenuViewModel(GameManager gm)
        {
            _gameManager = gm;
            IsSoundEnabled = Preferences.Default.Get(SoundKey, true);
        }

        partial void OnIsSoundEnabledChanged(bool value) => Preferences.Default.Set(SoundKey, value);
    }
}
