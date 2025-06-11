using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Werwolf.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class PlayerEntry : ObservableObject
    {
        private const string CameraPng = "camera.png";

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string avatarPath;

        public PlayerEntry(string name)
        {
            this.name = name;
            avatarPath = CameraPng;
        }
    }
}
