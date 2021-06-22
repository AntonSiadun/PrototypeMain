using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        SetStandartNickNameAndGameVersion();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinGame()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CountOfRooms == 0)
        {
            Debug.Log("Room is not created yet");
            return;
        }
        JoinRandomRoom();
    }

    private void SetStandartNickNameAndGameVersion()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 10000);
        PhotonNetwork.GameVersion = "0.0.1";
    }
    
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 2});
    }
    private static void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinedRoom()
    { 
        Debug.Log("Joined random room");
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }
}