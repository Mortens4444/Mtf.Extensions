using Mtf.Exceptions;
using Mtf.Extensions.Models;
using Mtf.Extensions.Services;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

namespace Mtf.Extensions.Tests;

/// <summary>
/// Regression tests for the defects found in the code review of the "Fix bugs and add tests" batch.
/// </summary>
public class BugFixRegressionTests
{
    [TestCase(90, 1, 1, 128, 255, 0)]
    [TestCase(45, 1, 1, 255, 191, 0)]
    [TestCase(0, 0, 1, 255, 255, 255)]
    [TestCase(0, 0.5, 1, 255, 128, 128)]
    [TestCase(90, 0.5, 0.75, 143, 191, 96)]
    [TestCase(210, 1, 1, 0, 128, 255)]
    [TestCase(330, 1, 1, 255, 0, 128)]
    public void ColorFromHSV_ArbitraryHues_ReturnsExpectedColor(double hue, double saturation, double value, int r, int g, int b)
    {
        var result = ColorExtensions.ColorFromHSV(hue, saturation, value);

        Assert.That(result.R, Is.EqualTo(r).Within(1));
        Assert.That(result.G, Is.EqualTo(g).Within(1));
        Assert.That(result.B, Is.EqualTo(b).Within(1));
    }

    [Test]
    public void ColorFromHSV_EveryIntegerHue_NeverThrows()
    {
        for (var hue = -360; hue <= 720; hue++)
        {
            var h = hue;
            Ensure.DoesNotThrow(() => ColorExtensions.ColorFromHSV(h, 1, 1));
        }
    }

    [Test]
    public void ConvertToBT709Grayscale_White_StaysWhite()
    {
        var result = Color.White.ConvertToBT709Grayscale();

        Assert.That(result.R, Is.EqualTo(255));
    }

    [Test]
    public void ConvertToBT601Grayscale_MidColor_IsOnFullByteScale()
    {
        var result = Color.FromArgb(120, 80, 200).ConvertToBT601Grayscale();

        // 0.299*120 + 0.587*80 + 0.114*200 = 105.6
        Assert.That(result.R, Is.EqualTo(106));
    }

    [Test]
    public void CMYColor_RgbToCmy_CmyToRgb_RoundTrips()
    {
        var cmy = CMYColor.RgbToCmy(Color.Red);

        var result = CMYColor.CmyToRgb(cmy.Item1, cmy.Item2, cmy.Item3);

        Assert.That(result.ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
    }

    [Test]
    public void CMYColor_ByteScaleConstructor_RoundTripsThroughRgb()
    {
        var original = Color.FromArgb(200, 150, 100);
        var fromRgb = new CMYColor(original);

        var fromCmy = new CMYColor(fromRgb.C, fromRgb.M, fromRgb.Y);

        Assert.That(fromCmy.R, Is.EqualTo(original.R));
        Assert.That(fromCmy.G, Is.EqualTo(original.G));
        Assert.That(fromCmy.B, Is.EqualTo(original.B));
    }

    [Test]
    public void GetSecureRandomByte_Distribution_IsNotModuloBiased()
    {
        const int samples = 40_000;
        const int range = 200;
        var counts = new int[range];
        for (var i = 0; i < samples; i++)
        {
            counts[RandomProvider.GetSecureRandomByte(0, range)]++;
        }

        // Expected 200 per bin, sigma ~14. The old biased sampler produced ~284 for 0..55 and ~142 for the rest.
        Assert.That(counts, Has.All.InRange(130, 270));
    }

    [Test]
    public void LogExceptions_CanceledTask_StaysCanceled()
    {
        var tcs = new TaskCompletionSource<object>();
        tcs.SetCanceled();

        var logged = tcs.Task.LogExceptions();
        Ensure.ThrowsAsync<TaskCanceledException>(async () => await logged);

        Assert.That(logged.IsCanceled, Is.True);
        Assert.That(logged.IsFaulted, Is.False);
    }

    [Test]
    public void LogExceptionsGeneric_CanceledTask_StaysCanceled()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetCanceled();

        var logged = tcs.Task.LogExceptions();
        Ensure.ThrowsAsync<TaskCanceledException>(async () => await logged);

        Assert.That(logged.IsCanceled, Is.True);
    }

