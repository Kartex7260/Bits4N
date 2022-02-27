
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using st = Bits4N.Bit8;
using sm = Bits4N.Bench.Bit8.SBit8;

namespace Bits4N.Bench.Bit8;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class Bit8Index
{
	private readonly st st = new st();
	private readonly sm sm = new sm();

	[Params(0, 3, 7)]
	public int Index { get; set; }

	[BenchmarkCategory("Set"), Benchmark(Baseline = true)]
	public void StandartSet()
	{
		st[Index] = true;
	}

	[BenchmarkCategory("Set"), Benchmark]
	public void SimpleSet()
	{
		sm[Index] = true;
	}

	[BenchmarkCategory("Get"), Benchmark(Baseline = true)]
	public bool StandartGet()
	{
		return st[Index];
	}

	[BenchmarkCategory("Get"), Benchmark]
	public bool SimpleGet()
	{
		return sm[Index];
	}
}
