using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    static SaveData m_LoadedData;

    public static SaveData GetSavedData()
    {
        if(m_LoadedData == null)
        {
            m_LoadedData = LoadSavedData();
        }

        return m_LoadedData;
    }

    static void SaveLevelReached(SaveData saveData)
    {
        m_LoadedData = saveData;
        string path = Application.persistentDataPath + "/Save.exe";

        string dataString = JsonUtility.ToJson(m_LoadedData);

        FileStream file = new FileStream(path, FileMode.Create);
        file.Close();
        File.WriteAllText(path, SecureHelper.DecryptAndCrypt(dataString));
    }

    static SaveData LoadSavedData()
    {

        string path = Application.persistentDataPath + "/Save.exe";

        if (!File.Exists(path))
        {
            SaveLevelReached(new SaveData(0, null));
        }

        string data = File.ReadAllText(path);

        string dataString = SecureHelper.DecryptAndCrypt(data);

        SaveData saveData;

        try
        {
            saveData = JsonUtility.FromJson(dataString, typeof(SaveData)) as SaveData;
        }
        catch
        {
            saveData = new SaveData(0, null);
            SaveLevelReached(saveData);
        }

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

    public static void SaveLevelReached(int levelReached)
    {
        SaveData saveData = GetSavedData();
        saveData.m_LevelReached = levelReached;

        SaveLevelReached(saveData);
    }

    public static void SaveLevelReached(int[] levelReached)
    {
        SaveData saveData = GetSavedData();
        saveData.m_LevelScores = levelReached;

        SaveLevelReached(saveData);
    }
}

public class SaveData
{
    public int m_LevelReached = 0;
    public int[] m_LevelScores = new int[0];

    public SaveData(int levelReached, int[] levelScores) 
    {
        m_LevelReached = levelReached;
        m_LevelScores = levelScores;
    }
}