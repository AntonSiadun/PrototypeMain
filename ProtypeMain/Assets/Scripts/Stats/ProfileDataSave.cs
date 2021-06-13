using System.IO;
using UnityEngine;

public class ProfileDataSave 
{
    private static string _path;

    public ProfileDataSave()
    {
        #if UNITY_ANDROID && ! UNITY_EDITOR
            _path = Path.Combine(Application.persistentDataPath,"Save.json");
        #else
            _path = Path.Combine(Application.dataPath, "Save.json");
        #endif
    }

    public static bool FileExist() => File.Exists(_path);

    public static void Save(Profile profile)
    {
        File.WriteAllText(_path,JsonUtility.ToJson(profile.saveData));
    }

    public static SaveData Read()
    {
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(_path));
    }

}
