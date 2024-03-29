﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameRules : MonoBehaviour
{
    //Local Player = we,Enemy = Instantiated copy of our Enemy on this Scene
    public static Player Enemy;
    public static Player Local;
    
    [SerializeField]public Text gameResult;
    
    private bool _endGame;
        
    public static void SetStatusByType(GameObject owner)
    {
        if(owner.CompareTag("Enemy"))
        {
            Enemy = owner.GetComponent<Player>();
            Debug.Log($"Enemy Synchronized{Enemy.name}");
        }
        
        if (!owner.CompareTag("Local")) return;
        
        Local = owner.GetComponent<Player>();
        Debug.Log($"Local Player Synchronized{Local.name}");
    }

    private void FixedUpdate()
    {
        if( _endGame || Local == null)
            return;

        if (Local.IsDead)
        {
            Lose();
        }
        
        if(!IsCountPlayersInRoom(2))
            return;
        
        //Check for the Situation,when we and enemy connect to the Room,but the Enemy link didn't get yet a Value
        if (Enemy == null)
        {
            return;
        }
        
        if(Enemy.IsDead)
        {
            Win();
        }
    }
    
    private void Win()
    {
        gameResult.text = "You Win";
        _endGame = true;
        LocalPlayerWin();
    }
    
    private void Lose()
    {
        gameResult.text = "You Lose";
        _endGame = true;
    }
    
    private void LocalPlayerWin()
    {
        PhotonView photonView=PhotonView.Get(Local);
        photonView.RPC("Win",RpcTarget.All);
    }
    
    private bool IsCountPlayersInRoom(int number)
    {
        return PhotonNetwork.PlayerList.Length == number;
    }
}