namespace Bits4N.Extensions;

/// <summary>
/// Класс методов расширения для BinaryWriter
/// </summary>
public static class BinaryWriterExtension
{
	/// <summary>
	/// Записывает любой <see cref="IByteable"/>.
	/// </summary>
	/// <param name="writer"></param>
	/// <param name="byteable"></param>
	public static void Write(this BinaryWriter writer, IByteable byteable)
	{
		writer.Write7BitEncodedInt(byteable.Bytes.Length);
		writer.Write(byteable.Bytes);
	}
}
