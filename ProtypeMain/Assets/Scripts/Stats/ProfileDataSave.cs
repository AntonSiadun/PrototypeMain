using System;
using System.IO;
using UnityEngine;

public class ProfileDataSave : MonoBehaviour
{
    private string _path;
    public ProfileData _savedData { private set; get; }
    
    public bool isExist = false;
    private void Awake()
    {
        _savedData =new ProfileData();
        
        #if UNITY_ANDROID && ! UNITY_EDITOR
            _path = Path.Combine(Application.persistentDataPath,"Save.json");
        #else
            _path = Path.Combine(Application.dataPath, "Save.json");
        #endif

        if (File.Exists(_path))
        {
            _savedData = Read();
            isExist = true;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void Save()
    {
        File.WriteAllText(_path,JsonUtility.ToJson(_savedData));
    }

    public ProfileData Read()
    {
        return JsonUtility.FromJson<ProfileData>(File.ReadAllText(_path));
    }

}
