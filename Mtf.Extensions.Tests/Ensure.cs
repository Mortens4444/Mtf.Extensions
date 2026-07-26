using NUnit.Framework.Constraints;

namespace Mtf.Extensions.Tests;

// NUnit 4.x added Assert.Throws/DoesNotThrow/That overloads accepting System.Action alongside
// the classic NUnit.Framework.TestDelegate ones. Since both delegate types have an identical
// void() signature, a bare lambda passed directly to Assert.Throws<T>(() => ...) etc. is now
// ambiguous (CS0121). Routing through these TestDelegate-typed wrappers fixes that: once the
// argument's static type is already TestDelegate, only one of NUnit's overloads can accept it.
internal static class Ensure
{
    public static TActual Throws<TActual>(TestDelegate code) where TActual : Exception
        => Assert.Throws<TActual>(code);

    public static void DoesNotThrow(TestDelegate code)
        => Assert.DoesNotThrow(code);

    public static void That(TestDelegate code, IResolveConstraint constraint)
        => Assert.That(code, constraint);

    public static TActual ThrowsAsync<TActual>(AsyncTestDelegate code) where TActual : Exception
        => Assert.ThrowsAsync<TActual>(code);

    public static void DoesNotThrowAsync(AsyncTestDelegate code)
        => Assert.DoesNotThrowAsync(code);
}
