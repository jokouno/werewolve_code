
#if ANDROID
using Android.App;
using Android.Media;
using System.Threading.Tasks;
using Stream = Android.Media.Stream;
using Application = Android.App.Application;

namespace Werwolf.Workflow
{
    public partial class SoundManager
    {
        private MediaPlayer? _player;

        public async Task PlayNightSoundAndroid()
        {
            ExceptionLogger.Log(nameof(PlayNightSoundAndroid));

            var assetMgr = Application.Context.Assets;

            try
            {
                // 1) Asset öffnen (inkl. Unterordner)
                using var afd = assetMgr.OpenFd("Resources/Sounds/nightStartSound.mp3");

                // 2) MediaPlayer initialisieren
                _player = new MediaPlayer();
                _player.Looping = false;

                // 3) Audio-Focus anfordern (sonst evtl. keine Ausgabe)
                var audioManager = (AudioManager)Application.Context.GetSystemService(Android.Content.Context.AudioService)!;
                audioManager.RequestAudioFocus(null, Stream.Music, AudioFocus.GainTransient);

                // 4) Wenn Prepared, dann Start()
                _player.Prepared += (s, e) => _player!.Start();

                // 5) Datenquelle setzen und async vorbereiten
                _player.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                _player.PrepareAsync();

                // 6) Optional: auf Ende hören und aufräumen
                _player.Completion += (s, e) =>
                {
                    _player.Release();
                    _player = null;
                };
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
        }
    }
}

#endif

namespace Werwolf.Workflow
{
    public partial class SoundManager
    {
        public async Task PlayNightSoundElse()
        {
            // TODO
        }
    }
}
