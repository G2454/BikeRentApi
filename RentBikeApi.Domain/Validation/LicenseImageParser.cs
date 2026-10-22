namespace RentBikeApi.Domain.Validation
{
    public static class LicenseImageParser
    {
        private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
        private static readonly byte[] BmpSignature = [0x42, 0x4D];

        public static bool TryParse(string? base64, out byte[] content, out string extension)
        {
            content = [];
            extension = string.Empty;

            if (string.IsNullOrWhiteSpace(base64))
                return false;

            var commaIndex = base64.IndexOf(',');
            if (commaIndex >= 0)
                base64 = base64[(commaIndex + 1)..];

            try
            {
                content = Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                return false;
            }
            if (content.AsSpan().StartsWith(PngSignature))
            {
                extension = ".png";
                return true;
            }
            if (content.AsSpan().StartsWith(BmpSignature))
            {
                extension = ".bmp";
                return true;
            }
            return false;
        }
    }
}
