using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerTriggerSingle : MonoBehaviour
{
    public int Health { get; private set; }
        
    private PlayerSingle _player;

    private void Awake()
    {
        _player = GetComponent<PlayerSingle>();
        Health = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsMobWeaponCollideWithPlayer(other) || _player.invincible)
            return;
        GetDamageFromMob(other);
        CheckPlayerHealthAndReactOrKill();
    }

    private bool IsMobWeaponCollideWithPlayer(Collider sword)
    {
        return sword.gameObject.CompareTag("MobSword");
    }
    
    private void GetDamageFromMob(Collider mob)
    {
        if(!IsMobWeaponCollideWithPlayer(mob) )
            return;
        Debug.Log("Mob Attack us!");
        Health -= 1;
    }
      
    private void CheckPlayerHealthAndReactOrKill()
    {
        if (Health <= 0)
        {
            _player.Kill();
        }
        else
        {
            _player.React();
        }
    }
        

}