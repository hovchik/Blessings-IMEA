namespace Blessings.User.Api.Cryptography;

public class PasswordResult
{
    public byte[] PasswordHash { get; set; }

    public byte[] Salting { get; set; }
}