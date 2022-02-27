using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using st = Bits4N.Bit8;
using sm = Bits4N.Bench.Bit8.SBit8;

namespace Bits4N.Bench.Bit8;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class Bit8Operations
{
	private readonly st st1 = new st();
	private readonly st st2 = new st();
	private readonly sm sm1 = new sm();
	private readonly sm sm2 = new sm();

	[BenchmarkCategory("Reverse"), Benchmark(Baseline = true)]
	public st StdReverse() => !st1;
	[BenchmarkCategory("Reverse"), Benchmark]
	public sm SmpReverse() => !sm1;

	[BenchmarkCategory("Or"), Benchmark(Baseline = true)]
	public st StdOr() => st1 | st2;
	[BenchmarkCategory("Or"), Benchmark]
	public sm SmpOr() => sm1 | sm2;

	[BenchmarkCategory("And"), Benchmark(Baseline = true)]
	public st StdAnd() => st1 & st2;
	[BenchmarkCategory("And"), Benchmark]
	public sm SmpAnd() => sm1 & sm2;
}
