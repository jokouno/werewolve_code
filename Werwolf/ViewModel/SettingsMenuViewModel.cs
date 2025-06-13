using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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


        [ObservableProperty] public bool isSoundEnabled;

        public SettingsMenuViewModel(GameManager gm)
        {
            _gameManager = gm;
            IsSoundEnabled = Preferences.Default.Get(SoundKey, true);

            SoundToggled();
        }

        public void SoundToggled()
        {
            Preferences.Default.Set(SoundKey, IsSoundEnabled);
            _gameManager.IsSoundActive = IsSoundEnabled;
        }
    }
}
