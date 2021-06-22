using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviourPunCallbacks
{
    public GameObject prefabPlayer;
    public static GameScene Instance;
    
    private void Awake()
    {
        Instance = this;
        if (prefabPlayer != null)
            PhotonNetwork.Instantiate(this.prefabPlayer.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
        base.OnLeftRoom();
    }
}