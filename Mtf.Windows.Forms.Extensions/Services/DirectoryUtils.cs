using System.IO;

namespace Mtf.Windows.Forms.Extensions.Services
{
    public static class DirectoryUtils
    {
        private static readonly string[] ApplicationDirectories = new string[]
        {
            PathProvider.Maps,
            PathProvider.Music,
            PathProvider.SoundEffects,
            PathProvider.Characters,
            PathProvider.VideoClips
        };

        public static void CreateApplicationDirectories()
        {
            foreach (var directory in ApplicationDirectories)
            {
                CreateIfNotExists(directory);
            }
        }

        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
