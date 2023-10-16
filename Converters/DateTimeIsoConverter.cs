using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace market.Converters;

public class DateTimeIsoConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var incomingValue = reader.GetString();
        if (!IsIso(incomingValue))
            throw new NotValidDateFormatException();

        var date = DateTime.Parse(incomingValue ?? string.Empty);
        return date.ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
    }

    private static bool IsIso(string value)
    {
        const string Pattern =
            @"^(\d{4})-(0[1-9]|1[0-2])-([0-2]\d|3[01])T([01]\d|2[0-3]):([0-5]\d):([0-5]\d)(\.\d+)?(Z|[+-]([01]\d|2[0-3]):([0-5]\d))$";

        return Regex.IsMatch(input: value, pattern: Pattern);
    }
}

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var incomingValue = reader.GetString();
        if (TimeOnly.TryParse(incomingValue, out var time))
        {
            return time;
        }

        throw new NotValidDateFormatException();
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm:ss"));
    }
}