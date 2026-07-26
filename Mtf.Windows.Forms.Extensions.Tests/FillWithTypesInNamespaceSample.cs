// FillWithTypesInNamespace scans and instantiates every type declared in the given namespace,
// so its sample type needs a namespace of its own - not the shared Mtf.Windows.Forms.Extensions.Tests
// one, which holds unrelated helpers (like the static Ensure class) that aren't instantiable.
namespace Mtf.Windows.Forms.Extensions.Tests.FillWithTypesInNamespaceSample;

public class SampleItem
{
}
