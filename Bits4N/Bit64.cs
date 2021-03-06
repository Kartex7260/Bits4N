
using System.Collections;
using System.Text;

namespace Bits4N;

/// <summary>
/// Класс для разбора примитивных типов на логический массив.
/// <para>
/// Работает с <see cref="ulong"/>, <see cref="long"/>, <see cref="double"/>
/// </para>
/// </summary>
public sealed class Bit64 : IBitable, IByteable, IEquatable<Bit64>, IComparable<Bit64>, IComparable, ICloneable<Bit64>, IEnumerable<bool>
{
	private readonly bool[] bits = new bool[BitCount];
	private long _long = 0;

	/// <summary>
	/// Предоставляет <see cref="int">инт со знаком</see> на основе логического массива.
	/// </summary>
	public long Long => _long;
	/// <summary>
	/// Предоставляет <see cref="uint">беззнаковый инт</see> на основе логического массива.
	/// </summary>
	public ulong ULong => Convert.ToUInt64(_long);
	/// <summary>
	/// Предоставляет <see cref="float">число с плавающей запятой одинарной точности</see> на
	/// основе логического массива.
	/// </summary>
	public double Double => Convert.ToDouble(_long);
	/// <summary>
	/// Предоставляет копию логического массива.
	/// </summary>
	public bool[] Bits => (bool[]) bits.Clone();
	/// <summary>
	/// Предоставляет массив байтов.
	/// </summary>
	public byte[] Bytes => BitConverter.GetBytes(_long);

	/// <summary>
	/// Создаёт пустой объект.
	/// </summary>
	public Bit64() { }
	/// <summary>
	/// Создаёт объект на основе <see cref="int">инта со знаком</see>.
	/// </summary>
	/// <param name="i">Разбираемый <see cref="int"/>.</param>
	public Bit64(long i)
	{
		_long = i;
		Demount();
	}
	/// <summary>
	/// Создаёт объект на основе <see cref="uint">беззнакового инта</see>.
	/// </summary>
	/// <param name="ui">Разбираемый <see cref="uint"/>.</param>
	public Bit64(ulong ui) : this(Convert.ToInt64(ui)) { }
	/// <summary>
	/// Создаёт объект на основе <see cref="float">числа с плавающей запятой одинарной точности</see>.
	/// </summary>
	/// <param name="f">Число с плавающей запятой.</param>
	public Bit64(double f) : this(Convert.ToInt64(f)) { }
	/// <summary>
	/// Создаёт объект на основе логического массива.
	/// </summary>
	/// <param name="bits">Логический массив.</param>
	public Bit64(params bool[] bits)
	{
		if (bits.Length > BitCount)
			throw new InvalidOperationException("Very large bit array");

		this.bits = NormalizeArray(bits, BitCount);
		_long = Package();
	}
	/// <summary>
	/// Создаёт объект на основе любого <see cref="IBitable">объекта работающего с битами</see>.
	/// </summary>
	/// <param name="bitable">Объект работающий с битами.</param>
	public Bit64(IBitable bitable)
	{
		bits = NormalizeArray(bitable.Bits, BitCount);
		_long = Package();
	}
	/// <summary>
	/// Создаёт объект на основе <see cref="byte">байт</see> массива.
	/// </summary>
	/// <param name="buffer">Байт массив.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public Bit64(byte[] buffer)
	{
		if (buffer.Length > Size)
			throw new InvalidOperationException($"{nameof(buffer)} very large");

		var newBuffer = NormalizeArray(buffer, Size);
		_long = BitConverter.ToInt64(newBuffer);
		Demount();
	}
	/// <summary>
	/// Создаёт объект на основе любого <see cref="IByteable">объекта работающего с байтами</see>.
	/// </summary>
	/// <param name="byteable">Объект работающий с байтами.</param>
	public Bit64(IByteable byteable)
	{
		var buffer = NormalizeArray(byteable.Bytes, Size);
		_long = BitConverter.ToInt32(buffer);
		Demount();
	}

	/// <summary>
	/// Переключает бит по индексу.
	/// </summary>
	/// <param name="index">Индекс переключаемого бита.</param>
	public void Toggle(int index) => bits[index] = !bits[index];

