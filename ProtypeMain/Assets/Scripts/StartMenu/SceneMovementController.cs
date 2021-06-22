using UnityEngine;

public class SceneMovementController : MonoBehaviour
{
    public void NextScene(int numberScene)
    {
        SceneLoaderByNumber.LoadScene(numberScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
