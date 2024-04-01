using Manager;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void Save(int levelReached)
    {
        string path = Application.persistentDataPath + "/Player.save";

        SaveData saveData = new SaveData(levelReached); //appelle la méthode

        string data = JsonUtility.ToJson(saveData);//transform les données
        string dataString = SecureHelper.DecryptAndCrypt(data);

        FileStream jsonFile = new FileStream(path, FileMode.Create);//créé le fichier json
        jsonFile.Close();//ferme le fichier
        File.WriteAllText(path, dataString); //écrit data avec le streamWriter json 
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/Player.save";

        if (!File.Exists(path))
        {
            Save(0);
        }

        string data = File.ReadAllText(path);

        string dataString = SecureHelper.DecryptAndCrypt(data);
        SaveData saveData = JsonUtility.FromJson<SaveData>(dataString);
        return saveData;
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/Player.save";

        if (!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }
}

[System.Serializable]
public class SaveData
{
    public int m_LevelReached = 0;

    public SaveData(int levelReached)
    {
        m_LevelReached = levelReached;
    }
}