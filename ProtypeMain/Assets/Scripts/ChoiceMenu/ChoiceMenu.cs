using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceMenu : MonoBehaviour
{
    void Start()
    {
        if (ProfileDataSave.FileExist())
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ChoseHero(int type)
    {
        Profile.Instance.saveData.HeroStats = new HeroStats((HeroType)type);
        ProfileDataSave.Save(Profile.Instance);
        
        SceneManager.LoadScene(1);
    }
}
