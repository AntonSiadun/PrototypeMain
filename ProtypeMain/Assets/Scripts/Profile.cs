using UnityEngine;

public class Profile : MonoBehaviour
{
    public SaveData saveData;
    
    private ProfileDataSave _profileDataSave;
    
    public static Profile Instance;
    
    private void Awake()
    {
        _profileDataSave = new ProfileDataSave();
        
        saveData = new SaveData();
        
        Instance = this;
        
        if (ProfileDataSave.FileExist())
            Instance.saveData = ProfileDataSave.Read();
    }
}
