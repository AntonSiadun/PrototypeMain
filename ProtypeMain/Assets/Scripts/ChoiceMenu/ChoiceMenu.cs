using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChoiceMenu : MonoBehaviour
{
    [SerializeField] public ProfileDataSave _profileSave;
    
    void Start()
    {
        if (_profileSave.isExist)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ChoseHero(HeroType type)
    {
        _profileSave._savedData.Create(type);
        _profileSave.Save();
        
        SceneManager.LoadScene(1);
    }

    public void CreateMaster()
    {
        ChoseHero(HeroType.Master);
    }
    public void CreateWarrior()
    {
        ChoseHero(HeroType.Warrior);
    }
    public void CreatePriest()
    {
        ChoseHero(HeroType.Priest);
    }
    

}
