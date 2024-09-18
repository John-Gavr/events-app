using AutoMapper;

public class Base64ToByteArrayConverter : IValueConverter<string, byte[]>
{
    public byte[] Convert(string sourceMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(sourceMember))
        {
            return Array.Empty<byte>();
        }
        return System.Convert.FromBase64String(sourceMember);
    }
}
