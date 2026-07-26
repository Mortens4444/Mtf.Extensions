namespace Mtf.Extensions.Tests;

public class TaskExtensionsTests
{
    // LogExceptions deliberately writes the faulted task's exception to Console.Error before
    // rethrowing it, so these two tests redirect stderr to avoid that expected output being
    // captured and surfaced as a build warning by the test platform - and use the capture to
    // additionally verify the logging itself happened, not just that the exception propagated.

    [Test]
    public void LogExceptions_FaultedTask_PropagatesOriginalExceptionAndLogsIt()
    {
        var originalError = Console.Error;
        var capturedError = new StringWriter();
        Console.SetError(capturedError);
        try
        {
            var task = Task.Run(() => throw new InvalidOperationException("boom"));

            var logged = task.LogExceptions();

            var ex = Ensure.ThrowsAsync<InvalidOperationException>(async () => await logged);
            Assert.That(ex.Message, Is.EqualTo("boom"));
        }
        finally
        {
            Console.SetError(originalError);
        }

        Assert.That(capturedError.ToString(), Does.Contain("boom"));
    }

    [Test]
    public async Task LogExceptions_SuccessfulTask_CompletesWithoutThrowing()
    {
        var task = Task.Run(() => { });

        Ensure.DoesNotThrowAsync(async () => await task.LogExceptions());
        await Task.CompletedTask;
    }

    [Test]
    public void LogExceptionsGeneric_FaultedTask_PropagatesOriginalExceptionAndLogsIt()
    {
        var originalError = Console.Error;
        var capturedError = new StringWriter();
        Console.SetError(capturedError);
        try
        {
            var task = Task.Run(new Func<int>(() => throw new InvalidOperationException("boom")));

            var logged = task.LogExceptions();

            var ex = Ensure.ThrowsAsync<InvalidOperationException>(async () => await logged);
            Assert.That(ex.Message, Is.EqualTo("boom"));
        }
        finally
        {
            Console.SetError(originalError);
        }

        Assert.That(capturedError.ToString(), Does.Contain("boom"));
    }

    [Test]
    public async Task LogExceptionsGeneric_SuccessfulTask_ReturnsOriginalResult()
    {
        var task = Task.Run(() => 42);

        var result = await task.LogExceptions();

        Assert.That(result, Is.EqualTo(42));
    }
}
