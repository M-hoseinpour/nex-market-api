using System.Security.Cryptography;
using market.SystemServices.Contracts;

namespace market.SystemServices;

public class RandomService : IRandomService
{
    private readonly RandomNumberGenerator _randomNumberGenerator;

    public RandomService(RandomNumberGenerator randomNumberGenerator)
    {
        _randomNumberGenerator = randomNumberGenerator;
    }


    public string GetSecureAlphaNumericString(int len)
    {
        return GetSecureRandomString(
            len: len,
            chars: "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );
    }

    public string GetSecureNumericString(int len)
    {
        return GetSecureRandomString(len: len, chars: "0123456789");
    }

    public string GetSecureRandomString(int len, string chars)
    {
        var result = "";

        var bytes = new byte[len];

        _randomNumberGenerator.GetBytes(bytes);

        for (var i = 0; i < len; i++)
        {
            var b = bytes[i];
            var index = (int)(b * (chars.Length - 1) / 255.0);
            result += chars[index];
        }

        return result;
    }
}