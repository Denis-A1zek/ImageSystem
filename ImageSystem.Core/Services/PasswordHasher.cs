using System.Security.Cryptography;

namespace ImageSystem.Core;

public class PasswordHasher : IPasswordHasher
{
    private const int SALT_LENGTH = 128 / 8;
    private const int KEY_SIZE = 256 / 8;
    private const int ITERATIONS = 10000;
    private static readonly HashAlgorithmName _algorithmName = HashAlgorithmName.SHA256;
    private const char DELIMETER = ';';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_LENGTH);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, _algorithmName, KEY_SIZE);
        return string.Join(DELIMETER, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Verify(string passwordHash, string passwordInput)
    {
        var elements = passwordHash.Split(DELIMETER);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(passwordInput, salt, ITERATIONS, _algorithmName, KEY_SIZE);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}
