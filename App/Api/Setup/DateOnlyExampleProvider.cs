using APIWeaver;

namespace Api.Setup;

public sealed class DateOnlyExampleProvider : IExampleProvider<DateOnly>
{
    public static DateOnly GetExample() => DateOnly.FromDateTime(DateTime.UtcNow);
}
