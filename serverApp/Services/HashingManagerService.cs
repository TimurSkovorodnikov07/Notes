public class HashingManagerService : IHasher, IHashVerify
{
    public string Hashing(string str)
    {
        return BCrypt.Net.BCrypt.HashPassword(str);
    }

    public bool Verify(string str, string hashStr)
    {
        return BCrypt.Net.BCrypt.Verify(str, hashStr);
    }
}