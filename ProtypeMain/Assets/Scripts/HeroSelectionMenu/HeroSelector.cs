using ProfileOperations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroSelector : MonoBehaviour
{
    private void Start()
    {
        if (Profile.Instance.IsExistSaveFile())
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ChoseHero(int typeCode)
    {
        HeroType heroType = (HeroType) typeCode;
        Profile.Instance.profileRecords.HeroStats = new HeroParams(heroType);
        Profile.Instance.Save();
        SceneManager.LoadScene(1);
    }
}