    [Test]
    public void LogExceptions_DiscardedFaultedTask_DoesNotRaiseUnobservedTaskException()
    {
        var originalError = Console.Error;
        Console.SetError(TextWriter.Null);
        var unobserved = 0;
        EventHandler<UnobservedTaskExceptionEventArgs> handler = (_, e) =>
        {
            if (e.Exception.Flatten().InnerExceptions.Any(x => x.Message == "fire-and-forget"))
            {
                Interlocked.Increment(ref unobserved);
            }
        };
        TaskScheduler.UnobservedTaskException += handler;
        try
        {
            FireAndForget();
            for (var i = 0; i < 3; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.That(unobserved, Is.EqualTo(0));
        }
        finally
        {
            TaskScheduler.UnobservedTaskException -= handler;
            Console.SetError(originalError);
        }

        static void FireAndForget()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetException(new InvalidOperationException("fire-and-forget"));
            tcs.Task.LogExceptions();
        }
    }

    [Test]
    public void EndPoint_IPv6_GetPortAndAddress_Work()
    {
        var endpoint = new IPEndPoint(IPAddress.IPv6Loopback, 8080);

        Assert.That(endpoint.GetPort(), Is.EqualTo((ushort)8080));
        Assert.That(endpoint.GetIpAddress(), Is.EqualTo("::1"));
        Assert.That(endpoint.GetIpAddressAndPort().Item2, Is.EqualTo((ushort)8080));
    }

    [Test]
    public void String_IPv6EndPointText_ParsesAtLastColon()
    {
        var result = "[::1]:8080".GetIpAddressAndPortFromEndPoint();

        Assert.That(result.Item1, Is.EqualTo("[::1]"));
        Assert.That(result.Item2, Is.EqualTo((ushort)8080));
        Assert.That("[::1]:8080".GetPortFromEndPoint(), Is.EqualTo((ushort)8080));
        Assert.That("[::1]:8080".GetIpAddressFromEndPoint(), Is.EqualTo("[::1]"));
    }

    [Test]
    public void HexaToInteger_MoreThanEightDigits_ThrowsOverflowInsteadOfWrapping()
    {
        Ensure.Throws<OverflowException>(() => "100000000".HexaToInteger());
        Ensure.Throws<OverflowException>(() => "FFFFFFFF".HexaToInteger());
    }

    [Test]
    public void HexaToInteger_MaxInt_Parses()
    {
        Assert.That("7FFFFFFF".HexaToInteger(), Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void SocketConnect_NothingListening_ThrowsConnectionFailedWithSocketExceptionInner()
    {
        var probe = new TcpListener(IPAddress.Loopback, 0);
        probe.Start();
        var freePort = ((IPEndPoint)probe.LocalEndpoint).Port;
        probe.Stop();

        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var ex = Ensure.Throws<ConnectionFailedException>(
            () => client.Connect("127.0.0.1", (ushort)freePort, 5000, _ => Enumerable.Empty<string>()));
        Assert.That(ex.InnerException, Is.InstanceOf<SocketException>());
    }

    [Test]
    public void ConnectionExceptions_InnerExceptionOverload_PreservesInner()
    {
        var inner = new SocketException((int)SocketError.ConnectionRefused);

        var failed = new ConnectionFailedException("10.0.0.1", 80, "local", inner);
        var timedOut = new ConnectionTimedOutException("10.0.0.1", 80, "local", inner);

        Assert.That(failed.InnerException, Is.SameAs(inner));
        Assert.That(timedOut.InnerException, Is.SameAs(inner));
        Assert.That(failed.Message, Does.Contain("Connection failed"));
        Assert.That(timedOut.Message, Does.Contain("Connection timed out"));
    }
}
