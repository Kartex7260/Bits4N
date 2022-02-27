
using System.Collections;
using System.Text;

namespace Bits4N.Value;

/// <summary>
/// Класс для разбора примитивных типов на логический массив.
/// <para>
/// Работает с <see cref="byte"/>, <see cref="sbyte"/>, <see cref="char"/>
/// </para>
/// </summary>
public struct VBit8 : IBitable, IByteable, IEquatable<VBit8>, IComparable<VBit8>, IComparable, ICloneable<VBit8>, IEnumerable<bool>
{
	private byte _byte = 0;

	/// <summary>
	/// Предоставляет байт без знака на основе логического массива.
	/// </summary>
	public byte Byte => _byte;
	/// <summary>
	/// Предоставляет байт со знаком на основе логического массива.
	/// </summary>
	public sbyte SByte => Convert.ToSByte(_byte);
	/// <summary>
	/// Предоставляет UTF-8 символ на основе логического массива.
	/// </summary>
	public char Char => Convert.ToChar(_byte);
	/// <inheritdoc cref="IBitable.Bits"/>
	public bool[] Bits => Demount();
	/// <inheritdoc cref="IByteable.Bytes"/>
	public byte[] Bytes => new byte[] { _byte };

	/// <summary>
	/// Создаёт пустой объект.
	/// </summary>
	public VBit8() { }
	/// <summary>
	/// Создаёт объект на основе байта без знака.
	/// </summary>
	/// <param name="b">Разбираемый байт.</param>
	public VBit8(byte b)
	{
		_byte = b;
	}
	/// <summary>
	/// Создаёт объект на основе байта со знаком.
	/// </summary>
	/// <param name="sb">Разбираемый байт.</param>
	public VBit8(sbyte sb) : this(Convert.ToByte(sb)) { }
	/// <summary>
	/// Создаёт объект на основе символа.
	/// </summary>
	/// <param name="c">UTF-8 символ.</param>
	public VBit8(char c) : this(Convert.ToByte(c)) { }
	/// <summary>
	/// Создаёт объект на основе логического массива.
	/// </summary>
	/// <param name="bits">Логический массив.</param>
	public VBit8(params bool[] bits)
	{
		if (bits.Length > BitCount)
			throw new InvalidOperationException("Very large bit array");

		Package(bits);
	}
	/// <summary>
	/// Создаёт объект на основе любого <see cref="IBitable">объекта работающего с битами</see>.
	/// </summary>
	/// <param name="bitable">Объект работающий с битами.</param>
	public VBit8(IBitable bitable)
	{
		Package(bitable.Bits);
	}
	/// <summary>
	/// Создаёт объект на основе <see cref="byte">байт</see> массива.
	/// </summary>
	/// <param name="buffer">Байт массив.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public VBit8(byte[] buffer)
	{
		if (buffer.Length > Size)
			throw new InvalidOperationException($"{nameof(buffer)} very large");

		_byte = NormalizeArray(buffer, Size)[0];
	}
	/// <summary>
	/// Создаёт объект на основе любого <see cref="IByteable">объекта работающего с байтами</see>.
	/// </summary>
	/// <param name="byteable">Объект работающий с байтами.</param>
	public VBit8(IByteable byteable)
	{
		_byte = NormalizeArray(byteable.Bytes, Size)[0];
	}

	/// <summary>
	/// Переключает бит по индексу.
	/// </summary>
	/// <param name="index">Индекс переключаемого бита.</param>
	public void Toggle(int index) => SetBit(index, !GetBit(index));

