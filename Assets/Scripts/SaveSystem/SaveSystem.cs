using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    static bool isLoaded = false;
    static int levelReached = 0;

    public static int GetLevelReached()
    {
        if(!isLoaded)
        {
            levelReached = LoadLevelReached();
        }

        return levelReached;
    }

    public static void SaveLevelReached(int levelReached)
    {
        isLoaded = false;
        string path = Application.persistentDataPath + "/Save.exe";

        string dataString = levelReached.ToString();

        FileStream file = new FileStream(path, FileMode.Create);
        file.Close();
        File.WriteAllText(path, SecureHelper.DecryptAndCrypt(dataString));
    }

    static int LoadLevelReached()
    {

        string path = Application.persistentDataPath + "/Save.exe";

        if (!File.Exists(path))
        {
            SaveLevelReached(0);
        }

        string data = File.ReadAllText(path);

        string dataString = SecureHelper.DecryptAndCrypt(data);
        int saveData = int.Parse(dataString);

        isLoaded = true;

        return saveData;
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/Save.exe";

        if (!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }
}