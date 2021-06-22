using Photon.Pun;
using UnityEngine;

public class SwordController : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField] public GameObject owner;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GetComponent<Collider>().enabled);
        }
        else
        { 
            GetComponent<Collider>().enabled = (bool)stream.ReceiveNext();
        }
    }


}