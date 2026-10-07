using System.Security.Cryptography;
using System.Text;

public static class HashUtility
{
    public static string CalculateSHA256(string rawData)
    {
        // 1. Create the SHA256 object
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // 2. Convert the input string to a byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // 3. Convert the byte array into a long hexadecimal string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}