namespace Bits4N;

/// <summary>
/// Интерфейс для передачи байтовых значений.
/// </summary>
public interface IByteable
{
	/// <summary>
	/// Предоставляет массив <see cref="byte">беззнаковых байтов</see>.
	/// </summary>
	byte[] Bytes { get; }
}
