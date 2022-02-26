
using System.Collections;
using System.Text;

namespace Bits4N;

/// <summary>
/// Класс для разбора примитивных типов на логический массив.
/// <para>
/// Работает с <see cref="ushort"/>, <see cref="short"/>, <see cref="char"/>
/// </para>
/// </summary>
public sealed class Bit16 : IBitable, IByteable, IEquatable<Bit16>, IComparable<Bit16>, IComparable, ICloneable<Bit16>, IEnumerable<bool>
{
	private readonly bool[] bits = new bool[BitCount];
	private short _short = 0;

	/// <summary>
	/// Предоставляет <see cref="short">шорт со знаком</see> на основе логического массива.
	/// </summary>
	public short Short => _short;
	/// <summary>
	/// Предоставляет <see cref="ushort">Беззнаковый шорт</see> на основе логического массива.
	/// </summary>
	public ushort UShort => Convert.ToUInt16(_short);
	/// <summary>
	/// Предоставляет UTF-16 символ на основе логического массива.
	/// </summary>
	public char Char => Convert.ToChar(_short);
	/// <summary>
	/// Предоставляет копию логического массива.
	/// </summary>
	public bool[] Bits => (bool[]) bits.Clone();
	/// <summary>
	/// Предоставляет массив байтов.
	/// </summary>
	public byte[] Bytes => BitConverter.GetBytes(_short);

	/// <summary>
	/// Создаёт пустой объект.
	/// </summary>
	public Bit16() { }
	/// <summary>
	/// Создаёт объект на основе <see cref="short">шорта со знаком</see>.
	/// </summary>
	/// <param name="s">Разбираемый <see cref="short"/>.</param>
	public Bit16(short s)
	{
		_short = s;
		Demount();
	}
	/// <summary>
	/// Создаёт объект на основе <see cref="ushort">беззнакового шорта</see>.
	/// </summary>
	/// <param name="us">Разбираемый <see cref="ushort"/>.</param>
	public Bit16(ushort us) : this(Convert.ToInt16(us)) { }
	/// <summary>
	/// Создаёт объект на основе символа.
	/// </summary>
	/// <param name="c">UTF-8 символ.</param>
	public Bit16(char c) : this(Convert.ToInt16(c)) { }
	/// <summary>
	/// Создаёт объект на основе логического массива.
	/// </summary>
	/// <param name="bits">Логический массив.</param>
	public Bit16(params bool[] bits)
	{
		if (bits.Length > BitCount)
			throw new InvalidOperationException("Very large bit array");

		for (int i = 0; i < bits.Length; i++)
		{
			this.bits[i] = bits[i];
		}

		_short = Package();
	}
	/// <summary>
	/// Создаёт объект на основе любого <see cref="IBitable">объекта работающего с битами</see>.
	/// </summary>
	/// <param name="bitable">Объект работающий с битами.</param>
	public Bit16(IBitable bitable)
	{
		int cycle = bitable.Bits.Length >= BitCount ? BitCount : bitable.Bits.Length;
		for (int i = cycle; i < BitCount; i++)
			bits[i] = bitable.Bits[i];
		_short = Package();
	}
	/// <summary>
	/// Создаёт объект на основе <see cref="byte">байт</see> массива.
	/// </summary>
	/// <param name="buffer">Байт массив.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public Bit16(byte[] buffer)
	{
		if (buffer.Length > Size)
			throw new InvalidOperationException($"{nameof(buffer)} very large");

		byte[] newBuffer = new byte[Size];
		for (int i = 0; i < buffer.Length; i++)
			newBuffer[i] = buffer[i];

		_short = BitConverter.ToInt16(newBuffer);
		Demount();
	}

	/// <summary>
	/// Переключает бит по индексу.
	/// </summary>
	/// <param name="index">Индекс переключаемого бита.</param>
	public void Toggle(int index) => bits[index] = !bits[index];

