using System.Security.Cryptography;

namespace VelocipedSite.Utilities;

public sealed class PasswordCrypt
{
    private const int SaltSize = 16, HashSize = 20, HashIter = 10000;

    public PasswordCrypt(string password)
    {
        Salt = RandomNumberGenerator.GetBytes(SaltSize);
        Hash = new Rfc2898DeriveBytes(password, Salt, HashIter, HashAlgorithmName.SHA256).GetBytes(HashSize);
    }
    public PasswordCrypt(byte[] hashBytes)
    {
        Array.Copy(hashBytes, 0, Salt = new byte[SaltSize], 0, SaltSize);
        Array.Copy(hashBytes, SaltSize, Hash = new byte[HashSize], 0, HashSize);
    }
    public PasswordCrypt(byte[] salt, byte[] hash)
    {
        Array.Copy(salt, 0, Salt = new byte[SaltSize], 0, SaltSize);
        Array.Copy(hash, 0, Hash = new byte[HashSize], 0, HashSize);
    }
    
    public byte[] ToArray()
    {
        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(Salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(Hash, 0, hashBytes, SaltSize, HashSize);
        return hashBytes;
    }
    
    public byte[] Salt { get; }

    public byte[] Hash { get; }

    public bool Verify(string password)
    {
        var test = new Rfc2898DeriveBytes(password, Salt, HashIter, HashAlgorithmName.SHA256).GetBytes(HashSize);
        return test.SequenceEqual(Hash);
        // for (var i = 0; i < HashSize; i++)
        //     if (test[i] != Hash[i])
        //         return false;
        // return true;
    }
}