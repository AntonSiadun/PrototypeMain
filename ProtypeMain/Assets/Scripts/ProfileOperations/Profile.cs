using UnityEngine;
using System.IO;

namespace ProfileOperations
{
    public class Profile : MonoBehaviour
    {
        public static Profile Instance;
        public ProfileRecords profileRecords;
        
        private string _path;

        private void Awake()
        {
            
            #if UNITY_ANDROID && ! UNITY_EDITOR
                _path = Path.Combine(Application.persistentDataPath,"Save.json");
            #else
                _path = Path.Combine(Application.dataPath, "Save.json");
            #endif
            profileRecords = new ProfileRecords();
            Instance = this;
            if (FileExist())
                Instance.profileRecords = Read();
        }
        
        private bool FileExist() => File.Exists(_path);

        public bool IsExistSaveFile()
        {
            return FileExist();
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonUtility.ToJson(profileRecords));
        }

        public ProfileRecords Read()
        {
            return JsonUtility.FromJson<ProfileRecords>(File.ReadAllText(_path));
        }
    }
    
}