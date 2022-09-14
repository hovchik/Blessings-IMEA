using System.Security.Cryptography;
using System.Text;

namespace Blessings.User.Api.Cryptography;

public static class PasswordHasher
{
    private const int Size = 64;

    public static PasswordResult Generate(string password, int iterations = 10000)
    {
        //generate a random salt for hashing
        PasswordResult result = new PasswordResult
        {
            Salting = new byte[Size]
        };
        new RNGCryptoServiceProvider().GetBytes(result.Salting);

        var deriveBytes = new Rfc2898DeriveBytes(password, result.Salting, iterations, HashAlgorithmName.SHA512);
        result.PasswordHash = deriveBytes.GetBytes(Size);

        return result;
    }

    public static string Generate(string password, byte[] salting, int iterations = 10000)
    {

        var deriveBytes = new Rfc2898DeriveBytes(password, salting, iterations, HashAlgorithmName.SHA512);
        return Encoding.Unicode.GetString(deriveBytes.GetBytes(Size));
    }
    public static bool IsValid(byte[] originalPasswordHash, string password, byte[] salting, int iterations = 10000)
    {
        //generate hash from test password and original salt and iterations
        var pbkdf2 = new Rfc2898DeriveBytes(password, salting, iterations, HashAlgorithmName.SHA512);
        byte[] hashBytes = pbkdf2.GetBytes(Size);


        return hashBytes.SequenceEqual(originalPasswordHash);
    }
}