	/// <summary>
	/// Сравнивает два <see cref="VBit8"/> объекта.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para>1 - если значение родного объекта больше.</para>
	/// <para>0 - если значения объектов одинаковые.</para>
	/// <para>-1 - если значение родного объекта меньше</para>
	/// </returns>
	public int CompareTo(VBit8 bb)
	{
		return _byte.CompareTo(bb._byte);
	}
	/// <summary>
	/// Сравнивает два <see cref="VBit8"/> объекта.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para>1 - если значение родного объекта больше.</para>
	/// <para>0 - если значения объектов одинаковые.</para>
	/// <para>-1 - если значение родного объекта меньше</para>
	/// </returns>
	public int CompareTo(VBit8? bb)
	{
		if (bb is null)
			return 1;

		return _byte.CompareTo(bb.Value._byte);
	}
	/// <summary>
	/// Сравнивает два <see cref="VBit8"/> объекта.
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
		if (obj is not VBit8 bb)
			return _byte.CompareTo(obj);
		return _byte.CompareTo(bb._byte);
	}

	/// <summary>
	/// Сравнивает родной объект с чужим на равенство.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public bool Equals(VBit8? bb)
	{
		if (bb is null)
			return false;
		return GetHashCode() == bb.GetHashCode();
	}
	/// <summary>
	/// Сравнивает родной объект с чужим на равенство.
	/// </summary>
	/// <param name="bb">Объект с которым идёт сравнение.</param>
	/// <returns>
	/// <para><c>true</c> - если значения объектов равны.</para>
	/// <para><c>false</c> - если значения объектов различаются.</para>
	/// </returns>
	public bool Equals(VBit8 bb)
	{
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
		if (obj is null || obj is not VBit8 bb)
			return false;
		return GetHashCode() == bb.GetHashCode();
	}
	/// <summary>
	/// Предоставляет хэш-код объекта.
	/// </summary>
	public override int GetHashCode() => _byte;
	/// <summary>
	/// Конвертирует объект в строковое представление.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		StringBuilder sb = new($"{_byte}: [");
		for (int i = 0; i < BitCount; i++)
		{
			sb.Append(GetBit(i) ? '1' : '0');
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
			yield return GetBit(i);
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Клонирует объект.
	/// </summary>
	/// <returns></returns>
	public VBit8 Clone() => new(_byte);
	object ICloneable.Clone() => Clone();

	/// <summary>
	/// Предоставляет безопастный доступ к логическому массиву.
	/// </summary>
	/// <param name="index">Индекс получаемого или задаваемого бита.</param>
	/// <returns></returns>
	public bool this[Index index]
	{
		get => GetBit(index);
		set => SetBit(index, value);
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
	public static bool operator ==(VBit8? x, VBit8? y)
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
	public static bool operator !=(VBit8? x, VBit8? y)
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
	public static bool operator <(VBit8? x, VBit8? y)
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
	public static bool operator >(VBit8? x, VBit8? y)
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
	public static bool operator >=(VBit8? x, VBit8? y)
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
	public static bool operator <=(VBit8? x, VBit8? y)
	{
		var result = Compare(y, x);
		return result >= 0;
	}

	/// <summary>
	/// Логическое отрицание.
	/// </summary>
	/// <param name="x">Отицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static VBit8 operator !(VBit8 x)
	{
		var result = new VBit8();
		for (int i = 0; i < BitCount; i++)
			result[i] = !x[i];
		return result;
	}
	/// <summary>
	/// Побитовое отрицание.
	/// </summary>
	/// <param name="x">отрицаемый объект.</param>
	/// <returns>Новый отрицательный к текущему объект</returns>
	public static VBit8 operator ~(VBit8 x)
	{
		return !x;
	}
	/// <summary>
	/// Логическое ИЛИ.
	/// </summary>
	/// <param name="x">Первый объект.</param>
	/// <param name="y">Второй объект.</param>
	/// <returns>Новый объект созданный на базе вычесление логического ИЛИ двух операндов.</returns>
	public static VBit8 operator |(VBit8 x, VBit8 y)
	{
		var result = new VBit8();
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
	public static VBit8 operator &(VBit8 x, VBit8 y)
	{
		var result = new VBit8();
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
	public static VBit8 operator ^(VBit8 x, VBit8 y)
	{
		var result = new VBit8();
		for (int i = 0; i < BitCount; i++)
			result[i] = x[i] ^ y[i];
		return result;
	}


	/// <summary>
	/// Неявное преобразование логического массива в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator VBit8(bool[] x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="byte">беззнакового байта</see> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator VBit8(byte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="sbyte">байта со знаком</see> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator VBit8(sbyte x) => new(x);
	/// <summary>
	/// Неявное преобразование <see cref="char">символа</see> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator VBit8(char x) => new(x);

	/// <summary>
	/// Явное преобразование <see cref="short"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(short x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="ushort"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(ushort x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="int"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(int x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="uint"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(uint x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="long"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(long x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="ulong"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(ulong x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="float"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(float x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="double"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(double x) => new(Convert.ToByte(x));
	/// <summary>
	/// Явное преобразование <see cref="decimal"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(decimal x) => new(Convert.ToByte(x));

	/// <summary>
	/// Явное преобразование <see cref="VBit16"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(VBit16 x) => new(Convert.ToByte(x.Short));
	/// <summary>
	/// Явное преобразование <see cref="VBit32"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(VBit32 x) => new(Convert.ToByte(x.Int));
	/// <summary>
	/// Явное преобразование <see cref="VBit64"/> в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="x"></param>
	public static explicit operator VBit8(VBit64 x) => new(Convert.ToByte(x.Long));


	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в логический массив.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator bool[](VBit8 x) => x.Bits;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="char">UTF-8 символ</see>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator char(VBit8 x) => x.Char;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="byte">беззнаковый байт</see>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator byte(VBit8 x) => x.Byte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="sbyte">байт со знаком</see>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator sbyte(VBit8 x) => x.SByte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="short"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator short(VBit8 x) => x.SByte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="ushort"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator ushort(VBit8 x) => x.Byte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="int"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator int(VBit8 x) => x.SByte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="uint"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator uint(VBit8 x) => x.Byte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="long"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator long(VBit8 x) => x.SByte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="ulong"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator ulong(VBit8 x) => x.Byte;
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="float"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator float(VBit8 x) => Convert.ToSingle(x.Byte);
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="double"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator double(VBit8 x) => Convert.ToDouble(x.Byte);
	/// <summary>
	/// Неявное преобразование <see cref="VBit8"/> в <see cref="decimal"/>.
	/// </summary>
	/// <param name="x"></param>
	public static implicit operator decimal(VBit8 x) => Convert.ToDecimal(x.Byte);

	#region private
	private bool GetBit(Index index)
	{
		byte clearbyte = (byte) (_byte & BITS[index]);
		return clearbyte == BITS[index];
	}
	private void SetBit(Index index, bool value)
	{
		if (value)
			_byte |= BITS[index];
		else
			_byte &= RBITS[index];
	}
	private void Package(bool[] bits)
	{
		_byte = 0;
		int cycle = bits.Length >= BitCount ? BitCount : bits.Length;
		for (int i = 0; i < cycle; i++)
			if (bits[i]) _byte |= BITS[i];
	}
	private bool[] Demount()
	{
		bool[] bits = new bool[BitCount];
		for (int i = 0; i < BitCount; i++)
			bits[i] = GetBit(i);
		return bits;
	}
	#endregion


	#region static
	/// <summary>
	/// Количество битов.
	/// <para>Размер логического массива.</para>
	/// </summary>
	public const int BitCount = 8;
	/// <summary>
	/// Размер (в байтах) хранимого значения.
	/// </summary>
	public const int Size = 1;
	/// <summary>
	/// Максимально возможно значение.
	/// </summary>
	public const byte MaxValue = byte.MaxValue;
	/// <summary>
	/// Минимально возможное значение.
	/// </summary>
	public const sbyte MinValue = sbyte.MinValue;

	/// <summary>
	/// Парсинг строкового значения в <see cref="VBit8"/>.
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">Если строка имеет неправильный формат.</exception>
	public static VBit8 Parse(string line)
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
	/// Попытка отпарсить строковое значение в <see cref="VBit8"/>
	/// </summary>
	/// <param name="line">Парсируемая строка.</param>
	/// <param name="result">Выходной результат парсинга.</param>
	/// <returns>
	/// <para><c>true</c> - если парсинг прошёл успешно.</para>
	/// <para><c>false</c> - если парсинг не удался.</para>
	/// </returns>
	public static bool TryParse(string line, out VBit8 result)
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
	public static int Compare(VBit8? x, VBit8? y)
	{
		if (x is null && y is null)
			return 0;
		if (x is not null && y is null)
			return 1;
		if (x is null && y is not null)
			return -1;

		return x!.Value._byte.CompareTo(y!.Value._byte);
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
	public static bool Equals(VBit8? x, VBit8? y)
	{
		if (x is null && y is null)
			return true;
		if (x is not null)
			return x.Equals(y);
		return false;
	}
	#endregion

	#region static private

	private static readonly byte[] BITS;
	private static readonly byte[] RBITS;

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

	static VBit8()
	{
		byte[] bs = new byte[BitCount];
		byte tmp = 1;
		for (int i = 0; i < BitCount; i++)
		{
			bs[i] = tmp;
			tmp *= 2;
		}
		BITS = bs.ToArray();

		byte[] rbs = new byte[BitCount];
		for (int i = 0; i < BitCount; i++)
			rbs[i] = (byte) ~BITS[i];
		RBITS = rbs;
	}
	#endregion
}
