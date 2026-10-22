namespace RentBikeApi.Domain.Validation;

public static class Cnpj
{
    private static readonly int[] FirstWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] SecondWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    // "12.345.678/0001-95" → "12345678000195"
    public static string Normalize(string? value)
        => new((value ?? string.Empty)
            .Where(char.IsLetterOrDigit)
            .Select(char.ToUpperInvariant)
            .ToArray());

    public static bool IsValid(string normalized)
    {
        if (normalized.Length != 14)
            return false;

        // 12 base characters (letters or digits) + 2 numeric check digits
        if (!normalized[..12].All(c => char.IsAsciiDigit(c) || char.IsAsciiLetterUpper(c)) ||
            !normalized[12..].All(char.IsAsciiDigit))
            return false;

        // "11111111111111" passes the math but isn't a real CNPJ
        if (normalized.Distinct().Count() == 1)
            return false;

        var firstCheckDigit = CalculateCheckDigit(normalized, FirstWeights);
        var secondCheckDigit = CalculateCheckDigit(normalized, SecondWeights);

        return normalized[12] - '0' == firstCheckDigit &&
               normalized[13] - '0' == secondCheckDigit;
    }

    private static int CalculateCheckDigit(string value, int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < weights.Length; i++)
            sum += (value[i] - '0') * weights[i];

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}