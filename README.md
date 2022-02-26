## Введение
Bits4N — небольшая библиотека для работы с битами некоторых примитивных типов.

Поддерживаемые примитивные типы:
- `Bit8` — `byte`, `sbyte`, `char`
- `Bit16` — `ushort`, `short`, `char`
- `Bit32` — `uint`, `int`, `float`
- `Bit64` — `ulong`, `long`, `double`

## Изменение битовой составляющей
Пример:
```C#
Bit8 b8 = 2;
b8[0] = true;
Console.WriteLine(b8.Byte) // 3
```