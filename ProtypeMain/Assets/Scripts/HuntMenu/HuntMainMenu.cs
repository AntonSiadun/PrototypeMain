using UnityEngine;
using UnityEngine.SceneManagement;
public class HuntMainMenu : MonoBehaviour
{
    public void LoadScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }
}
