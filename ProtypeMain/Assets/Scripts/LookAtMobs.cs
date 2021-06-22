using System.Collections.Generic;
using UnityEngine;

public class LookAtMobs : MonoBehaviour
{
    private List<GameObject> Mobs =new List<GameObject>();
    private GameObject player;
    
    private GameObject CalculateNearMob()
    {
        if (Mobs.Count == 0) 
            return null;
        
        var Near = Mobs[0];

        foreach (var mob in Mobs)
        {
            var mobPosition = mob.transform.position;
            var playerPosition = player.transform.position;
            var distance = Vector3.Distance(mobPosition, playerPosition);

            var nearDistance = Vector3.Distance(Near.transform.position, playerPosition);

            if (distance < nearDistance)
                Near = mob;
        }
        return Near;
    }

    public void AddObservabelMobToList(GameObject mob)
    {
        Mobs.Add(mob);
    }

    public void RemoveObservabelMobFromList(GameObject mob)
    {
        Mobs.Remove(mob);
    }
    
    private void Start()
    {
        player= GameObject.FindWithTag("Local");
    }

    private void FixedUpdate()
    {
        if (Mobs.Count == 0 || player.GetComponent<PlayerSingle>().PlayerHasSomeBlock())
            return;
        player.transform.LookAt(CalculateNearMob().transform);
    }
    
}