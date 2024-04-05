using UnityEngine;

public static class SecureHelper 
{
    public static string DecryptAndCrypt(string data)
    {
        string decrypted = "";
        string key = "rpvn 86146314ds 614 é'eé\"&\"&\"45\" é78\"3647#~{|[(8-576";

        for (int i = 0; i < data.Length; i++)
        {
            decrypted = decrypted + (char)(data[i] ^ key[i % key.Length]);
        }
        return decrypted;
    }
}
