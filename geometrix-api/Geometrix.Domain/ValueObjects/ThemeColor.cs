namespace Geometrix.Domain.ValueObjects;

[ValueObject<string>]
[Instance("Light", "light")]
[Instance("Dark", "dark")]
[Instance("Red", "red")]
[Instance("Yellow", "yellow")]
[Instance("Green", "green")]
[Instance("Blue", "blue")]
[Instance("Indigo", "indigo")]
[Instance("Purple", "purple")]
[Instance("Pink", "pink")]
public readonly partial struct ThemeColor;
