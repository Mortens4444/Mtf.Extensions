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

        var preExisting = expectedDirs.Where(Directory.Exists).ToHashSet();

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
            // Only remove directories this test created; anything that existed before belongs to someone else.
            foreach (var dir in expectedDirs.Where(d => !preExisting.Contains(d) && Directory.Exists(d)))
            {
                Directory.Delete(dir, recursive: true);
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

        Ensure.DoesNotThrow(() => DirectoryUtils.CreateIfNotExists(path));
    }
}
