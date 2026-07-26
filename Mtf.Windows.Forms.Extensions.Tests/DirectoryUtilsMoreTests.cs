using Mtf.Windows.Forms.Extensions.Services;
using System.Windows.Forms;

namespace Mtf.Windows.Forms.Extensions.Tests;

public class DirectoryUtilsMoreTests
{
    [Test]
    public void CreateApplicationDirectories_CreatesEveryConfiguredDirectory()
    {
        var expectedDirs = new[]
        {
            PathProvider.Maps,
            PathProvider.Music,
            PathProvider.SoundEffects,
            PathProvider.Characters,
            PathProvider.VideoClips
        };

        try
        {
            DirectoryUtils.CreateApplicationDirectories();

            foreach (var dir in expectedDirs)
            {
                Assert.That(Directory.Exists(dir), Is.True, $"Expected '{dir}' to exist.");
            }
        }
        finally
        {
            foreach (var dir in expectedDirs)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir);
                }
            }
        }
    }

    [Test]
    public void CreateIfNotExists_DirectoryDoesNotExist_CreatesIt()
    {
        var path = Path.Combine(Path.GetTempPath(), "MtfTests_" + Path.GetRandomFileName());

        try
        {
            Assert.That(Directory.Exists(path), Is.False);

            DirectoryUtils.CreateIfNotExists(path);

            Assert.That(Directory.Exists(path), Is.True);
        }
        finally
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }
    }

    [Test]
    public void CreateIfNotExists_DirectoryAlreadyExists_DoesNotThrow()
    {
        var path = Path.GetTempPath();

        Assert.DoesNotThrow(() => DirectoryUtils.CreateIfNotExists(path));
    }
}
