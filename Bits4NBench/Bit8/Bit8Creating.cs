using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using st = Bits4N.Bit8;
using sm = Bits4N.Bench.Bit8.SBit8;

namespace Bits4N.Bench.Bit8;

[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class Bit8Creating
{
	public readonly byte b = 127;
	public readonly bool[] bits = new bool[] { true, false, true, false, false, true, false, false };
	public readonly IBitable btble = new Bit16(13561);

	[BenchmarkCategory("Empty"), Benchmark(Baseline = true)]
	public st StandartEmpty() => new st();
	[BenchmarkCategory("Empty"), Benchmark]
	public sm SimpleEmpty() => new sm();

	[BenchmarkCategory("Byte"), Benchmark(Baseline = true)]
	public st StandartByte() => new st(b);
	[BenchmarkCategory("Byte"), Benchmark]
	public sm SimpleByte() => new sm(b);

	[BenchmarkCategory("Bool"), Benchmark(Baseline = true)]
	public st StandartBool() => new st(bits);
	[BenchmarkCategory("Bool"), Benchmark]
	public sm SimpleBool() => new sm(bits);

	[BenchmarkCategory("Bitable"), Benchmark(Baseline = true)]
	public st StandartByteable() => new st(btble);
	[BenchmarkCategory("Bitable"), Benchmark]
	public sm SimpleByteable() => new sm(btble);
}