	/// <summary>
	/// Сравнивает два <see cref="Bit16"/> объекта.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para>1 - если значение родного объекта больше.</para>
	/// <para>0 - если значения объектов одинаковые.</para>
	/// <para>-1 - если значение родного объекта меньше</para>
	/// </returns>
	public int CompareTo(Bit16? bb)
	{
		if (bb is null)
			return 1;

		return _short.CompareTo(bb._short);
	}
	/// <summary>
	/// Сравнивает два <see cref="Bit16"/> объекта.
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
		if (obj is not Bit16 bb)
			return _short.CompareTo(obj);
		return _short.CompareTo(bb._short);
	}

	/// <summary>
	/// Сравнивает родной объект с чужим на равенство.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public bool Equals(Bit16? bb)
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
		if (obj is null || obj is not Bit16 bb)
			return false;
		return GetHashCode() == bb.GetHashCode();
	}
	/// <summary>
	/// Предоставляет хэш-код объекта.
	/// </summary>
	public override int GetHashCode() => _short;
	/// <summary>
	/// Конвертирует объект в строковое представление.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		StringBuilder sb = new($"{_short}: [");
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
	public Bit16 Clone() => new(_short);
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
				_short |= BITS[index];
			else
				_short &= RBITS[index];
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
	public static bool operator ==(Bit16? x, Bit16? y)
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
	public static bool operator !=(Bit16? x, Bit16? y)
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
	public static bool operator <(Bit16? x, Bit16? y)
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
	public static bool operator >(Bit16? x, Bit16? y)
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
	public static bool operator >=(Bit16? x, Bit16? y)
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
	public static bool operator <=(Bit16? x, Bit16? y)
	{
		var result = Compare(y, x);
		return result >= 0;
	}

	/// <summary>
	/// Логическое отрицание.
	/// </summary>
	/// <param name="x">Отицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static Bit16 operator !(Bit16 x)
	{
		var result = new Bit16();
		for (int i = 0; i < BitCount; i++)
			result[i] = !x[i];
		return result;
	}
	/// <summary>
	/// Побитовое отрицание.
	/// </summary>
	/// <param name="x">отрицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static Bit16 operator ~(Bit16 x)
	{
		return !x;
	}
	/// <summary>
	/// Логическое ИЛИ.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>Новый объект созданный на базе вычесление логического ИЛИ двух операндов.</returns>
	public static Bit16 operator |(Bit16 x, Bit16 y)
	{
		var result = new Bit16();
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
	public static Bit16 operator &(Bit16 x, Bit16 y)
	{
		var result = new Bit16();
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
	public static Bit16 operator ^(Bit16 x, Bit16 y)
	{
		var result = new Bit16();
		for (int i = 0; i < BitCount; i++)
			result[i] = x[i] ^ y[i];
		return result;
	}


	/// <summary>
	/// Неявное преобразование логического массива в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(bool[] x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="byte">беззнакового байта</see> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(byte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="sbyte">байта со знаком</see> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(sbyte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="char">символа</see> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(char x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="short"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(short x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="ushort"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(ushort x) => new(x);

	/// <summary>
	/// Явное преобразование <see cref="int"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(int x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="uint"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(uint x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="long"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(long x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="ulong"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(ulong x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="float"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(float x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="double"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(double x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="decimal"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(decimal x) => new(Convert.ToByte(x));

	/// <summary>
	/// Неявное преобразование <see cref="Bit8"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator Bit16(Bit8 x) => new(x.Byte);
	/// <summary>
	/// Явное преобразование <see cref="Bit32"/> в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(Bit32 x) => new(Convert.ToInt16(x.Int));
	/// <summary>
	/// Явное преобразование <see cref="Bit64"/> в <see cref="Bit16"/>
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator Bit16(Bit64 x) => new(Convert.ToInt16(x.Long));


	/// <summary>
	/// Явное преобразование <see cref="Bit16"/> в <see cref="byte">беззнаковый байт</see>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator byte(Bit16 x) => Convert.ToByte(x.Short);
	/// <summary>
	/// Явное преобразование <see cref="Bit16"/> в <see cref="sbyte">байт со знаком</see>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator sbyte(Bit16 x) => Convert.ToSByte(x.UShort);

	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в логический массив.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator bool[](Bit16 x) => x.Bits;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="char">UTF-16 символ</see>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator char(Bit16 x) => x.Char;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="short"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator short(Bit16 x) => x.Short;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="ushort"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator ushort(Bit16 x) => x.UShort;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="int"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator int(Bit16 x) => x.Short;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="uint"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator uint(Bit16 x) => x.UShort;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="long"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator long(Bit16 x) => x.Short;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="ulong"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator ulong(Bit16 x) => x.UShort;
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="float"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator float(Bit16 x) => Convert.ToSingle(x.Short);
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="double"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator double(Bit16 x) => Convert.ToDouble(x.Short);
	/// <summary>
	/// Неявное преобразование <see cref="Bit16"/> в <see cref="decimal"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator decimal(Bit16 x) => Convert.ToDecimal(x.Short);

	#region private
	private short Package()
	{
		short result = 0;

		for (int i = 0; i < BitCount; i++)
			if (bits[i]) result |= BITS[i];

		return result;
	}
	private void Demount()
	{
		for (int i = 0; i < BitCount; i++)
		{
			short clearShort = (short) (_short & BITS[i]);
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
	public const int BitCount = 16;
	/// <summary>
	/// Размер (в байтах) хранимого значения.
	/// </summary>
	public const int Size = 2;
	/// <summary>
	/// Максимально возможно значение.
	/// </summary>
	public const ushort MaxValue = ushort.MaxValue;
	/// <summary>
	/// Минимально возможное значение.
	/// </summary>
	public const short MinValue = short.MinValue;

	/// <summary>
	/// Парсинг строкового значения в <see cref="Bit16"/>.
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">Если строка имеет неправильный формат.</exception>
	public static Bit16 Parse(string line)
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
	/// Попытка отпарсить строковое значение в <see cref="Bit16"/>
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <param name="result">Выходной результат парсинга.</param>
	/// <returns>
	/// <para><c>true</c> - если парсинг прошёл успешно.</para>
	/// <para><c>false</c> - если парсинг не удался.</para>
	/// </returns>
	public static bool TryParse(string line, out Bit16 result)
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
	public static int Compare(Bit16? x, Bit16? y)
	{
		if (x is null && y is null)
			return 0;
		if (x is not null && y is null)
			return 1;
		if (x is null && y is not null)
			return -1;

		return x!._short.CompareTo(y!._short);
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
	public static bool Equals(Bit16? x, Bit16? y)
	{
		if (x is null && y is null)
			return true;
		if (x is not null)
			return x.Equals(y);
		return false;
	}
	#endregion

	#region static private

	private static readonly short[] BITS;
	private static readonly short[] RBITS;

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

	static Bit16()
	{
		short[] bs = new short[BitCount];
		short tmp = 1;
		for (int i = 0; i < BitCount; i++)
		{
			bs[i] = tmp;
			tmp *= 2;
		}
		BITS = bs;

		short[] rbs = new short[BitCount];
		for (int i = 0; i < BitCount; i++)
			rbs[i] = (short) ~BITS[i];
		RBITS = rbs;
	}
	#endregion
}