	/// <summary>
	/// Сравнивает два <see cref="Bit64"/> объекта.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para>1 - если значение родного объекта больше.</para>
	/// <para>0 - если значения объектов одинаковые.</para>
	/// <para>-1 - если значение родного объекта меньше</para>
	/// </returns>
	public int CompareTo(Bit64? bb)
	{
		if (bb is null)
			return 1;

		return _long.CompareTo(bb._long);
	}
	/// <summary>
	/// Сравнивает два <see cref="Bit64"/> объекта.
	/// </summary>
	/// <param name="obj">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para>1 - если значение родного объекта больше.</para>
	/// <para>0 - если значения объектов одинаковые.</para>
	/// <para>-1 - если значение родного объекта меньше</para>
	/// </returns>
	public int CompareTo(object? obj)
	{
		if (obj is null)
			return 1;
		if (obj is not Bit64 bb)
			return _long.CompareTo(obj);
		return _long.CompareTo(bb._long);
	}

	/// <summary>
	/// Сравнивает родной объект с чужим на равенство.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public bool Equals(Bit64? bb)
	{
		if (bb is null)
			return false;
		return GetHashCode() == bb.GetHashCode();
	}
	/// <summary>
	/// Сравнивает родной объект с чужим на равенство.
	/// </summary>
	/// <param name="obj">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public override bool Equals(object? obj)
	{
		if (obj is null || obj is not Bit64 bb)
			return false;
		return GetHashCode() == bb.GetHashCode();
	}
	/// <summary>
	/// Предоставляет хэш-код объекта.
	/// </summary>
	public override int GetHashCode()
	{
		var buffer = BitConverter.GetBytes(_long);
		int firstHalf = BitConverter.ToInt32(buffer, 0);
		int secondHalf = BitConverter.ToInt32(buffer, 4);
		return HashCode.Combine(firstHalf, secondHalf);
	}
	/// <summary>
	/// Конвертирует объект в строковое представление.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		StringBuilder sb = new($"{_long}: [");
		for (int i = 0; i < BitCount; i++)
		{
			sb.Append(bits[i] ? '1' : '0');
			if (i < BitCount - 1)
				sb.Append(", ");
		}
		sb.Append(']');
		return sb.ToString();
	}

	/// <summary>
	/// Предоставляет енумератор для перебора логического массива.
	/// </summary>
	/// <returns></returns>
	public IEnumerator<bool> GetEnumerator()
	{
		for (int i = 0; i < BitCount; i++)
			yield return bits[i];
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Клонирует объект.
	/// </summary>
	/// <returns></returns>
	public Bit64 Clone() => new(_long);
	object ICloneable.Clone() => Clone();

	/// <summary>
	/// Предоставляет безопастный доступ к логическому массиву.
	/// </summary>
	/// <param name="index">Индекс получаемого или задаваемого бита.</param>
	/// <returns></returns>
	public bool this[Index index]
	{
		get => bits[index];
		set
		{
			bits[index] = value;
			if (value)
				_long |= BITS[index];
			else
				_long &= RBITS[index];
		}
	}


	/// <summary>
	/// Сравнивает два объекта на равенство значений.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public static bool operator ==(Bit64? x, Bit64? y)
	{
		return Equals(x, y);
	}
	/// <summary>
	/// Сравнивает два объекта на различие значений.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов различаются.</para>
	/// <para><c>false</c> - если значения объектов равны.</para>
	/// </returns>
	public static bool operator !=(Bit64? x, Bit64? y)
	{
		return !Equals(x, y);
	}
	/// <summary>
	/// Сравнивает два объекта на различия в значениях.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значение первого объекта меньше значения второго объекта.</para>
	/// <para><c>false</c> - если значение первого объекта равно или больше значения второго объекта.</para>
	/// </returns>
	public static bool operator <(Bit64? x, Bit64? y)
	{
		var result = Compare(y, x);
		return result > 0;
	}
	/// <summary>
	/// Сравнивает два объекта на различия в значениях.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значение первого объекта больше значения второго объекта.</para>
	/// <para><c>false</c> - если значение первого объекта равно или меньше значения второго объекта.</para>
	/// </returns>
	public static bool operator >(Bit64? x, Bit64? y)
	{
		var result = Compare(x, y);
		return result > 0;
	}
	/// <summary>
	/// Сравнивает два объекта на различия в значениях.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значение первого объекта равно или больше значения второго объекта.</para>
	/// <para><c>false</c> - если значение первого объекта меньше значения второго объекта.</para>
	/// </returns>
	public static bool operator >=(Bit64? x, Bit64? y)
	{
		var result = Compare(x, y);
		return result >= 0;
	}
	/// <summary>
	/// Сравнивает два объекта на различия в значениях.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значение первого объекта равно или меньше значения второго объекта.</para>
	/// <para><c>false</c> - если значение первого объекта больше значения второго объекта.</para>
	/// </returns>
	public static bool operator <=(Bit64? x, Bit64? y)
	{
		var result = Compare(y, x);
		return result >= 0;
	}

	/// <summary>
	/// Логическое отрицание.
	/// </summary>
	/// <param name="x">Отицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static Bit64 operator !(Bit64 x)
	{
		var result = new Bit64();
		for (int i = 0; i < BitCount; i++)
			result[i] = !x[i];
		return result;
	}
	/// <summary>
	/// Побитовое отрицание.
	/// </summary>
	/// <param name="x">отрицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static Bit64 operator ~(Bit64 x)
	{
		return !x;
	}
	/// <summary>
	/// Логическое ИЛИ.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>Новый объект созданный на базе вычесление логического ИЛИ двух операндов.</returns>
	public static Bit64 operator |(Bit64 x, Bit64 y)
	{
		var result = new Bit64();
		for (int i = 0; i < BitCount; i++)
			result[i] = x[i] | y[i];
		return result;
	}
	/// <summary>
	/// Логическое И.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>Новый объект созданный на базе вычесление логического И двух операндов.</returns>
	public static Bit64 operator &(Bit64 x, Bit64 y)
	{
		var result = new Bit64();
		for (int i = 0; i < BitCount; i++)
			result[i] = x[i] & y[i];
		return result;
	}
	/// <summary>
	/// Логическое исключение ИЛИ.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>Новый объект созданный на базе вычесление логического исключения ИЛИ двух операндов.</returns>
	public static Bit64 operator ^(Bit64 x, Bit64 y)
	{
		var result = new Bit64();
		for (int i = 0; i < BitCount; i++)
			result[i] = x[i] ^ y[i];
		return result;
	}


	/// <summary>
	/// Неявное преобразование логического массива в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(bool[] x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="byte">беззнакового байта</see> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(byte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="sbyte">байта со знаком</see> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(sbyte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="char">символа</see> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(char x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="short"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(short x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="ushort"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(ushort x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="int"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(int x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="uint"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(uint x) => new(x);

	/// <summary>
	/// Явное преобразование <see cref="long"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit64(long x) => new(x);
	/// <summary>
	/// Явное преобразование <see cref="ulong"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit64(ulong x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="float"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit64(float x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="double"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit64(double x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="decimal"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit64(decimal x) => new(Convert.ToByte(x));

	/// <summary>
	/// Неявное преобразование <see cref="Bit8"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(Bit8 x) => new(x.Byte);
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(Bit16 x) => new(x.Short);
	/// <summary>
	/// Неявное преобразование <see cref="Bit32"/> в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit64(Bit32 x) => new(x.Int);


	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="byte">беззнаковый байт</see>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator byte(Bit64 x) => Convert.ToByte(x.Long);
	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="sbyte">байт со знаком</see>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator sbyte(Bit64 x) => Convert.ToSByte(x.ULong);
	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="char">UTF-16 символ</see>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator char(Bit64 x) => Convert.ToChar(x.Long);
	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="short"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator short(Bit64 x) => Convert.ToInt16(x.Long);
	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="ushort"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator ushort(Bit64 x) => Convert.ToUInt16(x.Long);
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="int"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator int(Bit64 x) => Convert.ToInt32(x.Long);
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="uint"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator uint(Bit64 x) => Convert.ToUInt32(x.Long);
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="float"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator float(Bit64 x) => Convert.ToSingle(x.Long);

	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в логический массив.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator bool[](Bit64 x) => x.Bits;
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="long"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator long(Bit64 x) => x.Long;
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="ulong"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator ulong(Bit64 x) => x.ULong;
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="double"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator double(Bit64 x) => x.Double;
	/// <summary>
	/// Неявное преобразование <see cref="Bit64"/> в <see cref="decimal"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator decimal(Bit64 x) => Convert.ToDecimal(x.Long);

	#region private
	private long Package()
	{
		long result = 0;

		for (int i = 0; i < BitCount; i++)
			if (bits[i]) result |= BITS[i];

		return result;
	}
	private void Demount()
	{
		for (int i = 0; i < BitCount; i++)
		{
			long clearShort = _long & BITS[i];
			if (clearShort == BITS[i])
				bits[i] = true;
		}
	}
	#endregion


	#region static
	/// <summary>
	/// Количество битов.
	/// <para>Размер логического массива.</para>
	/// </summary>
	public const int BitCount = 64;
	/// <summary>
	/// Размер (в байтах) хранимого значения.
	/// </summary>
	public const int Size = 8;
	/// <summary>
	/// Максимально возможно значение.
	/// </summary>
	public const ulong MaxValue = ulong.MaxValue;
	/// <summary>
	/// Минимально возможное значение.
	/// </summary>
	public const long MinValue = long.MinValue;
	/// <summary>
	/// Максимальное значение для альтернативного типа данных.
	/// </summary>
	public const double AltMaxValue = double.MaxValue;
	/// <summary>
	/// Минимальное значение для альтернативного типа данных.
	/// </summary>
	public const double AltMinValue = double.MinValue;

	/// <summary>
	/// Парсинг строкового значения в <see cref="Bit64"/>.
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">Если строка имеет неправильный формат.</exception>
	public static Bit64 Parse(string line)
	{
		bool[] result = new bool[BitCount];
		int cycle = line.Length >= BitCount ? BitCount : line.Length;
		for (int i = 0; i < cycle; i++)
		{
			int valRes = ValideChar(line[i]);
			if (valRes == -1)
				throw new InvalidOperationException("Invalid binary line format");
			else if (valRes == 0)
				result[i] = line[i] == '1';
			else if (valRes == 1)
				result[i] = line[i] == 't' || line[i] == 'T';
		}
		return new(result);
	}
	/// <summary>
	/// Попытка отпарсить строковое значение в <see cref="Bit64"/>
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <param name="result">Выходной результат парсинга.</param>
	/// <returns>
	/// <para><c>true</c> - если парсинг прошёл успешно.</para>
	/// <para><c>false</c> - если парсинг не удался.</para>
	/// </returns>
	public static bool TryParse(string line, out Bit64 result)
	{
		result = new bool[BitCount];
		int cycle = line.Length >= BitCount ? BitCount : line.Length;
		for (int i = 0; i < cycle; i++)
		{
			int valRes = ValideChar(line[i]);
			if (valRes == -1)
				return false;
			else if (valRes == 0)
				result[i] = line[i] == '1';
			else if (valRes == 1)
				result[i] = line[i] == 't' || line[i] == 'T';
		}
		return true;
	}

	/// <summary>
	/// Сравнивает два объекта на различия в значениях.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>-1 и меньше</c> - если значение первого объекта меньше значения второго объекта.</para>
	/// <para><c>равно 0</c> - если значение первого объекта равно значению второго объекта.</para>
	/// <para><c>1 и больше</c> - если значение первого объекта больше значения второго объекта.</para>
	/// </returns>
	public static int Compare(Bit64? x, Bit64? y)
	{
		if (x is null && y is null)
			return 0;
		if (x is not null && y is null)
			return 1;
		if (x is null && y is not null)
			return -1;

		return x!._long.CompareTo(y!._long);
	}
	/// <summary>
	/// Сравнивает два объекта на равенство значений.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public static bool Equals(Bit64? x, Bit64? y)
	{
		if (x is null && y is null)
			return true;
		if (x is not null)
			return x.Equals(y);
		return false;
	}
	#endregion

	#region static private

	private static readonly long[] BITS;
	private static readonly long[] RBITS;

	private static int ValideChar(char c)
	{
		bool isNum = c == '1' || c == '0';
		if (isNum)
			return 0;
		bool isChar = c == 't' || c == 'T' || c == 'f' || c == 'F';
		if (isChar)
			return 1;
		return -1;
	}
	private static T[] NormalizeArray<T>(T[] ts, int maxLength)
	{
		if (ts.Length == BitCount)
			return ts;
		T[] buffer = new T[BitCount];
		int cycle = ts.Length >= maxLength ? maxLength : ts.Length;
		for (int i = 0; i < cycle; i++)
			buffer[i] = ts[i];
		return buffer;
	}

	static Bit64()
	{
		long[] bs = new long[BitCount];
		short tmp = 1;
		for (int i = 0; i < BitCount; i++)
		{
			bs[i] = tmp;
			tmp *= 2;
		}
		BITS = bs;

		long[] rbs = new long[BitCount];
		for (int i = 0; i < BitCount; i++)
			rbs[i] = ~BITS[i];
		RBITS = rbs;
	}
	#endregion
}
