using Photon.Pun;
using UnityEngine;

public class PlayerCharacterStatusSelector : MonoBehaviourPunCallbacks,IPunInstantiateMagicCallback
{
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.tag = GetTagByAffilation();
        GameRules.SetStatusByType(gameObject);
    }

    private string GetTagByAffilation() => photonView.IsMine ? "Local" : "Enemy";
}