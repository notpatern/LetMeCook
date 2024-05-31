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

    static void Save(SaveData saveData)
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
            Save(new SaveData(0, null, null));
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
            saveData = new SaveData(0, null, null);
            Save(saveData);
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

        Save(saveData);
    }

    public static void SaveLevelReached(int[] levelReached)
    {
        SaveData saveData = GetSavedData();
        saveData.m_LevelHighScores = levelReached;

        Save(saveData);
    }

    public static void SaveLevelsStar(int[] levelUnlockedStarsNumber)
    {
        SaveData saveData = GetSavedData();
        saveData.m_LevelUnlockedStarsNumber = levelUnlockedStarsNumber;

        Save(saveData);
    }
}

public class SaveData
{
    public int m_LevelReached = 0;
    public int[] m_LevelHighScores = new int[0];
    public int[] m_LevelUnlockedStarsNumber = new int[0];

    public SaveData(int levelReached, int[] levelScores, int[] levelUnlockedStarsNumber) 
    {
        m_LevelReached = levelReached;
        m_LevelHighScores = levelScores;
        m_LevelUnlockedStarsNumber = levelUnlockedStarsNumber;
    }
}