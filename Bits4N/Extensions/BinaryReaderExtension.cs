
namespace Bits4N.Extensions;

/// <summary>
/// Методы расширения для <see cref="BinaryReader"/>.
/// </summary>
public static class BinaryReaderExtension
{
	/// <summary>
	/// Чтение <see cref="IByteable"/> типа в <see cref="byte">байт</see> массив.
	/// </summary>
	/// <param name="reader"></param>
	/// <returns></returns>
	public static byte[] ReadByteable(this BinaryReader reader)
	{
		int length = reader.Read7BitEncodedInt();
		return reader.ReadBytes(length);
	}
	/// <summary>
	/// Чтение <see cref="IByteable"/> в указанный тип (<typeparamref name="T"/>).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="reader"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static T ReadByteable<T>(this BinaryReader reader) where T : IByteable
	{
		var buffer = reader.ReadByteable();
		var type = typeof(T);
		var argsTypes = new Type[] { typeof(byte[]) };
		var ctor = type.GetConstructor(argsTypes);
		if (ctor is null)
			throw new InvalidOperationException($"Type {nameof(T)} has no ctor(byte[])");
		return (T) ctor.Invoke(new object[] { buffer });
	}
}
