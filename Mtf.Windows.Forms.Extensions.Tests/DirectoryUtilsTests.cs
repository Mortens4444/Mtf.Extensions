using Mtf.Windows.Forms.Extensions.Services;
using System.Reflection;

namespace Mtf.Windows.Forms.Extensions.Tests;

public class DirectoryUtilsTests
{
    [Test]
    public void ApplicationDirectories_IncludesVideoClips()
    {
        var field = typeof(DirectoryUtils).GetField("ApplicationDirectories", BindingFlags.NonPublic | BindingFlags.Static);
        var directories = (string[])field!.GetValue(null)!;

        Assert.That(directories, Does.Contain(PathProvider.VideoClips));
    }
}
