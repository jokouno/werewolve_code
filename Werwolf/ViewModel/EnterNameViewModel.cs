
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Werwolf.Data;
using Werwolf.Workflow;

namespace Werwolf.ViewModel
{
    public partial class EnterNameViewModel : ObservableObject
    {
        private GameManager _gameManager;
        
        public EnterNameViewModel(GameManager gameManager)
        {
            _gameManager = gameManager;
            _gameManager.Restart();
            Items = new ObservableCollection<string>(_gameManager.Names);

#if DEBUG
            Items.Add(nameof(AlteSchrulle));
            Items.Add(nameof(Amor));
            Items.Add(nameof(Doctor));
            Items.Add(nameof(Dorfbewohner));
            Items.Add(nameof(Grabrauber));
            Items.Add(nameof(Hexe));
            Items.Add(nameof(KittenWerwolf));
            Items.Add(nameof(Raecher));
            Items.Add(nameof(Seherin));
            Items.Add(nameof(Data.Werwolf));
#endif
        }

        [ObservableProperty]
        private ObservableCollection<string> items;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            Items.Add($"{text}");
            Text = string.Empty;
        }

        [RelayCommand]
        void Delete(string s)
        {
            if (Items.Contains(s))
            {
                Items.Remove(s);
            }
        }

        [RelayCommand]
        void MoveUp(string item)
        {
            int oldIndex = Items.IndexOf(item);
            if (oldIndex > 0)
            {
                Items.Move(oldIndex, oldIndex - 1);
            }
        }

        [RelayCommand]
        void MoveDown(string item)
        {
            int oldIndex = Items.IndexOf(item);
            if (oldIndex < Items.Count - 1)
            {
                Items.Move(oldIndex, oldIndex + 1);
            }
        }

        [RelayCommand]
        public async void StartRoleSelection()
        {
            SaveNames();

            await Shell.Current.GoToAsync(nameof(RoleSelectionPage));
        }

        private void SaveNames()
        {
            _gameManager.SetNames(Items.ToList());
        }

    }
}
