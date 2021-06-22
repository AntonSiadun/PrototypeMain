using UnityEngine;

public class GameSceneSingle : MonoBehaviour
{
    [SerializeField]public GameObject prefabPlayer;
    
    private void Awake()
    {
        if (prefabPlayer != null)
            Instantiate(prefabPlayer, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void LeaveGame()
    {
        SceneLoaderByNumber.LoadScene(1);
    }
}