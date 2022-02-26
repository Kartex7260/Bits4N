namespace Bits4N;

/// <summary>
/// Обобщённый интерфейс клонирования.
/// </summary>
/// <typeparam name="T">Тип клонируемого объекта.</typeparam>
public interface ICloneable<T> : ICloneable
{
	/// <summary>
	/// Клонирует объект.
	/// </summary>
	/// <returns>Клон указанного объекта.</returns>
	new T Clone();
}
