using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameRulesSingle : MonoBehaviour
{
    private PlayerSingle Local;
    
    [SerializeField]public Text gameResult;
    
    private bool _endGame;

    private void Start()
    {
        Local = GameObject.FindWithTag("Local").GetComponent<PlayerSingle>();
        StartCoroutine(CheckMobCount());
    }

    private void FixedUpdate()
    {
        if( _endGame || Local == null)
            return;

        if (Local.IsDead)
        {
            Lose();
        }
    }
    
    private void Win()
    {
        gameResult.text = "You Win";
        Local.Win();
        _endGame = true;
    }
    
    private void Lose()
    {
        gameResult.text = "You Lose";
        StopCoroutine(CheckMobCount());
        _endGame = true;
    }

    IEnumerator CheckMobCount()
    {
        yield return new WaitForSeconds(2f);
        for (;;)
        {
            yield return new WaitForSeconds(1f);
            if (GetMobCount() == 0)
            {
                Win();
                yield break;
            }
        }
    }

    private int GetMobCount()
    {
        var mobs = GameObject.FindGameObjectsWithTag("Mob");
        return mobs.Length;
    }
    
}