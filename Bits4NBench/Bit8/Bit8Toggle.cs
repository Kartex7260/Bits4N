
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using st = Bits4N.Bit8;
using sm = Bits4N.Bench.Bit8.SBit8;

namespace Bits4N.Bench.Bit8;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
[MemoryDiagnoser]
public class Bit8Toggle
{
	private readonly st st = new st();
	private readonly sm sm = new sm();

	[Params(0, 3, 7)]
	public int Index { get; set; }

	[Benchmark(Baseline = true)]
	public void Standart()
	{
		st.Toggle(Index);
	}

	[Benchmark]
	public void Simple()
	{
		sm.Toggle(Index);
	}
}
