
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
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
            Items = new ObservableCollection<PlayerEntry>(_gameManager.PlayerEntries);

#if DEBUG
            Items.Add(new PlayerEntry(nameof(AlteSchrulle)));
            Items.Add(new PlayerEntry(nameof(Amor)));
            Items.Add(new PlayerEntry(nameof(Doctor)));
            Items.Add(new PlayerEntry(nameof(Dorfbewohner)));
            Items.Add(new PlayerEntry(nameof(Grabrauber)));
            Items.Add(new PlayerEntry(nameof(Hexe)));
            Items.Add(new PlayerEntry(nameof(KittenWerwolf)));
            Items.Add(new PlayerEntry(nameof(Raecher)));
            Items.Add(new PlayerEntry(nameof(Seherin)));
            Items.Add(new PlayerEntry(nameof(Data.Werwolf)));
#endif
        }

        [ObservableProperty]
        private ObservableCollection<PlayerEntry> items;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            Items.Add(new PlayerEntry(Text));
            Text = string.Empty;
        }

        [RelayCommand]
        void Delete(PlayerEntry s)
        {
            if (Items.Contains(s))
            {
                Items.Remove(s);
            }
        }

        [RelayCommand]
        void MoveUp(PlayerEntry item)
        {
            int oldIndex = Items.IndexOf(item);
            if (oldIndex > 0)
            {
                Items.Move(oldIndex, oldIndex - 1);
            }
        }

        [RelayCommand]
        void MoveDown(PlayerEntry item)
        {
            int oldIndex = Items.IndexOf(item);
            if (oldIndex < Items.Count - 1)
            {
                Items.Move(oldIndex, oldIndex + 1);
            }
        }

        [RelayCommand]
        public async Task TakePicture(PlayerEntry entry)
        {
            try
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    // z.B. in deiner ViewModel-Logik eine Meldung anzeigen
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                        await Application.Current.MainPage.DisplayAlert("Kein Kamera-Support",
                            "Dieses Gerät kann keine Fotos aufnehmen.", "OK"));
                    return;
                }

                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                    return;

                var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (storageStatus != PermissionStatus.Granted)
                    return;

                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                    return;

                string newFile = Path.Combine(FileSystem.CacheDirectory, $"{Path.GetRandomFileName()}.png");
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFile = File.OpenWrite(newFile);
                await sourceStream.CopyToAsync(localFile);

                entry.AvatarPath = newFile;
            }
            catch (Exception exception)
            {
                // ignore errors
            }
        }

        [RelayCommand]
        public async void StartRoleSelection()
        {
            SavePlayerEntries();

            await Shell.Current.GoToAsync(nameof(RoleSelectionPage));
        }

        private void SavePlayerEntries()
        {
            _gameManager.SetPlayerEntries(Items.ToList());
        }

    }
}
