
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using st = Bits4N.Bit8;
using sm = Bits4N.Bench.Bit8.SBit8;

namespace Bits4N.Bench.Bit8;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class Bit8Equals
{
	private readonly st st1 = new st();
	private readonly st st2 = new st();
	private readonly sm sm1 = new sm();
	private readonly sm sm2 = new sm();

	[BenchmarkCategory("Equals"), Benchmark(Baseline = true)]
	public bool StdEq() => st1 == st2;
	[BenchmarkCategory("Equals"), Benchmark]
	public bool SmpEq() => sm1 == sm2;

	[BenchmarkCategory("Greate"), Benchmark(Baseline = true)]
	public bool StdGr() => st1 > st2;
	[BenchmarkCategory("Greate"), Benchmark]
	public bool SmpGr() => sm1 > sm2;
}
