using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    public void NextScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
