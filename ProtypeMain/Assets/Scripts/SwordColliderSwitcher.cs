using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderSwitcher : MonoBehaviour
{
    [SerializeField] public GameObject sword;
    
    public void DisableSword()
    {
        sword.GetComponent<Collider>().enabled = false;
    }

    public void EnableSword()
    {
        sword.GetComponent<Collider>().enabled = true;
    }
}
